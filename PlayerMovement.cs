using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Setup that will determine the rate of our movement
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;

    //Unity references. Ground Check is an empty game object placed at the bottom of the player
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    // Update is called once per frame
    void Update()
    {
        //Saves the horizontal movement as visualized in a 2D Coordinate System
        //"Horizontal" is a Unity variable that can be looked up in the project settings. It used the A and D buttons. That's what we use to move
        horizontal = Input.GetAxisRaw("Horizontal");
        //Takes the abstract of the saved input and sets the Speed parameter of Unity's Animator equal to it. 
        //YOU have to create the "Speed" parameter beforehand in Unity's Animator window 
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        //If the player pressed W and is grounded, then we jump
        if (Input.GetKey(KeyCode.W) && IsGrounded())
        {
            animator.SetBool("Jumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        //If our Y-Axis velocity is 0, then it means we've hit the ground and are no longer jumping
        if (rb.velocity == new Vector2(rb.velocity.x, 0))
        {
            animator.SetBool("Jumping", false);
        }

        //Simply set the Crouching parameter of the Animator to true if we press S
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("Crouching", true);
        }

        //Sets the Crouching parameter to false as soon as we stop pressing S
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("Crouching", false);
        }

        //This function flips our player sprite by its Y-Axis. This allows us to look towards the direction we are moving to
        Flip();
    }

    private void FixedUpdate()
    {
        //Constantly updated our position based on our movement. This is how we move
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    //This function created a small circle at the groundCheck, to see if the circle overlaps with our groundLayer. True if it does and False if it doesn't
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //We flip the player horizontally
    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
