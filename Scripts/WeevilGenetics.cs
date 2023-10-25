/*
 * This class holds data for weevil genetics
 */

using System;
public class WeevilGenetics
{
    //************| Weevil traits |****************

    //This determines the regular actions of the weevils
    public string[] actions = { "right", "right", "left", "left", "right", "right", "left", "left" };

    //This determines the reactions of the weevils to things
    //                             food    death    
    public string[] reactions = { "jump", "armor" };

    //This determines what physical traits the weevils have
    //                       Armor  Lasers Agility
    public bool[] traits = { false, false, false };

    public int score;

    public WeevilGenetics(string[] actions, string[] reactions, bool[] traits, int score)
	{
        this.actions = actions;
        this.reactions = reactions;
        this.traits = traits;
        this.score = score;
	}

    public WeevilGenetics()
    {
    }


}

