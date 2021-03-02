using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Script : MonoBehaviour
{
    //gives my Player the ability to interact with other game elements using physics

    private Rigidbody2D myRigidbody;

    //this is set to public so I can interact with it in the inspector.  It will give the Player variable speed.

    public Animator myAnimator;

    public float movementSpeed;

    //can be set to true or false to change the Players facing direction

    private bool facingRight; 

    //SerializeField allows the variable to show in the Unity Inspector

    //an array that will create points that will indicate contact between Player and Ground

    [SerializeField]

    private Transform[] groundPoints; 

    /* be sure to create 3 child gameobjects inside the Player object. name them groundpoint_lt, groundpoint_rt, and groundpoint_center

    align them so that they are at the feet of the character. remember to assign these in the inspector of unity.

    */

    //variable to set the size of the contact points between the Ground and Player

    [SerializeField]

    private float groundRadius; 

    //Object mask that allows us to specify platforms as Ground

    [SerializeField]

    private LayerMask whatIsGround;

    //can be set to true or false based on contact with Ground

    private bool isGrounded; 

    //can be set to true or false to get Player to jump

    private bool jump;

    //allows a value to be entered in the inspector to set how high Player jumps

    [SerializeField]

    private float jumpForce;

    public bool isAlive;

    public GameObject reset;
    
    private Slider healthBar;

    public float health = 10f;

    private float healthBurn = 2f;



// Use this for initialization

    void Start () 

    {    //initial value to set the Player facing right

        facingRight = true;

        //associates the Rigidbody component of the Player with a variable we can use later on

        myRigidbody = GetComponent<Rigidbody2D> ();
        myAnimator = GetComponent<Animator> ();
        isAlive = true;
        reset.SetActive(false);
        healthBar = GameObject.Find ("health slider").GetComponent<Slider>();
        healthBar.minValue = 0f;
        healthBar.maxValue = health;
        healthBar.value = healthBar.maxValue;


       

    }

    void Update(){
        HandleInput ();
    }

    

    /* Update is called once per frame.  

Fixed Update locks in speed and performance regardless of console performance and quality*/

    void FixedUpdate () 

    {

        //access the keyboard controls and move left and right

        float horizontal = Input.GetAxis ("Horizontal");   
        if (isAlive){
        //just to see what is being reported by the keyboard on the console

        //Debug.Log (horizontal);

        
        
        
//calling the function in the game 

//controls Player on the x and y axis

        HandleMovement (horizontal); 

        HandleInput();
        //controls player facing direction

        Flip(horizontal);
        }
        else{
            return;
        }

        //loads the return of the IsGrounded function into the variable as true or false

        isGrounded = IsGrounded();

        //controls all keyboard input for jumping, shooting, and attacking

        

    

    }


    private void HandleMovement(float horizontal)

    {

        if(isGrounded && jump){ //if Player is both on the Ground AND Space Bar is pressed

            isGrounded = false; //turns on vertical movement
            jump = false;

            //moves Player vertically with the amount of force set in the jumpForce variable

            myRigidbody.AddForce(new Vector2(0,jumpForce)); 

            

        }

            //moves the Player on x axis and y axis

myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
myAnimator.SetFloat ("Speed", Mathf.Abs (horizontal));


    }

    private void Flip(float horizontal) //4th

    {

        //logical test to make sure that we are changing his facing direction

        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 

        {

            facingRight = !facingRight; //this sets the value of facingRight to its opposite

            Vector3 theScale = transform.localScale; //this accesses the local player scale component

            theScale.x *= -1;  //multiplying the x value of scale by -1 allows us to get either 1 or -1 as the result

            transform.localScale = theScale; //this reports the new value to the player's scale component

        }

    }

    private void HandleInput()

    {

        if(Input.GetKeyDown(KeyCode.Space)){

            jump = true; //if Space bar is pressed set the jump bool to true

            myAnimator.SetBool("Jumping", true);
            

            }

    }


        /* be sure to create 3 child gameobjects to the player object. name them groundpoint_lt, groundpoint_rt, and groundpoint_center

    align them so that they are at the feet of the character.

*/


    private bool IsGrounded()

    {

        if (myRigidbody.velocity.y <= 0) { 

//if player is not moving vertically test each of Player’s groundpoints for contact with the Ground

            foreach (Transform point in groundPoints) {

                Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

                for (int i = 0; 1 < colliders.Length; i++) {

                    if(colliders[i].gameObject != gameObject)

//if any of the groundpoints is in contact(collides) with anything other than the Player, return true

                        {

                            return true;

                        }

                    }

                }

            }

        return false;

        }

        void UpdateHealth(){
            if (health > 0){
                health -= healthBurn;
                healthBar.value = health;
            }
            if (health <= 0) {
             isAlive = false;
            }
        }

        void OnCollisionEnter2D(Collision2D target){
        if (target.gameObject.tag == "Ground") {
        myAnimator.SetBool ("Jumping", false);
            
        }
        if (target.gameObject.tag == "deadly") {
        health = 0;
        healthBar.value = health;
        isAlive = false;
        myAnimator.SetBool ("dead", true);
         reset.SetActive(true);   
         }
         
        if (target.gameObject.tag == "damage") {
        UpdateHealth();
        }
    }
}

    