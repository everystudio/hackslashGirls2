using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class UserChara : CsvModelParam
{
    public int chara_id;

    public int questPartyId;
    public int collectPartyId;

    public int level;
    public int rank;

    public int hp;

    public int hp_max { get { return chara_hp_max + assist_hp_max; } }
    public int strength { get { return chara_strength + assist_strength; } }
    public int defense { get { return chara_defense + assist_defense; } }
    public int speed { get { return chara_speed + assist_speed; } }
    public int luck { get { return chara_luck + assist_luck; } }
    public int spirit { get { return chara_spirit + assist_spirit; } }

    public int chara_hp_max;
    public int chara_strength;
    public int chara_defense;
    public int chara_speed;
    public int chara_luck;
    public int chara_spirit;

    public int assist_hp_max;
    public int assist_strength;
    public int assist_defense;
    public int assist_speed;
    public int assist_luck;
    public int assist_spirit;

    public int next_exp;

    // 装備中のitem_idの変数
    public int equiping_item_id_1;
    public int equiping_item_id_2;
    public int equiping_item_id_3;

    public bool IsEquiping(int item_id)
    {
        return equiping_item_id_1 == item_id || equiping_item_id_2 == item_id || equiping_item_id_3 == item_id;
    }
    public bool AddEquip(int item_id)
    {
        if (equiping_item_id_1 == 0)
        {
            equiping_item_id_1 = item_id;
        }
        else if (equiping_item_id_2 == 0)
        {
            equiping_item_id_2 = item_id;
        }
        else if (equiping_item_id_3 == 0)
        {
            equiping_item_id_3 = item_id;
        }
        else
        {
            return false;
        }

        return true;
    }

    public bool Rankup()
    {
        // equiping_item_idが全て埋まっているか確認する
        if (equiping_item_id_1 == 0 || equiping_item_id_2 == 0 || equiping_item_id_3 == 0)
        {
            return false;
        }
        rank += 1;
        equiping_item_id_1 = 0;
        equiping_item_id_2 = 0;
        equiping_item_id_3 = 0;
        return true;
    }



    public static readonly int MAX_STATUS_PARAM = 100;


    public int partyIndex
    {
        get
        {
            return questPartyId > 0 ? questPartyId : collectPartyId;
        }
    }
}
