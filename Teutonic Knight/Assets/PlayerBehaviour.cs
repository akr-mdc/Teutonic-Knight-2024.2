using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpforce = 15f;
    [SerializeField] float speed = 10f;

    private Animator Animator;
    private Rigidbody2D PlayerRB;
    private SpriteRenderer Sprite;

    private bool attack1button = false;
    private bool attack2button = false;
    private bool attack3button = false;
    private bool jumpbutton = false;
    private bool castfireballbutton = false;
    private bool shootarrowbutton = false;
    private bool dancebutton = false;
    private bool kickbutton = false;
    private bool runbutton = false;
    private bool laybutton = false;
    private float walkbutton = 0f;
    private bool isGrounded = false;

    enum State { Idle, Walk, Run, Jump1, Jump2, Attack1, Attack2, Attack3, CastFireball, Kick, ShootArrow, Lay, Dance }

    State state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        PlayerRB = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        attack1button = Input.GetKey(KeyCode.J);
        attack2button = Input.GetKey(KeyCode.K);
        attack3button = Input.GetKey(KeyCode.L);
        jumpbutton = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        castfireballbutton = Input.GetKey(KeyCode.O);
        shootarrowbutton = Input.GetKey(KeyCode.I);
        dancebutton = Input.GetKey(KeyCode.F);
        kickbutton = Input.GetKey(KeyCode.H);
        laybutton = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        runbutton = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        walkbutton = Input.GetAxisRaw("Horizontal");

        switch (state)
        {
            case State.Idle:         Idle();         break;
            case State.Walk:         Walk();         break;
            case State.Run:          Run();          break;
            case State.Jump1:        Jump1();        break;
            case State.Jump2:        Jump2();        break;
            case State.Attack1:      Attack1();      break;
            case State.Attack2:      Attack2();      break;
            case State.Attack3:      Attack3();      break;
            case State.Kick:         Kick();         break;
            case State.CastFireball: CastFireball(); break;
            case State.ShootArrow:   ShootArrow();   break;
            case State.Lay:          Lay();          break;
            case State.Dance:        Dance();        break;
        }

    }

    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    private void CloseState(string animationName)
    {
        if (IsAnimationFinished(animationName))
        {
            PlayerRB.gravityScale = 1;

            if (isGrounded)
            {
                state = State.Idle;
            }
            else state = State.Jump2;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void Idle()
    {
        Animator.Play("Idle");

        if (walkbutton != 0)
        {
            state = State.Walk;
        }
        else if (runbutton)
        {
            state = State.Run;
        }
        else if (attack1button)
        {
            state = State.Attack1;
        }
        else if (attack2button)
        {
            state = State.Attack2;
        }
        else if (attack3button)
        {
            state = State.Attack3;
        }
        else if (kickbutton)
        {
            state = State.Kick;
        }
        else if (shootarrowbutton)
        {
            state = State.ShootArrow;
        }
        else if (castfireballbutton)
        {
            state = State.CastFireball;
        }
        else if (laybutton)
        {
            state = State.Lay;
        }
        else if (dancebutton)
        {
            state = State.Dance;
        }
        else if (jumpbutton && isGrounded)
        {
            isGrounded = false;
            PlayerRB.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            state = State.Jump1;
        }
        else if (!isGrounded)
        {
            state = State.Jump2;
        }
    }

    // enum State { Idle, Walk, Run, Jump1, Jump2, Attack1, Attack2, Attack3, CastFireball, Kick, ShootArrow, Dance }
    private void Walk()
    {
        Animator.Play("Walk");
        if (walkbutton < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbutton > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        PlayerRB.velocity = new Vector2(walkbutton * speed, PlayerRB.velocity.y);
        if (runbutton)
        {
            state = State.Run;
        }
        if (jumpbutton && isGrounded)
        {
            isGrounded = false;
            PlayerRB.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            state = State.Jump1;
        }
       else CloseState("Walk");
    }
    private void Run()
    {
        Animator.Play("Run");
        if (walkbutton < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbutton > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        PlayerRB.velocity = new Vector2(walkbutton * 2 * speed, PlayerRB.velocity.y);
        if (jumpbutton && isGrounded)
        {
            isGrounded = false;
            PlayerRB.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            state = State.Jump1;
        }
        else CloseState("Run");
    }
    private void Jump1()
    {
        Animator.Play("Jump1");
        if (walkbutton < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbutton > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        PlayerRB.velocity = new Vector2(walkbutton * speed, PlayerRB.velocity.y);

        if (PlayerRB.velocity.y < 0)
        {
            state = State.Jump2;
        }
     
    }
    private void Jump2()
    {
        Animator.Play("Jump2");
        if (walkbutton < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbutton > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        PlayerRB.velocity = new Vector2(walkbutton * speed, PlayerRB.velocity.y);
        CloseState("Jump2");
    }
    private void Attack1()
    {
        Animator.Play("Attack1");

        CloseState("Attack1");
    }
    private void Attack2()
    {
        Animator.Play("Attack2");

        CloseState("Attack2");
    }
    private void Attack3()
    {
        Animator.Play("Attack3");

        CloseState("Attack3");
    }
    private void CastFireball()
    {
        Animator.Play("CastFireball");

        CloseState("CastFireball");
    }
    private void Kick()
    {
        Animator.Play("Kick");

        CloseState("Kick");
    }
    private void ShootArrow()
    {
        Animator.Play("ShootArrow");

        CloseState("ShootArrow");
    }
    private void Dance()
    {
        Animator.Play("Dance");

        CloseState("Dance");
    }
    private void Lay()
    {
        Animator.Play("Lay");

        CloseState("Lay");
    }
}
