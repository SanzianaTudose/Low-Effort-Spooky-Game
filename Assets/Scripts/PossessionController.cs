using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
    [SerializeField] Sprite possessedSprite;
    [SerializeField] Sprite defaultSprite;
    List<GameObject> objectsWithinRange = new List<GameObject>();

    public GameObject spawner;

    //Make sure the ghost does not have any NPC within detection radius at start
    private int detectionCounter = 0;
    private bool ableToPosses = false;
    private bool cleanUp = false;
    private bool possessing = false;
    private bool needFirstHighlight = true;
    private GameObject highlightClosest;
    private GameObject lastPossessed;

    private bool spawnedAgain = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCounter != 0){
            ableToPosses = true;
        }
        else {
            if (cleanUp){
                cleanUp = false;
                ableToPosses = false;
                objectsWithinRange.Clear();
            }
        }

        /*
        1. We select the closest "collision"
        2. We disable the sprite of our player
        3. We override the transform of our player with the chosen collision
        4. We override the sprite of our player with the chosen collision's sprite + eyes
        5. We enable the sprite of our player
        6. We disable the npc of our chosen collision
        */

        if (ableToPosses){
            /*
            is var mask equal to false then mask closest -> highlight
            if var mask is not same as current closest, mask closest -> highlight moved 
            */
            if (needFirstHighlight) {

                //Store the first target as the closest highlight
                highlightClosest = GetClosestTarget(objectsWithinRange);

                //Only do this when are not possessing someone
                if (!possessing)
                {
                    //Enable highlight on this object (added sprite to npc (disabled on default))
                    Debug.Log($"Enable highlight for {highlightClosest.name}");
                    highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

                //Set our need for a first highlight to false
                needFirstHighlight = false;
            }
            else if (highlightClosest != GetClosestTarget(objectsWithinRange)){

                //Only do this when are not possessing someone
                if (!possessing)
                {
                    //Disable highlight on old object and enable on new one
                    Debug.Log($"Disable highlight for {highlightClosest.name}");
                    Debug.Log($"Enable highlight for {GetClosestTarget(objectsWithinRange).name}");

                    highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    GetClosestTarget(objectsWithinRange).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

                //Update the current highlightClosest
                highlightClosest = GetClosestTarget(objectsWithinRange);
            }
            
            if (Input.GetKeyDown("e") && !possessing)
            {
                /*
                Once we posses someone we want to still
                keep track of everything but not give the
                player the chance to highlight or posses
                anyone else.

                1. Set possessing to true
                2. Disable player's sprite
                3. Disable AI of npc
                4. Move player to location of chosen target
                5. Copy target's sprite (possessed version)
                6. Disable sprite of npc
                7. Change sprite to possessed sprite
                8. Disable current highlight
                9. Store the target as lastPossessed
                */

                possessing = true;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                highlightClosest.GetComponent<NPCMovement>().enabled = false;

                highlightClosest.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.transform.position = highlightClosest.gameObject.transform.position;

                /*
                This is a temporary solution!
                A possible way of implementing sprite specific stuff
                is making use of a switch case based on a property of the target
                for example: public int variable.
                */
                gameObject.GetComponent<SpriteRenderer>().sprite = possessedSprite;

                highlightClosest.GetComponent<SpriteRenderer>().enabled = false;

                gameObject.GetComponent<SpriteRenderer>().enabled = true;

                highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

                lastPossessed = highlightClosest;

                //Show the name of the target we will posses
                Debug.Log($"Posses -> {GetClosestTarget(objectsWithinRange).name}");
            }
        }

        //Escape the current possession
        if (Input.GetKeyDown("q") && possessing)
        {
            spawnedAgain = false;
            /*
            1. Set possessing to false
            2. Move lastPossessed npc to player's location
            3. Enable lastPossessed npc's sprite
            4. Change back to the default sprite
            5. Enable the highlight for the current closest target if ableToPosses
            6. Enable lastPossessed npc's AI
            */

            while (!spawnedAgain)
            {
                Vector2 randPosition = new Vector2(Random.Range(-30.0f, 8.8f), Random.Range(-19.0f, 6.0f));
                lastPossessed.transform.position = randPosition;

                if (!Physics2D.OverlapCircle(lastPossessed.transform.position, 1f))
                {
                    Debug.Log("Nothing Touches b");
                    spawnedAgain = true;
                }
                else if (Physics2D.OverlapCircle(lastPossessed.transform.position, 1f))
                {
                    Debug.Log("Something touches");
                }
            }

            possessing = false;

            // change the position to a random place
            //lastPossessed.transform.position = gameObject.transform.position;

            lastPossessed.GetComponent<BoxCollider2D>().enabled = true;

            lastPossessed.GetComponent<NPCMovement>().enabled = true;

            lastPossessed.GetComponent<SpriteRenderer>().enabled = true;

            gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;


            
            if (ableToPosses) {
                highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            }

            lastPossessed.GetComponent<NPCMovement>().enabled = true;

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CanPosses")
        {
            detectionCounter += 1;
            //Debugging (check current counter)
            //Debug.Log($"Counter: {detectionCounter}");

            //Add the gameobject to our list
            if(!objectsWithinRange.Contains(collision.gameObject)) {
                objectsWithinRange.Add(collision.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CanPosses")
        {
            detectionCounter -= 1;
            //Debugging (check current counter)
            //Debug.Log($"Counter: {detectionCounter}");

            if (detectionCounter == 0){

                //We need to clean up our list of close range targets
                cleanUp = true;

                //Only do this when are not possessing someone
                if (!possessing)
                {
                    //Disable highlight on previous object
                    Debug.Log($"Disable highlight for {highlightClosest.name}");
                    highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }

                //We need to reset our need for a first highlight
                needFirstHighlight = true;
            }
        }
    }

    GameObject GetClosestTarget(List<GameObject> targets){
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in targets)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
