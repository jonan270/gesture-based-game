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
        descriptionTextCard1 = "Hilda will...";
        descriptionTextCard2 = "Hilda will...";
        descriptionTextCard3 = "Hilda will...";

        //Instantiate card for character
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card.prefab", typeof(GameObject));
        GameObject c1 = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;


        //add description text for card
        c1.GetComponent<Card>().description.text = descriptionTextCard1;
        c1.GetComponent<Card>().nameText.text = Name;


        currentHealth = maxHealth;

        healthBar = HealthBar.Create(new Vector3(0, 6), new Vector3(0.1f, 0.01f)); // Set position as character position but above it
        healthBar.SetSize(currentHealth);

    }

}
