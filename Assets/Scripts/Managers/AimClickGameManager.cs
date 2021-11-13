using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* Handles everything related to the Circle Click Mini-game - should be attached to the AimClickContainer
 *  Spawning new buttons
 *  On circle click
 *  Mini-game state management (win, fail etc.)
*/
public class AimClickGameManager : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;

    [Header("Game Properties")]
    [SerializeField] private Vector2Int circleCountRange = new Vector2Int(3, 6);
    [SerializeField] private Vector2 circleScaleRange = new Vector2(0.5f, 1f);
    
    private GameObject aimClickContainer;
    private Vector2 spawnBoundsX, spawnBoundsY;

    private int circleCount;

    void Start() {
        aimClickContainer = gameObject;
        CalculateSpawnBounds();

        SpawnCircles();
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
            Debug.LogError("AimClickGameManager: RectTransform component could not be found on {circlePrefab}.");

        float circleX = Random.Range(spawnBoundsX.x, spawnBoundsX.y);
        float circleY = Random.Range(spawnBoundsY.x, spawnBoundsY.y);
        circleRect.localPosition = new Vector3(circleX, circleY, 0.0f);

        // Set up scale 
        circleRect.localScale = circleRect.localScale * Random.Range(circleScaleRange.x, circleScaleRange.y);
    }

    // Referenced by the Event Trigger component on Button Image
    public void OnButtonClick(GameObject curCircle) {
        Destroy(curCircle);
    }

}
