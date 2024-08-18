using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CamMode
    {
        NORMAL,
        SLEWING,
        DEATH_SLEW
    }

    public GameObject player;
    public GameObject deathEdge;
    public Camera mainCam;

    private List<GameObject> allGirders;

    private CamMode mode;

    private float cameraY = 0f;
    private float minCameraY;
    public float pushHeight = 2f;
    private float cameraHeight;

    private float slewStartY;
    private float progress = 0f;
    private float slewSpeed = 1f;

    //deathDistance needs to be taller than the player
    private float deathDistance = 4f;

    // Start is called before the first frame update
    void Start()
    {
        mode = CamMode.NORMAL;
        mainCam.transform.position = new Vector3(0, cameraY, -10);
        minCameraY = cameraY;
        cameraHeight = mainCam.orthographicSize;
        deathEdge.transform.position = new Vector3(0, minCameraY - (cameraHeight + deathDistance), 0);

        allGirders = new List<GameObject>();
        GameObject[] girderArr = GameObject.FindGameObjectsWithTag("Girder");
        for(int i = 0; i < girderArr.Length; i++)
        {
            allGirders.Add(girderArr[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == CamMode.NORMAL)
        {
            float playerY = player.transform.position.y;

            if(playerY > minCameraY + pushHeight)
            {
                cameraY = playerY - pushHeight;
            }
            else {
                cameraY = minCameraY; 
            }
        }
        else if(mode == CamMode.SLEWING)
        {
            if(cameraY >= minCameraY)
            {
                mode = CamMode.NORMAL;
                player.GetComponent<Player>().EnableActions();
                DestroyPastGirders(minCameraY - cameraHeight);
            }
            else
            {
                cameraY = Mathf.Lerp(slewStartY, minCameraY, progress);
                progress += slewSpeed * Time.deltaTime;
            }
        }
        else if(mode == CamMode.DEATH_SLEW)
        {
            if(cameraY <= minCameraY)
            {
                GetComponent<RespawnController>().Spawn();
                mode = CamMode.NORMAL;
            }
            else
            {
                cameraY = Mathf.Lerp(slewStartY, minCameraY, progress);
                progress += slewSpeed * Time.deltaTime;
            }
        }

        mainCam.transform.position = new Vector3(0, cameraY, -10);
    }

    public void HitCheckpoint(int newMinCameraY)
    {
        player.GetComponent<Player>().DisableActions();

        //TODO add some sort of delay here?
        mode = CamMode.SLEWING;
        minCameraY = newMinCameraY;
        slewStartY = cameraY;
        progress = 0;
        deathEdge.transform.position = new Vector3(0, minCameraY - (cameraHeight + deathDistance), 0);
    }

    public void PlayerDied()
    {
        //TODO add some sort of delay here?
        mode = CamMode.DEATH_SLEW;
        progress = 0;
        slewStartY = cameraY;
    }

    public void DestroyPastGirders(float yCutoff)
    {
        List<GameObject> girdersToRemove = new List<GameObject>();

        foreach(GameObject girder in allGirders)
        {
            if(girder.transform.position.y < yCutoff)
            {
                girdersToRemove.Add(girder);
            }
        }

        foreach(GameObject girder in girdersToRemove)
        {
            allGirders.Remove(girder);
            Destroy(girder);
        }
    }

}
