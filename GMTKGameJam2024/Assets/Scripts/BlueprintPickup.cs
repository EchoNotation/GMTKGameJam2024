using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintPickup : MonoBehaviour
{
    public int blueprintID;
    private Sound sound;

    // Start is called before the first frame update
    void Start()
    {
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
            GameObject.Find("GameManager").GetComponent<BlueprintHandler>().UnlockBlueprint(blueprintID);
            sound.PlaySound(Sound.Sounds.PICKUP);
            Destroy(gameObject);
        }
    }
}
