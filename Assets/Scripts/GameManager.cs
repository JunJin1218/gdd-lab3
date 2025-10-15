using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int score { get; private set; }
    public bool IsPlaying { get; private set; }
    public TMPro.TMP_Text scoreText;
    private bool loadingCancelled = false;

    public GameObject PausePanel;

    void Start()
    {
        StartCoroutine(GameLoop());
        UpdateScoreUI();
    }

    public void AddOnePoint()
    {
        score++;
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

    // IEnumerator Fade()
    // {
    //     for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
    //     {
    //         c.alpha = alpha;
    //         yield return new WaitForSecondsRealtime(0.1f);
    //     }

    //     // once done, go to next scene
    //     SceneManager.LoadSceneAsync("World-1-1", LoadSceneMode.Single);
    // }

    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScene");
        ResetScore();
        IsPlaying = true;
        foreach (var audio in FindObjectsOfType<AudioSource>())
        {
            if (audio.clip.name != "bgm")
            {
                audio.Stop();
            }
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    public void ForceStopGame()
    {
        IsPlaying = false;
    }

    // --- Restart Logic --- //
    // Called by the Restart button in the UI
    public void OnRestartButtonPressed()
    {
        // unliek score and enemies, we need this resetPlayerPosition because player is
        // not destroyed on scene reload
        ResetScore();
        resetPlayerPosition();
        // load the current active scene again --> "RESTARTED"!
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

    // To "subscribe to sceneLoaded" means you are telling Unity:
    // "Hey, when a new scene loads, please call this function of mine"
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // run this once a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // we need this ebcause idk why the game manager is not finding the
        // score text in the new scene on restart

        if (scene.name == "LoadingScene")
        {
            StartCoroutine(LoadNextSceneAsync("Scene1"));
        }
        var go = GameObject.Find("ScoreText");
        if (go != null)
        {
            scoreText = go.GetComponent<TMPro.TMP_Text>();
            UpdateScoreUI();
        }
        var buttonGO = GameObject.Find("RestartButton");
        if (buttonGO != null)
        {
            var btn = buttonGO.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnRestartButtonPressed);
        }

        var mainMenuButtonGO = GameObject.Find("MainMenuButton");
        if (mainMenuButtonGO != null)
        {
            var btn = mainMenuButtonGO.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(BackToMainMenu);
        }

        var startButtonGO = GameObject.Find("StartButton");
        if (startButtonGO != null)
        {
            var btn = startButtonGO.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(StartGame);
        }
        var pauseButtonGO = GameObject.Find("PauseButton");
        if (pauseButtonGO != null)
        {
            var btn = pauseButtonGO.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(PauseGame);
        }

        var resumeButtonGO = GameObject.Find("ResumeButton");
        if (resumeButtonGO != null)
        {
            var btn = resumeButtonGO.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ResumeGame);
        }
    }

    // main menu button
    public void BackToMainMenu()
    {
        // Destroy the player if it exists
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        loadingCancelled = true;

        // Stop all audio except bgm
        foreach (var audio in FindObjectsOfType<AudioSource>())
        {
            if (audio.clip.name != "bgm")
            {
                audio.Stop();
            }
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameWithLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    // load the next scene asynchronously
    IEnumerator LoadNextSceneAsync(string nextScene)
    {
        loadingCancelled = false;
        yield return new WaitForSeconds(2f); // wait for 2 seconds

        // Check if loading was cancelled
        if (loadingCancelled)
            yield break; // Stop if cancelled

        // if not start loading the next scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        // while loading the next scene, check if loading was cancelled
        while (!asyncLoad.isDone)
        {
            if (loadingCancelled)
                yield break; // Stop if cancelled during loading
            yield return null;
        }
    }

    // this was done because there was a bug where when i click the main menu in the
    // loading screen, the game would still load the scene1 after 2f seconds

    // Pause and Resume buttons
    public void PauseGame()
    {
        Time.timeScale = 0f;
        IsPlaying = false;
        PausePanel.SetActive(true);
        foreach (var audio in FindObjectsOfType<AudioSource>())
        {
            audio.Stop(); // or audio.Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPlaying = true;
        PausePanel.SetActive(false);
        foreach (var audio in FindObjectsOfType<AudioSource>())
        {
            audio.UnPause(); // or audio.UnPause();
        }
    }
}
