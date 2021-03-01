using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Health;
    public string Name;
    public bool isAlive;
    public State CurrentState;

    public enum State
    {
        Idle,
        AttackMode

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hej");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
