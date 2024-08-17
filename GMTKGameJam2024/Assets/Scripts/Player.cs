using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    bool grounded = false;
    public float walkForce = 1.5f;
    public float jumpForce = 25f;
    public float jumpDistance = 0.55f;
    public float maxSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1);
            if(hit.collider != null && hit.distance < jumpDistance) grounded = true;
        }

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
    }
}
