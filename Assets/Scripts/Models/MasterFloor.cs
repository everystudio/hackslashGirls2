using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class MasterFloor : CsvModelParam
{
    public int floor_id;
    public int floor_start;
    public int floor_end;
    public string background;
    public int continue_num;    // 使わない
    public int enemy_id_1;
    public int enemy_id_2;
    public int enemy_id_3;

    public int GetRandomEnemyID()
    {
        int[] enemyIDs = new int[] { enemy_id_1, enemy_id_2, enemy_id_3 };
        return enemyIDs[Random.Range(0, enemyIDs.Length)];
    }

}
