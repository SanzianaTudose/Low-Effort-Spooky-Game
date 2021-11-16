using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ArrowController : Minigame
{
    [SerializeField] GameObject subsetGame;
    [SerializeField] GameObject rotationPointObj;
    [SerializeField] Image backgroundTargets;
    [SerializeField] Text countdown;
    [SerializeField] Text lives;
    [SerializeField] Text roundinfo;
    [SerializeField] TextMeshProUGUI bottomText;
    [SerializeField] GameObject prefabBig;
    [SerializeField] GameObject prefabMedium;
    [SerializeField] GameObject prefabSmall;
    [SerializeField] private GameObject overlayPanelLocal;
    [SerializeField] private GameObject gameContainer;

    [Header("Game Properties")]
    [SerializeField] int startGameWithLevel = 1;
    [SerializeField] float degreesPerSecond = 5f;
    [SerializeField] float rgbSpeed = 0.1f;

    private GameObject collidedTarget;

    private bool insideTarget = false;
    private int liveCount;

    //Integers that keep track of the amount of targets per level
    private int l1targets = 2;
    private int l2targets = 3;
    private int l3targets = 3;

    //level randomization variables for the levels
    private int l1t1;
    private int l1t2;
    private int l2t1;
    private int l2t2;
    private int l2t3;
    private int l3t1;
    private int l3t2;
    private int l3t3;

    //Lists containing all prefabs loaded in for each level
    private List<GameObject> l1prefabs = new List<GameObject>();
    private List<GameObject> l2prefabs = new List<GameObject>();
    private List<GameObject> l3prefabs = new List<GameObject>();

    public override void Update()
    {
        base.Update();

        HandleGameStates();
    }

    #region Initialization
    private void prepareLevel1()
    {
        /*
        For each instantiated prefab, change rotation by previously computed random angle
        Then set its name + parent and add it to the list.
        */
        
        
        lives.text = $"Lives: {liveCount}";
        countdown.text = "";
        roundinfo.text = "Round: 1/3";

        //Enable all basic components
        gameContainer.SetActive(true);
        overlayPanelLocal.SetActive(true);
        lives.enabled = true;
        countdown.enabled = true;
        roundinfo.enabled = true;
        subsetGame.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
        subsetGame.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
        subsetGame.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        subsetGame.transform.GetChild(2).gameObject.SetActive(true);
        backgroundTargets.enabled = true;

        var prefabTarget = Instantiate(prefabBig, new Vector3(0,0,100), Quaternion.Euler(0,0,l1t1));
        prefabTarget.name = $"Big Target 1";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l1prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabBig, new Vector3(0,0,100), Quaternion.Euler(0,0,l1t2));
        prefabTarget.name = $"Big Target 2";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l1prefabs.Add(prefabTarget);
    }

    private void prepareLevel2()
    {
        roundinfo.text = "Round: 2/3";

        var prefabTarget = Instantiate(prefabMedium, new Vector3(0,0,100), Quaternion.Euler(0,0,l2t1));
        prefabTarget.name = $"Medium Target 1";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l2prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabMedium, new Vector3(0,0,100), Quaternion.Euler(0,0,l2t2));
        prefabTarget.name = $"Medium Target 2";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l2prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabMedium, new Vector3(0,0,100), Quaternion.Euler(0,0,l2t3));
        prefabTarget.name = $"Medium Target 3";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l2prefabs.Add(prefabTarget);
    }

    private void prepareLevel3()
    {
        roundinfo.text = "Round: 3/3";

        var prefabTarget = Instantiate(prefabSmall, new Vector3(0,0,100), Quaternion.Euler(0,0,l3t1));
        prefabTarget.name = $"Small Target 1";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l3prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabSmall, new Vector3(0,0,100), Quaternion.Euler(0,0,l3t2));
        prefabTarget.name = $"Small Target 2";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l3prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabSmall, new Vector3(0,0,100), Quaternion.Euler(0,0,l3t3));
        prefabTarget.name = $"Small Target 3";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l3prefabs.Add(prefabTarget);
    }

    public override void StartGame() {
        base.StartGame();
        liveCount = 1;
        lives.text = $"Lives: {liveCount}";
        countdown.text = "";
        roundinfo.text = "Round: 1/3";
        bottomText.enabled = true;

        //Enable all basic components
        gameContainer.SetActive(true);
        overlayPanelLocal.SetActive(true);
        lives.enabled = true;
        countdown.enabled = true;
        roundinfo.enabled = false;
        subsetGame.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
        subsetGame.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
        subsetGame.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        subsetGame.transform.GetChild(2).gameObject.SetActive(true);
        backgroundTargets.enabled = true;

        //Set default color of background circle
        backgroundTargets.color = Color.HSVToRGB(.34f, .84f, .67f);

        //Generate random positions for level 1 (angle of 45 degrees)
        l1t1 = Random.Range(1, 361);
        l1t2 = (l1t1 + Random.Range((45+1), (360-(45+1)))) % 360;
        Debug.Log($"Level 1: {l1t1} | {l1t2}");

        //Generate random positions for level 2 (angle of 22.5 (23) degrees)
        l2t1 = Random.Range(1, 361);
        l2t2 = (l2t1 + Random.Range((23+1), (180-(23+1)))) % 360;
        l2t3 = (l2t2 + Random.Range((23+1), (180-(23+1)))) % 360;
        Debug.Log($"Level 2: {l2t1} | {l2t2} | {l2t3}");

        //Generate random positions for level 3 (angle of 11 degrees)
        l3t1 = Random.Range(1, 361);
        l3t2 = (l3t1 + Random.Range((11+1), (180-(11+1)))) % 360;
        l3t3 = (l3t2 + Random.Range((11+1), (180-(11+1)))) % 360;
        Debug.Log($"Level 3: {l3t1} | {l3t2} | {l3t3}");

        //Start with level 3 (play a single round)
        switch(startGameWithLevel)
        {
            case (2):
                prepareLevel2();
                break;
            case (3):
                prepareLevel3();
                break;
            default:
                prepareLevel1();
                break;
        }
    }

    #endregion Initialization

    // Update is called once per frame
    private void HandleGameStates()
    {
        //Decide what happens if you hit or miss
        if (Input.GetKeyDown("space")) { LevelManager(roundinfo.text); }

        //Keep rotating the arrow around its center point
        transform.RotateAround(rotationPointObj.transform.position, Vector3.back, degreesPerSecond * Time.deltaTime);

        //Keep shifting the hue of the background ring
        float h, s, v;
        Color.RGBToHSV(backgroundTargets.color, out h, out s, out v);
        backgroundTargets.color = Color.HSVToRGB(h + Time.deltaTime * rgbSpeed, s, v);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        insideTarget = true;
        collidedTarget = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        insideTarget = false;
    }

    private void LevelManager(string category)
    {
        if (category == "Round: 1/3")
        {
            if (insideTarget)
            {
                Debug.Log("YES");
                collidedTarget.SetActive(false);
                l1targets -= 1;

                if (l1targets == 0)
                {
                    Debug.Log("completed level 1");
                    //Destroy all targets from level 1
                    ModifyTargets(l1prefabs, true);
                    //Flip direction arrow
                    degreesPerSecond *= -1;
                    //Prepare level 2
                    prepareLevel2();
                }
            } else {
                Debug.Log("NO");
                //Enable both targets
                ModifyTargets(l1prefabs, false);
                //Reset the total amount
                l1targets = 2;
                //Decrease the liveCount by 1
                liveCount -= 1;
                //Check if you lost
                if (liveCount == 0)
                {
                    Debug.Log("Loss");
                    ModifyTargets(l1prefabs, true);
                    CustomEndGame(false);
                }
                else
                {
                    lives.text = $"Lives: {liveCount}";
                }
            }
        }
        else if (category == "Round: 2/3")
        {
            if (insideTarget)
            {
                Debug.Log("YES");
                collidedTarget.SetActive(false);
                l2targets -= 1;

                if (l2targets == 0)
                {
                    Debug.Log("completed level 2");
                    //Destroy all targets from level 2
                    ModifyTargets(l2prefabs, true);
                    //Flip direction arrow
                    degreesPerSecond *= -1;
                    //Prepare level 3
                    prepareLevel3();
                }
            } else {
                Debug.Log("NO");
                //Enable all targets
                ModifyTargets(l2prefabs, false);
                //Reset the total amount
                l2targets = 3;
                //Decrease the liveCount by 1
                liveCount -= 1;
                //Check if you lost
                if (liveCount == 0)
                {
                    Debug.Log("Loss");
                    ModifyTargets(l2prefabs, true);
                    CustomEndGame(false);
                }
                else
                {
                    lives.text = $"Lives: {liveCount}";
                }
            }
        }
        else if (category == "Round: 3/3")
        {
            if (insideTarget)
            {
                Debug.Log("YES");
                collidedTarget.SetActive(false);
                l3targets -= 1;

                if (l3targets == 0)
                {
                    Debug.Log("completed level 3");
                    CustomEndGame(true);
                    
                }
            } else {
                Debug.Log("NO");
                //Enable all targets
                ModifyTargets(l3prefabs, false);
                //Reset the total amount
                l3targets = 3;
                //Decrease the liveCount by 1
                liveCount -= 1;
                //Check if you lost
                if (liveCount == 0)
                {
                    Debug.Log("Loss");
                    CustomEndGame(false);
                }
                else
                {
                    lives.text = $"Lives: {liveCount}";
                }
            }
        }
    }

    private void ModifyTargets(List<GameObject> prefabList, bool kill)
    {
        foreach (var target in prefabList)
        {
            if (kill) { Destroy(target); } else { target.SetActive(true); }
        }

        if (kill)
            prefabList.Clear();
    }

    public void CustomEndGame(bool wincon)
    {
        
        //Destroy all targets from level 3
        ModifyTargets(l3prefabs, true);
        l3targets = 3;
        //Disable all basic components
        subsetGame.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        subsetGame.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
        subsetGame.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        subsetGame.transform.GetChild(2).gameObject.SetActive(false);
        backgroundTargets.enabled = false;
        bottomText.enabled = false;
        roundinfo.enabled = false;
        lives.enabled = false;
        //Let the script know the minigame is not running
        minigameRunning = false;

        if (wincon)
        {
            countdown.text = "Victory!";
            minigameState = 1;
            StartCoroutine(DisableMinigameAfterSeconds(2f));
        }
        else
        {
            countdown.text = "Loss!";
            minigameState = 0;
            StartCoroutine(DisableMinigameAfterSeconds(2f));
        }
    }

    IEnumerator DisableMinigameAfterSeconds(float sec) {
        yield return new WaitForSeconds(sec);
        gameContainer.SetActive(false);
        overlayPanelLocal.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}