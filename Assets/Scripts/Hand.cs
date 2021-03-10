using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public float speed = 1.0f;
    private Transform target;

    public List<GameObject> hand;

    int x = -20; //Initial positions for cards
    int y = 5;
    int z = 15;

    public Hand(List<GameObject> h)
    {
        hand = h;
    }

    public void showHand() //Puts the cards to use in round on the spelplan
    {
        for (int i = 0; i < hand.Count; i++)
        {
            //target = hand[i].transform;
            //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            hand[i].transform.position = new Vector3(x, y, z);
            x += 10;
        }
    }

   /* void Update() // Not working, not being called??? Would be used to animate cards into scene
    {
        float step = speed * Time.deltaTime;

        for (int i = 0; i < hand.Count; i++)
        {
            //target = hand[i].transform;
            //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            hand[i].transform.position = Vector3.Lerp(hand[i].transform.position, new Vector3(x, y, z), step);
            //hand[i].transform.position = new Vector3(x, y, z);
            x += 10;
        }

    }*/

}
