using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class UserArea : CsvModelParam
{
    public int area_id;

    public int floor_max;
    public int floor_count;

    public int enemy_max;
    public int enemy_count;

    public int item_max;
    public int item_count;

    public int PercentFloor()
    {
        return (int)((float)(floor_count * 100) / floor_max);
    }
    public int PercentEnemy()
    {
        /*
        Debug.Log("area_id:" + area_id);
        Debug.Log("enemy_count:" + enemy_count);
        Debug.Log("enemy_max:" + enemy_max);
        Debug.Log((int)((float)(enemy_count * 100) / enemy_max));
        */
        return (int)((float)(enemy_count * 100) / enemy_max);
    }
    public int PercentItem()
    {
        return (int)((float)(item_count * 100) / item_max);
    }

    public int TotalPercent()
    {
        return (PercentFloor() + PercentEnemy() + PercentItem()) / 3;
    }

}
