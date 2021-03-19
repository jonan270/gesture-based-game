using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Bjorn : Character
{

    protected override void Start()
    {
        base.Start();
        Element = ElementState.Fire;
        Name = "Bjorn";
        attackValue = 30;
        currentHealth = maxHealth;
        isAlive = true;

        descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Bj�rn will drink mead and ...";
        descriptionTextCard3 = "Bj�rn will do cleave and hurt multiple enemies.";
    }



    public void Attack(string whichAttack, Character ch) //Get input from which gesture has been done? And which character to attack?
    {
        if(whichAttack == "Berserk")
        {
            //TODO: animation
            ch.ModifyHealth(attackValue + 10);
        }
    }
}

