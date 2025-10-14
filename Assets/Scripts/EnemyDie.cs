using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;

    private AudioSource ad;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        ad = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() { }

    public void Die()
    {
        col.enabled = false;
        anim.SetTrigger("Death");
        ad.Play();
    }

    public void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
