using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.Events;

public class UserChara : CsvModelParam
{
    public UserChara() { }
    public UserChara(MasterChara masterChara)
    {
        chara_id = masterChara.chara_id;
        level = 0;

        rank = 0;
        star = Mathf.Max(1, masterChara.initial_star);
        /*
        chara_hp_max = 10;
        chara_strength = 1;
        chara_defense = 1;
        chara_speed = 1;
        chara_luck = 1;
        chara_spirit = 1;
        chara_heart = 1;
        */
        Levelup();
    }
    public int chara_id;

    public int questPartyId;
    public int collectPartyId;

    public int level;
    public int rank;
    public int star;
    public int exp;

    public int hp;

    public int hp_max { get { return (int)(chara_hp_max * Defines.GetStarRate(star)) + assist_hp_max; } }
    public int strength { get { return (int)(chara_strength * Defines.GetStarRate(star)) + assist_strength; } }
    public int defense { get { return (int)(chara_defense * Defines.GetStarRate(star)) + assist_defense; } }
    public int speed { get { return (int)(chara_speed * Defines.GetStarRate(star)) + assist_speed; } }
    public int luck { get { return (int)(chara_luck * Defines.GetStarRate(star)) + assist_luck; } }
    public int spirit { get { return (int)(chara_spirit * Defines.GetStarRate(star)) + assist_spirit; } }
    public int heart { get { return (int)(chara_heart * Defines.GetStarRate(star)) + assist_heart; } }

    public int chara_hp_max;
    public int chara_strength;
    public int chara_defense;
    public int chara_speed;
    public int chara_luck;
    public int chara_spirit;
    public int chara_heart;

    public int assist_hp_max;
    public int assist_strength;
    public int assist_defense;
    public int assist_speed;
    public int assist_luck;
    public int assist_spirit;
    public int assist_heart;

    public int next_exp;

    // 装備中のitem_idの変数
    public int equiping_item_id_1;
    public int equiping_item_id_2;
    public int equiping_item_id_3;

    public static UnityEvent<UserChara> OnAnyChanged = new UnityEvent<UserChara>();

    public bool IsEquiping(int item_id)
    {
        return equiping_item_id_1 == item_id || equiping_item_id_2 == item_id || equiping_item_id_3 == item_id;
    }
    public bool AddEquip(MasterItem masterItem)
    {
        if (equiping_item_id_1 == 0)
        {
            equiping_item_id_1 = masterItem.item_id;
        }
        else if (equiping_item_id_2 == 0)
        {
            equiping_item_id_2 = masterItem.item_id;
        }
        else if (equiping_item_id_3 == 0)
        {
            equiping_item_id_3 = masterItem.item_id;
        }
        else
        {
            return false;
        }

        hp += masterItem.hp_max;
        assist_hp_max += masterItem.hp_max;

        assist_strength += masterItem.strength;
        assist_defense += masterItem.defense;
        assist_speed += masterItem.speed;
        assist_luck += masterItem.luck;
        assist_spirit += masterItem.spirit;

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

    public bool Levelup()
    {
        if (level >= 99)
        {
            return false;
        }

        level += 1;
        exp = 0;

        MasterChara masterChara = ModelManager.Instance.GetMasterChara(chara_id);

        chara_hp_max = (int)Mathf.Lerp(1, masterChara.hp, level / 99f);
        chara_strength = (int)Mathf.Lerp(1, masterChara.strength, level / 99f);
        chara_defense = (int)Mathf.Lerp(1, masterChara.defense, level / 99f);
        chara_speed = (int)Mathf.Lerp(1, masterChara.speed, level / 99f);
        chara_luck = (int)Mathf.Lerp(1, masterChara.luck, level / 99f);
        chara_spirit = (int)Mathf.Lerp(1, masterChara.spirit, level / 99f);
        chara_heart = (int)Mathf.Lerp(1, masterChara.heart, level / 99f);

        hp = hp_max;
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
