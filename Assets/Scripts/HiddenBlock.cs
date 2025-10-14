using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    [Header("Player filter (둘 중 편한 걸 쓰면 됨)")]
    public string playerTag = "Player";
    public LayerMask playerMask;

    [Header("Anim")]
    public string hitTriggerName = "Hit";

    Animator anim;
    Collider2D col;
    bool triggered; // 중복 방지

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
            Debug.LogWarning("[HiddenBlock] 이 오브젝트의 Collider2D는 IsTrigger가 켜져 있어야 합니다.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        // 1) 플레이어인지 확인 (tag 또는 layermask)
        bool isPlayerByTag = !string.IsNullOrEmpty(playerTag) && other.CompareTag(playerTag);
        bool isPlayerByMask = (playerMask.value & (1 << other.gameObject.layer)) != 0;

        if (!(isPlayerByTag || isPlayerByMask)) return;

        // 2) 플레이어의 Y속도 확인 (위로 치고 올라올 때만)
        var rb = other.attachedRigidbody;
        if (rb == null) return;

        // Rigidbody2D는 velocity.y 사용 (linearVelocityY 아님)
        if (rb.linearVelocityY <= 0f) return;

        // 3) 조건 충족 → 트리거 해제 + 애니메이션 트리거
        triggered = true;

        if (col) col.isTrigger = false; // 이제 고형 블록이 되도록
        if (anim && !string.IsNullOrEmpty(hitTriggerName))
            anim.SetTrigger(hitTriggerName);
    }

    // === Animation Event에서 호출할 함수 ===
    public void RevealChild()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (!child.activeSelf)
                child.SetActive(true);
        }
    }
}
