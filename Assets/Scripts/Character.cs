using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]

public abstract class Character : ScriptableObject
{
    public int Health;
    public static int attackValue;
    public string Name;
    public static string Element;
    public bool isAlive;
    public State CurrentState;
    public GameObject characterModel;

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

    public void takeDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0) //Check if still alive
        {
            isAlive = false;
            Die();
        }
        else
            isAlive = true;
    }

    public void Die()
    {
        Debug.Log("Character died");
        Destroy(characterModel);
    }




}
