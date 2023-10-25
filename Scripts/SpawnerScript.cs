/*
 * This file is in charge of starting the round and ending the round
 * It will load data from a file to spawn the new generation of weevils,
 * and write to a file with how well the old generation of weevils did.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerScript : MonoBehaviour
{

    //Constants
    const int POP_SIZE = 10;

    private string[] ACTION_OPTIONS = {"right", "left", "jump", "shoot", "armor", "none"};

    private string[] REACTION_OPTIONS = { "away", "towards", "jump", "shoot", "armor", "none", "none", "none", "none", "none", "none"};


    private List<WeevilGenetics> deadWeevils;

    private List<WeevilGenetics> pastGeneration;

    private List<WeevilGenetics> newGeneration;


    //Weevil to copy for creation
    public GameObject WeevilObject;

    // Start is called before the first frame update
    void Start()
    {

        //Allocating space for the lists
        deadWeevils = new List<WeevilGenetics>();
        pastGeneration = new List<WeevilGenetics>();
        newGeneration = new List<WeevilGenetics>();

        SpawnWeevils();
    }

    //This is the main function that spawns all the weevils
    private void SpawnWeevils()
    {

        //Attempting to load previous generation data
        FileHandler handler = new FileHandler();

        bool loaded = handler.loadData(pastGeneration, POP_SIZE);

        if (loaded)
        {
            //This means we have past generation data
            //Time for the true genetic algorith!


            //The last two weevils in the list are the ones that score the highest,
            //so they will be selected to create the new generation
            WeevilGenetics bestWeevil = pastGeneration[POP_SIZE - 1];
            WeevilGenetics secondBestWeevil = pastGeneration[POP_SIZE - 2];

            //We will automatically add the best weevil to the new generation
            newGeneration.Add(bestWeevil);

            //Making seven children from the best and the next best ones
            for (int i = 0; i < 7; i++)
            {
                WeevilGenetics babyWeevil = makeChild(bestWeevil, pastGeneration[(POP_SIZE - 2) - i]);
                newGeneration.Add(babyWeevil);
            }

            //Making two "mutant" weevils
            WeevilGenetics mutantWeevil = getRandomWeevil();

            WeevilGenetics mutantBaby1 = makeChild(bestWeevil, mutantWeevil);
            WeevilGenetics mutantBaby2 = makeChild(secondBestWeevil, mutantWeevil);

            newGeneration.Add(mutantBaby1);
            newGeneration.Add(mutantBaby2);

            //Spawning all the weevils into the game
            foreach (var newWeevil in newGeneration)
            {
                spawnIndivdualWeevil(newWeevil.actions, newWeevil.reactions, newWeevil.traits);
            }

        }
        else
        {
            //Else we make a new random generation
            spawnRandomWeevils();
        }
        
        

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
        string[] actions = { "none", "none", "none", "none", "none", "none", "none", "none" };
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

        //Getting the current scene
        int scene = PlayerPrefs.GetInt("Scene");

        //Restarting the scene/round
        SceneManager.LoadScene("Scenario " + scene);

    }


    //This function "mates" two weevils to produce a child weevil
    private WeevilGenetics makeChild(WeevilGenetics weevil1, WeevilGenetics weevil2)
    {
        //Making the child weevil
        WeevilGenetics child = new WeevilGenetics();

        //Combining the actions
        for (int i = 0; i < 8; i++)
        {
            //This boolean decides randomly which of the parent's genetics will be handed down
            bool decider = (Random.value > 0.5f);

            if (decider)
            {
                child.actions[i] = weevil1.actions[i];
            }
            else
            {
                child.actions[i] = weevil2.actions[i];
            }
        }

        //Combining the reactions
        for (int i = 0; i < 2; i++)
        {
            //This boolean decides randomly which of the parent's genetics will be handed down
            bool decider = (Random.value > 0.5f);

            if (decider)
            {
                child.reactions[i] = weevil1.reactions[i];
            }
            else
            {
                child.reactions[i] = weevil2.reactions[i];
            }
        }

        //Combining the traits
        for (int i = 0; i < 3; i++)
        {
            //This boolean decides randomly which of the parent's genetics will be handed down
            bool decider = (Random.value > 0.5f);

            if (decider)
            {
                child.traits[i] = weevil1.traits[i];
            }
            else
            {
                child.traits[i] = weevil2.traits[i];
            }
        }

        return child;
    }


    //*********************Public functions for setting and resetting************************//
    public void restart()
    {
        FileHandler writer = new FileHandler();

        //Making a list of random weevils
        List<WeevilGenetics> randomList = new List<WeevilGenetics>();
        for (int i = 0; i < POP_SIZE; i++)
        {
            randomList.Add(getRandomWeevil());
        }

        //Writing them to the file
        writer.saveData(randomList);

        //Getting the current scene
        int scene = PlayerPrefs.GetInt("Scene");

        //Restarting the scene/round
        SceneManager.LoadScene("Scenario " + scene);


    }

    public void loadLaterData()
    {
        FileHandler handler = new FileHandler();

        //Making a list of weevils
        List<WeevilGenetics> laterGeneration = new List<WeevilGenetics>();

        bool canLoad = handler.loadData(laterGeneration, POP_SIZE, "X");

        if (canLoad)
        {
            //Saving the later generation to the current file
            handler.saveData(laterGeneration);

            //Getting the current scene
            int scene = PlayerPrefs.GetInt("Scene");

            //Restarting the scene/round
            SceneManager.LoadScene("Scenario " + scene);

        }
        else
        {
            print("No future data");
        }

        
    }


}
