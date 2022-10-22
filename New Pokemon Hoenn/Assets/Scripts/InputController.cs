using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public MenuAnimation menuAnimation;
    public Transform followSpot;
    private float moveSpeed;
    public LayerMask stopsMovement;
    public enum PlayerDirection { Up, Down, Left, Right }
    public SpriteRenderer activePlayerSprite;
    public Sprite[] playerSprites;
    private PlayerDirection isFacing;
    public Transform interactSpot;
    private float walkingSpeed = 3.5f;
    private float sprintSpeed = 7.5f;

    void Update()
    {
        if (GlobalGameEvents.blockPlayerInputOverworld == false)
        {
            GetPlayerInput();
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, followSpot.position, moveSpeed * Time.deltaTime);
    }

    void GetPlayerInput()
    {
        if (Vector3.Distance(transform.position, followSpot.position) == 0f)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                menuAnimation.ToggleMenu();
            }

            if (Time.timeScale == 1)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {

                    if (isFacing == PlayerDirection.Up)
                    {
                        interactSpot.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                    }
                    else if (isFacing == PlayerDirection.Right)
                    {
                        interactSpot.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
                    }
                    else if (isFacing == PlayerDirection.Down)
                    {
                        interactSpot.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                    }
                    else
                    {
                        interactSpot.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
                    }

                    StartCoroutine(StallInteract());

                }

                moveSpeed = GetSpeed();

                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (isFacing != PlayerDirection.Up)
                    {
                        isFacing = PlayerDirection.Up;
                        activePlayerSprite.sprite = playerSprites[0];
                        if (moveSpeed == walkingSpeed)
                        {
                            StartCoroutine(StallMovement());
                        }
                    }
                    else if (!Physics2D.OverlapCircle(followSpot.position + new Vector3(0f, 1f, 0f), 0.2f, stopsMovement))
                    {
                        moveSpeed = GetSpeed();
                        followSpot.position += new Vector3(0f, 1f, 0f);
                    }
                }

                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (isFacing != PlayerDirection.Right)
                    {
                        isFacing = PlayerDirection.Right;
                        activePlayerSprite.sprite = playerSprites[1];
                        if (moveSpeed == walkingSpeed)
                        {
                            StartCoroutine(StallMovement());
                        }
                    }
                    else if (!Physics2D.OverlapCircle(followSpot.position + new Vector3(1f, 0f, 0f), 0.2f, stopsMovement))
                    {
                        moveSpeed = GetSpeed();
                        followSpot.position += new Vector3(1f, 0f, 0f);
                    }
                }

                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (isFacing != PlayerDirection.Left)
                    {
                        isFacing = PlayerDirection.Left;
                        activePlayerSprite.sprite = playerSprites[3];
                        if (moveSpeed == walkingSpeed)
                        {
                            StartCoroutine(StallMovement());
                        }
                    }
                    else if (!Physics2D.OverlapCircle(followSpot.position + new Vector3(-1f, 0f, 0f), 0.2f, stopsMovement))
                    {
                        moveSpeed = GetSpeed();
                        followSpot.position += new Vector3(-1f, 0f, 0f);
                    }
                }

                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (isFacing != PlayerDirection.Down)
                    {
                        isFacing = PlayerDirection.Down;
                        activePlayerSprite.sprite = playerSprites[2];
                        if (moveSpeed == walkingSpeed)
                        {
                            StartCoroutine(StallMovement());
                        }
                    }
                    else if (!Physics2D.OverlapCircle(followSpot.position + new Vector3(0f, -1f, 0f), 0.2f, stopsMovement))
                    {
                        moveSpeed = GetSpeed();
                        followSpot.position += new Vector3(0f, -1f, 0f);
                    }
                }
            }
        }
    }

    float GetSpeed() //add different MovementType conditionals for surfing and cycling
    {
        return Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkingSpeed;
    }

    IEnumerator StallMovement()
    {
        followSpot.position += new Vector3(0f, 0f, 1f);
        yield return new WaitForSeconds(0.08f);
        followSpot.position += new Vector3(0f, 0f, -1f);
    }

    IEnumerator StallInteract()
    {
        yield return new WaitForSeconds(0.1f);
        interactSpot.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
    }

}