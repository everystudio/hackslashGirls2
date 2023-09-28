using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAsset", menuName = "CharacterAsset")]
public class CharacterAsset : ScriptableObject
{
    public int id;
    public string charaName;
    public string description;
    public Sprite icon;
    public Texture2D miniTexture;
}
