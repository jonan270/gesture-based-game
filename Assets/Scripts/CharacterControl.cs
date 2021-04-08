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
    [SerializeField] private GameObject freyrPrefab;
    [SerializeField]
    private Hexmap hexMap;
    //List<GameObject> deck = new List<GameObject>();
    [SerializeField]
    private GameObject handPrefab;
    [SerializeField]
    private GameObject deckPrefab;
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

    private GameObject hilda, bjorn,freyr, hand, deck;

    // Start is called before the first frame update
    void Start()
    {
        
        hand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        deck = Instantiate(deckPrefab, new Vector3(18, 0, 3), Quaternion.identity);



        //deckPrefab = new Deck(); //Create specific deck for player
        //var prefab = Resources.Load("Deck");
        //deck = SpawnDeck(deckPrefab);

        //Call function createCharacter()
        hilda = SpawnCharacter(hildaPrefab);
        bjorn = SpawnCharacter(bjornPrefab);
        freyr = SpawnCharacter(freyrPrefab);
		
        //Show hand of currently available cards

        PlayerManager.Instance.characters.Add(hilda.GetComponent<Character>());
        PlayerManager.Instance.characters.Add(bjorn.GetComponent<Character>());
        PlayerManager.Instance.characters.Add(freyr.GetComponent<Character>());


        listOfCharacters.Add(hilda);
        listOfCharacters.Add(bjorn);
        listOfCharacters.Add(freyr);


    }

    void Update()
    {

        hand.GetComponent<HandCards>().UpdateCardsOnHand();

        if (Input.GetMouseButtonDown(0) && hilda != null)
        {
        hilda.GetComponent<Hilda>().ModifyHealth(-70);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.gameObject);
                hand.GetComponent<HandCards>().checkGesture("Circle");
            }
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
        
        

        return obj;
    }

    public void RemoveCharacter(GameObject ob) //If character health <= 0, destroy object
    {
        PlayerManager.Instance.characters.Remove(ob.GetComponent<Character>());

        PhotonNetwork.Destroy(ob);
    }



    

  
}
