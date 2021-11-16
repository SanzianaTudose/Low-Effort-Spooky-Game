using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarHandler : MonoBehaviour
{
    public Image progress;
    public float time = 3;
    public int clicks = 3;

    public TextMeshProUGUI timeText;

    public Image buttonImage;
    public int buttonShakeAmount = 5;

    public TextMeshProUGUI countdownText;
    public int countdownTime = 3;

    public TextMeshProUGUI finalText;

    public GameObject bgOverlay;
    public GameObject minigameObj;

    public GameObject titleScroll;

    // Canvas groups for different screens, used to hide and show game screens
    public GameObject generalUICG; // Remains on screen during the entire game
    public GameObject introUICG;
    public GameObject gameUICG;
    public GameObject endUICG;

    private int clicksRemaining;
    private float timeRemaining;
    private float countdownTimeRemaining;
    private bool minigameRunning = false;
    private bool countdownRunning = false;
    private Vector3 initialButtonImagePos;


    void Start()
    {
        StartIntro();
    }


    void Update()
    {
        // Handle countdown timer
        if (countdownRunning && countdownTimeRemaining > 0)
        {
            countdownTimeRemaining -= Time.deltaTime;
            if (countdownTimeRemaining > 0)
            {
                // Update text
                countdownText.text = Mathf.FloorToInt(countdownTimeRemaining + 1 % 60).ToString();
            }
            else
            {
                // Termination: Initiate minigame
                StartMinigame();
            }
        }

        // Handle minigame whilest time is remaining and the game is running
        if (minigameRunning && timeRemaining > 0)
        {
            // Deal with time
            timeRemaining -= Time.deltaTime;

            // Only update text and handle keypresses if the time remaining is still larger than 0 after this step
            if (timeRemaining > 0)
            {
                // Update time text
                float seconds = Mathf.FloorToInt(timeRemaining % 60);
                float microseconds = Mathf.FloorToInt(timeRemaining * 100) % 100;
                string textString = string.Format("{0:00}.{1:00}", seconds, microseconds);
                timeText.text = textString;

                if (microseconds > 50)
                {
                    timeText.color = new Color(255, 0, 0, 255);
                }
                else
                {
                    timeText.color = new Color(255, 255, 255, 255);
                }

                // Shake button
                // shakeImage(buttonImage);

                Vector3 newButtonPos = initialButtonImagePos + Random.insideUnitSphere * buttonShakeAmount;
                newButtonPos.z = initialButtonImagePos.z;

                buttonImage.transform.position = newButtonPos;

                // Deal with key presses
                if (Input.GetKeyDown("x"))
                {
                    clicksRemaining--;
                    progress.GetComponent<Image>().fillAmount = (float)(clicks - clicksRemaining) / clicks;

                    // TODO: Add scale bump to button on press

                    // Minigame termination by winning
                    if (clicksRemaining == 0)
                    {
                        StartCoroutine(EndMinigame(true));
                    }
                }
            }
        }

        // Timer termination
        if (minigameRunning && timeRemaining <= 0)
        {
           StartCoroutine(EndMinigame(false));
        }
    }


    public static float RoundTo(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public void StartIntro()
    {
        Debug.Log("Minigame starting...");

        generalUICG.GetComponent<CanvasGroup>().alpha = 0f;

        float smoothTime = 0.2f;
        float alphaVelocity = 0f;

        float smoothFadeIn = Mathf.SmoothDamp(0f, 1f, ref alphaVelocity, smoothTime);

        // TODO: Use the above smoothing. For that, this code
        // must be in update cause it requires multiple updates
        generalUICG.GetComponent<CanvasGroup>().alpha = 1f;

        // Initialize variables
        initialButtonImagePos = buttonImage.transform.position;

        // Update time
        float seconds = Mathf.FloorToInt(time % 60);
        string textString = string.Format("{0:00}.00", seconds);
        timeText.text = textString;
        progress.GetComponent<Image>().fillAmount = 0;
        titleScroll.GetComponent<ScrollAnimationHandler>().OpenScroll();

        // Display explanation text + countdown
        introUICG.GetComponent<CanvasGroup>().alpha = 1f;
        gameUICG.GetComponent<CanvasGroup>().alpha = 0f;
        endUICG.GetComponent<CanvasGroup>().alpha = 0f;

        // Initiate the countdown
        countdownTimeRemaining = countdownTime;
        countdownText.text = Mathf.FloorToInt(countdownTimeRemaining % 60).ToString();
        countdownRunning = true;
    }

    public void StartMinigame()
    {
        // Display the game screen rather than the intro screen
        introUICG.GetComponent<CanvasGroup>().alpha = 0f;
        gameUICG.GetComponent<CanvasGroup>().alpha = 1f;

        // Start the timer
        minigameRunning = true;
        timeRemaining = time;
        clicksRemaining = clicks;
    }

    IEnumerator EndMinigame(bool success)
    {
        gameUICG.GetComponent<CanvasGroup>().alpha = 0f;
        endUICG.GetComponent<CanvasGroup>().alpha = 1f;

        titleScroll.GetComponent<ScrollAnimationHandler>().CloseScroll();

        if (success)
        {
            minigameRunning = false;
            Debug.Log("Minigame finished, you won!");

            // TODO: Hide button and start confetti explosion at its place

            finalText.text = "Congratulations, you won!";

        }
        else
        {
            minigameRunning = false;
            Debug.Log("Minigame finished, you lost...");

            finalText.text = "You lost :(";
        }

        // After two seconds, continue to the rest of the game
        yield return new WaitForSeconds(2);

        float smoothTime = 0.2f;
        float alphaVelocity = 0f;

        float smoothFadeOut = Mathf.SmoothDamp(1f, 0f, ref alphaVelocity, smoothTime);
        generalUICG.GetComponent<CanvasGroup>().alpha = 0f;
        endUICG.GetComponent<CanvasGroup>().alpha = 0f;
    }

}
