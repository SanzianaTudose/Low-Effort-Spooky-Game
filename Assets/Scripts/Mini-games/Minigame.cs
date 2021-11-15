using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Minigame : MonoBehaviour
{
    [SerializeField] protected GameObject minigameContainer;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject overlayPanel;

    public int minigameState = -1;
    public bool minigameRunning = false;
    protected float timeRemaining = 0;

    protected void Start() {
        minigameContainer = gameObject;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Handle time
        if (minigameRunning && timeRemaining > 0)
            timeRemaining -= Time.deltaTime;

        // Update UI
        if (minigameRunning && timeRemaining > 0) {
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            float microseconds = Mathf.FloorToInt(timeRemaining * 100) % 100;
            string textString = string.Format("{0:00}:{1:00}", seconds, microseconds);
            timeText.text = textString;
        }
    }

    public virtual void StartGame() {
        if (minigameRunning) return;

        minigameRunning = true;
        minigameContainer.SetActive(true);
    }

    public virtual void EndGame() {
        StartCoroutine(DisableMinigameAfterSeconds(2f));
    }

    IEnumerator DisableMinigameAfterSeconds(float sec) {
        yield return new WaitForSeconds(sec);
       
        minigameContainer.SetActive(false);
        overlayPanel.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        minigameRunning = false;
    }
}
