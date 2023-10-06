using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;

public class ModelManager : Singleton<ModelManager>
{
    public TextAsset dummyUserChara;
    public CsvModel<UserChara> userChara = new CsvModel<UserChara>();

    [SerializeField] private List<CharacterAsset> characterAssets = new List<CharacterAsset>();
    public List<CharacterAsset> CharacterAssets { get { return characterAssets; } }

    public TextAsset masterItemAsset;
    private CsvModel<MasterItem> masterItem = new CsvModel<MasterItem>();
    public CsvModel<MasterItem> MasterItem { get { return masterItem; } }

    private CsvModel<UserItem> userItem = new CsvModel<UserItem>();
    public CsvModel<UserItem> UserItem { get { return userItem; } }

    public override void Initialize()
    {
        userChara.Load(dummyUserChara);
        masterItem.Load(masterItemAsset);



        foreach (var item in masterItem.List)
        {
            Debug.Log(item.item_name);
        }
    }

    public CharacterAsset GetCharacterAsset(int id)
    {
        return characterAssets.Find(x => x.id == id);
    }

}
