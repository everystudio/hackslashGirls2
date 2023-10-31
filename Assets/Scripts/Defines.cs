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

    public static int CalculateRequiredLevelupCoin(int level)
    {
        float level_rate = (float)level / 50;
        //level_rate = 0f;
        return (int)(BaseExperience * Math.Pow(level, Exponent + level_rate));
    }

    public static string GetNumericString(int num)
    {
        if (num < 1000)
        {
            return num.ToString();
        }
        else if (num < 1000000)
        {
            return string.Format("{0}K", num / 1000);
        }
        else if (num < 1000000000)
        {
            return string.Format("{0}M", num / 1000000);
        }
        else
        {
            return string.Format("{0}B", num / 1000000000);
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
}
