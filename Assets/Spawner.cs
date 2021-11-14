using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject BobPrefab;
    private bool spawnedYet = false;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        while (spawnedYet == false)
        {
            SpawnEnemyNPC();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemyNPC()
    {
        if (!spawnedYet)
        {
            Vector2 randPosition = new Vector2(Random.Range(-30.0f, 8.8f), Random.Range(-19.0f, 6.0f));
            GameObject npc = Instantiate(BobPrefab);
            npc.transform.position = randPosition;

            if (!Physics2D.OverlapCircle(npc.transform.position, 2f))
            {
                Debug.Log("Nothing Touches b");
                spawnedYet = true;
            }
            else if (Physics2D.OverlapCircle(npc.transform.position, 2f))
            {
                Debug.Log("Something touches");
                Destroy(npc);
            }
            //GameObject npc = Instantiate(BobPrefab);
            //npc.transform.position = new Vector2(Random.Range(-30.0f, 8.8f), Random.Range(-19.0f, 6.0f));
        }
    }

    private bool InCollider()
    {
        Vector2 position = new Vector2(BobPrefab.transform.position.x - 0.22f, BobPrefab.transform.position.y);
        Vector2 direction = Vector2.down;
        float distance = 20f;

        //RaycastHit2D hit = Physics2D.Raycast(position, direction, distance);

        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D downRay = Physics2D.Raycast(position, Vector2.down, distance);
        Debug.DrawRay(position, direction, Color.red);
        RaycastHit2D upRay = Physics2D.Raycast(position, Vector2.up, distance);
        RaycastHit2D leftRay = Physics2D.Raycast(position, Vector2.left, distance);
        RaycastHit2D rightRay = Physics2D.Raycast(position, Vector2.right, distance);

        if (downRay.collider == null && upRay.collider == null && leftRay.collider == null && rightRay.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
