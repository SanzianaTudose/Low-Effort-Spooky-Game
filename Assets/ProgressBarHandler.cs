using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHandler : MonoBehaviour
{
    public GameObject progress;
    public float animationSpeed = 3f;
    public float time = 3;
    public int clicks = 3;
    public Text timeText;
    
    private int clicks_remaining;
    private float time_remaining;
    private bool minigame_running = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Minigame initiation
        if (Input.GetKeyDown("q"))
        {
            print("Minigame starting...");
            minigame_running = true;
            time_remaining = time;
            clicks_remaining = clicks;
            progress.transform.localScale = new Vector3(1f, 0f, 1f);
        }

        // Handle minigame whilest time is remaining and the game is running
        if (minigame_running && time_remaining > 0)
        {

            // Deal with time
            time_remaining -= Time.deltaTime;
            float seconds = Mathf.FloorToInt(time_remaining % 60);
            float microseconds = Mathf.FloorToInt(time_remaining * 100) % 100;
            string textString = string.Format("{0:00}.{1:00}", seconds, microseconds);
            // timeText.text = textString;
            print(textString);

            // Deal with key presses
            if (Input.GetKeyDown("x"))
            {
                progress.transform.localScale += new Vector3(0f, (float) 1 / clicks, 0f);
                clicks_remaining--;

                // Minigame termination by winning
                if (clicks_remaining == 0)
                {
                    minigame_running = false;
                    print("Minigame finished, you won!");
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

}
