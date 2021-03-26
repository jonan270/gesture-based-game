using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] public static GameObject SelectedCharacter;
    [SerializeField]
    private GameObject hildaPrefab;
    [SerializeField]
    private GameObject bjornPrefab;
    [SerializeField]
    private Hexmap hexMap;
    //List<GameObject> deck = new List<GameObject>();
    [SerializeField]
    private GameObject deckPrefab;
    [SerializeField]
    private GameObject handPrefab;
    [SerializeField]
    public List<GameObject> listOfCharacters = new List<GameObject>();

    private string[] gestureTypes = { "Circle", "Triangle", "Square" };

    private string doneGesture;

   // [SerializeField]
    //public AbilityManager abilityManager;

    //private HandCards hand;
    //private Deck deck;

    //public Ability[] bjornAbilities;
    //public Ability[] hildaAbilities;

    int round = 0;

    private GameObject hilda, bjorn, deck, hand;

    // Start is called before the first frame update
    void Start()
    {
        deck = Instantiate(deckPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        hand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //deckPrefab = new Deck(); //Create specific deck for player
        //var prefab = Resources.Load("Deck");
        //deck = SpawnDeck(deckPrefab);

        //Call function createCharacter()
        hilda = SpawnCharacter(hildaPrefab);
        bjorn = SpawnCharacter(bjornPrefab);

        listOfCharacters.Add(hilda);
        listOfCharacters.Add(bjorn);

        //Select character

        //Text on cards in hand shows. Text includes information about abilities

        //Character performs gesture -> certain card is activated

        //We know now what ability should be performed
        
        

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && hilda != null)
        {
            hilda.GetComponent<Hilda>().ModifyHealth(-10);
           
          
        }

        if (round == 0)
        {
            deck.GetComponent<Deck>().Shuffle(); //Must shuffle in order to get different cards each round
            List<GameObject> drawnCards = deck.GetComponent<Deck>().Draw(); //Draws the cards from deck


            hand.GetComponent<HandCards>().setHand(drawnCards);
            hand.GetComponent<HandCards>().showHand();


            round++;

        }

        if (hilda != null)
        {
            if (!hilda.GetComponent<Hilda>().IsAlive)
            {
                Debug.Log("REMOVING HILDA" + hilda.GetComponent<Hilda>().IsAlive);
                RemoveCharacter(hilda); // Try removing character and its cards
            }
        }

        if(SelectedCharacter != null)
        {
            Debug.Log("select char from CC" + SelectedCharacter.name);
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
        
        deck.GetComponent<Deck>().AddCardsToDeck(obj.GetComponent<Character>()); //Add cards to deck for character

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        //deck.RemoveCards(ob.GetComponent<Character>().Name);
        PhotonNetwork.Destroy(ob);
    }



    

  
}
