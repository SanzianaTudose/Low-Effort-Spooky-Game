using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarHandler : MonoBehaviour
{
    public GameObject progress;
    public float animationSpeed = 3f;
    public float time = 3;
    public int clicks = 3;
    public TextMeshProUGUI timeText;
    public Image buttonImage;
    public int buttonShakeAmount = 5;
    
    private int clicks_remaining;
    private float time_remaining;
    private bool minigame_running = false;
    private Vector3 initialButtonImagePos;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        initialButtonImagePos = buttonImage.transform.position;

        // Update time
        float seconds = Mathf.FloorToInt(time % 60);
        string textString = string.Format("{0:00}.00", seconds);
        timeText.text = textString;
    }

    // Update is called once per frame
    void Update()
    {
        // Minigame initiation
        if (Input.GetKeyDown("q"))
        {
            startMinigame();
        }

        // Handle minigame whilest time is remaining and the game is running
        if (minigame_running && time_remaining > 0)
        {
            // Deal with time
            time_remaining -= Time.deltaTime;

            // Only update text and handle keypresses if the time remaining is still larger than 0 after this step
            if (time_remaining > 0)
            {
                // Update time text
                float seconds = Mathf.FloorToInt(time_remaining % 60);
                float microseconds = Mathf.FloorToInt(time_remaining * 100) % 100;
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

                Debug.Log(newButtonPos);

                // Deal with key presses
                if (Input.GetKeyDown("x"))
                {
                    progress.transform.localScale += new Vector3(0f, (float)1 / clicks, 0f);
                    clicks_remaining--;

                    // Minigame termination by winning
                    if (clicks_remaining == 0)
                    {
                        minigame_running = false;
                        print("Minigame finished, you won!");
                    }
                }
            }
        }

        // Timer termination
        if (minigame_running && time_remaining <= 0)
        {
            minigame_running = false;
            print("Minigame finished, you failed!");
        }
    }


    public static float RoundTo(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public void shakeImage(Image objToShake)
    {
        float speed = 1.0f;
        float amount = 1.0f;

        float x = objToShake.transform.position.x * Mathf.Sin(Time.deltaTime * speed) * amount;
        float y = objToShake.transform.position.y;
        float z = objToShake.transform.position.z;

        objToShake.transform.position = new Vector3(x, y, z);
    }

    public void startMinigame()
    {
        Debug.Log("Minigame starting...");
        minigame_running = true;
        time_remaining = time;
        clicks_remaining = clicks;
        progress.transform.localScale = new Vector3(1f, 0f, 1f); // Reset progress bar

        // Show introduction
            // Display explanation text + countdown

        // Initiate actual minigame AFTER previous events ended
            // Hide explanation text + countdown
            // Display Button and bar
            // Start the timer
            // Start shaking the button

    }

    public void endMinigame(bool success)
    {
        if (success)
        {
            // Hide button and start confetti explosion at its place

            // Display "Congratulations, you won!" text
        } else
        {
            // Display "You lost :(" text
        }

        // Fade the elements out and animate the black overlay element's transparency to 0.
        // Simple rendition: Hide the elements and overlay
    }

}
