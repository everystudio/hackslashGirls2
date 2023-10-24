using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelGetAchievement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Queue<MasterAchievement> completedAchievements = new Queue<MasterAchievement>();

    [SerializeField] private TextMeshProUGUI detailText;

    private bool isPlayingAnimation = false;

    private void Start()
    {
        ModelManager.Instance.OnUnlockAchievement.AddListener(AddCompletedAchievement);
    }

    public void OnAnimationIn()
    {
        StartCoroutine(WaitRealtime());
    }
    IEnumerator WaitRealtime()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        animator.SetTrigger("out");
    }
    public void OnAnimationOut()
    {
        OnAnimationEnd();
    }

    public void AddCompletedAchievement(MasterAchievement masterAchievement)
    {
        //Debug.Log("AddCompletedAchievement");
        //Debug.Log(masterAchievement.title);
        completedAchievements.Enqueue(masterAchievement);

        if (isPlayingAnimation == false)
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        isPlayingAnimation = true;

        var masterAchievement = completedAchievements.Dequeue();
        detailText.text = masterAchievement.description;

        animator.SetTrigger("in");
    }

    public void OnAnimationEnd()
    {
        if (completedAchievements.Count > 0)
        {
            PlayAnimation();
        }
        else
        {
            isPlayingAnimation = false;
        }
    }



}
