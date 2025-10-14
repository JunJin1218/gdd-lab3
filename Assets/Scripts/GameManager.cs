using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int score { get; private set; }
    public bool IsPlaying { get; private set; }
    public TMPro.TMP_Text scoreText;

    void Start()
    {
        StartCoroutine(GameLoop());
        UpdateScoreUI();
    }

    public void OnRestartButtonPressed()
    {
        ResetScore();
        OnPlayerDied();
        resetPlayerPosition();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void resetPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void AddOnePoint()
    {
        score++;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

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

    // --- For Unity's scene manager --- //
    // Because this is a singleton, it will persist across scenes.
    // We need to reset the game state when a new scene is loaded.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // we need this ebcause idk why the game manager is not finding the
        // score text in the new scene on restart
        var go = GameObject.Find("ScoreText");
        if (go != null)
        {
            scoreText = go.GetComponent<TMPro.TMP_Text>();
            UpdateScoreUI();
        }
    }
}
