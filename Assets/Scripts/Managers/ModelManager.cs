using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;
using System;

public class ModelManager : Singleton<ModelManager>
{
    public TextAsset dummyUserChara;
    public CsvModel<UserChara> userChara = new CsvModel<UserChara>();
    public CsvModel<UserChara> UserChara { get { return userChara; } }

    public TextAsset masterItemAsset;
    private CsvModel<MasterItem> masterItem = new CsvModel<MasterItem>();
    public CsvModel<MasterItem> MasterItem { get { return masterItem; } }

    public TextAsset dummyUserItem;
    private CsvModel<UserItem> userItem = new CsvModel<UserItem>();
    public CsvModel<UserItem> UserItem { get { return userItem; } }

    public TextAsset masterCharaAsset;
    private CsvModel<MasterChara> masterChara = new CsvModel<MasterChara>();
    public CsvModel<MasterChara> MasterChara { get { return masterChara; } }

    public TextAsset masterEquipAsset;
    private CsvModel<MasterEquip> masterEquip = new CsvModel<MasterEquip>();
    public CsvModel<MasterEquip> MasterEquip { get { return masterEquip; } }

    public TextAsset masterEnemyAsset;
    private CsvModel<MasterEnemy> masterEnemy = new CsvModel<MasterEnemy>();
    public CsvModel<MasterEnemy> MasterEnemy { get { return masterEnemy; } }

    public TextAsset masterAreaAsset;
    private CsvModel<MasterArea> masterArea = new CsvModel<MasterArea>();
    public CsvModel<MasterArea> MasterArea { get { return masterArea; } }

    public TextAsset masterFloorAsset;
    private CsvModel<MasterFloor> masterFloor = new CsvModel<MasterFloor>();
    public CsvModel<MasterFloor> MasterFloor { get { return masterFloor; } }

    public UnityEvent<UserChara> OnUserCharaChanged = new UnityEvent<UserChara>();
    public UnityEvent<UserChara> OnPartyCharaChanged = new UnityEvent<UserChara>();

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData { get { return userGameData; } }

    public override void Initialize()
    {
        Debug.Log("ModelManager Initialize");
        userChara.Load(dummyUserChara);
        masterItem.Load(masterItemAsset);

        masterChara.Load(masterCharaAsset);
        masterEquip.Load(masterEquipAsset);
        masterEnemy.Load(masterEnemyAsset);
        masterArea.Load(masterAreaAsset);
        masterFloor.Load(masterFloorAsset);

        userItem.Load(dummyUserItem);

        // マスターデータからユーザーデータを作成する
        foreach (var masterItem in masterItem.List)
        {
            // ユーザーデータに含まれていない場合
            if (userItem.List.Find(item => item.item_id == masterItem.item_id) != null)
            {
                //Debug.Log("持ってる" + masterItem.item_id);
                continue;
            }

            UserItem item = new UserItem();
            item.item_id = masterItem.item_id;
            item.item_num = 0;
            this.userItem.List.Add(item);
        }


        /*
        // マスターキャラデータを確認するための表示
        foreach (var item in masterChara.List)
        {
            Debug.Log(item.chara_id + " " + item.chara_name + " " + item.description + " " + item.attack + " " + item.attack_range + " " + item.attack_speed);
        }
        */

    }

    public MasterChara GetMasterChara(int charaId)
    {
        return masterChara.List.Find(chara => chara.chara_id == charaId);
    }
    public UserChara GetUserChara(int charaId)
    {
        return userChara.List.Find(chara => chara.chara_id == charaId);
    }

    public MasterItem GetMasterItem(int itemId)
    {
        return masterItem.List.Find(item => item.item_id == itemId);
    }
    public UserItem GetUserItem(int itemId)
    {
        return userItem.List.Find(item => item.item_id == itemId);
    }

    public bool EquipItem(int charaId, int itemId)
    {
        UserChara userChara = GetUserChara(charaId);
        if (userChara == null)
        {
            return false;
        }

        UserItem userItem = GetUserItem(itemId);
        if (userItem == null)
        {
            return false;
        }

        MasterItem masterItem = GetMasterItem(itemId);
        if (masterItem == null)
        {
            return false;
        }

        // 既に装備している場合は何もしない
        if (userChara.IsEquiping(itemId))
        {
            return false;
        }

        // 現在のランクに対応しているアイテムかどうかを確認する
        MasterEquip masterEquip = this.masterEquip.List.Find(equip => equip.chara_id == charaId && equip.item_id == itemId && equip.rank == userChara.rank);
        if (masterEquip == null)
        {
            return false;
        }

        // 個数が十分か確認する
        if (userItem.item_num < masterEquip.require_count)
        {
            return false;
        }

        // ここまで来たら装備を行うtrycatchとか使った方がいいのかね
        try
        {
            userItem.item_num -= masterEquip.require_count;
            if (userItem.item_num < 0)
            {
                throw new System.Exception("装備に失敗しました:アイテム数が足りません");
            }
            if (!userChara.AddEquip(masterItem))
            {
                throw new System.Exception("装備に失敗しました:空きスロットなし");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return false;
        }
        OnUserCharaChanged.Invoke(userChara);
        if (userChara.questPartyId != 0)
        {
            OnPartyCharaChanged.Invoke(userChara);
        }
        return true;
    }

    public MasterEnemy GetMasterEnemy(int enemyId)
    {
        return masterEnemy.List.Find(enemy => enemy.enemy_id == enemyId);
    }

    public UserChara GetQuestPartyChara(int partyId)
    {
        foreach (var userChara in userChara.List)
        {
            //Debug.Log(userChara.chara_id + " " + userChara.questPartyId);
            if (userChara.questPartyId == partyId)
            {
                return userChara;
            }
        }
        return userChara.List.Find(chara => chara.questPartyId == partyId);
    }

    public void SaveUserGameData()
    {
        // メソッドだけ用意

    }

    public UserChara AddChara(int chara_id)
    {
        MasterChara masterChara = GetMasterChara(chara_id);
        if (masterChara == null)
        {
            return null;
        }

        UserChara addChara = new UserChara(masterChara);
        userChara.List.Add(addChara);
        return addChara;
    }
}
