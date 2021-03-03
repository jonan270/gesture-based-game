using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject // Shows a card specifik for the character
{
    public string description;
    public string nameCharacter;
    public int attack;
    public Ability ability1;

    /*public Card(Ability ab1, Ability ab2, Ability ab3) {
        ability1 = ab1;
        ability2 = ab2;
        ability3 = ab3;
    }
    */
    public Card()
    {

    }

    public void Print()
    {
        Debug.Log(name + ": " + description);
    }
}

