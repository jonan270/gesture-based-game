using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class CharacterControl : MonoBehaviour
{
    [Header("Character Prefabs")]
    [SerializeField] private GameObject hildaPrefab;
    [SerializeField] private GameObject bjornPrefab;
    [SerializeField] private GameObject freyrPrefab;

    [Header("Other prefabs")]
    [SerializeField] private Hexmap hexMap;
    //List<GameObject> deck = new List<GameObject>();
    [SerializeField] private GameObject deckPrefab;
    [SerializeField] private GameObject handPrefab;

    
    private GameObject hilda, bjorn,freyr, deck, hand;

    // Start is called before the first frame update
    void Awake()
    {
        deck = Instantiate(deckPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        hand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.identity);


    }
    private void Start()
    {
        //Call function createCharacter()
        hilda = SpawnCharacter(hildaPrefab);
        bjorn = SpawnCharacter(bjornPrefab);
        freyr = SpawnCharacter(freyrPrefab);

        //Show hand of currently available cards

        PlayerManager.Instance.characters.Add(hilda.GetComponent<Character>());
        PlayerManager.Instance.characters.Add(bjorn.GetComponent<Character>());
        PlayerManager.Instance.characters.Add(freyr.GetComponent<Character>());
        PlayerManager.Instance.RPC_UpdateCharacterList();
        PlayerManager.Instance.UpdateCharacterLists();

    }
    void Update()
    {


        //if (round == 0)
        //{
        //    deck.GetComponent<Deck>().Shuffle(); //Must shuffle in order to get different cards each round
        //    List<GameObject> drawnCards = deck.GetComponent<Deck>().Draw(); //Draws the cards from deck

        //    hand.GetComponent<HandCards>().setHand(drawnCards);
        //    //hand.GetComponent<HandCards>().showHand();

        //    round++;

        //}

        //if (hilda != null)
        //{
        //    if (!hilda.GetComponent<Hilda>().IsAlive)
        //    {
        //        Debug.Log("REMOVING HILDA" + hilda.GetComponent<Hilda>().IsAlive);
        //        RemoveCharacter(hilda); // Try removing character and its cards
        //    }
        //}
    }

    private GameObject SpawnCharacter(GameObject prefab)
    {
        Hextile spawnTile = hexMap.GetSpawnPosition(PhotonNetwork.IsMasterClient);
        Debug.LogError("Spawn Tile " + spawnTile.tileIndex);
        
        //Rotates character toward the other player
        Vector3 rotation = PhotonNetwork.IsMasterClient ? Vector3.zero : new Vector3(0, 180, 0);
        //Instantiate over network
        GameObject obj = PhotonNetwork.Instantiate(prefab.name, spawnTile.Position, Quaternion.Euler(rotation));
        obj.GetComponent<Character>().CurrentTile = spawnTile;
        spawnTile.SetOccupant(obj.GetComponent<Character>()); // Set occupant in tile class
        
        deck.GetComponent<Deck>().AddCardsToDeck(obj.GetComponent<Character>()); //Add cards to deck for character

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        //deck.RemoveCards(ob.GetComponent<Character>().Name);
        PhotonNetwork.Destroy(ob);
    }
}
