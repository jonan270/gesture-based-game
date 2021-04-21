using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class CharacterControl : MonoBehaviour
{
    public float modellScale = 1.0f;
    [Header("Character Prefabs")]
    [SerializeField] private GameObject hildaPrefab;
    [SerializeField] private GameObject bjornPrefab;
    [SerializeField] private GameObject freyrPrefab;

    [Header("Other prefabs")]
    [SerializeField] private Hexmap hexMap;
    //List<GameObject> deck = new List<GameObject>();
    [SerializeField] private GameObject handPrefab;

    
    private GameObject hilda, bjorn, freyr, hand;

    // Start is called before the first frame update
    void Awake()
    {
        hand = Instantiate(handPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        deck = Instantiate(deckPrefab, new Vector3(9, 0, 0), Quaternion.identity);
    }
    
    private void Start()
    {
        //Call function createCharacter()
        hilda = SpawnCharacter(hildaPrefab);
        bjorn = SpawnCharacter(bjornPrefab);
        freyr = SpawnCharacter(freyrPrefab);

        //Show hand of currently available cards

        PlayerManager.Instance.friendlyCharacters.Add(hilda.GetComponent<Character>());
        PlayerManager.Instance.friendlyCharacters.Add(bjorn.GetComponent<Character>());
        PlayerManager.Instance.friendlyCharacters.Add(freyr.GetComponent<Character>());

        StartCoroutine(UpdateCharacters()); //Wait till next frame and update, fixed bug where 2nd player did not find enemy characters in scene
    }

    IEnumerator UpdateCharacters()
    {
        yield return new WaitForFixedUpdate();
        PlayerManager.Instance.RPC_UpdateCharacterList();
        PlayerManager.Instance.UpdateCharacterLists();

    }

    private GameObject SpawnCharacter(GameObject prefab)
    {
        Hextile spawnTile = hexMap.GetSpawnPosition(PhotonNetwork.IsMasterClient);
        Debug.LogError("Spawn Tile " + spawnTile.tileIndex);
        
        //Rotates character toward the other player
        Vector3 rotation = PhotonNetwork.IsMasterClient ? Vector3.zero : new Vector3(0, 180, 0);
        //Instantiate over network
        GameObject obj = PhotonNetwork.Instantiate(prefab.name, spawnTile.Position, Quaternion.Euler(rotation));
        obj.transform.localScale *= modellScale;
        obj.GetComponent<Character>().CurrentTile = spawnTile;
        spawnTile.SetOccupant(obj.GetComponent<Character>()); // Set occupant in tile class
        
        return obj;
    }

}
