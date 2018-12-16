using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardPopUp : MonoBehaviour {

    public Button ClaimButton;
    public Button WatchAndEarnButton;

    private Canvas canvas;
    private int gainedDiamondCount;
    private DailyRewardChecker dailyRewardChecker;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        EventManager.OnDailyRewardEarn += EventManager_OnDailyRewardEarn;
        EventManager.OnDailyRewardRemainingTimeSetted += EventManager_OnDailyRewardRemainingTimeSetted;
    }

    private void Start()
    {
        gainedDiamondCount = 1;
        ClaimButton.onClick.AddListener(ClaimDailyReward);
        WatchAndEarnButton.onClick.AddListener(WatchAndEarnBonus);
        dailyRewardChecker = new DailyRewardChecker();
        CheckDailyReward();
    }

    private void OnDestroy()
    {
        EventManager.OnDailyRewardEarn -= EventManager_OnDailyRewardEarn;
        EventManager.OnDailyRewardRemainingTimeSetted -= EventManager_OnDailyRewardRemainingTimeSetted;
    }

    private void CheckDailyReward()
    {
        if (dailyRewardChecker.GetDailyRewardCanBeEarned())
        {
            EventManager.Instance.SetDailyRewardCanBeEarned();
        }
    }

    private void EventManager_OnDailyRewardEarn()
    {
        Activate();
    }

    private void Activate()
    {
        canvas.enabled = true;
        //listen is rewarded video watched
    }

    private void Deactivate()
    {
        canvas.enabled = false;
    }

    private void ClaimDailyReward()
    {
        //earn reward
        Deactivate();
        EventManager_OnDailyRewardRemainingTimeSetted(0);
    }

    private void WatchAndEarnBonus()
    {
        //show ad 
    }

    void EventManager_OnDailyRewardRemainingTimeSetted(double val)
    {
        StopCoroutine("RewardTimer");
        StartCoroutine("RewardTimer", val);
    }

    private IEnumerator RewardTimer(double seconds)
    {
        var wait = new WaitForSeconds(1f);

        var oneDayTotalSeconds = 86400;
        var remainingTime = oneDayTotalSeconds - seconds;

        while (remainingTime > 0)
        {
            yield return wait;
            remainingTime--;
            Debug.Log("Reward Remaining Time = " + remainingTime);
        }

        CheckDailyReward();

    }

}
