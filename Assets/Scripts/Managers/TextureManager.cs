using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogame;
using UnityEngine.U2D;

public class TextureManager : Singleton<TextureManager>
{
    [SerializeField] private SpriteAtlas charaIconSpriteAtlas;
    [SerializeField] private SpriteAtlas itemSpriteAtlas;
    [SerializeField] private SpriteAtlas enemiesSpriteAtlas;

    [SerializeField] private List<Texture2D> miniCharaTextureList;

    public Texture2D GetMiniCharaTexture(int charaId)
    {
        // sample
        //"chara00301_mini"
        string textureName = "chara" + charaId.ToString("D3") + "01_mini";
        return miniCharaTextureList.Find(texture => texture.name == textureName);
    }

    public Sprite GetIconCharaSprite(int charaId)
    {
        // sample
        //"chara00101_00_faceicon"
        string spriteName = "chara" + charaId.ToString("D3") + "01_00_faceicon";
        return charaIconSpriteAtlas.GetSprite(spriteName);
    }

    public Sprite GetIconItemSprite(string filename)
    {
        return itemSpriteAtlas.GetSprite(filename);
    }

    public Sprite GetIconItemSprite(int itemId)
    {
        // sample
        //"item00101_icon"
        string spriteName = ModelManager.Instance.GetMasterItem(itemId).filename;
        return GetIconItemSprite(spriteName);
    }
    public Sprite GetEnemySprite(string filename)
    {
        return enemiesSpriteAtlas.GetSprite(filename);
    }

}
