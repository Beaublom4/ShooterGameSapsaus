﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public Animator playerAnimation;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float animationLength = 1f;
    public float animationTime = 0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 move;
    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //playerAnimation = GetComponent<Animator>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float animX = x;
        float animZ = z;

        if(x != 0 && z != 0)
        {
            x *= .7f;
            z *= .7f;
        }

        move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight = -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        if (isGrounded)
        {
            playerAnimation.SetFloat("blendy", animZ);
            playerAnimation.SetFloat("Blendx", animX);

            //velocity.y = -2f;
            //animationTime += Time.deltaTime / animationLength;

            /*
            if (move.z > 0)
            {
                playerAnimation.SetFloat("Blendx", Mathf.Lerp(0, 1, animationLength * Time.deltaTime));
            }
            if (move.z < 0)
            {
                playerAnimation.SetFloat("Blendx", Mathf.Lerp(0, 1, animationLength * Time.deltaTime));
            }
            if (move.z == 0)
            {
                playerAnimation.SetFloat("Blendx", 0);
            }
            if (move.x > 0)
            {
                playerAnimation.SetFloat("blendy", Mathf.Lerp(0, 1, 1));
            }
            if (move.x < 0)
            {
                playerAnimation.SetFloat("blendy", Mathf.Lerp(0, -1, 1));
            }
            if (move.x == 0)
            {
                playerAnimation.SetFloat("blendy", 0);
            }
            */
        }
        }
}