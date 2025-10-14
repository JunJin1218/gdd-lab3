using UnityEngine;

public class EnableDash : MonoBehaviour
{
    // 플레이어와 충돌 시 PlayerMovement 싱글톤의 dashAvailable을 true로 설정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement.instance.dashAvailable = true;
        }
    }
}
