using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable]
public class PlayerStateEvent : UnityEvent<PlayerState>
{

}

/// <summary>
/// What state the player is currently in, found in PlayerManager.cs
/// </summary>
public enum PlayerState
{
    idle, waitingForMyTurn, chooseFriendlyCharacter, chooseEnemyCharacter,chooseTile, drawPath, makeGesture, characterWalking
}

/// <summary>
/// Keeps track of what state the player is in, has it picked up a character, is it drawing a path or making a gesture. 
/// Contains a reference to the selected character 
/// TODO: select area or enemy character to attack / cast ability on
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public GameObject arrow;

    public static PlayerManager Instance { get; private set; }

    public PlayerState PlayerState { get; private set; }

    public GameObject selectedCharacter;

    public List<Character> characters, enemyCharacters;

    public PlayerStateEvent toolChangedEvent;


    public delegate void SelectTargetCharacterHandler(Character character);
    private SelectTargetCharacterHandler characterTargetHandler;
    public delegate void SelectTargetTileHandler(Hextile hextile);
    private SelectTargetTileHandler tileTargetHandler;

    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        if(photonView == null)
        {
            Debug.LogError("MISSING PHOTONVIEW COMPONENT");
        }
        characters = new List<Character>();
        enemyCharacters = new List<Character>();
        arrow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            ChangeTool(PlayerState.drawPath);
        if (Input.GetKeyDown(KeyCode.L))
            ChangeTool(PlayerState.makeGesture);


        //raycast from mouse to find a character: TODO: move this function to the hands instead and raycast from the wand for example. 
        if(PlayerState == PlayerState.chooseFriendlyCharacter || PlayerState == PlayerState.chooseEnemyCharacter)
        {
            Camera camera = FindObjectOfType<Camera>();
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) //raycast into the world
            {
                GameObject obj = hit.transform.gameObject;
                bool targetFriendly = PlayerState == PlayerState.chooseFriendlyCharacter;
                PhotonView pv = obj.GetComponent<PhotonView>();
                if (pv != null && (pv.IsMine == targetFriendly)) //if we find a character 
                {
                    arrow.SetActive(true);
                    arrow.transform.position = obj.transform.position + new Vector3(0, 2.5f, 0);
                    obj.GetComponent<Outline>().enabled = true;
                    if (Input.GetMouseButtonDown(0)) //when the player presses left mouse btn invoke function
                    {
                        arrow.SetActive(false);
                        DeselectCharacters();
                        characterTargetHandler.Invoke(obj.GetComponent<Character>());
                    }
                }
                else
                {
                    arrow.SetActive(false);
                    DeselectCharacters();

                }
            }
        }
        //raycast from mouse to find a tile: TODO: move this function to the hands instead and raycast from the wand for example. 
        if (PlayerState == PlayerState.chooseTile)
        {
            Camera camera = FindObjectOfType<Camera>();
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) //raycast into the world
            {
                GameObject obj = hit.transform.gameObject;

                Hextile tile = obj.GetComponent<Hextile>();
                if (tile != null) //if we find a tile
                {
                    tile.OnSelectedTile();
                    
                    arrow.SetActive(true);
                    arrow.transform.position = obj.transform.position + new Vector3(0, 0.5f, 0); //TODO: add material to tile currently hover over
                    if (Input.GetMouseButtonDown(0)) //when the player presses left mouse btn invoke function
                    {
                        arrow.SetActive(false);
                        tileTargetHandler.Invoke(tile);
                    }
                }
                else
                {
                    arrow.SetActive(false);
                }
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// returns true if the player has a charcter selected
    /// </summary>
    public bool HoldingCharacter
    {
        get { return selectedCharacter != null; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    private void ChangeTool(PlayerState state)
    {
        if(HoldingCharacter)
        {
            OnPlayerStateChanged(state);
            toolChangedEvent.Invoke(PlayerState);
        }
    }
    /// <summary>
    /// Subscribes a function to call when appropriate
    /// </summary>
    /// <param name="e">function to call</param>
    public void SubscribeToSelectTargetCharacter(SelectTargetCharacterHandler e)
    {
        Debug.Log("Subscribing function to handler");
        characterTargetHandler += e;
    }
    /// <summary>
    /// unsubscribe the function to no longer recive calls
    /// </summary>
    /// <param name="e">function to unsubscribe</param>
    public void UnsubscribeFromSelectTargetCharacter(SelectTargetCharacterHandler e)
    {
        Debug.Log("Unsubscribing the function from the handler");
        if (characterTargetHandler != null)
            characterTargetHandler -= e;
    }
    /// <summary>
    /// Subscribes a function to call when appropriate
    /// </summary>
    /// <param name="e">function to call</param>
    public void SubscribeToSelectTargetTile(SelectTargetTileHandler e)
    {
        Debug.Log("Subscribing function to handler");
        tileTargetHandler += e;
    }
    /// <summary>
    /// unsubscribe the function to no longer recive calls
    /// </summary>
    /// <param name="e">function to unsubscribe</param>
    public void UnsubscribeFromSelectTargetTile(SelectTargetTileHandler e)
    {
        Debug.Log("Unsubscribing the function from the handler");
        if (tileTargetHandler != null)
            tileTargetHandler -= e;
    }

    /// <summary>
    /// Reset 
    /// </summary>
    public void OnEndTurn()
    {
        UpdateCharacterLists();
        RPC_UpdateCharacterList();

        foreach(Character character in characters)
        {
            character.SetState(Character.CharacterState.CanDoAction);
        }
    }

    /// <summary>
    /// Sets a new state for the player
    /// </summary>
    /// <param name="state"></param>
    public void OnPlayerStateChanged(PlayerState state)
    {
        PlayerState = state;
        Debug.Log("Player state changed to " + PlayerState);
        //do other things
    }
    /// <summary>
    /// Returns false if there are still actions to make on atleast one character
    /// </summary>
    /// <returns></returns>
    public bool HasAllCharacterDoneSomething()
    {
        foreach (Character character in characters)
        {
            if (character.CanDoAction())
                return false;
            
        }
        return true;
    }

    /// <summary>
    /// Returns a character at pos [x,y], null if no character is at [x,y]. Assumes all characters in the list are alive
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Character GetCharacterAt(int x , int y)
    {
        //Looks for enemycharacter
        foreach(Character character in enemyCharacters)
        {
            int posx = character.CurrentTile.tileIndex.x;
            int posy = character.CurrentTile.tileIndex.y;

            if (posx == x && posy == y)
                return character;
        }
        //looks for friendly character
        foreach (Character character in characters)
        {
            int posx = character.CurrentTile.tileIndex.x;
            int posy = character.CurrentTile.tileIndex.y;

            if (posx == x && posy == y)
                return character;
        }
        return null;
    }
    /// <summary>
    /// Updates local character lists
    /// </summary>
    public void UpdateCharacterLists()
    {
        UpdateFriendlyCharacterList();
        UpdateEnemyCharacterList();
    }
    /// <summary>
    /// Makes an RPC call so that the other player updates their character lists
    /// </summary>
    public void RPC_UpdateCharacterList()
    {
        photonView.RPC("RPC_UpdateList", RpcTarget.Others);
    }
    [PunRPC]
    private void RPC_UpdateList()
    {
        UpdateCharacterLists();
    }
    /// <summary>
    /// Updates internal list of friendly characters in the scene
    /// </summary>
    private void UpdateFriendlyCharacterList()
    {
        characters.Clear();
        var allcharacters = FindObjectsOfType<Character>();
        //Debug.Log("Found " + allcharacters.Length + " characters in scene when looking for friendly");
        foreach (var character in allcharacters)
        {
            if (character.GetComponent<PhotonView>().IsMine && character.IsAlive)
            {
                characters.Add(character);
            }
        }
        Debug.LogError("Updating friendly list, there are now " + characters.Count + " friendly characters in the scene");
    }

    /// <summary>
    /// Updates the list of enemy characters in the scene 
    /// </summary>
    private void UpdateEnemyCharacterList()
    {
        enemyCharacters.Clear();
        var allcharacters = FindObjectsOfType<Character>();
        //Debug.Log("Found " + allcharacters.Length + " characters in scene when looking for enemies");
        foreach (var character in allcharacters)
        {
            if (!character.GetComponent<PhotonView>().IsMine && character.IsAlive)
            {
                enemyCharacters.Add(character);
            }
        }
        Debug.LogError("Updating enemy list, there are now  " + enemyCharacters.Count + " enemies in the scene");
    }
    
    private void DeselectCharacters()
    {
        var allcharacters = FindObjectsOfType<Character>();

        foreach (var character in allcharacters)
        {
            character.GetComponent<Outline>().enabled = false;
        }
    }
}
