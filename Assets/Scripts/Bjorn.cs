using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bjorn : Character
{
    public Bjorn()
    {
        Element = "fire";
        Health = 100;
        isAlive = true;
        Name = "Bjorn";
        attackValue = 30;
    }

    public void compareElement(string enemyElement)
    {
        if (enemyElement == "water")
        {
            attackValue -= 5;
        }
    }

    public void createCard()
    {

        //card.Abilities.Add(new Berserk());
        //card.AddToList(new Berserk());

        //card.Abilities.add(Berserk());

        //Abilities = List<Berserk, DrinkMead>;
    }
}

