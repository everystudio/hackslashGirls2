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
    public int restart_quest_floor_id;
    public int last_collect_area_id;

    public int game_speed_index;

    public float master_volume;
    public float bgm_volume;
    public float sfx_volume;


}
