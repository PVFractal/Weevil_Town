using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //Constants
    const int POP_SIZE = 10;

    public GameObject WeevilObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This is the main function that spawns all the weevils
    private void SpawnWeevils()
    {

        string[] actions = { "right", "right", "left", "left" };
        string[] reactions = { "jump", "armor" };
        bool[] traits = { false, false, false };

        GameObject newWeevil = Instantiate(WeevilObject) as GameObject;

        for (int i = 0; i < POP_SIZE; i++)
        {

        }
    }
}
