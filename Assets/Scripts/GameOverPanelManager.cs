using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelManager : Singleton<GameOverPanelManager>
{
    public GameObject GameOverPanel;

    void Start()
    {
        // Make GameOverPanel persistent across scenes
        if (
            GameOverPanel != null
            && GameOverPanel.scene.name != null
            && GameOverPanel.scene.name != "DontDestroyOnLoad"
        )
        {
            DontDestroyOnLoad(GameOverPanel);
        }
    }
}
