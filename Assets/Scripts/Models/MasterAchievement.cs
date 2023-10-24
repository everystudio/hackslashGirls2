using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public enum AchievementType
{
    NONE,
    FLOOR,
    ENEMY,
    COLLECT,
    COMPLETE_RATE,
    MAX
}

public enum AchievementPrizeType
{
    NONE,
    COIN,
    GEM,
    TICKET,
    ITEM,
    MAX
}

public class MasterAchievement : CsvModelParam
{
    public int achievement_id;
    public int open_id;
    public string title;
    public string description;
    public int type;//	#type_name	
    public int param1;
    public int param2;

    public int prize_type;
    public int prize_id;
    public int prize_amount;

    public string GetPrizeText()
    {
        switch (prize_type)
        {
            case (int)AchievementPrizeType.COIN:
                return prize_amount.ToString() + "コイン";
            case (int)AchievementPrizeType.GEM:
                return prize_amount.ToString() + "ジェム";
            case (int)AchievementPrizeType.TICKET:
                return prize_amount.ToString() + "チケット";
            case (int)AchievementPrizeType.ITEM:
                var item = ModelManager.Instance.GetMasterItem(prize_id);
                return item.item_name + ":" + prize_amount.ToString() + "個";
            default:
                return "";
        }
    }

    public string GetPrizeSpriteName()
    {
        switch (prize_type)
        {
            case (int)AchievementPrizeType.COIN:
                return "icon_coin";
            case (int)AchievementPrizeType.GEM:
                return "icon_gem";
            case (int)AchievementPrizeType.TICKET:
                return "icon_ticket";
            case (int)AchievementPrizeType.ITEM:
                var item = ModelManager.Instance.GetMasterItem(prize_id);
                return item.filename;
            default:
                return "";
        }
    }


}
