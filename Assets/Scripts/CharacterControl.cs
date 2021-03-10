using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterControl : MonoBehaviour
{
    GameObject character;
    GameObject charactertwo;
    GameObject obj;
    //List<GameObject> deck = new List<GameObject>();
    Deck deck;
    Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        deck = new Deck(); //Create specific deck for player
        //Call function createCharacter()
        //character = new GameObject("Bjorn");
        character = createHilda();
        charactertwo = createBjorn();



        //Show hand of currently available cards
        deck.Shuffle(); //Must shuffle in order to get different cards each round
        List<GameObject> drawnCards = deck.Draw(); //Draws the cards from deck
        hand = new Hand(drawnCards);
        hand.showHand();

        Debug.Log(drawnCards.Count);

        Hilda h = character.GetComponent<Hilda>();

        Debug.Log(h.currentHealth);

        removeCharacter(character); // Try removing character and its cards

        //Debug.Log(charactertwo.Health);

        //Debug.Log(b.card.description);

        //Call function to move character
        
        //Call function to attack opponent

        //Call function to change healths of characters and modify health bars

        float remainingHealth = h.ModifyHealth(10);

        Debug.Log(remainingHealth);

        h.healthBar.SetSize(remainingHealth);
        
        //character.Attack("Berserk", charactertwo);
        // Debug.Log("Health after attack: " + charactertwo.Health);

        // Add assets to the CharacterOne/CharacterTwo

        //GameObject characterModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //characterModel.transform.position = new Vector3(0, 0, 0);


    }

    public GameObject createHilda()
    {

        Object prefabHilda = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Hilda.prefab");

        obj = Instantiate(prefabHilda, Vector3.zero, Quaternion.identity) as GameObject;

        deck.addCardsToDeck(obj.GetComponent<Hilda>()); //Add cards to deck for character

        return obj;
    }

    public GameObject createBjorn()
    {

        Object prefabBjorn = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bjorn.prefab");

        obj = Instantiate(prefabBjorn, Vector3.zero, Quaternion.identity) as GameObject;

        deck.addCardsToDeck(obj.GetComponent<Bjorn>()); //Add cards to deck for character


        return obj;
    }


    public void removeCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        deck.removeCards(ob.GetComponent<Character>().Name);
        Destroy(ob);
    }
    // Update is called once per frame
    void Update()
    {
        
    }




}
