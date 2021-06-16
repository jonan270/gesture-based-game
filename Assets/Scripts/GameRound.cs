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

    [SerializeField] public GameObject button;


    private void Start()
    {
        //button = Instantiate(button, new Vector3(-0.08f, 0f, 0.35f), Quaternion.identity);
        //button.onClick.AddListener(EndTurn());


        EndTurn();
    }

    //void Update()
    //{
    //    if (!myTurn)
    //    {
    //        HandCards.HandCardsInstance.UpdateCardsOnHand();

    //    }

    //}

    /// <summary>
    /// Begin a new turn, reseting some values if nessesary 
    /// </summary>
    void BeginTurn()
    {
        //if (gameover)
        //    return; 

        if (myTurn)
        {
            PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);
            UIText.Instance.DisplayText("[Your turn]");
            AbilityManager.ManagerInstance.ApplyTurnBasedEffects(); // TODO: Fix to only run at MY turn. pls help :(
            PlayerManager.Instance.ModifyGemstones((turnCounter / 2) + 1);
            Hexmap.Instance.generateGemstones(turnCounter / 2);
            foreach (Character character in PlayerManager.Instance.friendlyCharacters)
            {
                character.SetState(Character.CharacterState.CanDoAction);
               // character.characterAvailable(true);
            }
        }
        else
        {
            UIText.Instance.DisplayText("[Opponents turn]");
        }

        //if (PlayerManager.Instance.friendlyCharacters.Count <= 0 && turnCounter > 0)
        //{
        //    gameover = true;
        //    UIText.Instance.DisplayText("Game over: You lose!");
        //    photonView.RPC("RPC_GameOver", RpcTarget.Others, "Game over: You win!");
        //}
        Debug.Log("Player: " + PhotonNetwork.LocalPlayer + " turn is " + myTurn + " at turn : " + turnCounter);
        Debug.Log("Player state is " + PlayerManager.Instance.PlayerState);

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
        HandCards.HandCardsInstance.setCardType(false);
        PlayerManager.Instance.selectedCharacter = null;
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);

        
        var cards = FindObjectsOfType<cardDrawing>();
        foreach (var card in cards)
        {
            card.OnDropCharacter();
        }

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
        HandCards.HandCardsInstance.UpdateCardsOnHand();

        //Debug.Log("Number of players in the room " + PhotonNetwork.CurrentRoom.PlayerCount);
        //Logic for ending a turn, lets the other player begin their turn
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            BeginTurn();
            myTurn = true;
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
        Debug.Log("Player: " + PhotonNetwork.LocalPlayer + " turn is " + myTurn + " at turn : " + turnCounter  + " starting new turn");
        BeginTurn();
    }
}
