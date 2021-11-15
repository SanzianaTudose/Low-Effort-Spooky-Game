using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PossessionController : MonoBehaviour
{
    [SerializeField] MinigameController minigameController;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Sprite possessedSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] GameObject prefabDoorPopup;
    [SerializeField] GameObject gridFolder;
    [SerializeField] PlaytimeScript playtimescript;
    List<GameObject> objectsWithinRange = new List<GameObject>();

    public GameObject spawner;

    //Make sure the ghost does not have any NPC within detection radius at start
    private int detectionCounter = 0;
    private int doorDetectionCounter = 0;
    private bool ableToPosses = false;
    private bool cleanUp = false;
    private bool possessing = false;
    private bool needFirstHighlight = true;
    private GameObject highlightClosest;
    private GameObject lastPossessed;
    private GameObject chosenDoorPopup;

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
                   // Debug.Log($"Enable highlight for {highlightClosest.name}");
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
                   // Debug.Log($"Disable highlight for {highlightClosest.name}");
                    //Debug.Log($"Enable highlight for {GetClosestTarget(objectsWithinRange).name}");

                    highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    GetClosestTarget(objectsWithinRange).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }

                //Update the current highlightClosest
                highlightClosest = GetClosestTarget(objectsWithinRange);
            }

            if (Input.GetKeyDown("e") && !possessing)
            {
                //Make sure we can't cheat or get time deducted while in a minigame
                playtimescript.pauseDisabled = true;
                playtimescript.gamePaused = true;
                //Trigger the minigame
                TriggerMinigame();
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

            /* 
               I check if the NPC's collider touches some other collider
               If it does, generate a new random location and check again
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

        //Action while we are possessing someone
        if (Input.GetKeyDown("e") && possessing)
        {
            /*
            Once we have possessed someone and want to perform an action (e)
            We can check if we have collided with the housedoor collider
            If so we can start trick or treat function
            */

            if (doorDetectionCounter == 2)
                playtimescript.OnHouseInteraction();
        }
    }

    void TriggerMinigame() {
        playtimescript.getInput = false;

        NPCMinigame npcMinigame = highlightClosest.GetComponent<NPCMinigame>();
        if (npcMinigame == null || npcMinigame.minigame == null) { // NPC doesn't have an NPCMinigame component or minigame at all
            StartPossession(); // just posses without any minigame
            return;
        }


        // Disable player movement and NPC Movement
        GetComponent<PlayerMovement>().enabled = false;
        highlightClosest.GetComponent<NPCMovement>().enabled = false;
        // Set pause animation for the NPC
        highlightClosest.GetComponent<NPCMovement>().PauseAnimation();

        Minigame minigame = npcMinigame.minigame;
        minigameController.startIntro(minigame);
        StartCoroutine(WaitForMinigameEnd(minigame));

    }

    IEnumerator WaitForMinigameEnd(Minigame minigame) {
        while (minigameController.countdownRunning || minigame.minigameRunning)
            yield return null;

        playtimescript.getInput = true;

        if (minigame.minigameState == 1) {
            StartCoroutine(StartPossessionAfterSeconds(2.5f));
        } else {
            // Enable player movement and NPC Movement
            GetComponent<PlayerMovement>().enabled = true;
            highlightClosest.GetComponent<NPCMovement>().enabled = true;
            StartCoroutine(EnableTimerAfterSeconds(2f));
        }
    }

    IEnumerator StartPossessionAfterSeconds(float sec) {
        yield return new WaitForSeconds(sec);
        // Enable player movement
        GetComponent<PlayerMovement>().enabled = true;

        StartPossession();
        //Enable pause and timer again
        playtimescript.pauseDisabled = false;
        playtimescript.gamePaused = false;
    }

    IEnumerator EnableTimerAfterSeconds(float sec) {
        yield return new WaitForSeconds(sec);
        // Enable pause and timer again
        playtimescript.pauseDisabled = false;
        playtimescript.gamePaused = false;
    }

    void StartPossession() {
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

        //highlightClosest.GetComponent<NPCMovement>().enabled = false;

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
        //Debug.Log($"Posses -> {GetClosestTarget(objectsWithinRange).name}");
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
        else if (collision.gameObject.tag == "HouseDoor") 
        {
            //Debug.Log("ENTER DOOR?");
            //Set out door detection boolean to true
            doorDetectionCounter += 1;

            if (doorDetectionCounter == 2 && possessing)
            {
                //Position of collision
                var own_location = transform.TransformPoint(collision.transform.position);

                bool firstTileSeen = false;
                float closestDistance = 100000;
                Vector3 closestTilePos = new Vector3();

                //Figure out the closest Tile Position
                foreach (var position in tilemap.cellBounds.allPositionsWithin)
                {
                    if (tilemap.HasTile(position))
                    {
                        if (!firstTileSeen){
                            firstTileSeen = true;
                            closestDistance = Vector3.Distance(position, own_location);
                            closestTilePos = position;
                        } else {
                            if (Vector3.Distance(position, own_location) < closestDistance){
                                closestDistance = Vector3.Distance(position, own_location);
                                closestTilePos = position;
                            }
                        }
                    }
                }

                Vector3 popupPos = new Vector3(closestTilePos.x+0.5f, closestTilePos.y+1.7f, closestTilePos.z);
                
                chosenDoorPopup = Instantiate(prefabDoorPopup, popupPos, Quaternion.identity);
                chosenDoorPopup.name = $"Popup Door";
                chosenDoorPopup.transform.parent = gridFolder.transform;

                
            }
            //Add gameobject to our list (so we can track which one we are close to)
            //Still to be implemented
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
                   // Debug.Log($"Disable highlight for {highlightClosest.name}");
                    highlightClosest.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }

                //We need to reset our need for a first highlight
                needFirstHighlight = true;
            }
        }
        else if (collision.gameObject.tag == "HouseDoor") 
        {
            //Debug.Log("LEAVE DOOR?");
            //Keep track of doors detected with our 2 colliders
            doorDetectionCounter -= 1;
            //remove gameobject to our list (so we can track which one we are close to)
            //Still to be implemented
            if (doorDetectionCounter == 1)
            {
                Destroy(chosenDoorPopup);
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
