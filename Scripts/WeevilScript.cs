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
    private string[] actions = {"right", "right", "left", "left", "right", "right", "left", "left"};

    //This determines how long the weevil actions last
    private int[] action_times = { 100,100,100,100,100,100,100,100 };

    //This determines the reactions of the weevils to things
    //                             food    death    
    private string[] reactions = {"jump", "armor"};

    //This determines what physical traits the weevils have
    //                       Armor  Lasers Agility
    private bool[] traits = {false, false, false};


    //************| Other things |****************

    //Constants
    const int MIN_ACTION_TIME = 20;
    const float WEEVIL_SPEED = 3f;
    const float JUMP_HEIGHT = 8f;
    const float BULLET_SPEED = 15f;
    const float RAYCAST_LENGTH = 0.1f;

    const int FULL_FOOD = 15 * 60;
    const int FULL_FOOD_DECREASED = 12 * 60;
    const int FOOD_VALUE = 10 * 60;

    const int POINT_VALUE = 5 * 60;

    const int DEATH_SPEED = 60;


    //This is a constant set by physical traits
    private float SPEED_MODIFIER = 1f;


    //Non-constants
    private int action_timer = 0;
    private int action_counter = 0;

    private int food_level = FULL_FOOD;

    private Sprite default_texture;
    private Sprite armor_texture;

    private int death_level = 0;

    private int life_score = 0;

    private int current_action_time = 0;


    //Laser prefab
    public GameObject laserPrefab;

    
    //Action variables
    private string next_action = "";

    private int moving = 0;
    private bool jumping = false;
    private bool armored = false;
    private bool dying = false;






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
        action_timer = Random.Range(0, 100);

        //Loading the armor texture
        armor_texture = Resources.Load<Sprite>("Textures/Curled_Weevil");

        //Making sure the weevils are full
        food_level = FULL_FOOD;

        //If it is the smaller, quicker weevil, then it can't hold nearly as much food
        if (traits[2])
        {
            food_level = FULL_FOOD_DECREASED;
        }


        //It takes two seconds for a weevil to die
        death_level = DEATH_SPEED;

        life_score = 0;

    }


    // FixedUpdate is called at regular intervals
    void FixedUpdate()
    {
        food_level -= 1;
        if (food_level <= 0)
        {
            dying = true;
        }

        life_score += 1;

        startActions();
        completeActions();
    }

    //This will make the weevil act every time the timer reaches 0
    void startActions()
    {

        
        action_timer--;
        if (action_timer <= 0)
        {
            
            //Now that the timer has reached 0, it is time to do the next action

            //If the next action hasn't been determined yet,
            //then the weevil should just do its next regular action.
            if (next_action == "")
            {
                action_timer = action_times[action_counter];

                next_action = actions[action_counter];

                //Incrementing the action counter,
                //and making sure it keeps looping once it gets to the end of the array
                action_counter++;
                if (action_counter == actions.Length)
                {
                    action_counter = 0;
                }
            }
            else
            {
                //Setting the timer to the default time in case the weevil is reacting to something
                action_timer = 100;

            }

            //Remembering what the current action time is
            current_action_time = action_timer;

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
                    startArmor();
                    break;
                case "shoot":
                    shoot();
                    break;
                case "none":
                    //Sometimes it might just need to do nothing
                    break;
            }

            //Clearing the action
            next_action = "";

        }


    }

    //This funtion makes the weevil shoot a laser from its eyes
    private void shoot()
    {
        //Making sure it has laser eyes before it shoots
        if (traits[1] == false) { return; }

        //Creating the gameobject
        GameObject newLaser = Instantiate(laserPrefab);

        //Getting the position to shoot from
        Transform EyePosition = transform.GetChild(0);

        newLaser.transform.position = EyePosition.position;


        //Getting the direction
        float direction_multiplier = 1f;

        if (transform.localScale.x < 0)
        {
            direction_multiplier = -1f;
        }

        //Setting the speed
        newLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(BULLET_SPEED * direction_multiplier, 0);


    }

    //Function in charge of making the weevil curl up with armor
    private void startArmor()
    {
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
    }


    //This function is in charge of continuous actions, such as moving
    private void completeActions()
    {
        //Moving the weevil along the ground if that is it's current action
        if (moving != 0 && grounded())
        {
            Vector2 previous_velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moving * WEEVIL_SPEED * SPEED_MODIFIER, previous_velocity.y);
        }
        if (jumping && grounded())
        {
            Vector2 previous_velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(previous_velocity.x, JUMP_HEIGHT * SPEED_MODIFIER);
            jumping = false;
        }

        if (dying)
        {
            death_level -= 1;



            float death_ratio = (float)death_level / (float)DEATH_SPEED;

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, death_ratio);

            if (death_level <= 0)
            {
                die();
            }
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

    //This function determines whether or not the weevil is on the ground or not
    private bool grounded()
    {
        //Getting what the raycast should hit
        LayerMask layer = LayerMask.GetMask("Ground");

        //Getting the positions to start raycast from
        Vector3 leg1 = transform.GetChild(1).transform.position;
        Vector3 leg2 = transform.GetChild(2).transform.position;
        Vector3 leg3 = transform.GetChild(3).transform.position;

        //Getting the raycasts from each leg
        RaycastHit2D leg1_hit = Physics2D.Raycast(leg1, -Vector2.up, RAYCAST_LENGTH, layer);
        RaycastHit2D leg2_hit = Physics2D.Raycast(leg2, -Vector2.up, RAYCAST_LENGTH, layer);
        RaycastHit2D leg3_hit = Physics2D.Raycast(leg3, -Vector2.up, RAYCAST_LENGTH, layer);

        if (leg1_hit || leg2_hit || leg3_hit)
        {
            return true;
        }

        return false;
    }


    //This event will be triggered once the outer collider of the weevil hits something
    //We will use this to make the weevil react to things
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //In this case, we only want there to be a reaction if we aren't already reacting to something else
        if (next_action == "")
        {

            //Layers 8 and 9 are all things the weevil needs to react to
            if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
            {

                //Layer 8 is danger
                if (collision.gameObject.layer == 8)
                {
                    //Reactions[1] is the weevil's reaction to danger
                    next_action = reactions[1];
                }

                //Layer 9 is food
                if (collision.gameObject.layer == 9)
                {
                    //Reactions[0] is the weevil's reaction to food
                    next_action = reactions[0];
                }


                //In the event that the reaction demands going a certain direction,
                //we need to translate that to completable actions
                if (next_action == "away")
                {
                    //In this case, we need to go left
                    if (collision.transform.position.x > transform.position.x)
                    {
                        next_action = "left";
                    }
                    else
                    {
                        //Otherwise right
                        next_action = "right";
                    }
                }
                if (next_action == "towards")
                {
                    //In this case, we need to go right
                    if (collision.transform.position.x > transform.position.x)
                    {
                        next_action = "right";
                    }
                    else
                    {
                        //Otherwise left
                        next_action = "left";
                    }
                }

                //Making the weevil not react if the reaction is "none"
                if (next_action == "none")
                {
                    next_action = "";
                    return;
                }
                //Since it is a reaction, the weevil will act quicker
                //We need to make sure it doesn't immediately interrupt the previous action

                int time_to_remove = current_action_time - MIN_ACTION_TIME;

                action_timer -= time_to_remove;
            }

        }
    }

    //This will be called when the weevil hits something that is solid
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This means the weevil has hit something dangerous
        if (collision.gameObject.layer == 8)
        {
            if (!armored)
            {
                dying = true;
            }
        }

        //This means the weevil has found food
        if (collision.gameObject.layer == 9)
        {
            
            //Checking to make sure the weevil isn't full before it eats
            if (traits[2])
            {
                //The smaller weevils can't handle as much food
                if (food_level >= FULL_FOOD_DECREASED)
                {
                    return;
                }
            }
            else
            {
                //Bigger weevils can
                if (food_level >= FULL_FOOD)
                {
                    return;
                }
            }

            //Checking to see if the donut is food or points
            if (collision.gameObject.GetComponent<DonutScript>().isFood)
            {
                food_level += FOOD_VALUE;
            }
            else
            {
                life_score += POINT_VALUE;
            }

            //Killing the donut
            collision.gameObject.GetComponent<DonutScript>().die();
        }

    }

    //In the event that the weevil is touching something dangerous,
    //and lets go of armor, it should die
    //This could also apply to situations where a weevil is touching food
    private void OnCollisionStay2D(Collision2D collision)
    {
        //This means the weevil is touching something dangerous
        if (collision.gameObject.layer == 8)
        {
            if (!armored)
            {
                dying = true;
            }
        }

        //This means the weevil is touching food
        if (collision.gameObject.layer == 9)
        {

            //Checking to make sure the weevil isn't full before it eats
            if (traits[2])
            {
                //The smaller weevils can't handle as much food
                if (food_level >= FULL_FOOD_DECREASED)
                {
                    return;
                }
            }
            else
            {
                //Bigger weevils can
                if (food_level >= FULL_FOOD)
                {
                    return;
                }
            }

            //Checking to see if the donut is food or points
            if (collision.gameObject.GetComponent<DonutScript>().isFood)
            {
                food_level += FOOD_VALUE;
            }
            else
            {
                life_score += POINT_VALUE;
            }

            //Killing the donut
            collision.gameObject.GetComponent<DonutScript>().die();
        }
    }

    //This function causes the weevil to die
    private void die()
    {
        //Giving the spawner weevil data
        GameObject.Find("Weevil Spawner").GetComponent<SpawnerScript>().acceptDeadWeevil(actions, action_times, reactions, traits, life_score);

        //Truly destroying the weevil
        Destroy(gameObject);
    }


    //These next three functions are for setting the options
    public void setActions(string[] actions)
    {
        this.actions = actions;
    }
    public void setActionTimes(int[] action_times)
    {
        this.action_times = action_times;
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
