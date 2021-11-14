using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1f, 2f);
    [SerializeField] private Vector2 decisionTimeRange = new Vector2(2f, 4f);
    [SerializeField] private Vector2 waitTimeRange = new Vector2(1f, 2f);
    [SerializeField] private float waitProbability = 0.2f;

    private Animator anim;

    private float moveSpeed;
    private float decisionTimeCount = 0;

    private bool facingRight = true;

    private Vector3 curMoveDir;

    private SpriteRenderer srNPC;

    private void Start() {
        anim = GetComponent<Animator>();

        srNPC = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
    }

    private void Update() {
        /*// Move the object in the chosen direction at the set speed
        if (curMoveDir.x > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (curMoveDir.x < 0 && facingRight)
        {

            // ... flip the player.
            Flip();
        }*/


        transform.position += curMoveDir * Time.deltaTime * moveSpeed;
        AnimateNPC();


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

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);

        if(srNPC.flipX) { srNPC.flipX = false; } else { srNPC.flipX = true; }
    }

    void AnimateNPC()
    {
        if (curMoveDir.x > 0 && !facingRight)
        {
            anim.SetBool("IsWalking", true);
            Flip();

        }
        else if (curMoveDir.x < 0 && facingRight)
        {
            anim.SetBool("IsWalking", true);
            Flip();
        }
        else if (curMoveDir.x == 0)
        {
            anim.SetBool("IsWalking", false);
        }
    }
}
