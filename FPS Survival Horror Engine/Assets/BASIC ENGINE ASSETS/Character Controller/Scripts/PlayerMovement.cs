using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float x;
    float z;
    public CharacterController controller;

    public float speed = 7f;
    public float runMultiplier = 1.75f;
    public float gravity = -9.81f; // public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;

    public Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = this.gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // Grounded //
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed * runMultiplier;
            animator.SetBool("Running", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed / runMultiplier;
            animator.SetBool("Running", false);
        }



        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        // Movement //
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "VisionCone")
        {
            Debug.Log("Within Range");
            col.GetComponentInParent<EnemyMovement>().Seen();
        }
    }
}

