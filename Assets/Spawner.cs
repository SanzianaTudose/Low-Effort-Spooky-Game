using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Prefabs;
    private bool[] spawnedYet2 = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all prefabs as not spawned yet
        for (int i = 0; i <Prefabs.Length; i++)
        {
            spawnedYet2[i] = false;
        }

        // start spawning things and changing their status accordingly
        for (int i = 0; i < Prefabs.Length; i++)
        {
            while (spawnedYet2[i] == false)
            {
                SpawnEnemyNPC(i);
            }
        }
        
    }

    private void SpawnEnemyNPC(int i)
    {
        Vector2 randPosition = new Vector2(Random.Range(-30.0f, 8.8f), Random.Range(-19.0f, 6.0f));
        GameObject npc = Instantiate(Prefabs[i]);
        npc.transform.position = randPosition;

        if (!Physics2D.OverlapCircle(npc.transform.position, 2f))
        {
            Debug.Log("Nothing Touches b");
            spawnedYet2[i] = true;
        }
        else if (Physics2D.OverlapCircle(npc.transform.position, 2f))
        {
            Debug.Log("Something touches");
            Destroy(npc);
        }
    }
}
