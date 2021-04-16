using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Valve.VR;

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
    public static PlayerManager Instance { get; private set; }

    public PlayerState PlayerState { get; private set; }

    public GameObject selectedCharacter;

    public List<Character> friendlyCharacters, enemyCharacters;

    public PlayerStateEvent toolChangedEvent;


    public delegate void SelectTargetCharacterHandler(Character character);
    public SelectTargetCharacterHandler characterTargetHandler;
    public delegate void SelectTargetTileHandler(Hextile hextile);
    public SelectTargetTileHandler tileTargetHandler;

    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        if(photonView == null)
        {
            Debug.LogError("MISSING PHOTONVIEW COMPONENT");
        }
        friendlyCharacters = new List<Character>();
        enemyCharacters = new List<Character>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || SteamVR_Actions.default_SnapTurnLeft.GetStateDown(SteamVR_Input_Sources.Any))
            ChangeTool(PlayerState.drawPath);
        if (    SteamVR_Actions.default_SnapTurnRight.GetStateDown(SteamVR_Input_Sources.Any))
            ChangeTool(PlayerState.makeGesture);

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
    }

    /// <summary>
    /// Sets a new state for the player
    /// </summary>
    /// <param name="state"></param>
    public void OnPlayerStateChanged(PlayerState state)
    {
        if(PlayerState == PlayerState.drawPath || PlayerState == PlayerState.makeGesture)
            toolChangedEvent.Invoke(state);

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
        foreach (Character character in friendlyCharacters)
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
        foreach (Character character in friendlyCharacters)
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
        friendlyCharacters.Clear();
        var allcharacters = FindObjectsOfType<Character>();
        //Debug.Log("Found " + allcharacters.Length + " characters in scene when looking for friendly");
        foreach (var character in allcharacters)
        {
            if (character.GetComponent<PhotonView>().IsMine && character.IsAlive)
            {
                friendlyCharacters.Add(character);
            }
        }
        Debug.LogError("Updating friendly list, there are now " + friendlyCharacters.Count + " friendly characters in the scene");
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
    
    public void DeselectCharacters()
    {
        var allcharacters = FindObjectsOfType<Character>();

        foreach (var character in allcharacters)
        {
            character.GetComponent<Outline>().enabled = false;
        }
    public int CountCharacters()
    {
        return friendlyCharacters.Count;
    }
}
