using System;
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

	public bool loadData(List<WeevilGenetics> weevilList)
	{
        //Getting which scene we are in
        int scene = PlayerPrefs.GetInt("Scene");
        string current_file = "scene" + scene + ".txt";



        return true;
	}

}

