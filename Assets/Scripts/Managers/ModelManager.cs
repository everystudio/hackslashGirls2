using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using anogame;
using System;

[System.Serializable]
public class SaveData
{
    public UserGameData savedUserGameData;

    public List<UserChara> savedUserChara;
    public List<UserItem> savedUserItem;
    public List<UserEnemy> savedUserEnemy;
    public List<UserAchievement> savedUserAchievement;
    public List<UserArea> savedUserArea;
    public List<UserStar> savedUserStar;
}

public class ModelManager : Singleton<ModelManager>
{
    private bool isInitializedLocal = false;
    public TextAsset dummyUserChara;
    public CsvModel<UserChara> userChara = new CsvModel<UserChara>();
    public CsvModel<UserChara> UserChara { get { return userChara; } }

    public TextAsset masterItemAsset;
    private CsvModel<MasterItem> masterItem = new CsvModel<MasterItem>();
    public CsvModel<MasterItem> MasterItem { get { return masterItem; } }

    public TextAsset dummyUserItem;
    private CsvModel<UserItem> userItem = new CsvModel<UserItem>();
    public CsvModel<UserItem> UserItem { get { return userItem; } }
    public UnityEvent OnUserItemChanged = new UnityEvent();

    [SerializeField] private TextAsset masterCharaAsset;
    private CsvModel<MasterChara> masterChara = new CsvModel<MasterChara>();
    public CsvModel<MasterChara> MasterChara { get { return masterChara; } }

    [SerializeField] private TextAsset masterEquipAsset;
    private CsvModel<MasterEquip> masterEquip = new CsvModel<MasterEquip>();
    public CsvModel<MasterEquip> MasterEquip { get { return masterEquip; } }

    [SerializeField] private TextAsset masterEnemyAsset;
    private CsvModel<MasterEnemy> masterEnemy = new CsvModel<MasterEnemy>();
    public CsvModel<MasterEnemy> MasterEnemy { get { return masterEnemy; } }

    private CsvModel<UserEnemy> userEnemy = new CsvModel<UserEnemy>();
    public CsvModel<UserEnemy> UserEnemy { get { return userEnemy; } }

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

    [SerializeField] private TextAsset masterGachaAsset;
    private CsvModel<MasterGacha> masterGacha = new CsvModel<MasterGacha>();
    public CsvModel<MasterGacha> MasterGacha { get { return masterGacha; } }

    public UnityEvent<UserChara> OnUserCharaChanged = new UnityEvent<UserChara>();
    public UnityEvent<UserChara> OnPartyCharaChanged = new UnityEvent<UserChara>();

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData { get { return userGameData; } }

    public UnityEvent<MasterAchievement> OnUnlockAchievement = new UnityEvent<MasterAchievement>();

    public UnityEvent<UserGameData> OnChangeUserGameData = new UnityEvent<UserGameData>();

    private CsvModel<UserArea> userArea = new CsvModel<UserArea>();
    public CsvModel<UserArea> UserArea { get { return userArea; } }

    private CsvModel<UserStar> userStar = new CsvModel<UserStar>();
    public CsvModel<UserStar> UserStar { get { return userStar; } }

    [SerializeField] private TextAsset masterHelpAsset;
    private CsvModel<MasterHelp> masterHelp = new CsvModel<MasterHelp>();
    public CsvModel<MasterHelp> MasterHelp { get { return masterHelp; } }


    private SaveData saveData = new SaveData();

    private void Save()
    {
        if (!isInitializedLocal)
        {
            return;
        }


        saveData.savedUserGameData = userGameData;

        saveData.savedUserChara = userChara.All;
        saveData.savedUserItem = userItem.All;
        saveData.savedUserEnemy = userEnemy.All;
        saveData.savedUserAchievement = userAchievement.All;
        saveData.savedUserArea = userArea.All;
        saveData.savedUserStar = userStar.All;

        ES3.Save<SaveData>("saveData", saveData);

    }
    private void Load()
    {
        if (!ES3.KeyExists("saveData"))
        {
            return;
        }
        saveData = ES3.Load<SaveData>("saveData");

        userGameData = saveData.savedUserGameData;
        //Debug.Log(userGameData.last_quest_floor_id);
        userGameData.last_quest_floor_id = Mathf.Clamp(userGameData.last_quest_floor_id, 1, 500);

        //Debug.Log(userGameData.max_floor_id);
        userGameData.restart_quest_floor_id = Mathf.Max(1, userGameData.restart_quest_floor_id); ;

        userChara.List.Clear();
        foreach (var userChara in saveData.savedUserChara)
        {
            this.userChara.List.Add(userChara);
        }

        userItem.List.Clear();
        foreach (var userItem in saveData.savedUserItem)
        {
            this.userItem.List.Add(userItem);
        }

        userEnemy.List.Clear();
        foreach (var userEnemy in saveData.savedUserEnemy)
        {
            this.userEnemy.List.Add(userEnemy);
        }

        userAchievement.List.Clear();
        foreach (var userAchievement in saveData.savedUserAchievement)
        {
            this.userAchievement.List.Add(userAchievement);
        }

        userArea.List.Clear();
        foreach (var userArea in saveData.savedUserArea)
        {
            this.userArea.List.Add(userArea);
        }
        userStar.List.Clear();
        foreach (var userStar in saveData.savedUserStar)
        {
            this.userStar.List.Add(userStar);
        }

        // デバッグ用の対応
        List<UserChara> collectPartyMemberList = userChara.List.FindAll(chara => 0 < chara.collectPartyId);
        if (collectPartyMemberList.Count < 4)
        {
            for (int i = 0; i < collectPartyMemberList.Count; i++)
            {
                collectPartyMemberList[i].collectPartyId = i + 1;
            }
            int nokori = 4 - collectPartyMemberList.Count;


            foreach (var chara in userChara.List)
            {
                if (chara.partyIndex == 0)
                {
                    chara.collectPartyId = 4 - nokori;
                    nokori--;
                    if (nokori <= 0)
                    {
                        break;
                    }
                }


            }
        }



    }

    public override void Initialize()
    {
        //Debug.Log("ModelManager Initialize");


        masterItem.Load(masterItemAsset);

        masterChara.Load(masterCharaAsset);
        masterEquip.Load(masterEquipAsset);
        masterEnemy.Load(masterEnemyAsset);
        masterArea.Load(masterAreaAsset);
        masterFloor.Load(masterFloorAsset);

        masterAchievement.Load(masterAchievementAsset);
        masterGacha.Load(masterGachaAsset);
        masterHelp.Load(masterHelpAsset);

        var initialChara = new CsvModel<UserChara>();
        initialChara.Load(dummyUserChara);
        foreach (var userChara in initialChara.List)
        {
            var chara = AddChara(userChara.chara_id, out bool isNew);
            //Debug.Log(userChara.chara_id);
            //Debug.Log(chara);
            chara.questPartyId = userChara.questPartyId;
        }


        // データを復元する
        Load();

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
        EnemyBase.OnAnyDie.AddListener(OnEnemyDie);


        if (dummyUserItem != null)
        {
            userItem.Load(dummyUserItem);
        }

        // userItemをitem_idでソートする
        userItem.List.Sort((a, b) => a.item_id - b.item_id);

        if (userGameData.max_floor_id == 0)
        {
            userGameData.coin = 0;
            userGameData.gem = 100;
            userGameData.ticket = 30;
            userGameData.last_quest_floor_id = 1;
            userGameData.restart_quest_floor_id = 1;
            userGameData.last_collect_area_id = 1;
            userGameData.game_speed_index = 0;
            userGameData.master_volume = 1f;
            userGameData.bgm_volume = 1f;
            userGameData.sfx_volume = 1f;
        }

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

        // 各エリアの達成率を更新する
        foreach (MasterArea masterArea in masterArea.List)
        {
            UpdateAchievementRate(masterArea.area_id);
        }

        CollectableItem.OnCollect.AddListener(CollectItem);

        isInitializedLocal = true;
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

    public UserChara AddChara(int chara_id, out bool isNew)
    {
        // すでに持っているキャラかどうかを確認する
        UserChara checkChara = GetUserChara(chara_id);
        if (checkChara != null)
        {
            Debug.Log("checkChara !=null");
            isNew = false;
            return checkChara;
        }

        // マスターデータからキャラを取得する
        MasterChara masterChara = GetMasterChara(chara_id);
        if (masterChara == null)
        {
            Debug.Log("masterChara !=null");
            isNew = false;
            return null;
        }

        UserChara addChara = new UserChara(masterChara);
        userChara.List.Add(addChara);

        // userCharaをchara_idでソートする
        userChara.List.Sort((a, b) => a.chara_id - b.chara_id);

        isNew = true;
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

    public void AddGem(int gem)
    {
        userGameData.gem += gem;
        OnChangeUserGameData.Invoke(userGameData);
    }
    public bool UseGem(int gem)
    {
        if (userGameData.gem < gem)
        {
            return false;
        }
        userGameData.gem -= gem;
        OnChangeUserGameData.Invoke(userGameData);
        return true;
    }

    public void AddTicket(int ticket)
    {
        userGameData.ticket += ticket;
        OnChangeUserGameData.Invoke(userGameData);
    }
    public bool UseTicket(int ticket)
    {
        if (userGameData.ticket < ticket)
        {
            return false;
        }
        userGameData.ticket -= ticket;
        OnChangeUserGameData.Invoke(userGameData);
        return true;
    }

    public void SetGameSpeedIndex(int gameSpeedIndex)
    {
        userGameData.game_speed_index = gameSpeedIndex;
        OnChangeUserGameData.Invoke(userGameData);
    }

    public MasterArea GetMasterArea(int areaId)
    {
        return masterArea.List.Find(area => area.area_id == areaId);
    }

    public void AddResourcesItem(int item_id, int amount)
    {
        switch (item_id)
        {
            case Defines.CoinItemID:
                AudioManager.Instance.PlayCoinGetSFX();
                AddCoin(amount);
                break;
            case Defines.GemItemID:
                AddGem(amount);
                break;
            case Defines.TicketItemID:
                AddTicket(amount);
                break;
            default:
                break;
        }
    }

    public bool IsResourcesItem(int item_id)
    {
        // 特に識別できるものがなかったので、area_id == 0のものをリソースアイテムとする
        MasterItem masterItem = GetMasterItem(item_id);
        if (masterItem == null)
        {
            return false;
        }
        return masterItem.area_id == 0;
    }

    public void CollectItem(int item_id, int amount = 1)
    {
        if (IsResourcesItem(item_id))
        {
            AddResourcesItem(item_id, amount);
            return;
        }

        UserItem userItem = GetUserItem(item_id);
        if (userItem == null)
        {
            MasterItem masterItem = GetMasterItem(item_id);

            userItem = new UserItem();
            userItem.item_id = item_id;
            userItem.item_num = 0;
            userItem.area_id = masterItem.area_id;
            this.userItem.List.Add(userItem);

            // userItemをitem_idでソートする
            this.userItem.List.Sort((a, b) => a.item_id - b.item_id);

            MasterAchievement masterAchievement = this.masterAchievement.List.Find(achievement => achievement.type == (int)AchievementType.COLLECT && achievement.param1 == item_id);

            //Debug.Log("アイテム初取得");
            if (masterAchievement != null)
            {
                //Debug.Log("実績初解除");
                UnlockAchievement(masterAchievement.achievement_id);
            }


        }
        AudioManager.Instance.PlayItemGetSFX();
        userItem.item_num += amount;

        OnUserItemChanged.Invoke();
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
            case (int)AchievementPrizeType.COIN:
                AddCoin(master.prize_amount);
                break;
            case (int)AchievementPrizeType.GEM:
                AddGem(master.prize_amount);
                break;
            case (int)AchievementPrizeType.TICKET:
                AddTicket(master.prize_amount);
                break;
            case (int)AchievementPrizeType.ITEM:
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
            userAchievement = new UserAchievement();
            userAchievement.achievement_id = achievementId;
            userAchievement.is_completed = false;
            userAchievement.is_received = false;
            this.userAchievement.List.Add(userAchievement);
            this.userAchievement.List.Sort((a, b) => a.achievement_id - b.achievement_id);
        }
        else if (userAchievement.is_completed == true)
        {
            return;
        }
        userAchievement.is_completed = true;

        MasterAchievement unlockAchievement = this.masterAchievement.List.Find(achievement => achievement.achievement_id == achievementId);
        OnUnlockAchievement.Invoke(unlockAchievement);

        MasterAchievement masterAchievement = this.masterAchievement.List.Find(achievement => achievement.open_id == achievementId);
        if (masterAchievement != null)
        {
            Debug.Log(masterAchievement.achievement_id + " " + masterAchievement.title + " " + masterAchievement.open_id);
            UserAchievement nextUserAchievement = GetUserAchievement(masterAchievement.achievement_id);
            if (masterAchievement != null && nextUserAchievement == null)
            {
                nextUserAchievement = new UserAchievement();
                nextUserAchievement.achievement_id = masterAchievement.achievement_id;
                nextUserAchievement.is_completed = false;
                nextUserAchievement.is_received = false;
                this.userAchievement.List.Add(nextUserAchievement);

                // id順に並べ直す
                this.userAchievement.List.Sort((a, b) => a.achievement_id - b.achievement_id);
            }
        }

        // 各エリアの達成率を更新する
        foreach (MasterArea masterArea in masterArea.List)
        {
            UpdateAchievementRate(masterArea.area_id);
        }
    }

    private void UpdateAchievementRate(int area_id)
    {
        List<MasterFloor> masterFloors = ModelManager.Instance.MasterFloor.List.FindAll(floor => floor.area_id == area_id);

        int minFloorId = masterFloors[0].floor_start;
        int maxFloorId = masterFloors[0].floor_end;
        foreach (var floor in masterFloors)
        {
            minFloorId = Mathf.Min(minFloorId, floor.floor_start);
            maxFloorId = Mathf.Max(maxFloorId, floor.floor_end);
        }
        // 差分的な補正
        minFloorId -= 1;
        int currentMaxFloorId = Mathf.Min(UserGameData.max_floor_id, maxFloorId);
        int floorDiffMax = maxFloorId - minFloorId;
        int floorDiffCurrent = Mathf.Max(0, currentMaxFloorId - minFloorId);

        // 登場する敵の数を数える
        int enemyCount = 0;
        List<MasterEnemy> masterEnemies = ModelManager.Instance.MasterEnemy.List.FindAll(enemy => enemy.area_id == area_id);
        foreach (var enemy in masterEnemies)
        {
            enemyCount += enemy.rarity;
        }

        // 登場するアイテムの数を数える
        int itemCount = 0;
        List<MasterItem> masterItems = ModelManager.Instance.MasterItem.List.FindAll(item => item.area_id == area_id);
        itemCount = masterItems.Count;

        // 達成率を計算する
        int total = (floorDiffMax) + enemyCount + itemCount;

        int areaEnemyCount = userEnemy.List.FindAll(enemy => enemy.area_id == area_id).Count;
        int areaItemCount = userItem.List.FindAll(item => item.area_id == area_id).Count;
        int current = (floorDiffCurrent) + areaEnemyCount + areaItemCount;

        UserArea userArea = ModelManager.Instance.userArea.List.Find(area => area.area_id == area_id);
        if (userArea == null)
        {
            userArea = new UserArea();
            userArea.area_id = area_id;

            Debug.Log("userAreaがnullだったので作成" + area_id);

            ModelManager.Instance.userArea.List.Add(userArea);
        }
        userArea.floor_max = floorDiffMax;
        userArea.floor_count = floorDiffCurrent;
        userArea.enemy_max = enemyCount;
        userArea.enemy_count = userEnemy.List.FindAll(enemy => enemy.area_id == area_id).Count;
        userArea.item_max = itemCount;
        userArea.item_count = userItem.List.FindAll(item => item.area_id == area_id).Count;

        List<MasterAchievement> currentAreaAchievements = masterAchievement.List.FindAll(achievement => achievement.type == (int)AchievementType.COMPLETE_RATE && achievement.param1 == area_id);
        foreach (var achievement in currentAreaAchievements)
        {
            int rate = (int)(float)(100f * current / total);
            if (achievement.param2 <= rate)
            {
                var userAchievement = GetUserAchievement(achievement.achievement_id);
                if (userAchievement == null)
                {
                    UnlockAchievement(achievement.achievement_id);
                }
                else if (userAchievement.is_completed == false)
                {
                    UnlockAchievement(achievement.achievement_id);
                }
                else
                {
                    // コンプ済みなので何もしない
                }
            }
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

        userGameData.max_floor_id = newFloorID;
        // 各エリアの達成率を更新する
        foreach (MasterArea masterArea in masterArea.List)
        {
            UpdateAchievementRate(masterArea.area_id);
        }
    }

    private void OnEnemyDie(UserEnemy argEnemy, MasterEnemy masterEnemy, Vector3 position)
    {
        UserEnemy diedEnemy = userEnemy.List.Find(enemy => enemy.enemy_id == argEnemy.enemy_id && argEnemy.rarity == enemy.rarity);

        if (diedEnemy == null)
        {
            diedEnemy = new UserEnemy();
            diedEnemy.enemy_id = argEnemy.enemy_id;
            diedEnemy.area_id = argEnemy.area_id;
            diedEnemy.rarity = argEnemy.rarity;
            diedEnemy.count = argEnemy.count;
            userEnemy.List.Add(diedEnemy);

            //初めて倒す敵の場合
            MasterAchievement masterAchievement = this.masterAchievement.List.Find(achievement => achievement.type == (int)AchievementType.ENEMY && achievement.param1 == argEnemy.enemy_id);
            if (masterAchievement != null)
            {
                UnlockAchievement(masterAchievement.achievement_id);
            }
        }
    }

    public int GetTotalPartyHeart()
    {
        int totalHeart = 0;
        foreach (var userChara in userChara.List.FindAll(chara => 0 < chara.partyIndex))
        {
            totalHeart += userChara.heart;
        }
        return totalHeart;
    }

    public int GetTotalCollectPartyLuck()
    {
        int totalLuck = 0;
        foreach (var userChara in userChara.List.FindAll(chara => 0 < chara.collectPartyId))
        {
            totalLuck += userChara.luck;
        }
        return totalLuck;
    }

    public UserEnemy GetUserEnemy(int enemy_id, int rarity)
    {
        return userEnemy.List.Find(enemy => enemy.enemy_id == enemy_id && enemy.rarity == rarity);
    }

    public UserStar GetUserStar(int chara_id)
    {
        return userStar.List.Find(star => star.chara_id == chara_id);
    }

    public void AddStar(int chara_id, int num)
    {
        UserStar userStar = GetUserStar(chara_id);
        if (userStar == null)
        {
            userStar = new UserStar();
            userStar.chara_id = chara_id;
            userStar.num = 0;
            this.userStar.List.Add(userStar);
        }
        userStar.num += num;
    }

    public bool UseStar(int chara_id, int num)
    {
        UserStar userStar = GetUserStar(chara_id);
        if (userStar == null)
        {
            return false;
        }
        if (userStar.num < num)
        {
            return false;
        }
        userStar.num -= num;
        return true;
    }

    public int GetGachaResult(int gacha_id)
    {
        List<MasterGacha> masterGachas = masterGacha.List.FindAll(gacha => gacha.gacha_id == gacha_id);

        int[] probArray = new int[masterGachas.Count];
        for (int i = 0; i < masterGachas.Count; i++)
        {
            probArray[i] = masterGachas[i].prob;
        }
        int resultIndex = UtilRand.GetIndex(probArray);

        return masterGachas[resultIndex].chara_id;

    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // スマホだとこっちが呼ばれそう
            string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PlayerPrefs.SetString(Defines.LastPlayTimeKey, strDateTime);
            Debug.Log(Defines.LastPlayTimeKey + strDateTime);

            Save();
        }
        else
        {
            // PauseTimeから復帰するまでの時間(秒数)
            if (PlayerPrefs.HasKey(Defines.LastPlayTimeKey))
            {
                string strDateTime = PlayerPrefs.GetString(Defines.LastPlayTimeKey);
                DateTime pauseTime = DateTime.Parse(strDateTime);
                DateTime nowTime = DateTime.Now;
                TimeSpan span = nowTime - pauseTime;
                //Debug.Log("span" + span.TotalSeconds);

                int minutes = (int)span.TotalMinutes;
                int hours = (int)span.TotalHours;

                List<string> bonusMessageList = new List<string>();
#if UNITY_EDITOR
                bonusMessageList.Add("テスト用にメッセージが表示されます");
#endif
                if (0 < minutes)
                {
                    bonusMessageList.Add("パーティーメンバーが経験値を獲得しました");
                    // パーティーに編成しているキャラのループ
                    foreach (var chara in userChara.List.FindAll(x => 0 < x.partyIndex))
                    {
                        int addExp = minutes * Defines.HOUCHI_EXP_PER_MINUTE;
                        chara.exp += addExp;
                    }

                    int addCoin = Mathf.Min(minutes * Defines.HOUCHI_COIN_PER_MINUTE, 999999999);
                    AddCoin(addCoin);
                    bonusMessageList.Add("コインを" + addCoin + "獲得しました");
                }

                // LastGemTimeを使って、最後に受け取った時間を記録しておく
                int lastGemTimeMinutes = PlayerPrefs.GetInt(Defines.GemTimeKey, 0);
                lastGemTimeMinutes += minutes;

                if (0 < lastGemTimeMinutes / 60)
                {
                    int addGem = lastGemTimeMinutes / 60 * Defines.HOUCHI_GEM_PER_HOUR;
                    AddGem(addGem);
                    bonusMessageList.Add("ジェムを" + addGem + "個Get!!");

                    lastGemTimeMinutes = lastGemTimeMinutes % 60;
                }
                PlayerPrefs.SetInt(Defines.GemTimeKey, lastGemTimeMinutes);

                int lastTicketTimeMinutes = PlayerPrefs.GetInt(Defines.TicketTimeKey, 0);
                lastTicketTimeMinutes += minutes;
                int OneDayMinutes = 60 * 24;

                if (0 < lastTicketTimeMinutes / OneDayMinutes)
                {
                    int addTicket = lastTicketTimeMinutes / OneDayMinutes * Defines.HOUCHI_TICKET_PER_DAY;
                    AddTicket(addTicket);
                    bonusMessageList.Add("チケットを" + addTicket + "枚Get!!");

                    lastTicketTimeMinutes = lastTicketTimeMinutes % OneDayMinutes;
                }
                if (0 < bonusMessageList.Count)
                {
                    PanelLoginBonus.Instance.Show(bonusMessageList);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        // エディタで確認する場合はここを利用する
        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        PlayerPrefs.SetString(Defines.LastPlayTimeKey, strDateTime);
        Debug.Log(Defines.LastPlayTimeKey + ":" + strDateTime);

        Save();
    }


    public void ArriveCheck(int floor_id)
    {
        MasterFloor masterFloor = this.masterFloor.List.Find(v => v.floor_start <= floor_id && floor_id <= v.floor_end);

        if (masterFloor == null)
        {
            return;
        }
        //Debug.Log(floor_id + " " + masterFloor.area_id + " " + masterFloor.floor_start + " " + masterFloor.floor_end);

        // エリアデータがあるかどうかを調べる
        UserArea userArea = this.userArea.List.Find(v => v.area_id == masterFloor.area_id);
        //Debug.Log(userArea);
        if (userArea == null)
        {
            UpdateAchievementRate(masterFloor.area_id);
        }
    }

}
