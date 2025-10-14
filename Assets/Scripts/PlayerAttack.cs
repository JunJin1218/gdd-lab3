using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerAttack : Singleton<PlayerAttack>
{
    public LayerMask enemyMask;
    public float reboundForce = 4f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyMask) != 0 && rb.linearVelocityY < -0.1f)
        {

            // Enemy Death
            EnemyDie enemyDie = other.GetComponent<EnemyDie>();
            if (enemyDie != null)
            {
                Debug.Log("stomp");
                enemyDie.Die();
            }

            // Rebound
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
            rb.AddForce(Vector2.up * reboundForce, ForceMode2D.Impulse);
        }
    }
}
