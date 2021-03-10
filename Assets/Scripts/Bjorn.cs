using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Bjorn : Character
{

    public Bjorn()
    {
        Element = "fire";
        isAlive = true;
        Name = "Bjorn";
        attackValue = 30;
        descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Björn will drink mead and ...";
        descriptionTextCard3 = "Björn will do cleave and hurt multiple enemies.";

    }

    public void compareElement(string enemyElement)
    {
        if (enemyElement == "water")
        {
            attackValue -= 5;
        }
    }

    public void Attack(string whichAttack, Character ch) //Get input from which gesture has been done? And which character to attack?
    {
        if(whichAttack == "Berserk")
        {
            //do animation
            ch.ModifyHealth(attackValue + 10);
        }
    }

    public void OnEnable()
    {

        currentHealth = maxHealth;
        
        healthBar = HealthBar.Create(new Vector3(0, 6), new Vector3(0.08f, 0.01f)); // Set position as character position but above it
        healthBar.SetSize(currentHealth);


    }


}

