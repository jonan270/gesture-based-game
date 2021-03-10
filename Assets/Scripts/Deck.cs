using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Deck : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> drawnCards;

    public static int handSize = 5;
    public static int maxCardsCharacter = 3;

    // Start is called before the first frame update

    void Start()
    {
        
        //Shuffle();
    }
    public List<GameObject> Draw() //Draws card from deck into hand
    {
        drawnCards = new List<GameObject>();

        for(int i = 0; i < handSize; i++)
        {
            drawnCards.Add(deck[i]);
        }
        return drawnCards;
    }

    public void Shuffle() //Shuffles the card so random cards are shown
    {
        for(int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject c = deck[j];
            deck[j] = deck[i];
            deck[i] = c;
        }
        
    }

    public void addCardsToDeck(Character ch)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card.prefab", typeof(GameObject));
        GameObject c1 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

        //add description text for card
        c1.GetComponent<Card>().description.text = ch.descriptionTextCard1;
        c1.GetComponent<Card>().nameText.text = ch.Name;

        GameObject c2 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

        //add description text for card
        c2.GetComponent<Card>().description.text = ch.descriptionTextCard2;
        c2.GetComponent<Card>().nameText.text = ch.Name;

        GameObject c3 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

        //add description text for card
        c3.GetComponent<Card>().description.text = ch.descriptionTextCard3;
        c3.GetComponent<Card>().nameText.text = ch.Name;
        //ch.cards.Add(c1);

        deck.Add(c1);
        deck.Add(c2);
        deck.Add(c3);

    }

    public void removeCards(string name) //if character dies we need to remove its cards
    {
        int maxDestroy = 3;
        for (int i = 0; i < deck.Count; i++)
        {
            if(deck[i].GetComponent<Card>().nameText.text == name && maxDestroy > 0)
            {
                Destroy(deck[i]);
                maxDestroy--;
               
            }
            
        }
    }
}
