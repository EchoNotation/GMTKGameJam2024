using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEdge : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().Die();
        }
        else if(collision.CompareTag("Constructed"))
        {
            Destroy(collision.gameObject);
        }
    }
}
