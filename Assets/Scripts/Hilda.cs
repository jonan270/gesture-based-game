using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Hilda : Character
{
    public Hilda()
    {
        Element = "earth";
        isAlive = true;
        Name = "Hilda";
        attackValue = 15;
        descriptionTextCard1 = "Hilda will conjure a potion";
        descriptionTextCard2 = "Hilda will see the future";
        descriptionTextCard3 = "Hilda will summon a raven";
    }
    
    public void compareElement(string enemyElement)
    {
        if (enemyElement == "fire")
        {
            attackValue -= 5;
        }
    }

    public void OnEnable()
    {


        //Instantiate card for character
       

        currentHealth = maxHealth;

        healthBar = HealthBar.Create(new Vector3(0, 6), new Vector3(0.08f, 0.01f)); // Set position as characters position in x
        healthBar.SetSize(currentHealth);

    }

}
