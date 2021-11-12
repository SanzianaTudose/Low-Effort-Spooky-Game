using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1.5f, 2f);
    [SerializeField] private Vector2 decisionTimeRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 waitTimeRange = new Vector2(1f, 2f);
    [SerializeField] private float waitProbability = 0.2f;

    private float moveSpeed;
    private float decisionTimeCount = 0;
    
    private Vector3 curMoveDir;

    private void Start() {
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
    }

    private void Update() {
        // Move the object in the chosen direction at the set speed
        transform.position += curMoveDir * Time.deltaTime * moveSpeed;

        if (decisionTimeCount > 0) decisionTimeCount -= Time.deltaTime;
        else StartNewMovement();
    }

    private void StartNewMovement() {
        decisionTimeCount = Random.Range(decisionTimeRange.x, decisionTimeRange.y);

        // Generate new movement direction
        curMoveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        float isWaiting = Random.Range(0.0f, 1f);
        if (isWaiting <= waitProbability) {
            curMoveDir = Vector3.zero;
            decisionTimeCount = Random.Range(waitTimeRange.x, waitTimeRange.y);
        }
        curMoveDir.Normalize();

        decisionTimeCount = Random.Range(decisionTimeRange.x, decisionTimeRange.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Move away when colliding with an object
        curMoveDir = -curMoveDir;
    }
}
