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

    public void Attack(string whichAttack, Character ch)
    {
        if(whichAttack == "Berserk")
        {
            ch.takeDamage(40);
        }
    }

    public void OnEnable()
    {
        card = ScriptableObject.CreateInstance<Card>();
        card.nameCharacter = "Bjorn";
        card.description = "Bj�rn will go berserk and do ... extra damage" + "\n" + "Bj�rn will drink mead and ..." + "Bj�rn will do cleave and hurt multiple enemies.";
        

    }


}

