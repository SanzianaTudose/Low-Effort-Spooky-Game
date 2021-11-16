using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaysGameManager : Minigame
{
    [SerializeField] private Image dot1;
    [SerializeField] private Image dot2;
    [SerializeField] private Image dot3;
    [SerializeField] private Image dot4;
    [SerializeField] private Image dot5;
    [SerializeField] private GameObject keypadColors;
    [SerializeField] private GameObject topColors;
    [SerializeField] private Text endText;

    [Header("Game Properties")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color button1Color;
    [SerializeField] private Color button2Color;
    [SerializeField] private Color button3Color;
    [SerializeField] private Color button4Color;
    [SerializeField] private Color button5Color;
    [SerializeField] private Color button6Color;
    [SerializeField] private float waitingTimeSolution = 1f;

    private List<Image> dotList;
    private List<int> solution;
    private List<int> input;
    private bool solutionSequence;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // Start is called before the first frame update
    public override void StartGame()
    {
        base.StartGame();

        //Clear +disable end text and enable basic components
        endText.text = "";
        endText.enabled = false;
        keypadColors.SetActive(true);
        topColors.SetActive(true);

        //Create the dot list
        dotList = new List<Image>{dot1,dot2,dot3,dot4,dot5};

        //Set the colors of the dots
        ResetDots();

        //Set the default colors of the buttons
        for (int j = 1; j <= 6; j++)
        {
            keypadColors.transform.GetChild(j-1).gameObject.GetComponent<Image>().color = GetColor(0);
        }

        //Create empty input list
        input = new List<int>();

        //Generate random solution
        solution = new List<int>{rc(),rc(),rc(),rc(),rc()};

        //Visualize the solution
        solutionSequence = true;
        StartCoroutine(ShowSolution());
    }

    IEnumerator ShowSolution()
    {
        while (solutionSequence)
        {
            yield return (new WaitForSeconds(0.5f));
            for (int i = 1; i <= 5; i++ )
            {
                dotList[i-1].color = GetColor(solution[i-1]);
                keypadColors.transform.GetChild(solution[i-1]-1).gameObject.GetComponent<Image>().color = GetColor(solution[i-1]);
                yield return new WaitForSeconds(waitingTimeSolution);
                dotList[i-1].color = GetColor(0);
                keypadColors.transform.GetChild(solution[i-1]-1).gameObject.GetComponent<Image>().color = GetColor(0);
            }
            //Set the colors of the buttons
            for (int j = 1; j <= 6; j++)
            {
                keypadColors.transform.GetChild(j-1).gameObject.GetComponent<Image>().color = GetColor(j);
            }
            solutionSequence = false;
        }
    }

    private int rc()
    {
        //Generate random int ranging from 1-6
        int randomValue = Random.Range(1,7);
        Debug.Log($"--RC -> {randomValue}");
        return (randomValue);
    }

    private void ResetDots()
    {
        for (int i = 1; i <= 5; i++ )
        {
            dotList[i-1].color = defaultColor;
        }
    }

    private Color GetColor(int idx)
    {
        switch(idx)
        {
            case (1):
                return(button1Color);
            case (2):
                return(button2Color);
            case (3):
                return(button3Color);
            case (4):
                return(button4Color);
            case (5):
                return(button5Color);
            case (6):
                return(button6Color);
            default:
                return(defaultColor);
        }
    }

    public void SelectColor(int index)
    {
        if (solutionSequence) { return; }
        Color selected = GetColor(index);
        Debug.Log(index);
        //input.Count is 0 if empty, if 5 it's full
        input.Add(index);
        //Give the correct dot the color
        dotList[input.Count-1].color = selected;

        //if input is full (5 in total) check if you won
        if (input.Count == 5)
        {
            if (IsTheSame())
            {
                Debug.Log("Victory");
                keypadColors.SetActive(false);
                topColors.SetActive(false);
                endText.text = "Victory!";
                endText.enabled = true;

                minigameState = 1;
                EndGame();
            }
            else
            {
                Debug.Log("Loss");
                keypadColors.SetActive(false);
                topColors.SetActive(false);
                endText.text = "Loss!";
                endText.enabled = true;
                
                minigameState = 0;
                EndGame();
            }
        }
    }

    private bool IsTheSame()
    {
        for (int i = 0; i < solution.Count; i++)
        {
            if (input[i] != solution[i])
            {
                return false;
            }
        }
        return true;
    }

    public override void EndGame()
    {
        base.EndGame();
    }
}
