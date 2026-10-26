using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollDuration = 8f / 14f;          // same value the pack uses
    [SerializeField, Range(0.3f, 1f)] float rollColliderScale = 0.5f;
    [SerializeField] float baseGameSpeed = 6f;               // match GameManager's starting Speed
    [SerializeField] float maxAnimSpeed = 1.8f;
    [SerializeField] bool faceLeft = false;

    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D col;
    Sensor_HeroKnight groundSensor;

    Vector2 colSize, colOffset;
    bool grounded, rolling, dead;
    float rollTimer;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();

        colSize = col.size;
        colOffset = col.offset;

        GetComponent<SpriteRenderer>().flipX = faceLeft;
        anim.SetInteger("AnimState", 1);                     // 1 = run, set once and never change
    }

    void Update()
    {
        var gm = GameManager.Instance;

        if (gm.IsGameOver)
        {
            if (!dead)
            {
                dead = true;
                anim.speed = 1f;
                anim.SetTrigger("Death");
            }
            return;
        }

        // Grounded state, same logic as the pack
        bool sensorGrounded = groundSensor.State();
        if (sensorGrounded != grounded)
        {
            grounded = sensorGrounded;
            anim.SetBool("Grounded", grounded);
        }
        anim.SetFloat("AirSpeedY", rb.linearVelocity.y);

        // Animation plays faster as the game speeds up
        anim.speed = Mathf.Clamp(gm.Speed / baseGameSpeed, 1f, maxAnimSpeed);

        // Roll timer
        if (rolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f) EndRoll();
        }

        var kb = Keyboard.current;

        // Jump
        if (kb.spaceKey.wasPressedThisFrame && grounded && !rolling)
        {
            anim.SetTrigger("Jump");
            grounded = false;
            anim.SetBool("Grounded", false);
            rb.linearVelocity = new Vector2(0f, jumpForce);
            groundSensor.Disable(0.2f);
        }

        // Roll
        bool rollPressed = kb.downArrowKey.wasPressedThisFrame || kb.sKey.wasPressedThisFrame;
        if (rollPressed && grounded && !rolling)
            StartRoll();
    }

    void StartRoll()
    {
        rolling = true;
        rollTimer = rollDuration;
        anim.SetTrigger("Roll");

        float newHeight = colSize.y * rollColliderScale;
        col.size = new Vector2(colSize.x, newHeight);
        // Lower the collider so its bottom edge stays on the ground
        col.offset = new Vector2(colOffset.x, colOffset.y - (colSize.y - newHeight) / 2f);
    }

    void EndRoll()
    {
        rolling = false;
        col.size = colSize;
        col.offset = colOffset;
    }
}