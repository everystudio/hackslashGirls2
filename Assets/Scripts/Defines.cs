using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defines
{
    public const int CollectStartFloorID = 2;

    public const int BaseExperience = 100;
    public const float Exponent = 1.25f;

    // １フロアでの最大敵数
    public const int MaxEnemyNum = 5;

    public const int CoinItemID = 10001;
    public const int GemItemID = 10002;
    public const int TicketItemID = 10003;

    public const string LastPlayTimeKey = "LastPlayTime";
    public const string GemTimeKey = "GemTimeKey";
    public const string TicketTimeKey = "TicketTimeKey";

#if UNITY_EDITOR || UNITY_IOS
    public const string AD_APP_ID = "ca-app-pub-5869235725006697~5509514419";
    public const string AD_BOTTOM_BANNER_ID = "ca-app-pub-5869235725006697/8581318749";
    public const string AD_INITIAL_ID = "ca-app-pub-5869235725006697/7731271504";
    public const string AD_BATTLE_END_INTERSTITIAL_ID = "ca-app-pub-5869235725006697/8633289637";
    public const string AD_REWARD_ID = "ca-app-pub-5869235725006697/9560267653";
#elif UNITY_ANDROID
    public const string AD_APP_ID = "ca-app-pub-5869235725006697~8776064366";
    public const string AD_INITIAL_ID = "ca-app-pub-5869235725006697/4242471728";
    public const string AD_BOTTOM_BANNER_ID = "ca-app-pub-5869235725006697/6868635069";
    public const string AD_BATTLE_END_INTERSTITIAL_ID = "ca-app-pub-5869235725006697/6760823854";
    public const string AD_REWARD_ID = "ca-app-pub-5869235725006697/8122006985";

#endif    

    public static int CalculateRequiredLevelupCoin(int level)
    {
        float level_rate = (float)level / 50;
        //level_rate = 0f;
        return (int)(BaseExperience * Math.Pow(level, Exponent + level_rate));
    }

    public static string GetNumericString(int num)
    {
        if (num < 1000000)
        {
            return num.ToString();
        }
        else if (num < 1000000000)
        {
            return string.Format("{0}K", num / 1000);
        }

        else
        {
            return string.Format("{0}M", num / 1000000);
        }
    }

    public static int GetRankUpGem(int nextRank)
    {
        switch (nextRank)
        {
            case 2:
                return 5;
            case 3:
                return 25;
            case 4:
                return 100;
            case 5:
                return 500;
            default:
                return 0;
        }
    }

    public static int GetStarUp(int nextStar)
    {
        switch (nextStar)
        {
            case 2:
                return 5;
            case 3:
                return 25;
            case 4:
                return 100;
            case 5:
                return 500;
            default:
                return 0;
        }
    }

    public static float GetStarRate(int star)
    {
        switch (star)
        {
            case 1:
                return 1.0f;
            case 2:
                return 1.2f;
            case 3:
                return 1.5f;
            case 4:
                return 1.75f;
            case 5:
                return 2.0f;
            default:
                return 1.0f;
        }
    }


    public static int GetCurrentGameSpeed(int currentSpeedIndex)
    {
        int[] spped_array = new int[] { 1, 2, 5, 10 };
        return spped_array[currentSpeedIndex];
    }


    public static (int, int) GetNextGameSpeed(int currentSpeedIndex)
    {
        int[] spped_array = new int[] { 1, 2, 5, 10 };
        int nextSpeedIndex = currentSpeedIndex + 1;
        if (spped_array.Length <= nextSpeedIndex)
        {
            nextSpeedIndex = 0;
        }
        return (spped_array[nextSpeedIndex], nextSpeedIndex);

    }


    public static int HOUCHI_EXP_PER_MINUTE = 50;
    public static int HOUCHI_COIN_PER_MINUTE = 100;

    public static int HOUCHI_GEM_PER_HOUR = 20;
    public static int HOUCHI_TICKET_PER_DAY = 30;

    public static readonly int DEFENCE_DIVIDE = 50;
    public static readonly int CRITICAL_DIVIDE = 50;
    public static readonly int DODGE_DIVIDE = 50;
    public static readonly float HEART_DIVIDE = 1500;
    public static readonly float HEART_HEAL_RATE = 0.1f;

}
