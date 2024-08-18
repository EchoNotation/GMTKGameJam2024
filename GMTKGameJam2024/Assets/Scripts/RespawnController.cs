using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private GameObject activeCheckpoint = null;
    private int respawnMaterialCount = 30;

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

    public void Respawn()
    {
        if(activeCheckpoint == null) {
            Debug.Log("Failed to respawn due to lack of checkpoint");
            return;
        }

        Vector3 spawnLocation = activeCheckpoint.transform.position;
        //Instantiate()

        GetComponent<BlueprintHandler>().SetMaterials(respawnMaterialCount);
    }
}
