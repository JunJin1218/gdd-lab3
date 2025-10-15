using UnityEngine;
using System.Collections;

public class PlayerHiddenScore : Singleton<PlayerHiddenScore>
{
    public AudioSource audioSource;
    private Coroutine hiddenScoreCoroutine;
    IEnumerator GiveHiddenScore()
    {
        // special BGM? need to cancel the original BGM first 
        if (audioSource != null) audioSource.Play();

        // 10 seconds -> 1 per second
        for (int i = 0; i < 10; i++)
        {
            GameManager.instance.AddOnePoint();
            yield return new WaitForSeconds(1f);

        }
    }

    public void StartHiddenScore()
    {
        hiddenScoreCoroutine = StartCoroutine(GiveHiddenScore());
    }

    public void StopHiddenScore()
    {
        if (hiddenScoreCoroutine != null)
        {
            StopCoroutine(hiddenScoreCoroutine);
            hiddenScoreCoroutine = null;
        }
    }
}