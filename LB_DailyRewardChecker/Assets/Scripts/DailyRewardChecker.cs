using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DailyRewardChecker {

    private bool isFirstLogin;
    private NtpServerConnection ntpServerConnection;
    private DateTime lastDailyRewardTime;
    private DateTime currentTime;
    private JsonSerializerSettings jsonSerializerSettings;
    private string filePath;
    private bool dailyRewardCanBeEarned;

    public DailyRewardChecker()
    {
        jsonSerializerSettings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-ddTH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };

        filePath = Application.persistentDataPath + "/DailyRewardDate.txt";
        Debug.Log(filePath);
        ntpServerConnection = new NtpServerConnection();

        if (ntpServerConnection.GetIsConnected())
        {
            GetLastRewardTime();
            currentTime = ntpServerConnection.GetTime();
            CheckRewardEarnAble();
        }
        else
        {
            dailyRewardCanBeEarned = false;
        }

    }

    public double GetRemainingTimeToEarnDailyReward()
    {
        var timeDiference = currentTime - lastDailyRewardTime;
        return timeDiference.TotalSeconds;
    }

    private void GetLastRewardTime()
    {
        if (File.Exists(filePath))
        {
            lastDailyRewardTime = JsonConvert.DeserializeObject<System.DateTime>(File.ReadAllText(filePath), jsonSerializerSettings);
        }
        else
        {
            if (ntpServerConnection.GetIsConnected())
            {
                lastDailyRewardTime = ntpServerConnection.GetTime();
                SaveLastDailyRewardTime();
                isFirstLogin = true;
            }
        }

    }

    private void SaveLastDailyRewardTime()
    {
        var serializedDateTime = JsonConvert.SerializeObject(lastDailyRewardTime, jsonSerializerSettings);
        File.WriteAllText(filePath, serializedDateTime);
    }

    private void CheckRewardEarnAble()
    {
        var timeDiference = currentTime.Subtract(lastDailyRewardTime);

        if (timeDiference.Days >= 1)
        {
            lastDailyRewardTime = currentTime;
            SaveLastDailyRewardTime();

            dailyRewardCanBeEarned = true;
        }
        else
        {
            dailyRewardCanBeEarned = false;
            EventManager.Instance.SetTimeToEarnDailyReward(timeDiference.TotalSeconds);
        }
    }

    public bool GetDailyRewardCanBeEarned()
    {
        return dailyRewardCanBeEarned || isFirstLogin;
    }
}
