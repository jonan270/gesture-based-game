using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCards : MonoBehaviour
{

    public float speed = 1.0f;
    private Transform target;

    public List<GameObject> hand;

    public bool cardsShown = false;

    int counter = 0;
    int x = -20; //Initial positions for cards
    int y = 5;
    int z = 15;

    public void setHand(List<GameObject> h)
    {
        hand = h;
    }

    /*public HandCards(List<GameObject> h)
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
     }*/



    //private void Update()  // Not working, not being called??? Would be used to get hand of cards into scene
    // {
    //if(!cardsShown)
    //showHand(speed * Time.deltaTime);

    //hand[counter].transform.position = Vector3.Lerp(hand[counter].transform.position, new Vector3(x, y, z), speed * Time.deltaTime);

    // }

    public void showHand()
    {
        //cardsShown = false;

        for (int i = 0; i < hand.Count; i++)
        {
            
            hand[i].transform.position = new Vector3(x, y, z);
           
            x += 10;
        }

        /*if (counter == hand.Count - 1)
        {
            cardsShown = true;
            //x = -20;
        }*/

    }
}
