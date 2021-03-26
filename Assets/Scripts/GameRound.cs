using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 Controls the game flow in one round
 */
public class GameRound : MonoBehaviourPun
{
    private int turnCounter = -1;

    private bool myTurn = false;

    private void Start()
    {
        BeginTurn();
    }

    //Round begin 
    void BeginTurn()
    {
        photonView.RPC("RPC_NewTurn", RpcTarget.All);

        if (myTurn)
        {
            //Gems are given
            //Draw new cards
        }

        Debug.Log("Player: " + PhotonNetwork.LocalPlayer + " turn is " + myTurn);

    }

    //Wait till player has selected character

    //Player selects a character and chooses which tool to use, either brush or wand for gestures

    //player makes an action with either tool 

    /// <summary>
    /// After an action has completed, check if the player can do any other moves otherwise end turn
    /// </summary>
    public void OnActionTaken()
    {
        PlayerManager.Instance.playerState = PlayerState.idle;

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
        PlayerManager.Instance.OnEndTurn();
        //Logic for ending a turn, lets the other player begin his turn
        BeginTurn();
    }

    [PunRPC]
    void RPC_NewTurn()
    {
        ++turnCounter;
        myTurn = false;

        if (PhotonNetwork.IsMasterClient)
        {
            if (turnCounter % 2 == 0)
            {
                myTurn = true;
            }
        }
        else
        {
            if (turnCounter % 2 != 0)
            {
                myTurn = true;
            }
        }
    }
}
