using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public class Sounds
    {
        [HideInInspector] public float walkSoundVolume, jumpSoundVolume;
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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        col = GetComponent<SphereCollider>();
        //playerAnimation = GetComponent<Animator>();
    }
    private void Step()
    {
        movementSounds.walkSound.volume = Random.Range(movementSounds.walkSoundVolume - .1f, movementSounds.walkSoundVolume + .1f);
        movementSounds.walkSound.pitch = Random.Range(1 - .1f, 1 + .1f);
        movementSounds.walkSound.Play();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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

        if (isGrounded)
        {
            playerAnimation.SetFloat("Blendy", animZ);
            playerAnimation.SetFloat("Blendx", animX);

            Step();

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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            canPlayLandingSound = false;

            velocity.y = Mathf.Sqrt(jumpHeight = -2f * gravity);

            movementSounds.jumpSound.volume = Random.Range(movementSounds.jumpSoundVolume - .1f, movementSounds.jumpSoundVolume + .1f);
            movementSounds.jumpSound.pitch = Random.Range(1 - .1f, 1 + .1f);
            movementSounds.jumpSound.Play();
        }
        else
        {
            return;
        }
    }
    public void OnCollisionEnter(Collision haslanded)
    {
        if (isGrounded == true)
        {
            canPlayLandingSound = true;
            if (canPlayLandingSound == true)
            {
                movementSounds.jumplandSound.volume = Random.Range(movementSounds.jumpSoundVolume - .1f, movementSounds.jumpSoundVolume + .1f);
                movementSounds.jumplandSound.pitch = Random.Range(1 - .1f, 1 + .1f);
                movementSounds.jumplandSound.Play();
            }
        }
    }
}