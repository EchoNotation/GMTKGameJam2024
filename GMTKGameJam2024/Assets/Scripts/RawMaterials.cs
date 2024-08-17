using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawMaterials : MonoBehaviour
{
    private int value = 25;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<BlueprintHandler>().AddMaterials(value);
            Destroy(gameObject);
        }
    }
}
