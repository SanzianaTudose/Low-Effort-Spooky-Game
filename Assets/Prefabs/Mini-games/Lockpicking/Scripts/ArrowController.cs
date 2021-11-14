using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    [SerializeField] float degreesPerSecond = 5f;
    [SerializeField] float rgbSpeed = 0.1f;
    [SerializeField] GameObject subsetGame;
    [SerializeField] GameObject rotationPointObj;
    [SerializeField] Image backgroundTargets;
    [SerializeField] Text countdown;
    [SerializeField] Text roundinfo;
    [SerializeField] Text instructions;
    [SerializeField] GameObject prefabBig;
    [SerializeField] GameObject prefabMedium;
    [SerializeField] GameObject prefabSmall;

    private GameObject collidedTarget;

    private bool insideTarget = false;

    private bool introSequence = true;

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

    // Taking care of "animation" for starting timer
    IEnumerator StartTimer()
    {
        while (introSequence) {
            roundinfo.text = "Round: 1/3";
            yield return new WaitForSeconds(2f);
            countdown.text = "3";
            countdown.fontSize = 200;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "3";
            countdown.fontSize = 250;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "3";
            countdown.fontSize = 300;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "2";
            countdown.fontSize = 200;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "2";
            countdown.fontSize = 250;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "2";
            countdown.fontSize = 300;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "1";
            countdown.fontSize = 200;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "1";
            countdown.fontSize = 250;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "1";
            countdown.fontSize = 300;
            yield return new WaitForSeconds(1f);
            countdown.text = "Start!";
            countdown.fontSize = 200;
            yield return new WaitForSeconds(0.5f);
            countdown.text = "";
            introSequence = false;
        }
    }

    void prepareLevel1()
    {
        /*
        For each instantiated prefab, change rotation by previously computed random angle
        Then set its name + parent and add it to the list.
        */
        
        var prefabTarget = Instantiate(prefabBig, new Vector3(0,0,100), Quaternion.Euler(0,0,l1t1));
        prefabTarget.name = $"Big Target 1";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l1prefabs.Add(prefabTarget);

        prefabTarget = Instantiate(prefabBig, new Vector3(0,0,100), Quaternion.Euler(0,0,l1t2));
        prefabTarget.name = $"Big Target 2";
        prefabTarget.transform.SetParent(subsetGame.transform, false);
        l1prefabs.Add(prefabTarget);
    }

    void prepareLevel2()
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

    void prepareLevel3()
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


    void Start()
    {
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

        //Prepare level 1
        prepareLevel1();
        
        //Intro sequence
        StartCoroutine (StartTimer ());
    }

    // Update is called once per frame
    void Update()
    {
        if (!introSequence)
        {
            if (Input.GetKeyDown("space"))
            {
                switch(roundinfo.text)
                {
                    case "Round: 1/3":
                        if (insideTarget)
                        {
                            Debug.Log("YES");
                            collidedTarget.SetActive(false);
                            l1targets -= 1;

                            if (l1targets == 0)
                            {
                                Debug.Log("completed level 1");
                                //Destroy all targets from level 1
                                foreach (var target in l1prefabs)
                                {
                                    Destroy(target);
                                }
                                //Flip direction arrow
                                degreesPerSecond *= -1;
                                //Prepare level 2
                                prepareLevel2();
                            }
                        } else {
                            Debug.Log("NO");
                            //Enable both targets
                            foreach (var target in l1prefabs)
                            {
                                target.SetActive(true);
                            }
                            l1targets = 2;
                        }
                        break;
                    case "Round: 2/3":
                        if (insideTarget)
                        {
                            Debug.Log("YES");
                            collidedTarget.SetActive(false);
                            l2targets -= 1;

                            if (l2targets == 0)
                            {
                                Debug.Log("completed level 2");
                                //Destroy all targets from level 2
                                foreach (var target in l2prefabs)
                                {
                                    Destroy(target);
                                }
                                //Flip direction arrow
                                degreesPerSecond *= -1;
                                //Prepare level 3
                                prepareLevel3();
                            }
                        } else {
                            Debug.Log("NO");
                            //Enable all targets
                            foreach (var target in l2prefabs)
                            {
                                target.SetActive(true);
                            }
                            l2targets = 3;
                        }
                        break;
                    case "Round: 3/3":
                        if (insideTarget)
                        {
                            Debug.Log("YES");
                            collidedTarget.SetActive(false);
                            l3targets -= 1;

                            if (l3targets == 0)
                            {
                                Debug.Log("completed level 3");
                                //Destroy all targets from level 3
                                foreach (var target in l3prefabs)
                                {
                                    Destroy(target);
                                }
                                //Victory screen?
                                Debug.Log("VICTORY");
                                subsetGame.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                                subsetGame.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                                subsetGame.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                subsetGame.transform.GetChild(2).gameObject.SetActive(false);
                                backgroundTargets.enabled = false;
                                roundinfo.text = "Victory";
                                roundinfo.enabled = false;
                                countdown.text = "Victory!";
                                
                            }
                        } else {
                            Debug.Log("NO");
                            //Enable all targets
                            foreach (var target in l3prefabs)
                            {
                                target.SetActive(true);
                            }
                            l3targets = 3;
                        }
                        break;
                    case "Victory":
                        /*
                        Here you can put the scene loader code to
                        transition to the next part of the game
                        */
                        Debug.Log("Now you should be transported to the next part!");
                        break;
                }
            }

            transform.RotateAround(rotationPointObj.transform.position, Vector3.back, degreesPerSecond * Time.deltaTime);

            float h, s, v;
            Color.RGBToHSV(backgroundTargets.color, out h, out s, out v);
            backgroundTargets.color = Color.HSVToRGB(h + Time.deltaTime * rgbSpeed, s, v);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        insideTarget = true;
        collidedTarget = collision.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        insideTarget = false;
    }
}