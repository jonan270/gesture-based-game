using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCards : MonoBehaviour
{

    public List<GameObject> cardsOnHand = new List<GameObject>();

    [SerializeField]
    List<GameObject> cardPrefabs = new List<GameObject>();

    private static int maxCardsOnHand = 5;

    public Vector3 positionOne;
    public Vector3 positionTwo;
    public Vector3 positionThree;

    private float counter = 0f;

    private float CardX; //Position for card to spawn on
    private float CardY;
    private float CardZ;

    void Start()
    {
        CardX = 18f;
        CardY = 0f;
        CardZ = -3f;
    }

    void Update()
    {
        UpdateCardsPosition();
    }


    private GameObject GenerateNewCard(float x, float y, float z)
    {
        //if (cardsOnHand.Count <= 3) 

        int prefabIndex = UnityEngine.Random.Range(0, 3);

        GameObject ob = Instantiate(cardPrefabs[prefabIndex], new Vector3(x, y, z), Quaternion.Euler(90f, 0f, 0f)); // Lerp?

        ob.transform.parent = this.transform;

        //UpdateCardsPosition();

        return ob;
    }


    public void UpdateCardsOnHand()
    {
        int size = PlayerManager.Instance.CountCharacters();
        //Debug.Log("Nr of chars: " + size);
       
        if(size > cardsOnHand.Count && cardsOnHand.Count < maxCardsOnHand) //We have more characters than cards on field, and not more than 5 cards => add more cards to hand
        {
            //UpdateCardsPosition();

            cardsOnHand.Add(GenerateNewCard(CardX, CardY, CardZ));

            //CardX += 5f;
        }

    }

    public void RemoveCardOnHand(GameObject card)
    {
        Debug.Log("Card to be removed: " + card);
        //card.cardShown = false;
        Vector3 removedCardPos = card.GetComponent<Card>().cardPosition();
        
        if (card != null)
        {
            //Remove card from cardsOnHand
            Destroy(card);
        }


        //Update all cards positions
        //UpdateCardsPosition(removedCardPos);

        //Replace card
        cardsOnHand.Add(GenerateNewCard(CardX, CardY, CardZ)); //Set new card to old cards position
    }

    private void UpdateCardsPosition()
    {
        counter += Time.deltaTime;
        //Move cards to the left hand side
        for (int i = 1; i < cardsOnHand.Count; i++)
        {

            if (cardsOnHand[i].transform.position.x > 10f ) // If card are positioned on right side of the recently removed card
            {
                cardsOnHand[i].transform.position = Vector3.Lerp(cardsOnHand[i].transform.position, 
                    new Vector3(10f,0f,-3f), counter);

                cardsOnHand[i+1].transform.position = Vector3.Lerp(cardsOnHand[i+1].transform.position,
                    new Vector3(5f, 0f, -3f), counter);

            }
            if (cardsOnHand[i].transform.position.x <= 10f) // If card are positioned on right side of the recently removed card
            {
                cardsOnHand[i].transform.position = Vector3.Lerp(cardsOnHand[i].transform.position,
                    new Vector3(5f,0f,-3f), counter);
                Debug.Log("Cards position: " + cardsOnHand[i].GetComponent<Card>().cardPosition());

            }
            
        }
        counter = 0f;
    }


    


    //private void Update()  // Not working, not being called??? Would be used to get hand of cards into scene
    // {
    //if(!cardsShown)
    //showHand(speed * Time.deltaTime);

    //hand[counter].transform.position = Vector3.Lerp(hand[counter].transform.position, new Vector3(x, y, z), speed * Time.deltaTime);

    // }

    /*public void showHand()
    {
        //cardsShown = false;

        for (int i = 0; i < hand.Count; i++)
        {
            
            hand[i].transform.position = new Vector3(x, y, z);
            hand[i].transform.LookAt(Camera.main.transform);
            hand[i].transform.Rotate(0, 180, 0);
            x += 10;
        }

        /*if (counter == hand.Count - 1)
        {
            cardsShown = true;
            //x = -20;
        }*/

    
}
