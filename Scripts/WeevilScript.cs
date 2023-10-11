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
    string[] actions = {"right", "right", "left", "left"};

    //This determines the reactions of the weevils to things
    //                     food    death    
    string[] reactions = {"jump", "armor"};

    //This determines what physical traits the weevils have
    //               Armor  Lasers Agility
    bool[] traits = {false, false, false};


    //************| Other things |****************

    //Constants:
    const int ACTION_TIME = 100;
    const int MIN_ACTION_TIME = 20;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called at regular intervals
    void FixedUpdate()
    {
        
    }
}
