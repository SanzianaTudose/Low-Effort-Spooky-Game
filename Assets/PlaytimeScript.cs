using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;    

public class PlaytimeScript : MonoBehaviour
{
    [SerializeField] private float playtimeSession = 0;

    [SerializeField] private TextMeshProUGUI playtimeTimer;
    [SerializeField] private TextMeshProUGUI infoMiddle;
    [SerializeField] private TextMeshProUGUI infoTop;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject retryButton;

    private bool gameOver = false;
    private float playtime;
    public bool gamePaused = false;
    public bool pauseDisabled = false;

    public bool getInput = true;

    public int candyScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        playtime = playtimeSession;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        resumeButton.SetActive(false);
        retryButton.SetActive(false);
        playtimeTimer.text = GenerateClockText(Mathf.RoundToInt(playtime));
        DeactivatePauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            if (playtime <= 0)
            {
                gameOver = true;
                ActivatePauseMenu(1); 
            }
            else
            {
                playtime -= Time.deltaTime;
                string newPlaytime = GenerateClockText(Mathf.RoundToInt(playtime));
                if (newPlaytime != playtimeTimer.text)
                { 
                    playtimeTimer.text = GenerateClockText(Mathf.RoundToInt(playtime));
                }
                
            }
        }

        if (!getInput) return;

        if (!pauseDisabled && !gamePaused)
        {
            if (Input.GetKeyDown("p"))
            {
                Debug.Log("ACTIVATED!");
                ActivatePauseMenu();
            }
        }
        else if (gamePaused && !gameOver)
        {
            if (Input.GetKeyDown("p"))
            {
                Debug.Log("DEACTIVATED!");
                DeactivatePauseMenu();
            }
        }
        
    }

    private string GenerateClockText(int time)
    {
        //Deal with time outside our capacity
        if (time < 0 || time > 3599) { return $"--:--"; }
        //Calculate the left and right hand side
        int seconds = time % 60;
        int minutes = (time - seconds) / 60;
        //Return in the format XX:XX
        if (seconds < 10 && minutes < 10) { return $"0{minutes}:0{seconds}"; }
        else if (seconds < 10) { return $"{minutes}:0{seconds}"; }
        else if (minutes < 10) { return $"0{minutes}:{seconds}"; }
        else { return $"{minutes}:{seconds}"; }
    }

    private void ActivatePauseMenu(int note = 0)
    {
        if (note == 1)
        {
            infoTop.text = "Time is up!";
            infoMiddle.text = $"You have earned a\ncandy score of:\n{candyScore}\n";
            resumeButton.SetActive(false);
            retryButton.SetActive(true);
        }
        else
        {
            infoTop.text = "Pause Menu";
            infoMiddle.text = "TIP: Use the hotkey \"p\" to quickly switch between the pause menu and gameplay.";
            retryButton.SetActive(false);
            resumeButton.SetActive(true);
            
        }
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    private void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    private void RestartSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}