using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defines
{
    public const int CollectStartFloorID = 2;

    public const int BaseExperience = 100;
    public const float Exponent = 1.25f;

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
}
