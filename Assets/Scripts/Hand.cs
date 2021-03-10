using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<GameObject> hand;
    public Hand(List<GameObject> h)
    {
        hand = h;
    }

    public void showHand() //Puts the cards to use in round on the spelplan
    {
        int x = -20;
        int y = 5;
        int z = 15;

        for(int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.position = new Vector3(x, y, z);
            x += 10;
        }
    }

}
