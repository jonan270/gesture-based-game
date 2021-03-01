using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Health;
    public static int attackValue;
    public static string Name;
    public static string Element;
    public bool isAlive;
    public State CurrentState;
    public Card card;

    public enum State
    {
        ChooseAbilityMode,
        AttackMode
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void setState(State state) {
        CurrentState = state;
    }

    void takeDamage(int amount)
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

    void Die()
    {
        Debug.Log("Character died");
    }

    void Attack()
    {
        
    }

    public void createCard()
    {
        card = new Card();
        //card.ability1 = new Berserk();
        //card.Abilities.Add(new Berserk());
        //card.AddToList(new Berserk());

        //card.Abilities.add(Berserk());

        //Abilities = List<Berserk, DrinkMead>;
    }

}
