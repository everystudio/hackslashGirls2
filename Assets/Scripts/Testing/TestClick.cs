using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    string[] ReactionArr = new string[5] { "1", "2", "3", "4", "5" };


    private void Start()
    {
        StartCoroutine(Reaction(0, 5));
    }

    IEnumerator Reaction(int reactionNum, int lengthOfText)
    {
        for (int i = 0; i < lengthOfText; i++)
        {
            Debug.Log(ReactionArr[reactionNum + i]);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            Debug.Log("clicked");
            yield return null;
        }
    }


}
