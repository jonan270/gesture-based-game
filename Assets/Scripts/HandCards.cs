using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCards : MonoBehaviour
{

    public List<GameObject> cardsOnHand = new List<GameObject>();

    [SerializeField]
    List<GameObject> cardPrefabs = new List<GameObject>();

    private static int maxCardsOnHand = 4;

    public Vector3 startingPosition = new Vector3( 18f, 0f, 3f );

    private float counter = 0f;

    private GameObject ob;


    private string description = "";

    void Start()
    {
       
    }

    void Update()
    {
        UpdateCardsOnHand();
        UpdateCardsPosition();

    }

    /// <summary>
    /// Creates a new card 
    /// </summary>
    private GameObject GenerateNewCard(Vector3 vec)
    {

        int prefabIndex = UnityEngine.Random.Range(0, 3);

        GameObject ob = Instantiate(cardPrefabs[prefabIndex], vec, Quaternion.Euler(90f, 0f, 0f)); // Lerp?

        ob.transform.parent = this.transform;

        return ob;
    }

    /// <summary>
    /// Updates the card on hand if missing card (Depends on number of characters in game)
    /// </summary>
    public void UpdateCardsOnHand()
    {
        int size = PlayerManager.Instance.CountCharacters();

        if (cardsOnHand.Count < maxCardsOnHand || size > cardsOnHand.Count)
        {

            cardsOnHand.Add(GenerateNewCard(startingPosition));

        }

    }

    /// <summary>
    /// Removes card on hand
    /// </summary>
    private void RemoveCardOnHand(GameObject card)
    {
        if (card != null)
        {
            Destroy(card);
            cardsOnHand.Remove(card);
        }

    }

    /// <summary>
    /// Updates positions for cards on hand
    /// </summary>
    private void UpdateCardsPosition()
    {
        counter += Time.deltaTime;
        //Move cards to the left hand side

        Vector3[] cardEndPositions = new Vector3[5];

        cardEndPositions[0] = new Vector3(1.5f, 0f, -1f); // Cards on the ground
        cardEndPositions[1] = new Vector3(3.0f, 0f, -1f);
        cardEndPositions[2] = new Vector3(4.5f, 0f, -1f);
        cardEndPositions[3] = new Vector3(6.0f, 0f, -1f);


        //cardEndPositions[0] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.12f, 4f)); //Cards on the screen, follows the camera
        //cardEndPositions[1] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.37f, 4f));
        //cardEndPositions[2] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.62f, 4f));
        //cardEndPositions[3] = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.87f, 4f));

        /*cardEndPositions[0] = new Vector3(2f, 3.3f, -2f); //Cards on the screen, stays on one spot
        cardEndPositions[1] = new Vector3(2f, 5.9f, -1.5f);
        cardEndPositions[2] = new Vector3(2f, 7.9f, -1f);*/

        for (int i = 0; i < cardsOnHand.Count; i++)
        {
            if (cardsOnHand[i].transform.position != cardEndPositions[i])
            {
                cardsOnHand[i].transform.position = Vector3.Lerp(cardsOnHand[i].transform.position, cardEndPositions[i], counter);
                cardsOnHand[i].transform.rotation = Quaternion.Lerp(cardsOnHand[i].transform.rotation, Camera.main.transform.rotation, counter);
            }
        }
        counter = 0f;
    }

    /// <summary>
    /// Gets which gesture has been done, removes card and activates ability depending on gesture.
    /// </summary>
    public void activateCard(GestureType gesture)
    {
        foreach (GameObject card in cardsOnHand)
        {
            if (card.GetComponent<Card>().gestureType == gesture)
            {

                RemoveCardOnHand(card);

               // PlayerManager.Instance.PlayerState = PlayerState.makeGesture;
                
                AbilityManager.ManagerInstance.ActivateAbilityFromGesture(gesture, PlayerManager.Instance.selectedCharacter.GetComponent<Character>());

                return;
            }
        }

        UIText.Instance.DisplayText("No card for that ability!");
    }

    /// <summary>
    /// Sets text on card. Used in CharacterSelector script in funcs ReleaseCharacter() and PickupCharacter()
    /// </summary>
    public void setCardType(bool textValue)
    {
        if (PlayerManager.Instance.selectedCharacter != null)
        {
            ob = PlayerManager.Instance.selectedCharacter;

            if (textValue == true)
            {
                for (int i = 0; i < cardsOnHand.Count; i++)
                {
                    if (cardsOnHand[i].GetComponent<Card>().gestureType == GestureType.circle)
                    {
                        description = ob.GetComponent<Character>().ListAbilityData[0].abilityDescription;

                    }
                    else if (cardsOnHand[i].GetComponent<Card>().gestureType == GestureType.horizontalline)
                    {
                        description = ob.GetComponent<Character>().ListAbilityData[1].abilityDescription;

                    }
                    else if (cardsOnHand[i].GetComponent<Card>().gestureType == GestureType.verticalline)
                    {
                        description = ob.GetComponent<Character>().ListAbilityData[2].abilityDescription;
                    }

                    setElementSymbol(cardsOnHand[i]);

                    cardsOnHand[i].GetComponent<Card>().setText(description, ob.GetComponent<Character>().Name); //Set text on card
                    cardsOnHand[i].GetComponent<Card>().model.SetActive(true);
                    cardsOnHand[i].GetComponent<Card>().model2.SetActive(false);
                    cardsOnHand[i].GetComponent<Card>().model.GetComponent<MeshRenderer>().material = ob.GetComponent<Character>().MaterialType; //Set material on card

                }
            }
            else //if textValue==false
            {
                for (int i = 0; i < cardsOnHand.Count; i++)
                {
                    resetCard(cardsOnHand[i]);

                }
            }
        }
    }

    public void setElementSymbol(GameObject card)
    {
        if (ob.GetComponent<Character>().Element == ElementState.Fire)
        {
            card.GetComponent<Card>().fireSymbol.SetActive(true);

        }else if (ob.GetComponent<Character>().Element == ElementState.Earth)
        {
            card.GetComponent<Card>().earthSymbol.SetActive(true);
        }else if (ob.GetComponent<Character>().Element == ElementState.Water)
        {
            card.GetComponent<Card>().waterSymbol.SetActive(true);
        }
    }

    /// <summary>
    /// Resets card info
    /// </summary>
    public void resetCard(GameObject card)
    {
        card.GetComponent<Card>().setText("  ", "  ");
        card.GetComponent<Card>().model2.SetActive(true);
        card.GetComponent<Card>().model.SetActive(false);
        card.GetComponent<Card>().fireSymbol.SetActive(false);
        card.GetComponent<Card>().waterSymbol.SetActive(false);
        card.GetComponent<Card>().earthSymbol.SetActive(false);

    }

}




