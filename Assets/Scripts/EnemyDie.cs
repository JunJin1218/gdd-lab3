using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        col.enabled = false;
        anim.SetTrigger("Death");
    }

    public void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
