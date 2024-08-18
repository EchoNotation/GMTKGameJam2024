using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    private Camera cam;
    bool grounded = false;
    bool active;
    public float walkForce = 1.5f;
    public float jumpForce = 25f;
    public float jumpDistance = 0.05f;
    public float maxSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y <= 0)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down);
            if(hit.collider != null && hit.distance < jumpDistance) grounded = true;
            else grounded = false;
        }

        if(active) CheckInputs();
    }

    private void CheckInputs()
    {
        if(Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(new Vector2(-walkForce, 0));
        }
        else if(Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(new Vector2(walkForce, 0));
        }

        if(Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mouseLocation = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseLocation, new Vector2());
            if(hit.collider != null)
            {
                Placeable p = hit.collider.gameObject.GetComponent<Placeable>();
                if(p != null) p.BreakDown();
            }
        }
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
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>().PlayerDied();
        Destroy(gameObject);
    }
}
