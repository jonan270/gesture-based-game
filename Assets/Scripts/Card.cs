using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour // Shows a card specifik for the character
{
    public List<Ability> Abilities;

    
    public Card() {
        
    }

    public void AddToList(Ability ab)
    {
        Abilities.Add(ab);
    }

}

