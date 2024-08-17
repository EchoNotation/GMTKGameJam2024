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
    private List<Blueprint> blueprints;
    private int materialsCount = 30;

    public static int totalBlueprints = 5;

    public Camera mainCam;
    public GameObject[] sizeButtons = new GameObject[3];
    public Button[] blueprintButtons = new Button[5];
    public Blueprint[] blueprintDictionary = new Blueprint[totalBlueprints];
    public TMP_Text materialsText;

    public bool allowedToOpen = true;

    void Start()
    {
        blueprints = new List<Blueprint>();

        for(int i = 0; i < totalBlueprints; i++)
        {
            blueprintButtons[i].enabled = false;
            blueprintButtons[i].GetComponent<Image>().enabled = false;
        }

        UnlockBlueprint(0);
        UnlockBlueprint(1);
        UnlockBlueprint(2);
        UnlockBlueprint(3);
        UnlockBlueprint(4);

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
        blueprints.Add(blueprintDictionary[id]);
        blueprintButtons[id].enabled = true;
        blueprintButtons[id].GetComponent<Image>().enabled = true;
    }

    private int[] costs = { 1, 5, 10 };

    public void OpenBlueprint(int position)
    {
        if(!allowedToOpen) return;

        if(position >= blueprints.Count) return;

        if(costs[currentSize] > materialsCount) return;

        GameObject temp = Instantiate(blueprints[position].prefab);
        temp.GetComponent<Placeable>().RecieveCamera(mainCam);
        temp.GetComponent<Placeable>().Scale(currentSize, costs[currentSize]);
        materialsCount -= costs[currentSize];
        UpdateMaterialsCount();
        allowedToOpen = false;
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
}
