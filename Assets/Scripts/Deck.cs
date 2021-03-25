using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class Deck : MonoBehaviour
{
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> drawnCards;

    public Card cardPrefab;

    private const int handSize = 5;
    private const int maxCardsCharacter = 3;

    // Start is called before the first frame update

    void Start()
    {
        
        //Shuffle();
    }

    /// <summary>
    /// Draws card from deck into hand
    /// </summary>
    /// <returns></returns>
    public List<GameObject> Draw() 
    {
        drawnCards = new List<GameObject>();

        for(int i = 0; i < handSize; i++)
        {
            drawnCards.Add(deck[i]);
        }
        return drawnCards;
    }

    /// <summary>
    /// Shuffles the card so random cards are shown
    /// </summary>
    public void Shuffle()
    {
        for(int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject c = deck[j];
            deck[j] = deck[i];
            deck[i] = c;
        }
        
    }

    public void AddCardsToDeck(Character ch)
    {
        var prefab = Resources.Load("TriangleCard");
        if(prefab == null)
        {
            throw new FileNotFoundException("... No file found");
        }
        else {
            //Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card.prefab", typeof(GameObject));
            GameObject c1 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c1.transform.localScale = Vector3.one;
            c1.transform.parent = this.transform;

            //Debug.Log("c1: " + c1.name);

            //add description text for card
            c1.GetComponent<Card>().description.text = ch.descriptionTextCard1;
            c1.GetComponent<Card>().nameText.text = ch.Name;
            //c1.GetComponent<Card>().gestureText.text = "Circle";
           //c1.GetComponentsInChildren<MeshRenderer>()[0].material = ch.MaterialType; // Does not work. Get cardModels' mesh renderer??

            GameObject c2 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c2.transform.localScale = Vector3.one;
            c2.transform.parent = this.transform;

            //add description text for card
            c2.GetComponent<Card>().description.text = ch.descriptionTextCard2;
            c2.GetComponent<Card>().nameText.text = ch.Name;
            //c2.GetComponent<Card>().gesture.GetComponent<Image>() = Resources.Load("triangle.png");
            //c2.GetComponentsInChildren<MeshRenderer>()[0].material = ch.MaterialType;

            GameObject c3 = Instantiate(prefab, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c3.transform.localScale = Vector3.one;
            c3.transform.parent = this.transform;

            //add description text for card
            c3.GetComponent<Card>().description.text = ch.descriptionTextCard3;
            c3.GetComponent<Card>().nameText.text = ch.Name;

            //Canvas.c3.SetActive(false);
            //text.c1.SetActive(false);

            //ch.cards.Add(c1);



            deck.Add(c1);
            deck.Add(c2);
            deck.Add(c3);
        }

    }

    public void RemoveCards(string name) //if character dies we need to remove its cards
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
