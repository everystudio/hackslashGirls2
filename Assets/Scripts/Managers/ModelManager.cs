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

    public override void Initialize()
    {
        userChara.Load(dummyUserChara);
    }

    public CharacterAsset GetCharacterAsset(int id)
    {
        return characterAssets.Find(x => x.id == id);
    }

}
