using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bjorn : Character
{

    public Bjorn()
    {
        Element = "fire";
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
        }
    }

    public void OnEnable()
    {
        //Instantiate characterModel here with prefab???
        //Instantiate(characterModel, new Vector3(0, 0, 0), Quaternion.identity);
        currentHealth = maxHealth;

        healthBar = HealthBar.Create(new Vector3(0, 0), new Vector3(10 , 1)); // Set position as character position but above it
        healthBar.SetSize(currentHealth);

        card = ScriptableObject.CreateInstance<Card>();
        card.nameCharacter = "Bjorn";
        card.description = "Björn will go berserk and do ... extra damage" + "\n" + "Björn will drink mead and ..." + "\n" + "Björn will do cleave and hurt multiple enemies.";
        

    }


}

