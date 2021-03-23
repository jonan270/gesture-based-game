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
    private GameObject deckPrefab;
    [SerializeField]
    private GameObject handPrefab;
    //private HandCards hand;
    //private Deck deck;

    public Ability[] bjornAbilities;
    public Ability[] hildaAbilities;

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

        //Testing abilities
        bjornAbilities = bjorn.GetComponent<Character>().GetComponents<Ability>();
        hildaAbilities = hilda.GetComponent<Character>().GetComponents<Ability>();

        int dmg = hildaAbilities[0].TriggerAbility(bjorn.GetComponent<Character>(), hilda.GetComponent<Character>());

        Debug.Log("Damage done: " + dmg); //20

        

    }

    void Update()
    {
        buffTick(hildaAbilities);

        if (Input.GetMouseButtonDown(0) && hilda != null)
        {
            hilda.GetComponent<Hilda>().ModifyHealth(-10);
           
        }

        if (round == 0)
        {
            deck.GetComponent<Deck>().Shuffle(); //Must shuffle in order to get different cards each round
            List<GameObject> drawnCards = deck.GetComponent<Deck>().Draw(); //Draws the cards from deck


            hand.GetComponent<HandCards>().setHand(drawnCards);
           // hand.GetComponent<HandCards>().showHand();


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

    }

    private GameObject SpawnCharacter(GameObject prefab)
    {

        //TODO: spawn character at each side
        Hextile spawnTile = hexMap.GetSpawnPosition(PhotonNetwork.IsMasterClient);
        
        GameObject obj = PhotonNetwork.Instantiate(prefab.name, spawnTile.getPosition(), Quaternion.identity);
        obj.GetComponent<Character>().CurrentTile = spawnTile;
        
        deck.GetComponent<Deck>().AddCardsToDeck(obj.GetComponent<Character>()); //Add cards to deck for character

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        //deck.RemoveCards(ob.GetComponent<Character>().Name);
        PhotonNetwork.Destroy(ob);
    }

    public void buffTick(Ability[] ab)
    {
        

    }

  
}
