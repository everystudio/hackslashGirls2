using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogame;

public class ListParty : StateMachineBase<ListParty>
{
    [SerializeField] private Transform partyRoot;
    [SerializeField] private GameObject partyPrefab;

    [SerializeField] private Transform memberRoot;
    [SerializeField] private GameObject memberPrefab;


    public bool isQuest;

    private List<UICharacterCore> partyCharacterCoreList = new List<UICharacterCore>();
    private List<UICharacterCore> memberCharacterCoreList = new List<UICharacterCore>();

    public void Initialize()
    {
        ChangeState(new ListParty.Newtral(this));
        // partyRootの子要素を全て削除
        foreach (Transform child in partyRoot)
        {
            Destroy(child.gameObject);
        }

        List<UserChara> partyList = null;
        if (isQuest)
        {
            partyList = ModelManager.Instance.userChara.List.FindAll(x => x.questPartyId > 0);
        }
        else
        {
            partyList = ModelManager.Instance.userChara.List.FindAll(x => x.collectPartyId > 0);
        }

        partyCharacterCoreList.Clear();
        foreach (var userChara in partyList)
        {
            var party = Instantiate(partyPrefab, partyRoot).GetComponent<UICharacterCore>();
            partyCharacterCoreList.Add(party);

            var characterAsset = ModelManager.Instance.GetCharacterAsset(userChara.id);
            party.Set(characterAsset, userChara);
            party.OnClick.AddListener((chara) =>
            {
                foreach (var partyCharacterCore in partyCharacterCoreList)
                {
                    partyCharacterCore.Select(chara.id);
                }
            });
        }

        // memberRootの子要素を全て削除
        foreach (Transform child in memberRoot)
        {
            Destroy(child.gameObject);
        }
        foreach (var chara in ModelManager.Instance.CharacterAssets)
        {
            var member = Instantiate(memberPrefab, memberRoot).GetComponent<UICharacterCore>();
            var userChara = ModelManager.Instance.userChara.List.Find(x => x.id == chara.id);
            member.Set(chara, userChara);
            member.OnClick.AddListener((chara) =>
            {
                foreach (var memberCharacterCore in memberCharacterCoreList)
                {
                    memberCharacterCore.Select(chara.id);
                }
            });
            memberCharacterCoreList.Add(member);
        }


    }

    private class Newtral : StateBase<ListParty>
    {
        public Newtral(ListParty machine) : base(machine)
        {
        }
    }
}
