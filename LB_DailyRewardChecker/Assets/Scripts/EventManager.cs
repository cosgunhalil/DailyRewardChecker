using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    private static readonly EventManager instance = new EventManager();

    static EventManager()
    {

    }

    private EventManager()
    {

    }

    public static EventManager Instance
    {
        get
        {
            return instance;
        }
    }

    public delegate void MessageDelegate();
    public static event MessageDelegate OnDailyRewardEarn;

    public delegate void DoubleSettedDelegate(double val);
    public static event DoubleSettedDelegate OnDailyRewardRemainingTimeSetted;

    public void SetDailyRewardCanBeEarned()
    {
        if (OnDailyRewardEarn != null)
        {
            OnDailyRewardEarn();
        }
    }

    public void SetTimeToEarnDailyReward(double totalSeconds)
    {
        if (OnDailyRewardRemainingTimeSetted != null)
        {
            OnDailyRewardRemainingTimeSetted(totalSeconds);
        }
    }
}
