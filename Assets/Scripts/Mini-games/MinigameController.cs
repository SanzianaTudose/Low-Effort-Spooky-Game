using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameController : MonoBehaviour
{
    [SerializeField] GameObject introUI;
    [SerializeField] public TextMeshProUGUI countdownText;
    [SerializeField] Minigame minigameManager;

    [Header("Properties")]
    [SerializeField] private int countdownTime = 3;

    private float countdownTimeRemaining;
    private bool countdownRunning = false;

    // Update is called once per frame
    void Update()
    {
        // Handle player input
        if (Input.GetKeyDown(KeyCode.Q))
            startIntro();

        // Handle countdown timer
        if (countdownRunning) {
            if (countdownTimeRemaining > 0)
                countdownTimeRemaining -= Time.deltaTime;
            else {
                countdownRunning = false;
                // Termination: Initiate minigame
                minigameManager.StartGame();
            }
        }

        // Update UI
        if (countdownTimeRemaining > 0) 
            countdownText.text = Mathf.FloorToInt(countdownTimeRemaining + 1 % 60).ToString();
    }

    public void startIntro() {
        // Show introduction
        // Display explanation text + countdown
        introUI.SetActive(true);

        // Initiate the countdown
        countdownTimeRemaining = countdownTime;
        countdownText.text = Mathf.FloorToInt(countdownTimeRemaining % 60).ToString();
        countdownRunning = true;
    }
}
