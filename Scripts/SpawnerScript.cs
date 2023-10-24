/*
 * This file is in charge of starting the round and ending the round
 * It will load data from a file to spawn the new generation of weevils,
 * and write to a file with how well the old generation of weevils did.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //Constants
    const int POP_SIZE = 10;

    private string[] ACTION_OPTIONS = {"right", "left", "jump", "shoot", "armor", "none"};

    private string[] REACTION_OPTIONS = { "away", "towards", "jump", "shoot", "armor", "none", "none", "none", "none", "none", "none"};


    private List<WeevilGenetics> deadWeevils;


    //Weevil to copy for creation
    public GameObject WeevilObject;

    // Start is called before the first frame update
    void Start()
    {

        //Allocating space for the list
        deadWeevils = new List<WeevilGenetics>();


        SpawnWeevils();
    }

    //This is the main function that spawns all the weevils
    private void SpawnWeevils()
    {
        
        spawnRandomWeevils();

        //Hiding the Weevil Spawner
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    //This will spawn a bunch of random weevils according to the population size
    private void spawnRandomWeevils()
    {

        //Not much of a function right now, but might be more complicated later
        for (int i = 0; i < POP_SIZE; i++)
        {
            var newWeevil = getRandomWeevil();
            spawnIndivdualWeevil(newWeevil.actions, newWeevil.reactions, newWeevil.traits);
        }
    }

    //Returns random weevil genetics
    private WeevilGenetics getRandomWeevil()
    {
        string[] actions = { "none", "none", "none", "none" };
        string[] reactions = { "none", "none" };
        bool[] traits = { false, false, false };

        //Setting the actions
        for (int i = 0; i < actions.Length; i++)
        {
            //Getting a random index
            int r = Random.Range(0, ACTION_OPTIONS.Length - 1);

            //Setting the action to a random action
            actions[i] = ACTION_OPTIONS[r];
        }

        //Setting the reactions
        for (int i = 0; i < reactions.Length; i++)
        {
            //Getting a random index
            int r = Random.Range(0, REACTION_OPTIONS.Length - 1);

            //Setting the action to a random action
            reactions[i] = REACTION_OPTIONS[r];
        }

        //Setting the traits
        for (int i = 0; i < traits.Length; i++)
        {
            //Getting a random index
            int r = Random.Range(0, 2);

            //Setting the traits
            if (r == 1) { traits[i] = true; }

        }

        return new WeevilGenetics(actions, reactions, traits, 0);

    }


    //This function will spawn a single weevil given genetic traits
    private void spawnIndivdualWeevil(string[] actions, string[] reactions, bool[] traits)
    {

        //Creating the new object from the template (WeevilObject)
        GameObject newWeevil = Instantiate(WeevilObject);

        //Assigning it the genetic traits
        newWeevil.GetComponent<WeevilScript>().setActions(actions);
        newWeevil.GetComponent<WeevilScript>().setReactions(reactions);
        newWeevil.GetComponent<WeevilScript>().setTraits(traits);

        //Setting the weevil's position to the spawner's position
        newWeevil.transform.position = transform.position;
    }

    //Every time a weevil dies, it should send the data here
    public void acceptDeadWeevil(string[] actions, string[] reactions, bool[] traits, int score)
    {
        var newDeadWeevil = new WeevilGenetics(actions, reactions, traits, score);
        //Putting the newly dead weevil into the list
        deadWeevils.Add(newDeadWeevil);
        //This means all the weevils have died
        if (deadWeevils.Count == POP_SIZE)
        {
            endGame();
        }
    }

    //This is in charge of saving data and ending the round
    private void endGame()
    {
        FileHandler writer = new FileHandler();

        writer.saveData(deadWeevils);

    }


}
