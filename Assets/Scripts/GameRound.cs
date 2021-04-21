using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
/*
 Controls the game flow in one round
 */
public class GameRound : MonoBehaviourPun
{
    private int turnCounter = -1;

    private bool myTurn = true;

    public UnityEvent OnActionTaken;

    private bool gameover = false;

    private void Start()
    {
        EndTurn();
    }

    void Update()
    {
        if (!myTurn)
        {
            HandCards.HandCardsInstance.UpdateCardsOnHand();

        }
    }

    /// <summary>
    /// Begin a new turn, reseting some values if nessesary 
    /// </summary>
    void BeginTurn()
    {
        if (gameover)
            return; 

        if (myTurn)
        {
            PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);
            UIText.Instance.DisplayText("[Your turn]");
            foreach (Character character in PlayerManager.Instance.friendlyCharacters)
            {
                character.SetState(Character.CharacterState.CanDoAction);
            }
            AbilityManager.ManagerInstance.ApplyTurnBasedEffects(); // TODO: Fix to only run at MY turn. pls help :(
        }
        else
        {
            UIText.Instance.DisplayText("[Opponents turn]");
        }

        if (PlayerManager.Instance.friendlyCharacters.Count <= 0)
        {
            gameover = true;
            UIText.Instance.DisplayText("Game over: You lose!");
            photonView.RPC("RPC_GameOver", RpcTarget.Others, "Game over: You win!");
        }
        Debug.LogError("Player: " + PhotonNetwork.LocalPlayer + " turn is " + myTurn + " at turn : " + turnCounter);
        Debug.LogError("Player state is " + PlayerManager.Instance.PlayerState);

    }

    //Wait till player has selected character

    //Player selects a character and chooses which tool to use, either brush or wand for gestures

    //player makes an action with either tool 

    /// <summary>
    /// After an action has completed sets the player to idle and check if the player can do any other moves otherwise end turn
    /// </summary>
    public void ActionTaken()
    {
        OnActionTaken.Invoke();
        var tmp = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        if (tmp.IsAlive)
            tmp.SetState(Character.CharacterState.ActionCompleted);
        //else
        //    tmp.SetState(Character.CharacterState.Dead);

        PlayerManager.Instance.selectedCharacter = null;
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);

        bool roundComplete = PlayerManager.Instance.HasAllCharacterDoneSomething();
        
        if (roundComplete)
        {
            //other players turn
            EndTurn();
        }
        else { Debug.Log("Did not end turn"); }
    }
    
    /// <summary>
    /// Client has ended its turn
    /// </summary>
    void EndTurn()
    {
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.waitingForMyTurn);
        PlayerManager.Instance.OnEndTurn();

        //Logic for ending a turn, lets the other player begin their turn
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            BeginTurn(); 
        }
        else
        {
            photonView.RPC("RPC_NewTurn", RpcTarget.All, turnCounter);
        }
    }

    [PunRPC] private void RPC_GameOver(string msg)
    {
        UIText.Instance.DisplayText(msg);
        gameover = true;
    }

    /// <summary>
    /// Check whos turn it is
    /// </summary>
    /// <param name="c"></param>
    [PunRPC]
    void RPC_NewTurn(int c)
    {
        turnCounter = c;     
        ++turnCounter;
        myTurn = false;

        if (PhotonNetwork.IsMasterClient) // Player 1
        {
            // Turnbased effects should be applied to all characters at the end of player 1 turn
            if (turnCounter % 2 == 0)
            {
                myTurn = true;
            }
        }
        else // Player 2
        {
            if (turnCounter % 2 != 0)
            {
                myTurn = true;
            }
        }
        Debug.LogError("Player: " + PhotonNetwork.LocalPlayer + " turn is " + myTurn + " at turn : " + turnCounter  + " starting new turn");
        BeginTurn();
    }
}
