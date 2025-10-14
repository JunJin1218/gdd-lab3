using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int score { get; private set; }
    public bool IsPlaying { get; private set; }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    public void AddOnePoint() => score++;
    public void ResetScore() => score = 0;

    public void OnPlayerDied()
    {
        IsPlaying = false;
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return StartCoroutine(State_WaitForStart());
            yield return StartCoroutine(State_Playing());
            yield return StartCoroutine(State_GameOver());
        }
    }

    IEnumerator State_WaitForStart()
    {
        yield return new WaitUntil(() => IsPlaying);
    }

    IEnumerator State_Playing()
    {
        yield return new WaitUntil(() => IsPlaying == false);
    }

    IEnumerator State_GameOver()
    {
        yield return new WaitForSeconds(0.5f); // short pause before restart
        yield return new WaitUntil(() => IsPlaying);
    }

    public void StartGame()
    {
        ResetScore();
        IsPlaying = true;
    }

    public void ForceStopGame()
    {
        IsPlaying = false;
    }
}
