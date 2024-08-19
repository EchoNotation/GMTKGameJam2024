using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool activated;
    public int minCameraY = 0;

    private Sound sound;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        sound = GameObject.Find("SoundManager").GetComponent<Sound>();
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
            GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
            gm.GetComponent<CameraController>().HitCheckpoint(minCameraY);
            gm.GetComponent<RespawnController>().SetActiveCheckpoint(gameObject);

            sound.PlaySound(Sound.Sounds.DRINK);
        }
    }
}
