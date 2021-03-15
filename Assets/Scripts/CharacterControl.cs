using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private GameObject hildaPrefab;
    [SerializeField]
    private GameObject bjornPrefab;
    //List<GameObject> deck = new List<GameObject>();
    Deck deck;
    Hand hand;

    int round = 0;

    private GameObject hilda, bjorn;

    // Start is called before the first frame update
    void Start()
    {

        deck = new Deck(); //Create specific deck for player

        //Call function createCharacter()
        hilda = SpawnCharacter(hildaPrefab);
        bjorn = SpawnCharacter(bjornPrefab);
        //Show hand of currently available cards





        //Call function to move character

        //Call function to attack opponent

        //Call function to change healths of characters and modify health bars

        //float remainingHealth = hilda.ModifyHealth(10);

        //Debug.Log(remainingHealth);
        //hilda.healthBar.SetSize(remainingHealth);
        
        //character.Attack("Berserk", charactertwo);
        // Debug.Log("Health after attack: " + charactertwo.Health
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //hilda.ModifyHealth(5);
        }
        
        if (round == 0)
        {
            deck.Shuffle(); //Must shuffle in order to get different cards each round
            List<GameObject> drawnCards = deck.Draw(); //Draws the cards from deck
            hand = new Hand(drawnCards);
            hand.showHand();

            Debug.Log(drawnCards.Count);

            round++;
        }

        //if(!hilda.isAlive)
        //{
        //    RemoveCharacter(hildaPrefab); // Try removing character and its cards
        //}
    }

    private GameObject SpawnCharacter(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);

        deck.addCardsToDeck(obj.GetComponent<Character>()); //Add cards to deck for character

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        deck.removeCards(ob.GetComponent<Character>().Name);
        Destroy(ob);
    }




}
