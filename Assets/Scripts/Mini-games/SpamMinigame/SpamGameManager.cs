using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpamGameManager : Minigame
{
    [Header("Game Properties")]
    [SerializeField] private int clicks = 10;
    [SerializeField] private float time = 3f;

    [Header("GameObject references & properties")]
    [SerializeField] private Image progress;
    [SerializeField] private Image buttonImage;
    [SerializeField] private int buttonShakeAmount = 5;

    [SerializeField] private TextMeshProUGUI finalText;

    // Canvas groups for different screens, used to hide and show game screens
    [SerializeField] private GameObject generalUICG;
    [SerializeField] private GameObject gameUICG;
    [SerializeField] private GameObject endUICG;

    private int clicksRemaining;
    private Vector3 initialButtonImagePos;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        HandleGameStates();
    }

    #region Initialization
    public override void StartGame()
    {
        base.StartGame();

        Debug.Log("Starting game...");

        timeRemaining = time;

        // Display the game screen rather than the intro screen
        generalUICG.GetComponent<CanvasGroup>().alpha = 1f;
        gameUICG.GetComponent<CanvasGroup>().alpha = 1f;
        endUICG.GetComponent<CanvasGroup>().alpha = 0f;

        // Initialize variables
        initialButtonImagePos = buttonImage.transform.position;
        clicksRemaining = clicks;

        // Initialize progress bar
        progress.GetComponent<Image>().fillAmount = 0;
    }
    #endregion

    public override void EndGame()
    {
        base.EndGame();

        generalUICG.GetComponent<CanvasGroup>().alpha = 1f;
        gameUICG.GetComponent<CanvasGroup>().alpha = 0f;
        endUICG.GetComponent<CanvasGroup>().alpha = 1f;
    }

    void HandleGameStates()
    {
        // Handle FAIL state
        if (minigameRunning && timeRemaining <= 0)
        {
            minigameState = 0;
            finalText.text = "Unfortunately, you lost :(";
            EndGame();
        }

        if (minigameRunning && timeRemaining > 0)
        {
            // Shake the button
            Vector3 newButtonPos = initialButtonImagePos + Random.insideUnitSphere * buttonShakeAmount;
            newButtonPos.z = initialButtonImagePos.z;

            buttonImage.transform.position = newButtonPos;

            // Deal with key presses
            if (Input.GetKeyDown("x"))
            {
                clicksRemaining--;
                progress.GetComponent<Image>().fillAmount = (float)(clicks - clicksRemaining) / clicks;

                Debug.Log($"You pressed the button, only {clicksRemaining}/{clicks} remaining");

                // Handle WIN state
                if (clicksRemaining == 0)
                {
                    minigameState = 1;
                    finalText.text = "Congratulations, you won!";

                    EndGame();

                    Debug.Log("No clicks remaining, you won!");
                }
            }
        }
    }
}
