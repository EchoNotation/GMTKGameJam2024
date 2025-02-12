using System.Diagnostics;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[10];
    
    public enum AnimationState
    {
        STAND,
        BLINK,
        WALK1,
        WALK2,
        PRE_JUMP,
        PRE_BLINK,
        SHOCKED1,
        SHOCKED2,
        HAT
    }

    public AnimationState animState;
    bool facingLeft;
    Stopwatch timer;
    public long nextSwitch;

    public enum DeathType
    {
        FALLING,
        CRUSHED,
        ELECTROCUTED
    }

    public DeathType deathType;
    bool inDeathAnim = false;

    Rigidbody2D rb;
    private Camera cam;
    bool grounded = false;
    Vector2 box;
    bool active;
    public float walkForce = 40f;
    public float jumpForce = 25f;
    public float jumpDistance = 0.05f;
    public float maxSpeed = 6f;

    private Sound sound;
    bool justGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.Find("SoundManager").GetComponent<Sound>();

        cam = GameObject.Find("Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        active = true;
        animState = AnimationState.STAND;
        facingLeft = false;
        timer = new Stopwatch();
        timer.Start();
        nextSwitch = 0;
        box = GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y <= 0)
        {
            int layerMask = LayerMask.GetMask("Default");
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, box, 0, Vector2.down, 0.05f, layerMask);

            if(hit.collider != null)
            {
                float angle = Mathf.Rad2Deg * Mathf.Asin(Vector2.Dot(Vector2.up, hit.normal));
                if(angle > 45 && angle < 135) grounded = true;
            } 
            else grounded = false;
        }

        if(grounded && !justGrounded) sound.PlaySound(Sound.Sounds.LAND);
        justGrounded = grounded;

        if(active) CheckInputs();

        if(inDeathAnim) UpdateAnimationStateDead();
        else UpdateAnimationStateNormal();
        UpdateSprite();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - 0.05f, 0), new Vector3(box.x, box.y, 0.1f));
    }

    private void CheckInputs()
    {
        if(Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(new Vector2(-walkForce, 0) * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if(Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(new Vector2(walkForce, 0) * Time.deltaTime, ForceMode2D.Impulse);
        }

        if(Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            grounded = false;

            sound.PlaySound(Sound.Sounds.JUMP);
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 mouseLocation = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseLocation, new Vector2());
            if(hit.collider != null)
            {
                Placeable p = hit.collider.gameObject.GetComponent<Placeable>();
                if(p != null)
                {
                    p.BreakDown();
                    sound.PlaySound(Sound.Sounds.DESTRUCT);
                }
            }
        }
    }

    private void UpdateAnimationStateNormal()
    {
        bool pressingLeft = Input.GetKey(KeyCode.A);
        bool pressingRight = Input.GetKey(KeyCode.D);

        if(pressingLeft) facingLeft = true;
        else if(pressingRight) facingLeft = false;

        if(!grounded)
        {
            animState = AnimationState.STAND;
            nextSwitch = 0;
            return;
        }

        switch(animState)
        {
            case AnimationState.STAND:
                if(pressingLeft || pressingRight)
                {
                    //Start walking
                    animState = AnimationState.WALK1;
                    nextSwitch = 125;
                    timer.Restart();
                }
                else
                {
                    animState = AnimationState.PRE_BLINK;
                    nextSwitch = Random.Range(500, 2500);
                    timer.Restart();
                }
                break;
            case AnimationState.PRE_BLINK:
                if(pressingLeft || pressingRight)
                {
                    //Start walking
                    animState = AnimationState.WALK1;
                    nextSwitch = 125;
                    timer.Restart();
                }
                else if(timer.ElapsedMilliseconds > nextSwitch)
                {
                    animState = AnimationState.BLINK;
                    nextSwitch = 100;
                    timer.Restart();
                }
                break;
            case AnimationState.BLINK:
                if(pressingLeft || pressingRight)
                {
                    //Start walking
                    animState = AnimationState.WALK1;
                    nextSwitch = 125;
                    timer.Restart();
                }
                else if(timer.ElapsedMilliseconds > nextSwitch)
                {
                    animState = AnimationState.STAND;
                    nextSwitch = 0;
                    timer.Restart();
                }
                break;
            case AnimationState.WALK1:
                if(pressingLeft || pressingRight)
                {
                    if(timer.ElapsedMilliseconds > nextSwitch)
                    {
                        animState = AnimationState.WALK2;
                        nextSwitch = 125;
                        timer.Restart();
                    }
                }
                else
                {
                    animState = AnimationState.STAND;
                    nextSwitch = 0;
                    timer.Restart();
                }
                break;
            case AnimationState.WALK2:
                if(pressingLeft || pressingRight)
                {
                    if(timer.ElapsedMilliseconds > nextSwitch)
                    {
                        animState = AnimationState.WALK1;
                        nextSwitch = 125;
                        timer.Restart();
                    }
                }
                else
                {
                    animState = AnimationState.STAND;
                    nextSwitch = 0;
                    timer.Restart();
                }
                break;

        }

    }

    private void UpdateAnimationStateDead()
    {
        if(animState == AnimationState.HAT)
        {
            if(timer.ElapsedMilliseconds > nextSwitch)
            {
                Die();
                animState = AnimationState.STAND;
            } 
            return;
        }

        if(deathType == DeathType.CRUSHED)
        {
            animState = AnimationState.HAT;
            nextSwitch = 1500;
            timer.Restart();
        }
        else if(deathType == DeathType.ELECTROCUTED)
        {
            if(timer.ElapsedMilliseconds > nextSwitch)
            {
                animState = AnimationState.HAT;
                nextSwitch = 1500;
                timer.Restart();
            }
            else
            {
                if(timer.ElapsedMilliseconds % 250 > 125)
                {
                    animState = AnimationState.SHOCKED1;
                }
                else
                {
                    animState = AnimationState.SHOCKED2;
                }
            }
        }
        else if(deathType == DeathType.FALLING)
        {
            animState = AnimationState.HAT;
            nextSwitch = 1500;
            timer.Restart();
        }
    }

    private void UpdateSprite()
    {
        int spriteIndex = 0;

        switch(animState)
        {
            case AnimationState.STAND:
            case AnimationState.PRE_BLINK:
                spriteIndex = 0;
                break;
            case AnimationState.BLINK:
                spriteIndex = 1;
                break;
            case AnimationState.WALK1:
                spriteIndex = 2;
                break;
            case AnimationState.WALK2:
                spriteIndex = 3;
                break;
            case AnimationState.PRE_JUMP:
                spriteIndex = 4;
                break;
            case AnimationState.HAT:
                spriteIndex = 5;
                break;
            case AnimationState.SHOCKED1:
                spriteIndex = 6;
                break;
            case AnimationState.SHOCKED2:
                spriteIndex = 7;
                break;
        }

        GetComponent<SpriteRenderer>().sprite = sprites[facingLeft ? spriteIndex : spriteIndex + 8];
    }

    public void DisableActions()
    {
        active = false;
    }

    public void EnableActions()
    {
        active = true;
    }

    public void Die()
    {
        inDeathAnim = false;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>().PlayerDied();
        Destroy(gameObject);
    }

    private float lethalForce = 20f;
    private float irrelevantSpeedThreshold = 2f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(inDeathAnim) return;

        if(collision.collider.CompareTag("Constructed"))
        {
            Rigidbody2D incoming = collision.collider.GetComponent<Rigidbody2D>();
            Vector2 incomingVel = collision.collider.GetComponent<PhysicsHandler>().GetAverageVelocity();
            Vector2 playerVel = GetComponent<PhysicsHandler>().GetAverageVelocity();

            if((incomingVel - playerVel).magnitude < irrelevantSpeedThreshold) return;


            Vector2 resultantVel = (playerVel / 6) - incomingVel;
            float force = incoming.mass * resultantVel.magnitude;

            //UnityEngine.Debug.Log($"mass {incoming.mass} vel {resultantVel.magnitude} force {force}");
            if(Mathf.Abs(force) >= lethalForce)
            {
                deathType = DeathType.CRUSHED;
                nextSwitch = 0;
                timer.Restart();
                EnterDeathAnim();
                sound.PlaySound(Sound.Sounds.SPLAT);
            }
        }
        else if(collision.collider.CompareTag("Wiring"))
        {
            if(collision.collider.GetComponent<Wiring>().IsActivated())
            {
                deathType = DeathType.ELECTROCUTED;
                nextSwitch = 700;
                timer.Restart();
                EnterDeathAnim();
                sound.PlaySound(Sound.Sounds.SHOCK);
            }
        }
    }

    private void EnterDeathAnim()
    {
        inDeathAnim = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    public void DeathByFalling()
    {
        deathType = DeathType.FALLING;
        nextSwitch = 0;
        timer.Restart();
        EnterDeathAnim();
        sound.PlaySound(Sound.Sounds.FALL);
    }
}
