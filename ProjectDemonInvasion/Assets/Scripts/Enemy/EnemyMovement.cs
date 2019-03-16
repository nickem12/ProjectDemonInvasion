using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public CombatManager manager;
    public float moveSpeed;

    private Animator anim;
    private Rigidbody2D myBody;

    private bool playerMoving;

    private Vector2 lastMove;
    private Vector2 direction;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.InCombat)
        {
            playerMoving = false;

            direction.x = Random.Range(-1f, 1.1f);
            direction.y = Random.Range(-1f, 1.1f);

            if (direction.x > 0.3f || direction.x < -0.3f)
            {
                
                myBody.velocity = new Vector2(direction.x * moveSpeed, myBody.velocity.y);
                playerMoving = true;
                lastMove = new Vector2(direction.x, 0f);

            }
            if (direction.y > 0.3f || direction.y < -0.3f)
            {
                
                myBody.velocity = new Vector2(myBody.velocity.x, direction.y * moveSpeed);
                playerMoving = true;
                lastMove = new Vector2(0f, direction.y);

            }
            if (direction.x < 0.3f && direction.x > -0.3f)
            {
                myBody.velocity = new Vector2(0f, myBody.velocity.y);
            }
            if (direction.y < 0.3f && direction.y > -0.3f)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0f);
            }

            anim.SetFloat("MovX", direction.x);
            anim.SetFloat("MovY", direction.y);
            anim.SetBool("PlayerMoving", playerMoving);
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);
        }
        else
        {
            myBody.velocity = new Vector2(0f, 0f);
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
            anim.SetBool("PlayerMoving", false);
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);
        }
    }
}
