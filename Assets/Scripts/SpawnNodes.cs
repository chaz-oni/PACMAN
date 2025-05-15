using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnNodes : MonoBehaviour
{
    int numToSpawn = 25;
    public float spawnOffSet = 0.3f;
    public float currentSpawnOffset;
    void Start()
    {
        if (gameObject.name == "Node")
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                //Duplicar el nodo
                GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x + currentSpawnOffset, transform.position.y, 0), Quaternion.identity);
                currentSpawnOffset += spawnOffSet;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
