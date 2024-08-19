using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public enum Sounds
    {
        BLUEPRINT_OPEN = 0,
        CONSTRUCT = 1,
        DESTRUCT = 2,
        DRINK = 3,
        FALL = 4,
        SPLAT = 5,
        SHOCK = 6,
        JUMP = 7,
        LAND = 8,
        PICKUP = 9,
        POP = 10,
        BUTTON = 11,
        ERROR = 12
    }

    public AudioSource[] sources;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(Sounds sound)
    {
        sources[(int) sound].Stop();
        sources[(int) sound].Play();
    }
}
