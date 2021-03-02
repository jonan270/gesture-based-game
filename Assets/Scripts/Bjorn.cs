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
        //card = new Card(new Berserk(),new DrinkMead(), new Cleave());
        
    }

    public void compareElement(string enemyElement)
    {
        if (enemyElement == "water")
        {
            attackValue -= 5;
        }
    }

    void Attack(string whichAttack)
    {
        if(whichAttack == "Berserk")
        {
            
        }
    }


}

