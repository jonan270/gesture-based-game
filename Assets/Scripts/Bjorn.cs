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
        descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Björn will drink mead and ...";
        descriptionTextCard3 = "Björn will do cleave and hurt multiple enemies.";

        //Instantiate card for character
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card.prefab", typeof(GameObject));
        GameObject c1 = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;


        //add description text for card
        c1.GetComponent<Card>().description.text = descriptionTextCard1;
        c1.GetComponent<Card>().nameText.text = Name;
       

        currentHealth = maxHealth;
        
        healthBar = HealthBar.Create(new Vector3(0, 6), new Vector3(0.1f , 0.01f)); // Set position as character position but above it
        healthBar.SetSize(currentHealth);

        //Card card = hand.add(ScriptableObject.CreateInstance<Card>());

        //card.description = "Björn will go berserk and do ... extra damage" + "\n" + "Björn will drink mead and ..." + "\n" + "Björn will do cleave and hurt multiple enemies.";
        //c1 = ScriptableObject.CreateInstance<Card>();

    }


}

