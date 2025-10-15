using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneTransfer : Singleton<PlayerSceneTransfer>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // other instructions
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SetStartingPosition;
    }

    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "Scene2" || next.name == "Scene1")
        {
            transform.position = new Vector3(0f, 0f, 0f);
        }

        if (next.name == "MainMenu" || next.name == "GameOver" || next.name == "LoadingScene")
        {
            transform.position = new Vector3(200, 835, 0);
        }
    }
}
