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
        var prefabTri = Resources.Load("TriangleCard");
        var prefabCir = Resources.Load("CircleCard");
        var prefabSq = Resources.Load("SquareCard");

        if (prefabTri == null || prefabCir == null || prefabSq == null)
        {
            throw new FileNotFoundException("... No file found");
        }
        else {
            GameObject c1 = Instantiate(prefabTri, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c1.transform.localScale = Vector3.one;
            c1.transform.parent = this.transform;


            //add description text for card
            //c1.GetComponent<Card>().description.text = ch.ListAbilityData[1].abilityDescription;
            //c1.GetComponent<Card>().nameText.text = ch.Name;
            //c1.GetComponent<Card>().gestureText.text = "Circle";
           //c1.GetComponentsInChildren<MeshRenderer>()[0].material = ch.MaterialType; // Does not work. Get cardModels' mesh renderer??

            GameObject c2 = Instantiate(prefabCir, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c2.transform.localScale = Vector3.one;
            c2.transform.parent = this.transform;

            //add description text for card
            //c2.GetComponent<Card>().description.text = ch.ListAbilityData[2].abilityDescription;
            //c2.GetComponent<Card>().nameText.text = ch.Name;

            GameObject c3 = Instantiate(prefabSq, new Vector3(0, 0, -20), Quaternion.Euler(90f, 0f, 0f)) as GameObject;

            c3.transform.localScale = Vector3.one;
            c3.transform.parent = this.transform;

            //add description text for card
            //c3.GetComponent<Card>().description.text = ch.ListAbilityData[3].abilityDescription;
            //c3.GetComponent<Card>().nameText.text = ch.Name;

            //c1.GetComponent<Card>().description.SetActive(true); // Hide / show text?

            deck.Add(c1);
            deck.Add(c2);
            deck.Add(c3);
        }

    }

    //public void RemoveCards(string name) //if character dies we need to remove its cards
    //{
    //    int maxDestroy = 3;
    //    for (int i = 0; i < deck.Count; i++)
    //    {
    //        if(deck[i].GetComponent<Card>().nameText.text == name && maxDestroy > 0)
    //        {
    //            Destroy(deck[i]);
    //            maxDestroy--;
               
    //        }
            
    //    }
    //}
}
