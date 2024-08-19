using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawMaterials : MonoBehaviour
{
    private int value = 25;

    private Sound sound;

    public void Start()
    {
        sound = GameObject.Find("SoundManager").GetComponent<Sound>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<BlueprintHandler>().AddMaterials(value);
            sound.PlaySound(Sound.Sounds.PICKUP);
            Destroy(gameObject);
        }
    }
}
