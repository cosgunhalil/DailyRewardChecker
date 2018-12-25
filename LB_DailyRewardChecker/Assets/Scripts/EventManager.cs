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

    public delegate void SecondsSettedDelegate(double val);
    public static event SecondsSettedDelegate OnDailyRewardRemainingTimeSetted;

    public void SetTimeToEarnDailyReward(double totalSeconds)
    {
        if (OnDailyRewardRemainingTimeSetted != null)
        {
            OnDailyRewardRemainingTimeSetted(totalSeconds);
        }
    }
}
