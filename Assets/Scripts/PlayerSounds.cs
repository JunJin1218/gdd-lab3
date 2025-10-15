using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSounds : Singleton<PlayerSounds>
{
    public AudioSource dashAudio;
    public AudioSource scoreAudio;

    public void PlayDashAudio()
    {
        dashAudio.Play();
    }

    public void PlayScoreAudio()
    {
        scoreAudio.Play();
    }

}
