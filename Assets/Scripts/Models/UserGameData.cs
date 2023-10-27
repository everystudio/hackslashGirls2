using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class UserGameData : CsvModelParam
{
    public int max_floor_id;
    public int coin;
    public int gem;
    public int ticket;

    public int last_quest_floor_id;
    public int last_collect_floor_id;
}
