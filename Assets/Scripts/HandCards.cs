using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCards : MonoBehaviour
{

    public List<GameObject> cardsOnHand = new List<GameObject>();

    [SerializeField]
    List<GameObject> cardPrefabs = new List<GameObject>();

    private static int maxCardsOnHand = 5;

    public Vector3 startingPosition = new Vector3( 18f, 0f, 3f );

    private float counter = 0f;


    private string description;

    void Start()
    {
        description = "";

    }

    void Update()
    {
        UpdateCardsPosition();
        //setTextHand();
    }


    private GameObject GenerateNewCard(Vector3 vec)
    {

        int prefabIndex = UnityEngine.Random.Range(0, 3);

        GameObject ob = Instantiate(cardPrefabs[prefabIndex], vec, Quaternion.Euler(0f, 0f, 0f)); // Lerp?

        ob.transform.parent = this.transform;

        return ob;
    }


    public void UpdateCardsOnHand()
    {
        int size = PlayerManager.Instance.CountCharacters();

        if (size > cardsOnHand.Count) //We have more characters than cards on field => add more cards to hand
        {

            cardsOnHand.Add(GenerateNewCard(startingPosition));

        }

    }

    private void RemoveCardOnHand(GameObject card)
    {

        Vector3 removedCardPos = card.GetComponent<Card>().cardPosition();

        if (card != null)
        {
            Destroy(card);
            cardsOnHand.Remove(card);
        }

    }

    private void UpdateCardsPosition()
    {
        counter += Time.deltaTime;
        //Move cards to the left hand side

        Vector3[] cardEndPositions = new Vector3[3];

        /*cardEndPositions[0] = new Vector3(5f, 0f, -3f);
        cardEndPositions[1] = new Vector3(10f, 0f, -3f);
        cardEndPositions[2] = new Vector3(17f, 0f, -3f);*/
        //cardEndPositions.Add(new Vector3(18f, 0f, -3f));
        //float distanceFromCamera = camera.nearClipPlane; // Change this value if you want
        cardEndPositions[0] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.8f, 7));
        cardEndPositions[1] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.5f, 7));
        cardEndPositions[2] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.2f, 7));

        for (int i = 0; i < cardsOnHand.Count; i++)
        {
            if (cardsOnHand[i].transform.position != cardEndPositions[i])
            {
                cardsOnHand[i].transform.position = Vector3.Lerp(cardsOnHand[i].transform.position, cardEndPositions[i], counter);
                cardsOnHand[i].transform.rotation = Quaternion.Lerp(cardsOnHand[i].transform.rotation, Camera.main.transform.rotation, counter);
            }


        }
        counter = 0f;
        //var v3Pos = new Vector3(0.1f, 0.25f, 5f);
        //cardsOnHand[0].transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
    }

    public void checkGesture(string gesture)
    {
        string gestureString = "";

        gestureString = gesture + "Card(Clone)";

        GameObject ob = GameObject.Find(gestureString);

        RemoveCardOnHand(ob);
        //Activate ability from ability manager
    }

    public void setTextHand(bool textValue)
    {
        if (textValue == true)
        {
            //GameObject ob = PlayerManager.Instance.selectedCharacter;

            for (int i = 0; i < cardsOnHand.Count; i++)
            {
                if (cardsOnHand[i].name == "CircleCard(Clone)")
                {
                    description = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().ListAbilityData[0].abilityDescription;
                   
                }
                else if (cardsOnHand[i].name == "HorizCard(Clone)")
                {
                    description = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().ListAbilityData[1].abilityDescription;

                }
                else if (cardsOnHand[i].name == "VertCard(Clone)")
                {
                    description = PlayerManager.Instance.selectedCharacter.GetComponent<Character>().ListAbilityData[2].abilityDescription;
                }

                cardsOnHand[i].GetComponent<Card>().setText(description, PlayerManager.Instance.selectedCharacter.GetComponent<Character>().Name);
            }
        }
        else if(textValue == false)
        {
            for (int i = 0; i < cardsOnHand.Count; i++)
            {
                cardsOnHand[i].GetComponent<Card>().setText("  ", "  ");

            }
        }
            
        

    }
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



