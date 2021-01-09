using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float walkSoundVolume = .2f, jumpSoundVolume = .5f;
        public AudioSource walkSound, jumpSound, jumplandSound;
    }

    public Sounds movementSounds;
    public bool canPlayLandingSound;

    public CharacterController controller;
    public Animator playerAnimation;
    public SphereCollider col;

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

    bool playWalkingSound;
    public float stepTimerTime;
    public float stepTimer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        col = GetComponent<SphereCollider>();
        stepTimer = stepTimerTime;
        //playerAnimation = GetComponent<Animator>();
    }
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -groundDistance, 0), out hit, groundDistance , groundMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
        }
        else if(isGrounded == true)
        {
            isGrounded = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float animX = x;
        float animZ = z;

        if (x != 0 && z != 0)
        {
            x *= .7f;
            z *= .7f;
        }

        move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(x != 0 || z != 0)
        {
            playWalkingSound = true;
        }
        else
        {
            playWalkingSound = false;
        }

        if (isGrounded)
        {
            playerAnimation.SetFloat("Blendy", animZ);
            playerAnimation.SetFloat("Blendx", animX);

            if (playWalkingSound)
            {
                if (stepTimer > 0)
                {
                    stepTimer -= Time.deltaTime;
                }
                else if (stepTimer < 0)
                {
                    stepTimer = stepTimerTime;

                    movementSounds.walkSound.volume = Random.Range(movementSounds.walkSoundVolume - .001f, movementSounds.walkSoundVolume + .001f);
                    movementSounds.walkSound.pitch = Random.Range(1 - .1f, 1 + .1f);
                    movementSounds.walkSound.Play();
                }
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            canPlayLandingSound = false;

            velocity.y = Mathf.Sqrt(jumpHeight = -2f * gravity);

            movementSounds.jumpSound.volume = Random.Range(movementSounds.jumpSoundVolume - .001f, movementSounds.jumpSoundVolume + .001f);
            movementSounds.jumpSound.pitch = Random.Range(1 - .1f, 1 + .1f);
            movementSounds.jumpSound.Play();
        }
        else
        {
            return;
        }
    }
    public void WalkSound()
    {
        print("ahha");
    }
}