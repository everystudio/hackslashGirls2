using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogame;
using System;

public class ListParty : StateMachineBase<ListParty>
{
    [SerializeField] private Transform partyRoot;
    [SerializeField] private GameObject partyPrefab;

    [SerializeField] private Transform memberRoot;
    [SerializeField] private GameObject memberPrefab;

    public bool isQuest;

    private List<UICharacterCore> partyCharacterCoreList = new List<UICharacterCore>();
    private List<UICharacterCore> memberCharacterCoreList = new List<UICharacterCore>();

    private void RefreshPartyList()
    {
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

        // partyListのpartyIndexの昇順に並び替え
        partyList.Sort((a, b) => a.partyIndex - b.partyIndex);


        partyCharacterCoreList.Clear();
        foreach (var userChara in partyList)
        {
            var party = Instantiate(partyPrefab, partyRoot).GetComponent<UICharacterCore>();
            party.transform.SetAsFirstSibling();
            partyCharacterCoreList.Add(party);

            var characterAsset = ModelManager.Instance.GetMasterChara(userChara.chara_id);
            party.Set(characterAsset, userChara);

            /*
            party.OnClick.AddListener((chara) =>
            {
                foreach (var partyCharacterCore in partyCharacterCoreList)
                {
                    partyCharacterCore.Select(chara.id);
                }
            });
            */
        }

    }

    public void Initialize()
    {
        RefreshPartyList();

        // memberRootの子要素を全て削除
        foreach (Transform child in memberRoot)
        {
            Destroy(child.gameObject);
        }
        foreach (var userChara in ModelManager.Instance.UserChara.List)
        {
            var member = Instantiate(memberPrefab, memberRoot).GetComponent<UICharacterCore>();
            var masterChara = ModelManager.Instance.GetMasterChara(userChara.chara_id);
            member.Set(masterChara, userChara);

            if (0 < userChara.partyIndex)
            {
                bool block = false;
                if (isQuest && 0 < userChara.collectPartyId)
                {
                    block = true;
                }
                else if (!isQuest && 0 < userChara.questPartyId)
                {
                    block = true;
                }
                if (block)
                {
                    member.SetBlockClick();
                }
            }


            /*
            member.OnClick.AddListener((chara) =>
            {
                foreach (var memberCharacterCore in memberCharacterCoreList)
                {
                    memberCharacterCore.Select(chara.id);
                }
            });
            */
            memberCharacterCoreList.Add(member);
        }
        ChangeState(new ListParty.Newtral(this));

    }

    private class Newtral : StateBase<ListParty>
    {
        public Newtral(ListParty machine) : base(machine)
        {
        }
        public override void OnEnterState()
        {
            foreach (var party in machine.partyCharacterCoreList)
            {
                party.Select(-1);
                party.OnClick.AddListener(OnPartySelect);
            }
            foreach (var member in machine.memberCharacterCoreList)
            {
                member.Select(-1);
                member.OnClick.AddListener(OnPartySelect);
            }
        }

        private void OnPartySelect(UserChara arg0)
        {
            foreach (var party in machine.partyCharacterCoreList)
            {
                party.Select(arg0.chara_id);
            }
            foreach (var member in machine.memberCharacterCoreList)
            {
                member.Select(arg0.chara_id);
            }

            ChangeState(new ListParty.SelectingParty(machine, arg0));
        }

        public override void OnExitState()
        {
            base.OnExitState();
            foreach (var party in machine.partyCharacterCoreList)
            {
                party.OnClick.RemoveListener(OnPartySelect);
            }
            foreach (var member in machine.memberCharacterCoreList)
            {
                member.OnClick.RemoveListener(OnPartySelect);
            }
        }
    }

    private class SelectingParty : StateBase<ListParty>
    {
        private UserChara selectedChara;

        public SelectingParty(ListParty machine, UserChara selectedChara) : base(machine)
        {
            this.machine = machine;
            this.selectedChara = selectedChara;
        }

        public override void OnEnterState()
        {
            foreach (var member in machine.partyCharacterCoreList)
            {
                member.OnClick.AddListener(OnMemberSelect);
            }
            foreach (var member in machine.memberCharacterCoreList)
            {
                member.OnClick.AddListener(OnMemberSelect);
            }
        }

        private void OnMemberSelect(UserChara arg1)
        {
            if (selectedChara.chara_id == arg1.chara_id)
            {
                foreach (var member in machine.partyCharacterCoreList)
                {
                    member.OnClick.RemoveListener(OnMemberSelect);
                }
                ChangeState(new ListParty.Newtral(machine));
                return;
            }
            else
            {
                if (machine.isQuest)
                {
                    int tempPartyId = selectedChara.questPartyId;
                    selectedChara.questPartyId = arg1.questPartyId;
                    arg1.questPartyId = tempPartyId;
                }
                else
                {
                    int tempPartyId = selectedChara.collectPartyId;
                    selectedChara.collectPartyId = arg1.collectPartyId;
                    arg1.collectPartyId = tempPartyId;
                }

                machine.RefreshPartyList();
                ChangeState(new ListParty.Newtral(machine));
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();
            foreach (var member in machine.partyCharacterCoreList)
            {
                member.OnClick.RemoveListener(OnMemberSelect);
            }
            foreach (var member in machine.memberCharacterCoreList)
            {
                member.OnClick.RemoveListener(OnMemberSelect);
            }

        }
    }
}
