using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool activated;
    public int minCameraY = 0;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(activated) return;

            activated = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>().HitCheckpoint(minCameraY);
        }
    }
}
