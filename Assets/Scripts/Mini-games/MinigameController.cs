using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameController : MonoBehaviour
{
    [SerializeField] private GameObject introUI;
    [SerializeField] public TextMeshProUGUI countdownText;
    [SerializeField] private GameObject overlayPanel;

    [Header("Properties")]
    [SerializeField] private int countdownTime = 3;

    Minigame curMinigame;
    private float countdownTimeRemaining;
    private bool countdownRunning = false;

    // Update is called once per frame
    void Update()
    {
        // Handle countdown timer
        if (countdownRunning) {
            if (countdownTimeRemaining > 0)
                countdownTimeRemaining -= Time.deltaTime;
            else {
                countdownRunning = false;
                introUI.SetActive(false);
                // Termination: Initiate minigame
                curMinigame.StartGame();
            }
        }

        // Update UI
        if (countdownTimeRemaining > 0) 
            countdownText.text = Mathf.FloorToInt(countdownTimeRemaining + 1 % 60).ToString();
    }

    public void startIntro(Minigame minigameToStart) {
        curMinigame = minigameToStart;

        // Show introduction
        // Display explanation text + countdown
        introUI.SetActive(true);
        overlayPanel.SetActive(true);

        // Initiate the countdown
        countdownTimeRemaining = countdownTime;
        countdownText.text = Mathf.FloorToInt(countdownTimeRemaining % 60).ToString();
        countdownRunning = true;
    }
}
