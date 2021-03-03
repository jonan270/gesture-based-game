using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Character", menuName = "Character")]

public abstract class Character : MonoBehaviour
{
    public HealthBar healthBar;
    public float currentHealth;
    public float maxHealth = 100;
    public static int attackValue;
    public string Name;
    public static string Element;
    public bool isAlive;
    public State CurrentState;
   // public GameObject characterModel;

    public string descriptionText;
    public string attackText;
    public string healthText;

    public Card card;

    public enum State
    {
        LookAtCard, // "Idle" mode, Character standing still
        Walk, //walking mode
        AttackMode // Attack mode, Character is about to perform an attack
    }

    public void setState(State state) {
        CurrentState = state;
    }

    public bool checkHealth()
    {

        if (currentHealth <= 0) //Check if still alive
        {
            isAlive = false;
        }
        else
            isAlive = true;
        return isAlive;
    }

    public float ModifyHealth(int amount)
    {
        currentHealth -= amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth; //Calculate current health percentage

        return currentHealthPct;
    }





}
