/*
 * This is the script that each weevil has.
 * Each weevil has "genetic" traits and characteristics that determine how they act and react.
 * Ideally, each weevil will inherit the best traits, and become competant at surviving.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeevilScript : MonoBehaviour
{

    //************| Weevil traits |****************

    //This determines the regular actions of the weevils
    private string[] actions = {"right", "right", "left", "left"};

    //This determines the reactions of the weevils to things
    //                             food    death    
    private string[] reactions = {"jump", "armor"};

    //This determines what physical traits the weevils have
    //                       Armor  Lasers Agility
    private bool[] traits = {false, false, false};


    //************| Other things |****************

    //Constants
    const int ACTION_TIME = 100;
    const int REACTION_TIME = 80;
    const float WEEVIL_SPEED = 3f;
    const float JUMP_HEIGHT = 8f;

    //This is a constant set by physical traits
    private float SPEED_MODIFIER = 1f;


    //Non-constants
    private int action_timer = ACTION_TIME;
    private int action_counter = 0;

    private Sprite default_texture;
    private Sprite armor_texture;

    
    //Action variables
    private string next_action = "";

    private bool grounded = false;
    private int moving = 0;
    private bool jumping = false;
    private bool armored = false;




    // Start is called before the first frame update
    void Start()
    {
        //Determining what the weevils look like based on their physical traits


        //Default texture
        default_texture = Resources.Load<Sprite>("Textures/Weevil");
        
        if (traits[0] == false && traits[1] == true)
        {
            default_texture = Resources.Load<Sprite>("Textures/Evil_Weevil");
        }
        else if (traits[0] == true && traits[1] == false)
        {
            default_texture = Resources.Load<Sprite>("Textures/Armor_Weevil");
        }
        else if (traits[0] == true && traits[1] == true)
        {
            default_texture = Resources.Load<Sprite>("Textures/Evil_Armor_Weevil");
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = default_texture;

        //Scaling it down if it is supposed to be more agile
        if (traits[2] == true)
        {
            transform.localScale = new Vector3(0.75f, 0.75f);
        }

        //Increasing or decreasing the speed based on physical traits

        if (traits[0] == true)
        {
            //Decreasing speed if it has lasers
            SPEED_MODIFIER *= 0.9f;
        }
        if (traits[1] == true)
        {
            //Decreasing speed if it has armor
            SPEED_MODIFIER *= 0.8f;
        }
        if (traits[2] == true)
        {
            //Increasing speed if it has agility
            SPEED_MODIFIER *= 1.25f;
        }

        //Setting the delay before the first action
        action_timer = Random.Range(0, ACTION_TIME);

        //Loading the armor texture
        armor_texture = Resources.Load<Sprite>("Textures/Curled_Weevil");
    }


    // FixedUpdate is called at regular intervals
    void FixedUpdate()
    {
        startActions();
        completeActions();
    }

    //This will make the weevil act every time the timer reaches 0
    void startActions()
    {

        
        action_timer--;
        if (action_timer <= 0)
        {
            action_timer = ACTION_TIME;
            //Now that the timer has reached 0, it is time to do the next action

            //If the next action hasn't been determined yet,
            //then the weevil should just do its next regular action.
            if (next_action == "")
            {
                next_action = actions[action_counter];

                //Incrementing the action counter,
                //and making sure it keeps looping once it gets to the end of the array
                action_counter++;
                if (action_counter == actions.Length)
                {
                    action_counter = 0;
                }
            }

            //Making sure that actions don't carry over
            stopActions();

            Vector3 previous_scale = transform.localScale;

            //Translating strings into actual actions
            switch (next_action)
            {
                case "right":
                    moving = 1;
                    //This next line changes the direction of the weevil to match it's movement
                    //In this case, it makes it face right
                    
                    transform.localScale = new Vector3(Mathf.Abs(previous_scale.x), previous_scale.y);

                    break;
                case "left":
                    moving = -1;
               
                    //Similar to the previous line, but facing left
                    transform.localScale = new Vector3(-Mathf.Abs(previous_scale.x), previous_scale.y);

                    break;
                case "jump":
                    jumping = true;
                    break;
                case "armor":
                    //Making sure the weevil has armor before it can do anything
                    if (traits[0])
                    {
                        armored = true;
                        gameObject.GetComponent<SpriteRenderer>().sprite = armor_texture;
                        //Making sure the capsule collider disappears, which will shrink the physical body
                        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                        //Making sure it can't physically roll
                        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    break;

            }

            //Clearing the action
            next_action = "";

        }


    }

    //This function is in charge of continuous actions, such as moving
    private void completeActions()
    {
        //Moving the weevil along the ground if that is it's current action
        if (moving != 0 && grounded)
        {
            Vector2 previous_velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moving * WEEVIL_SPEED * SPEED_MODIFIER, previous_velocity.y);
        }
        if (jumping && grounded)
        {
            Vector2 previous_velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(previous_velocity.x, JUMP_HEIGHT * SPEED_MODIFIER);
            jumping = false;
        }

    }

    //This function is in charge of stopping the weevil's current action
    private void stopActions()
    {
        //Stopping movement
        moving = 0;

        jumping = false;

        armored = false;

        gameObject.GetComponent<SpriteRenderer>().sprite = default_texture;

        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }


    //This is called once per frame when the trigger is touching another collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        //7 is the layer of the ground, so now we know if the weevil is touching the ground
        if (collision.gameObject.layer == 7)
        {
            grounded = true;
        }
    }

    //This is called when the trigger leaves a collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the weevil's trigger is no longer touching the ground,
        //then we need to update the variable to say so
        if (collision.gameObject.layer == 7)
        {
            grounded = false;
        }
    }


    //These next three functions are for setting the options
    public void setActions(string[] actions)
    {
        this.actions = actions;
    }
    public void setReactions(string[] reactions)
    {
        this.reactions = reactions;
    }
    public void setTraits(bool[] traits)
    {
        this.traits = traits;
    }


}
