using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/* Handles everything related to the Circle Click Mini-game - should be attached to the AimClickContainer
 *  Spawning new buttons
 *  On circle click
 *  Mini-game state management (win, fail etc.)
*/
public class AimClickGameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private List<Sprite> spriteList; 

    [Header("Game Properties")]
    [SerializeField] private Vector2Int circleCountRange = new Vector2Int(3, 6);
    [SerializeField] private Vector2 circleScaleRange = new Vector2(0.5f, 1f);
    [SerializeField] private float timeLimit = 5f;

    private GameObject aimClickContainer;
    private Vector2 spawnBoundsX, spawnBoundsY;

    private int circleCount;
    private int circlesClicked = 0;
    private float timeRemaining = 0;
    private bool minigameRunning = false;

    private void Update() {
        // Handle time
        if (minigameRunning && timeRemaining > 0) 
            timeRemaining -= Time.deltaTime;

        HandleGameStates();

        // Update UI
        if (minigameRunning) {
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            float microseconds = Mathf.FloorToInt(timeRemaining * 100) % 100;
            string textString = string.Format("{0:00}:{1:00}", seconds, microseconds);
            timeText.text = textString;
        }
    }

    #region Initialization
    public void StartGame() {
        if (minigameRunning) return;

        aimClickContainer = gameObject;
        CalculateSpawnBounds();

        SpawnCircles();
        aimClickContainer.SetActive(true);

        circlesClicked = 0;
        minigameRunning = true;
        timeRemaining = timeLimit;
    }
   
    private void CalculateSpawnBounds() {
        RectTransform containerRect = aimClickContainer.GetComponent<RectTransform>();
        RectTransform circleRect = circlePrefab.GetComponent<RectTransform>();
        if (containerRect == null)
            Debug.LogError("AimClickGameManager: RectTransform component could not be found on {aimClickContainer}.");
        if (circleRect == null)
            Debug.LogError("AimClickGameManager: RectTransform component could not be found on {circlePrefab}.");

        // Get container and circle bounds
        Vector2 containerSize = new Vector2(containerRect.rect.width, containerRect.rect.height);
        Vector2 circleSize = new Vector2(circleRect.rect.width, circleRect.rect.height);

        // Set bounds related to container and circle prefab size
        spawnBoundsX = new Vector2((-containerSize.x + circleSize.x) / 2.0f, (containerSize.x - circleSize.x) / 2.0f);
        spawnBoundsY = new Vector2((-containerSize.y + circleSize.y) / 2.0f, (containerSize.y - circleSize.y) / 2.0f);
    }

    private void SpawnCircles() {
        // Determine random number of buttons to spawn from {buttonCountRange}
        circleCount = Mathf.FloorToInt(Random.Range(circleCountRange.x, circleCountRange.y));

        for (int i = 1; i <= circleCount; i++) {
            GameObject circle = Instantiate(circlePrefab, aimClickContainer.transform);
            SetUpCircleProperties(circle);
        }
    }

    private void SetUpCircleProperties(GameObject circle) {
        // Set up Event Trigger for newly instantiated Circle
        EventTrigger trigger = circle.GetComponent<EventTrigger>();
        if (trigger == null)
            Debug.LogError("ButtonClickGameManager: Event Trigger component not found on Circle");


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnButtonClick(circle); });
        trigger.triggers.Add(entry);

        // Set up position 
        RectTransform circleRect = circle.GetComponent<RectTransform>();
        if (circleRect == null)
            Debug.LogError("AimClickGameManager: RectTransform component could not be found on {circle}.");

        float circleX = Random.Range(spawnBoundsX.x, spawnBoundsX.y);
        float circleY = Random.Range(spawnBoundsY.x, spawnBoundsY.y);
        circleRect.localPosition = new Vector3(circleX, circleY, 0.0f);

        // Set up scale 
        circleRect.localScale = circleRect.localScale * Random.Range(circleScaleRange.x, circleScaleRange.y);

        // Set up sprite
        Image circleImage = circle.GetComponent<Image>();
        if (circleImage == null)
            Debug.LogError("AimClickGameManager: Image component could not be found on {circle}.");
        int spriteIndex = Mathf.FloorToInt(Random.Range(0.0f, spriteList.Count));
        circleImage.sprite = spriteList[spriteIndex];
    }
    #endregion

    private void HandleGameStates() {
        // Handle FAIL state
        if (minigameRunning && timeRemaining <= 0) {
            minigameRunning = false;
            print("fail");
        }

        // Handle WIN state
        if (minigameRunning && timeRemaining > 0 && circlesClicked == circleCount) {
            minigameRunning = false;
            print("win");
        }
    }

    // Referenced by the Event Trigger component on Button Image
    public void OnButtonClick(GameObject curCircle) {
        if (!minigameRunning)
            return;

        circlesClicked++;
        Destroy(curCircle);
    }

}
