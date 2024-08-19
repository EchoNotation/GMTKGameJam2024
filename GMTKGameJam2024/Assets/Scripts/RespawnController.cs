using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject activeCheckpoint = null;
    private int respawnMaterialCount = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveCheckpoint(GameObject checkpoint)
    {
        activeCheckpoint = checkpoint;
    }

    public void Spawn()
    {
        Vector3 spawnLocation;

        if(activeCheckpoint == null) {
            spawnLocation = new Vector3(-3, 0, 0);
        }
        else
        {
            spawnLocation = activeCheckpoint.transform.position + new Vector3(0, 0.1f, 0);
        }

        GameObject player = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);

        GameObject[] builtObjs = GetComponent<BlueprintHandler>().GetBuiltObjects();
        for(int i = 0; i < builtObjs.Length; i++)
        {
            Destroy(builtObjs[i]);
        }

        ButtonMechanic[] buttons = GameObject.FindObjectsOfType<ButtonMechanic>();
        for(int i = 0; i < buttons.Length; i++) buttons[i].ResetButton();

        GetComponent<BlueprintHandler>().SetMaterials(respawnMaterialCount);
        GetComponent<BlueprintHandler>().allowedToOpen = true;
        GetComponent<CameraController>().player = player;
    }
}
