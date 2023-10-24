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

    [SerializeField] private TextAsset masterCharaAsset;
    private CsvModel<MasterChara> masterChara = new CsvModel<MasterChara>();
    public CsvModel<MasterChara> MasterChara { get { return masterChara; } }

    [SerializeField] private TextAsset masterEquipAsset;
    private CsvModel<MasterEquip> masterEquip = new CsvModel<MasterEquip>();
    public CsvModel<MasterEquip> MasterEquip { get { return masterEquip; } }

    [SerializeField] private TextAsset masterEnemyAsset;
    private CsvModel<MasterEnemy> masterEnemy = new CsvModel<MasterEnemy>();
    public CsvModel<MasterEnemy> MasterEnemy { get { return masterEnemy; } }

    [SerializeField] private TextAsset masterAreaAsset;
    private CsvModel<MasterArea> masterArea = new CsvModel<MasterArea>();
    public CsvModel<MasterArea> MasterArea { get { return masterArea; } }

    [SerializeField] private TextAsset masterFloorAsset;
    private CsvModel<MasterFloor> masterFloor = new CsvModel<MasterFloor>();
    public CsvModel<MasterFloor> MasterFloor { get { return masterFloor; } }

    [SerializeField] private TextAsset masterAchievementAsset;
    private CsvModel<MasterAchievement> masterAchievement = new CsvModel<MasterAchievement>();
    public CsvModel<MasterAchievement> MasterAchievement { get { return masterAchievement; } }

    private CsvModel<UserAchievement> userAchievement = new CsvModel<UserAchievement>();
    public CsvModel<UserAchievement> UserAchievement { get { return userAchievement; } }

    public UnityEvent<UserChara> OnUserCharaChanged = new UnityEvent<UserChara>();
    public UnityEvent<UserChara> OnPartyCharaChanged = new UnityEvent<UserChara>();

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData { get { return userGameData; } }


    public UnityEvent<UserGameData> OnChangeUserGameData = new UnityEvent<UserGameData>();

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

        masterAchievement.Load(masterAchievementAsset);
        foreach (MasterAchievement achievement in masterAchievement.List)
        {
            if (achievement.open_id == 0)
            {
                var userAchievement = GetUserAchievement(achievement.achievement_id);
                if (userAchievement == null)
                {
                    userAchievement = new UserAchievement();
                    userAchievement.achievement_id = achievement.achievement_id;
                    userAchievement.is_completed = false;
                    userAchievement.is_received = false;
                    this.userAchievement.List.Add(userAchievement);
                }
            }
        }

        // クエスト(Achievement)達成確認用
        MissionBanner.OnRecieve.AddListener(OnAchievementRecieve);
        FloorManager.OnUpdateMaxFloor.AddListener(OnUpdateMaxFloor);




        if (dummyUserItem != null)
        {
            userItem.Load(dummyUserItem);
        }

        // userItemをitem_idでソートする
        userItem.List.Sort((a, b) => a.item_id - b.item_id);

        userGameData.coin = 0;
        userGameData.last_quest_floor_id = 1;
        userGameData.last_collect_floor_id = 1;

        /*
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
        */


        /*
        // マスターキャラデータを確認するための表示
        foreach (var item in masterChara.List)
        {
            Debug.Log(item.chara_id + " " + item.chara_name + " " + item.description + " " + item.attack + " " + item.attack_range + " " + item.attack_speed);
        }
        */

        CollectableItem.OnCollect.AddListener(CollectItemOne);
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

    public void AddCoin(int coin)
    {
        userGameData.coin += coin;
        OnChangeUserGameData.Invoke(userGameData);
    }
    public bool UseCoin(int coin)
    {
        if (userGameData.coin < coin)
        {
            return false;
        }
        userGameData.coin -= coin;
        OnChangeUserGameData.Invoke(userGameData);
        return true;
    }

    public MasterArea GetMasterArea(int areaId)
    {
        return masterArea.List.Find(area => area.area_id == areaId);
    }

    private void CollectItemOne(int item_id)
    {
        CollectItem(item_id, 1);
    }

    private void CollectItem(int item_id, int amount = 1)
    {
        UserItem userItem = GetUserItem(item_id);
        if (userItem == null)
        {
            userItem = new UserItem();
            userItem.item_id = item_id;
            userItem.item_num = 0;
            this.userItem.List.Add(userItem);

            // userItemをitem_idでソートする
            this.userItem.List.Sort((a, b) => a.item_id - b.item_id);
        }
        userItem.item_num += amount;
    }

    public UserAchievement GetUserAchievement(int achievementId)
    {
        return userAchievement.List.Find(achievement => achievement.achievement_id == achievementId);
    }
    public MasterAchievement GetMasterAchievement(int achievementId)
    {
        return masterAchievement.List.Find(achievement => achievement.achievement_id == achievementId);
    }

    private void OnAchievementRecieve(MasterAchievement master, UserAchievement user)
    {
        user.is_received = true;
        switch (master.prize_type)
        {
            case 1:
                AddCoin(master.prize_amount);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                CollectItem(master.prize_id, master.prize_amount);
                break;
        }
    }

    private void UnlockAchievement(int achievementId)
    {
        UserAchievement userAchievement = GetUserAchievement(achievementId);
        if (userAchievement == null)
        {
            return;
        }
        userAchievement.is_completed = true;

        MasterAchievement masterAchievement = this.masterAchievement.List.Find(achievement => achievement.open_id == achievementId);
        if (masterAchievement != null)
        {
            UserAchievement nextUserAchievement = new UserAchievement();
            nextUserAchievement.achievement_id = masterAchievement.achievement_id;
            nextUserAchievement.is_completed = false;
            nextUserAchievement.is_received = false;
            this.userAchievement.List.Add(nextUserAchievement);

            // id順に並べ直す
            this.userAchievement.List.Sort((a, b) => a.achievement_id - b.achievement_id);
        }
    }

    private void OnUpdateMaxFloor(int newFloorID)
    {
        List<UserAchievement> list = userAchievement.List.FindAll(achievement => achievement.is_completed == false);

        foreach (UserAchievement userAchievement in list)
        {
            MasterAchievement masterAchievement = GetMasterAchievement(userAchievement.achievement_id);
            if (masterAchievement == null)
            {
                continue;
            }

            if (masterAchievement.type == (int)AchievementType.FLOOR)
            {
                if (masterAchievement.param1 <= newFloorID)
                {
                    UnlockAchievement(userAchievement.achievement_id);
                }
            }

        }

    }


}
