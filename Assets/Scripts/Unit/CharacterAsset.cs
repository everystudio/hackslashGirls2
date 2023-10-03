using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterAsset", menuName = "CharacterAsset")]
public class CharacterAsset : ScriptableObject
{
    public int id;
    public string charaName;
    public string description;
    public Sprite icon;
    public Texture2D miniTexture;

    public float attack;
    public float attackRange = 0.5f;

    public float attackSpeed = 1f;

}
