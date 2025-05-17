using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnNodes : MonoBehaviour
{
    int numToSpawn = 26;
    public float currentSpawnOffset;

    public float spawnOffSet = 0.3f;

    void Start()
    {
        // gameObject.name = "Node";
        // currentSpawnOffset = 0;
        // return;

        // if (gameObject.name == "Node")
        // {
        //     for (int i = 0; i < numToSpawn; i++)
        //     {
        //         //Duplicar el nodo
        //         GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x + currentSpawnOffset, transform.position.y, 0), Quaternion.identity);
        //         currentSpawnOffset += spawnOffSet;
        //     }
        // }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
