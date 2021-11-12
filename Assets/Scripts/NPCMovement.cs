using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected Vector2 decisionTimeRange = new Vector2(1f, 2f);

    private Vector3 curMoveDir;
    private float decisionTimeCount = 0;

    void Update() {
        // Move the object in the chosen direction at the set speed
        transform.position += curMoveDir * Time.deltaTime * moveSpeed;

        if (decisionTimeCount > 0) decisionTimeCount -= Time.deltaTime;
        else {
            decisionTimeCount = Random.Range(decisionTimeRange.x, decisionTimeRange.y);
            GenerateMoveDirection();
        }
    }

    void GenerateMoveDirection() {
        curMoveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        curMoveDir.Normalize();
    }
}
