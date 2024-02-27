using UnityEngine;

/// <summary>
/// A controller for the player.  This is for demo purposes and can be extended/replaced.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public GameManager gm;
    private const string InputHorizontalMovement = "Horizontal";
    private const string InputVerticalMovement = "Vertical";
    private const string InputJump = "Jump";
    private const string HorizontalSpeedAnimatorParameter = "Horizontal Speed";
    private const string VerticalSpeedAnimatorParameter = "Vertical Speed";

    [Tooltip("Whether or not the player will be treated as though the Level is a platformer")]
    [SerializeField]
    private bool platformer = false;

    [Tooltip("Speed at which the player moves")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float speed = 5.0f;

    [Tooltip("Force with which the player jumps, if platformer is enabled")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float jump = 5.0f;
    public GameObject part; 

    private Rigidbody2D body2D;
    private Animator animator;

    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis(InputHorizontalMovement), Input.GetAxis(InputVerticalMovement));
        if (movement.x != 0 && Mathf.Sign(movement.x) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if (platformer)
        {
            body2D.velocity = new Vector2(movement.x * speed, body2D.velocity.y);
            //animator.SetFloat(HorizontalSpeedAnimatorParameter, Mathf.Abs(body2D.velocity.x));
            //animator.SetFloat(VerticalSpeedAnimatorParameter, Mathf.Abs(body2D.velocity.y));
        }
        else
        {
            body2D.velocity = movement * speed;
            //animator.SetFloat(HorizontalSpeedAnimatorParameter, Mathf.Sqrt(Mathf.Abs(body2D.velocity.x) + Mathf.Abs(body2D.velocity.y)));
            //animator.SetFloat(VerticalSpeedAnimatorParameter, 0);
        }
        if (platformer && Input.GetButtonDown(InputJump))
        {
            body2D.velocity = new Vector2(body2D.velocity.x, jump);
        }

        if (gm._gameOver)
        {
            speed = 0f; 
        }
    }

    void OnValidate()
    {
        body2D = GetComponent<Rigidbody2D>();
        if (platformer)
        {
            body2D.gravityScale = 1.0f;
        }
        else
        {
            body2D.gravityScale = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LifeXPSphere")
        {
            gm.XP+= 10;
            Instantiate(part, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}