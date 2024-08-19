using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentSize = 0;
    private Blueprint[] blueprints;
    private int materialsCount = 50;

    private List<GameObject> objectsBuilt;

    public static int totalBlueprints = 5;

    public Camera mainCam;
    public GameObject[] sizeButtons = new GameObject[3];
    public Button[] blueprintButtons = new Button[5];
    public Image[] childImages = new Image[5];
    public Blueprint[] blueprintDictionary = new Blueprint[totalBlueprints];
    public TMP_Text materialsText;
    public GameObject panel;

    public bool allowedToOpen = true;

    void Start()
    {
        objectsBuilt = new List<GameObject>();
        blueprints = new Blueprint[totalBlueprints];

        for(int i = 0; i < totalBlueprints; i++)
        {
            blueprintButtons[i].enabled = false;
            blueprintButtons[i].GetComponent<Image>().enabled = false;
            childImages[i].enabled = false;
        }

        /*UnlockBlueprint(0);
        UnlockBlueprint(1);
        UnlockBlueprint(2);
        UnlockBlueprint(3);
        UnlockBlueprint(4);*/

        SetCurrentSize(0);
        UpdateMaterialsCount();
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
        panel.SetActive(true);
        blueprints[id] = blueprintDictionary[id];
        blueprintButtons[id].enabled = true;
        blueprintButtons[id].GetComponent<Image>().enabled = true;
        childImages[id].enabled = true;
    }

    private int[] costs = { 1, 5, 10 };
    private float[] masses = { 1, 2.5f, 5 };

    public void OpenBlueprint(int position)
    {
        if(!allowedToOpen) return;

        if(blueprints[position] == null) return;

        if(costs[currentSize] > materialsCount) return;

        GameObject temp = Instantiate(blueprints[position].prefab);
        temp.GetComponent<Placeable>().RecieveCamera(mainCam);
        temp.GetComponent<Placeable>().Scale(currentSize, costs[currentSize], masses[currentSize]);
        materialsCount -= costs[currentSize];
        UpdateMaterialsCount();
        allowedToOpen = false;
        BuiltObject(temp);
    }

    public void SetMaterials(int amount)
    {
        materialsCount = amount;
        UpdateMaterialsCount();
    }

    public void AddMaterials(int amount)
    {
        materialsCount += amount;
        UpdateMaterialsCount();
    }

    private void UpdateMaterialsCount()
    {
        materialsText.text = $"x{materialsCount}";
    }

    public void BuiltObject(GameObject obj)
    {
        objectsBuilt.Add(obj);
    }

    public void RemovedObject(GameObject obj)
    {
        objectsBuilt.Remove(obj);
    }

    public GameObject[] GetBuiltObjects()
    {
        return objectsBuilt.ToArray();
    }
}
