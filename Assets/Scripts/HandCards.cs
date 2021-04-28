using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandCards : MonoBehaviour
{

    public static HandCards HandCardsInstance { get; private set; }

    /// <summary>
    /// list of cards player currently has
    /// </summary>
    public List<Card> cardsOnHand = new List<Card>();

    [SerializeField]
    List<GameObject> cardPrefabs = new List<GameObject>();

    private static int maxCardsOnHand = 4;

    public Vector3 startingPosition;

    private Vector3[] cardEndPositions;

    private float counter = 0f;

    private GameObject ob;

    [SerializeField] private GameObject deckPrefab;

    private GameObject deck;

    //private string description = "";
    private void Awake()
    {
        HandCardsInstance = this;

    }
    void Start()
    {

        cardEndPositions = new Vector3[maxCardsOnHand];

        setDeck(PhotonNetwork.IsMasterClient);

        startingPosition = deck.transform.position;

        for(int i = 0; i < maxCardsOnHand; i++)
        {
            cardsOnHand.Add(GenerateNewCard(startingPosition));
        }
        

    }

    void Update()
    {
        UpdateCardsPosition();
       // UpdateCardsOnHand();
        

    }

    /// <summary>
    /// Creates a new card 
    /// </summary>
    private Card GenerateNewCard(Vector3 vec)
    {

        int prefabIndex = Random.Range(0, 3);
        GameObject _ob = PhotonNetwork.Instantiate(cardPrefabs[prefabIndex].name, vec, Quaternion.identity);
        //GameObject _ob = Instantiate(cardPrefabs[prefabIndex], vec, Quaternion.identity); // Lerp?
        if (!PhotonNetwork.IsMasterClient)
            _ob.transform.Rotate(0, 180, 0);

        _ob.transform.parent = this.transform;

        return _ob.GetComponent<Card>();
    }

    /// <summary>
    /// Updates the card on hand if missing card (Depends on number of characters in game)
    /// </summary>
    public void UpdateCardsOnHand()
    {
        int maxcardstohandout = PlayerManager.Instance.CountCharacters();

        while (cardsOnHand.Count < maxCardsOnHand && maxcardstohandout > 0)
        {
            cardsOnHand.Add(GenerateNewCard(startingPosition));
            --maxcardstohandout;
        }

    }

    /// <summary>
    /// Removes card on hand
    /// </summary>
    private void RemoveCardOnHand(GameObject card)
    {
        if (card != null)
        {
            cardsOnHand.Remove(card.GetComponent<Card>());
            PhotonNetwork.Destroy(card);
        }

    }

    /// <summary>
    /// Updates positions for cards on hand
    /// </summary>
    private void UpdateCardsPosition()
    {
        counter += Time.deltaTime;
        //Move cards to the left hand side

        //Vector3[] cardEndPositions = new Vector3[5];

        cardEndPositions = GetCardPosition(PhotonNetwork.IsMasterClient);

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
                //cardsOnHand[i].transform.rotation = Quaternion.Lerp(cardsOnHand[i].transform.rotation, Camera.main.transform.rotation, counter);
            }
        }
        counter = 0f;
    }

    /// <summary>
    /// Gets which gesture has been done, removes card and activates ability depending on gesture.
    /// </summary>
    public bool activateCard(GestureType gesture)
    {
        foreach (var card in cardsOnHand)
        {
            if (card.gestureType == gesture)
            {

                RemoveCardOnHand(card.gameObject);

               // PlayerManager.Instance.PlayerState = PlayerState.makeGesture;
                
                AbilityManager.ManagerInstance.ActivateAbilityFromGesture(gesture, PlayerManager.Instance.selectedCharacter.GetComponent<Character>());

                return true;
            }
        }

        UIText.Instance.DisplayText("No card for that ability!");
        return false;
    }

    /// <summary>
    /// Sets text on card. Used in CharacterSelector script in funcs ReleaseCharacter() and PickupCharacter()
    /// </summary>
    public void setCardType(bool textValue)
    {
        if (PlayerManager.Instance.selectedCharacter != null)
        {
            Character selecetedCharacter = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

            if (textValue == true)
            {
                for (int i = 0; i < cardsOnHand.Count; i++)
                {
                    cardsOnHand[i].ResetCard(); //pre reset quick fix to remove icon if one picks up another character

                    AbilityData data;
                    if (cardsOnHand[i].gestureType == GestureType.circle)
                    {
                        //description = ob.GetComponent<Character>().ListAbilityData[0].abilityDescription;
                        data = selecetedCharacter.ListAbilityData[0];

                    }
                    else if (cardsOnHand[i].gestureType == GestureType.horizontalline)
                    {
                        //description = ob.GetComponent<Character>().ListAbilityData[1].abilityDescription;
                        data = selecetedCharacter.ListAbilityData[1];


                    }
                    else //(cardsOnHand[i].GetComponent<Card>().gestureType == GestureType.verticalline)
                    {
                        //description = ob.GetComponent<Character>().ListAbilityData[2].abilityDescription;
                        data = selecetedCharacter.ListAbilityData[2];

                    }

                    cardsOnHand[i].SetCardData(data, selecetedCharacter.MaterialType);
                    //cardsOnHand[i].GetComponent<Card>().SetElementSymbol(ob.GetComponent<Character>().Element);

                    //cardsOnHand[i].GetComponent<Card>().SetText(description, ob.GetComponent<Character>().Name); //Set text on card
                    //cardsOnHand[i].GetComponent<Card>().model.SetActive(true);
                    //cardsOnHand[i].GetComponent<Card>().model2.SetActive(false);
                    //cardsOnHand[i].GetComponent<Card>().model.GetComponent<MeshRenderer>().material = ob.GetComponent<Character>().MaterialType; //Set material on card

                }
            }
            else //if textValue==false
            {
                for (int i = 0; i < cardsOnHand.Count; i++)
                {
                    cardsOnHand[i].ResetCard();

                }
            }
        }
    }

    //public void setElementSymbol(GameObject card)
    //{
    //    if (ob.GetComponent<Character>().Element == ElementState.Fire)
    //    {
    //        card.GetComponent<Card>().fireSymbol.SetActive(true);

    //    }else if (ob.GetComponent<Character>().Element == ElementState.Earth)
    //    {
    //        card.GetComponent<Card>().earthSymbol.SetActive(true);
    //    }else if (ob.GetComponent<Character>().Element == ElementState.Water)
    //    {
    //        card.GetComponent<Card>().waterSymbol.SetActive(true);
    //    }
    //}

    /// <summary>
    /// Resets card info
    /// </summary>
    //public void resetCard(GameObject card)
    //{
    //    card.GetComponent<Card>().setText("  ", "  ");
    //    //card.GetComponent<Card>().model2.SetActive(true);
    //    //card.GetComponent<Card>().model.SetActive(false);
    //    card.GetComponent<Card>().fireSymbol.SetActive(false);
    //    card.GetComponent<Card>().waterSymbol.SetActive(false);
    //    card.GetComponent<Card>().earthSymbol.SetActive(false);

    //}

    public Vector3[] GetCardPosition(bool master)
    {
        // retunera fr�n hosts tiles yo
       
        if (master)
        {
            cardEndPositions[0] = new Vector3(1.5f, 0f, -1f); // Cards on the ground
            cardEndPositions[1] = new Vector3(3.0f, 0f, -1f);
            cardEndPositions[2] = new Vector3(4.5f, 0f, -1f);
            cardEndPositions[3] = new Vector3(6.0f, 0f, -1f);

            startingPosition = new Vector3(8f, 0f, 0f); //Deck position as well

        }
        else //if (!master)
        {
            cardEndPositions[0] = new Vector3(6.0f, 0f, 10f);
            cardEndPositions[1] = new Vector3(4.5f, 0f, 10f);
            cardEndPositions[2] = new Vector3(3.0f, 0f, 10f);
            cardEndPositions[3] = new Vector3(1.5f, 0f, 10f);
            
            startingPosition = new Vector3(-1f, 0f, 9f); //Deck position as well
        }

        return cardEndPositions;
    }

    private void setDeck(bool master)
    {
        if (master)
        {
            deck = PhotonNetwork.Instantiate(deckPrefab.name, new Vector3(8, 0, 0), Quaternion.identity);
            //deck = Instantiate(deckPrefab, new Vector3(8f, 0f, 0f), Quaternion.identity);

        }
        else
        {
            deck = PhotonNetwork.Instantiate(deckPrefab.name, new Vector3(-1, 0, 8), Quaternion.identity);
            //transform.Rotate(0, 180, 0); // rotate the cards 180 degree to face thesecond player instead
            //deck = Instantiate(deckPrefab, new Vector3(-1f, 0, 8f), Quaternion.identity);
        }
    }

}




