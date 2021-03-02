using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject // Shows a card specifik for the character
{
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;

    public Card(Ability ab1, Ability ab2, Ability ab3) {
        ability1 = ab1;
        ability2 = ab2;
        ability3 = ab3;
    }
}

