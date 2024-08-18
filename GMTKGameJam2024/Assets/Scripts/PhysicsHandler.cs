using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    private Vector2 velocity1, velocity2, velocity3;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity1 = rb.velocity;
        velocity2 = velocity1;
        velocity3 = velocity1;
    }

    // Update is called once per frame
    void Update()
    {
        velocity3 = velocity2;
        velocity2 = velocity1;
        velocity1 = rb.velocity;
    }

    public Vector2 GetAverageVelocity()
    {
        return (velocity1 + velocity2 + velocity3) / 3;
    }
}
