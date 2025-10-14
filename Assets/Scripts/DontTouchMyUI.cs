using UnityEngine;

public class DontTouchMyUI : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
