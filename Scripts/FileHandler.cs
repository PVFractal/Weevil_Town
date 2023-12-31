﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler
{

	const string MAIN_PATH = "Assets/Resources/Data/";
	public FileHandler()
	{

	}

	public void saveData(List<WeevilGenetics> weevilList)
	{
		//Getting which scene we are in
		int scene = PlayerPrefs.GetInt("Scene");
		string current_file = "scene" + scene + ".txt";

		//The 'false' in the parameter means we are starting a new file or overwriting
		StreamWriter writer = new StreamWriter(MAIN_PATH + current_file, false);

		//Going through the list of weevils and writing their genetics down
		foreach (var weevil in weevilList)
		{

			//Writing all actions
			foreach (var action in weevil.actions)
			{
				writer.WriteLine(action);
			}

            //Writing all action times
            foreach (var action_time in weevil.action_times)
            {
                writer.WriteLine(action_time);
            }

            //Writing all reactions
            foreach (var reaction in weevil.reactions)
            {
                writer.WriteLine(reaction);
            }

            //Writing all traits
            foreach (var trait in weevil.traits)
            {
                writer.WriteLine(trait);
            }


            //Writing the score last
            writer.WriteLine(weevil.score);
        }

		//Closing the file
		writer.Close();
	}

	public bool loadData(List<WeevilGenetics> weevilList, int POP_SIZE, string later_data = "")
	{
        //Getting which scene we are in
        int scene = PlayerPrefs.GetInt("Scene");

        //If later data is not "", then we are loading future generations
        string current_file = "scene" + later_data + scene + ".txt";

		StreamReader reader;

        //Trying to open the file
        try
        {
            reader = new StreamReader(MAIN_PATH + current_file);
        }
        catch
        {
            //If the data doesn't exist, we return false
            return false;
        }


        for (int i = 0; i < POP_SIZE; i++)
        {
            //Allocating new weevil
            WeevilGenetics newWeevil = new WeevilGenetics();

            //Loading all the actions
            for (int j = 0; j < 8; j++)
            {
                newWeevil.actions[j] = reader.ReadLine();
            }

            //Loading all the action times
            for (int j = 0; j < 8; j++)
            {
                newWeevil.action_times[j] = int.Parse(reader.ReadLine());
            }

            //Loading all the reactions
            for (int j = 0; j < 2; j++)
            {
                newWeevil.reactions[j] = reader.ReadLine();
            }

            //Loading all the traits
            for (int j = 0; j < 3; j++)
            {
                string trait = reader.ReadLine();
                newWeevil.traits[j] = true;
                if (trait == "False")
                {
                    newWeevil.traits[j] = false;
                }
            }

            //Loading the score
            newWeevil.score = int.Parse(reader.ReadLine());

            //Adding the weevil to the list
            weevilList.Add(newWeevil);

        }

       



        //Closing the reader
        reader.Close();

        return true;
	}



}

