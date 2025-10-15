using UnityEngine;
using System.Collections;

public class EnableHiddenScore : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSounds.instance.PlayScoreAudio();
            PlayerHiddenScore.instance.StartHiddenScore();
            gameObject.SetActive(false);
            // Jae's TODO: pls add some sound here
        }
    }

}
