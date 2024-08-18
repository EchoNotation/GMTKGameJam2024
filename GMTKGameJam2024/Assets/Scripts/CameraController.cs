using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CamMode
    {
        NORMAL,
        SLEWING
    }

    public GameObject player;
    public Camera mainCam;

    private CamMode mode;

    private float cameraY = 0f;
    private float minCameraY;
    public float pushHeight = 2f;
    private float cameraHeight;

    private float slewStartY;
    private float progress = 0f;
    private float slewSpeed = 1f;

    //deathDistance needs to be taller than the player
    private float deathDistance = 1f;
    private float deathEdge;

    // Start is called before the first frame update
    void Start()
    {
        mode = CamMode.NORMAL;
        mainCam.transform.position = new Vector3(0, cameraY, -10);
        minCameraY = cameraY;
        deathEdge = minCameraY - deathDistance;
        cameraHeight = mainCam.orthographicSize;
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

            if(cameraY < minCameraY) cameraY = minCameraY;
        }
        else if(mode == CamMode.SLEWING)
        {
            if(cameraY >= minCameraY)
            {
                mode = CamMode.NORMAL;
                player.GetComponent<Player>().EnableActions();
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
    }

}
