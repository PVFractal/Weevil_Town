using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //Constants
    const int POP_SIZE = 10;

    //Weevil to copy for creation
    public GameObject WeevilObject;

    // Start is called before the first frame update
    void Start()
    {
        SpawnWeevils();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This is the main function that spawns all the weevils
    private void SpawnWeevils()
    {

        string[] actions = { "shoot", "right", "shoot", "left" };
        string[] reactions = { "jump", "armor" };
        bool[] traits = { false, true, false };
        bool[] traits2 = { false, false, true };

        spawnIndivdualWeevil(actions, reactions, traits);
        //spawnIndivdualWeevil(actions, reactions, traits2);

        for (int i = 0; i < POP_SIZE; i++)
        {

        }

        //Hiding the Weevil Spawner
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
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
}
