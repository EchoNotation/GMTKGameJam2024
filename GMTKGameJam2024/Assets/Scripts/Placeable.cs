using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    private new Camera camera;
    private Color startingColor;
    private bool tracking = false;
    private int cost = 0;

    public GameObject blueprintHandler;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindAnyObjectByType<Camera>();
        blueprintHandler = GameObject.FindGameObjectWithTag("GameManager");

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        startingColor = GetComponent<SpriteRenderer>().color;
        Color tempColor = startingColor;
        tempColor.a = 0.25f;
        GetComponent<SpriteRenderer>().color = tempColor;

        //Can appear behind the player or other game elements, sorting layer change?
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
                TryPlace();
            }

            float currentAngle = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle + (22.5f * Input.mouseScrollDelta.y)));

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);
                blueprintHandler.GetComponent<BlueprintHandler>().allowedToOpen = true;
                blueprintHandler.GetComponent<BlueprintHandler>().AddMaterials(cost);
            }

        }
        
    }

    public void TryPlace()
    {
        GetComponent<Collider2D>().enabled = true;

        //This is a crash contender later
        RaycastHit2D[] res = new RaycastHit2D[10];
        GetComponent<Collider2D>().Cast(new Vector2(0, 0), res, 0);

        if(res[0].collider == null)
        {
            //Should be able to place
            GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<SpriteRenderer>().color = startingColor;
            tracking = false;
            blueprintHandler.GetComponent<BlueprintHandler>().allowedToOpen = true;
            blueprintHandler.GetComponent<BlueprintHandler>().BuiltObject(gameObject);

        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void RecieveCamera(Camera camera)
    {
        this.camera = camera;
        tracking = true;
    }

    public void Scale(int size, int cost, float mass)
    {
        switch(size)
        {
            case 0:
                gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                break;
            case 1:
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                break;
            default:
                break;
        }

        this.cost = cost;
        GetComponent<Rigidbody2D>().mass = mass;
    }
}
