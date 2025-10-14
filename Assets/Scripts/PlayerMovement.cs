using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Singleton<PlayerMovement>
{
    // PUBLIC
    // public PlayerActions playerActions;
    public float accelX = 20f;
    public float decelX = 40f;
    public float maxSpeedX = 20f;
    public float jumpForce = 8f;
    public float attackForce = 10f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public float dashForce = 4f;

    [NonSerialized]
    public bool dashAvailable = false;

    // PRIVATE
    private Rigidbody2D rb;
    private Collider2D cd;
    private SpriteRenderer sr;
    private Animator animator;
    private int dir;
    private float velDeadzone = 0.001f;
    private bool isGrounded = false;
    private bool jumpPressed = false;
    private bool extraJump = false;
    private bool extraJumpAvailable = true;
    private bool moving = false;
    private bool fallingAttack = false;
    private bool fallingAttackAvailable = false;
    private bool dashPressed = false;

    // SFX

    private AudioSource ad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GET components
        rb = gameObject.GetComponent<Rigidbody2D>();
        cd = gameObject.GetComponent<Collider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        ad = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Visual control
        animator.SetBool("running", math.abs(rb.linearVelocityX) > 0);
        animator.SetBool("jumping", !isGrounded && (rb.linearVelocityY > 0));
        animator.SetBool("falling", !isGrounded && (rb.linearVelocityY <= 0));
        sr.flipX = rb.linearVelocityX < 0;

        // ResetPosition
        if (transform.position.y < -6)
            ResetPosition();
    }

    void FixedUpdate()
    {
        // Movement (x axis)
        if (moving)
            Move(dir);

        // Ground check & jump
        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ad.Play();
            jumpPressed = false;
            extraJumpAvailable = true;
            fallingAttackAvailable = true;
        }
        if (extraJump && extraJumpAvailable)
        {
            rb.AddForce(Vector2.up * jumpForce * 50, ForceMode2D.Force);
            extraJump = false;
            extraJumpAvailable = false;
        }

        // Falling attack
        if (fallingAttack && !isGrounded)
        {
            rb.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
            fallingAttack = false;
            fallingAttackAvailable = false;
        }

        // Dash
        if (dashPressed && dashAvailable)
        {
            rb.AddForce(dir * Vector2.right * dashForce, ForceMode2D.Impulse);
            dashPressed = false;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpPressed = true;
        }

        if (context.canceled)
            jumpPressed = false;
    }

    void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        jumpPressed = false;
        extraJumpAvailable = false;
    }

    public void Move(int dir)
    {
        // Boilerplates
        float vx = rb.linearVelocity.x;
        float vy = rb.linearVelocity.y;
        float target = dir * maxSpeedX;

        // X movement
        if (dir == 0 && Mathf.Abs(vx) < velDeadzone)
            vx = 0f;

        float rate = (dir != 0) ? accelX : decelX;
        float newVX = Mathf.MoveTowards(vx, target, rate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newVX, vy);
    }

    public void MoveCheck(int value)
    {
        dir = value;
        moving = dir != 0;
    }

    public void Jump(bool b)
    {
        jumpPressed = b;
    }

    public void Attack(bool b)
    {
        fallingAttack = b;
    }

    public void Dash(bool b)
    {
        dashPressed = b;
    }

    public void JumpHold(bool b)
    {
        if (b && extraJumpAvailable)
            extraJump = true;
    }
}
