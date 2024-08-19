using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Wiring : MonoBehaviour
{
    public ButtonMechanic button;
    public Sprite[] sprites;
    private Stopwatch timer;
    private long nextSwitch;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Stopwatch();
        nextSwitch = 0;
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(button != null && button.IsActivated())
        {
            sr.sprite = sprites[0];
        }
        else if(timer.ElapsedMilliseconds > nextSwitch)
        {
            int chosen = Random.Range(0, 3);
            sr.sprite = sprites[chosen];
            
            switch(chosen)
            {
                case 0:
                    nextSwitch = Random.Range(250, 1000);
                    break;
                case 1:
                    nextSwitch = Random.Range(50, 75);
                    break;
                case 2:
                    nextSwitch = Random.Range(50, 75);
                    break;
            }

            timer.Restart();
        }
    }

    public bool IsActivated()
    {
        if(button != null && button.IsActivated()) return false;

        return true;
    }

}
