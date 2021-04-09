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

    public List<Character> characters;

    public PlayerStateEvent toolChangedEvent;


    public delegate void SelectTargetCharacterHandler(Character character);
    private SelectTargetCharacterHandler characterTargetHandler;
    public delegate void SelectTargetTileHandler(Hextile hextile);
    private SelectTargetTileHandler tileTargetHandler;

    private void Awake()
    {
        Instance = this;
        characters = new List<Character>();
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
                    if (Input.GetMouseButtonDown(0)) //when the player presses left mouse btn invoke function
                    {
                        arrow.SetActive(false);
                        characterTargetHandler.Invoke(obj.GetComponent<Character>());
                    }
                }
                else
                {
                    arrow.SetActive(false);
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

    //public void foo()
    //{
    //    Debug.Log("Finding the character");
    //    target = characters[0];
    //    Debug.Log("Invoking the function");
    //    targetHandler.Invoke(target);

    //}

    /// <summary>
    /// Reset 
    /// </summary>
    public void OnEndTurn()
    {
        foreach(Character character in characters)
        {
            character.CurrentState = Character.CharacterState.CanDoAction;
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
            if (character.CurrentState == Character.CharacterState.CanDoAction)
                return false;
            
        }
        return true;
    }

    public int CountCharacters()
    {
        return characters.Count;
    }
}
