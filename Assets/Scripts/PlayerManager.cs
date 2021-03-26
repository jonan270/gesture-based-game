using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerStateEvent : UnityEvent<PlayerState>
{

}

/// <summary>
/// What state the player is currently in, found in PlayerManager.cs
/// </summary>
public enum PlayerState
{
    idle, chooseCharacter, drawPath, makeGesture
}

/// <summary>
/// Keeps track of what state the player is in, has it picked up a character, is it drawing a path or making a gesture. 
/// Contains a reference to the selected character 
/// TODO: select area or enemy character to attack / cast ability on
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerState playerState;

    public GameObject selectedCharacter;

    public List<Character> characters;

    public PlayerStateEvent toolChangedEvent;


    private void Awake()
    {
        Instance = this;
        characters = new List<Character>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            ChangeTool(PlayerState.drawPath);
        if (Input.GetKeyDown(KeyCode.L))
            ChangeTool(PlayerState.makeGesture);
    }


    public bool HoldingCharacter
    {
        get { return selectedCharacter != null; }
    }

    private void ChangeTool(PlayerState state)
    {
        if(HoldingCharacter)
        {
            playerState = state;
            toolChangedEvent.Invoke(playerState);
        }
    }

    public void OnEndTurn()
    {
        foreach(Character character in characters)
        {
            character.CurrentState = Character.CharacterState.CanDoAction;
        }
    }

    public bool HasAllCharacterDoneSomething()
    {
        foreach (Character character in characters)
        {
            if (character.CurrentState == Character.CharacterState.CanDoAction)
                return false;
            
        }
        return true;
    }
}
