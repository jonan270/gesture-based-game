using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private GameObject hildaPrefab;
    [SerializeField]
    private GameObject bjornPrefab;
    [SerializeField]
    private Hexmap hexMap;
    //List<GameObject> deck = new List<GameObject>();
    [SerializeField]
    private Deck deck;
    private Hand hand;

    int round = 0;

    private GameObject hilda, bjorn;

    // Start is called before the first frame update
    void Start()
    {

        //deck = new Deck(); //Create specific deck for player


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

        if (Input.GetMouseButtonDown(0) && hilda != null)
        {
            hilda.GetComponent<Hilda>().ModifyHealth(-10);
        }

        //if (round == 0)
        //{
        //    deck.Shuffle(); //Must shuffle in order to get different cards each round
        //    List<GameObject> drawnCards = deck.Draw(); //Draws the cards from deck
        //    hand = new Hand(drawnCards);
        //    hand.showHand();

        //    Debug.Log(drawnCards.Count);

        //    round++;
        //}

        if (hilda != null)
        {
            if (!hilda.GetComponent<Hilda>().IsAlive)
            {
                Debug.Log("REMOVING HILDA" + hilda.GetComponent<Hilda>().IsAlive);
                RemoveCharacter(hilda); // Try removing character and its cards
            }
        }
    }

    private GameObject SpawnCharacter(GameObject prefab)
    {
        //TODO: spawn character at each side
        Hextile spawnTile = hexMap.GetSpawnPosition(PhotonNetwork.IsMasterClient);
        Debug.LogError("Spawn Tile " + spawnTile.Position);


        Vector3 rotation = PhotonNetwork.IsMasterClient ? Vector3.zero : new Vector3(0, 180, 0);

        GameObject obj = PhotonNetwork.Instantiate(prefab.name, spawnTile.Position, Quaternion.Euler(rotation));
        obj.GetComponent<Character>().CurrentTile = spawnTile;
        //deck.AddCardsToDeck(obj.GetComponent<Character>()); //Add cards to deck for character

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        //deck.RemoveCards(ob.GetComponent<Character>().Name);
        PhotonNetwork.Destroy(ob);
    }
}
