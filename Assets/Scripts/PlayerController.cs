using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollDuration = 8f / 14f;
    [SerializeField, Range(0.3f, 1f)] float rollColliderScale = 0.5f;
    [SerializeField] float baseGameSpeed = 6f;
    [SerializeField] float maxAnimSpeed = 1.8f;
    [SerializeField] bool faceLeft = false;

    [Header("Lives")]
    [SerializeField] int maxLives = 3;
    [SerializeField] float invincibleTime = 1.5f;
    [SerializeField] float blinkInterval = 0.1f;
    [SerializeField] bool noBlood = false;

    public int Lives { get; private set; }
    public int MaxLives => maxLives;

    public float AirTime => 2f * jumpForce / (Physics2D.gravity.magnitude * GetComponent<Rigidbody2D>().gravityScale);

    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D col;
    SpriteRenderer sprite;
    Sensor_HeroKnight groundSensor;

    Vector2 colSize, colOffset;
    bool grounded, rolling, dead, invincible;
    float rollTimer;

    void Awake()
    {
        Lives = maxLives;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();

        colSize = col.size;
        colOffset = col.offset;

        sprite.flipX = faceLeft;
        anim.SetInteger("AnimState", 1);   // 1 = run animation
    }

    void Update()
    {
        var gm = GameManager.Instance;

        if (gm.IsGameOver)
        {
            if (!dead)
            {
                dead = true;
                StopAllCoroutines();
                sprite.enabled = true;
                anim.speed = 1f;
                anim.SetBool("noBlood", noBlood);
                anim.SetTrigger("Death");
            }
            return;
        }

        if (gm.IsPaused) return;

        bool sensorGrounded = groundSensor.State();
        if (sensorGrounded != grounded)
        {
            grounded = sensorGrounded;
            anim.SetBool("Grounded", grounded);
        }
        anim.SetFloat("AirSpeedY", rb.linearVelocity.y);

        anim.speed = Mathf.Clamp(gm.Speed / baseGameSpeed, 1f, maxAnimSpeed);

        if (rolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f) EndRoll();
        }

        var kb = Keyboard.current;

        if (kb.spaceKey.wasPressedThisFrame && grounded && !rolling)
        {
            anim.SetTrigger("Jump");
            grounded = false;
            anim.SetBool("Grounded", false);
            rb.linearVelocity = new Vector2(0f, jumpForce);
            groundSensor.Disable(0.2f);
        }

        bool rollPressed = kb.leftShiftKey.wasPressedThisFrame;
        if (rollPressed && grounded && !rolling)
            StartRoll();
    }

    // Called by obstacles when they touch the player
    public void TakeDamage(int amount)
    {
        if (dead || invincible) return;
        if (GameManager.Instance.IsGameOver) return;

        Lives = Mathf.Max(0, Lives - amount);
        if (CameraShake.Instance != null) CameraShake.Instance.Shake();

        if (rolling) EndRoll();

        if (Lives <= 0)
        {
            GameManager.Instance.GameOver();    // the Update above then plays the death animation
        }
        else
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(InvincibleRoutine());
        }
    }

    IEnumerator InvincibleRoutine()
    {
        invincible = true;

        float elapsed = 0f;
        while (elapsed < invincibleTime)
        {
            sprite.enabled = !sprite.enabled;           // blink
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sprite.enabled = true;
        invincible = false;
    }

    void StartRoll()
    {
        rolling = true;
        rollTimer = rollDuration;
        anim.SetTrigger("Roll");

        float newHeight = colSize.y * rollColliderScale;
        col.size = new Vector2(colSize.x, newHeight);
        col.offset = new Vector2(colOffset.x, colOffset.y - (colSize.y - newHeight) / 2f);
    }

    void EndRoll()
    {
        rolling = false;
        col.size = colSize;
        col.offset = colOffset;
    }
}