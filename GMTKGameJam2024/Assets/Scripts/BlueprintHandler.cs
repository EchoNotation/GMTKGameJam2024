using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentSize = 0;
    private List<Blueprint> blueprints;

    public static int totalBlueprints = 5;

    public Camera mainCam;
    public GameObject[] sizeButtons = new GameObject[3];
    public Button[] blueprintButtons = new Button[5];
    public Blueprint[] blueprintDictionary = new Blueprint[totalBlueprints];

    public bool allowedToOpen = true;

    void Start()
    {
        blueprints = new List<Blueprint>();

        UnlockBlueprint(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentSize(int newSize)
    {
        sizeButtons[currentSize].GetComponent<Image>().color = UnityEngine.Color.white;
        currentSize = newSize;
        sizeButtons[currentSize].GetComponent<Image>().color = UnityEngine.Color.green;
    }

    public void UnlockBlueprint(int id)
    {
        blueprints.Add(blueprintDictionary[id]);
    }

    public void OpenBlueprint(int position)
    {
        if(!allowedToOpen) return;

        if(position >= blueprints.Count) return;
        //Size changing magic

        GameObject temp = Instantiate(blueprints[position].prefab);
        temp.GetComponent<Placeable>().RecieveCamera(mainCam);
        allowedToOpen = false;
    }
}
