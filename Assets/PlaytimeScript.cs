using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;    

public class PlaytimeScript : MonoBehaviour
{
    

    [SerializeField] private TextMeshProUGUI playtimeTimer;
    [SerializeField] private TextMeshProUGUI infoMiddle;
    [SerializeField] private TextMeshProUGUI infoTop;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI trickOrTreatText;
    [SerializeField] private Animator trickOrTreatAnimator;

    [Header("Game Properties")]
    [SerializeField] private float playtimeSession = 0;
    [SerializeField] private float trickProbability = 0.3f;
    

    private bool gameOver = false;
    private float playtime;
    public bool gamePaused = false;
    public bool pauseDisabled = false;

    public bool getInput = true;
    private bool scoreIsUpdating = false;

    public int candyScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        playtime = playtimeSession;
        // pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        retryButton.SetActive(false);
        playtimeTimer.text = GenerateClockText(Mathf.RoundToInt(playtime));
        ActivatePauseMenu();
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
        if (time < 0 || time > 3599) { return $"-:-"; }
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
            infoMiddle.text = "You've recently become a ghost. Since it's Halloween night, you want to get as many candies as possible. You can do so by possessing the people on the street and then going trick-or-treating." +
                "\n\n CONTROLS: \n WASD - movement  E - interact  Q - stop possesion P -pause" +
                "\n\n CREDITS: \n HochuPitsu - Pixel Bob; LimeZu - Serene Village (itch.io) \n peony - Plants & Flowers (opengameart.org)" +
                "\n Pizzadude - ArcadeClassic font (1001fonts.com)";
            retryButton.SetActive(false);
            resumeButton.SetActive(true);
            
        }
        //pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
        // pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void RestartSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int OnHouseInteraction() {
        if (scoreIsUpdating) return -1;

        // Determine if it's Trick or Treat
        int amount = 0;
        float trickOrTreat = Random.Range(0.0f, 1);
        if (trickOrTreat <= trickProbability) {
            // TRICK
            amount = -5;
        } else {
            // TREAT
            amount = Mathf.FloorToInt(Random.Range(5, 10));
        }
        print(trickOrTreat + " " + trickProbability);

        candyScore += amount;
        if (candyScore < 0)
            candyScore = 0;

        // Update UI - show text in red/green to give player feedback on outcome

        if (amount < 0) {
            scoreText.color = Color.red;
            scoreText.text += " - " + Mathf.Abs(amount);

            trickOrTreatText.text = "TRICK!";
            trickOrTreatText.color = new Color32(162, 73, 73, 255);

        } else {
            scoreText.color = Color.green;
            scoreText.text += " + " + amount;

            trickOrTreatText.text = "TREAT!";
            trickOrTreatText.color = new Color32(73, 162, 73, 255);
        }

        trickOrTreatAnimator.Play("TrickOrTreat");
        StartCoroutine(UpdateScoreText());

        if (amount >= 0)
            return 1;
        return 0;
    }

    IEnumerator UpdateScoreText() {
        scoreIsUpdating = true;
        yield return new WaitForSeconds(2f);
        scoreText.text = "SCORE: " + candyScore;
        scoreText.color = Color.white;
        scoreIsUpdating = false;
    }
}
