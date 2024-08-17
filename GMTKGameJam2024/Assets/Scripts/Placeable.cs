using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    private new Camera camera;
    private Color startingColor;
    private bool tracking = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindAnyObjectByType<Camera>();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();

        startingColor = GetComponent<SpriteRenderer>().color;
        Color tempColor = startingColor;
        tempColor.a = 100;
        GetComponent<SpriteRenderer>().color = tempColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPoint = camera.ScreenToWorldPoint(mousePos);
            gameObject.transform.position = worldPoint;
            //Debug.Log($"X: {worldPoint.x}, Y: {worldPoint.y}, Z: {worldPoint.z}");

            if(Input.GetMouseButtonDown(0))
            {
                Place();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);
            }
        }
    }

    public void Place()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().WakeUp();
        GetComponent<SpriteRenderer>().color = startingColor;
        tracking = false;
    }

    public void RecieveCamera(Camera camera)
    {
        this.camera = camera;
        tracking = true;
    }
}
