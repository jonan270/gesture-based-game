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
    //public Card card;

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


    void checkHealth()
    {
        if (Health == 0)
        {
            isAlive = false;
        }
        else
            isAlive = true;
    }




}
