using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMechanic : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer sr;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetButton()
    {
        activated = false;
        sr.sprite = sprites[0];
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Constructed") || collision.CompareTag("Player"))
        {
            activated = true;
            sr.sprite = sprites[1];
        }
    }
}
