using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager ManagerInstance { get; private set; }
    private List<Character> turnBasedEffected;
    private Hexmap map;

    void Start()
    {
        map = FindObjectOfType<Hexmap>();
        ManagerInstance = this;
    }

    //[PunRPC]
    //void RPC_test()
    //{
    //    Debug.Log("hej");
    //}


    [PunRPC]
    void RPC_AffectHealth(int x, int y, int amount)
    {
        map.hexTiles[x, y].occupant.ModifyHealth(amount);
    }

    [PunRPC]
    void RPC_ApplyTurnBased(int x, int y)
    {
        Character occupant = map.hexTiles[x, y].occupant;
        occupant.turnBasedEffect.ApplyTurnBased(occupant);
    }

    [PunRPC]
    void RPC_SetTurnBased(int x, int y, int hMod, float aMod, float maxMod, int turns)
    {
        map.hexTiles[x, y].occupant.AddTurnBasedEffect(hMod, aMod, maxMod, turns);
    }

    public void TurnBasedTick(Character character)
    {
        if(character.turnBasedEffect && character.turnBasedEffect.isActive)
        {
            GetComponent<PhotonView>().RPC("RPC_ApplyTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x,
                character.CurrentTile.tileIndex.y);
        }
    }

    public void DamageCharacter(int x, int y)
    {
        GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y,
            -PlayerManager.Instance.selectedCharacter.GetComponent<Character>().attackValue);
    }

    public void HealCharacter(int x, int y, int amount)
    {
        //GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y,
        //    PlayerManager.Instance.selectedCharacter.GetComponent<Character>().attackValue);
        map.hexTiles[x, y].occupant.ModifyHealth(amount);
    }

    public void ActivateTurnBasedAbility(Character character, int hMod, float aMod, float maxMod, int turns)
    {
        //Debug.Log(character.CurrentTile.tileIndex.x + character.CurrentTile.tileIndex.y + hMod + aMod + maxMod + turns);
        //GetComponent<PhotonView>().RPC("RPC_test", RpcTarget.All);

        //GetComponent<PhotonView>().RPC("RPC_DamageCharacter", RpcTarget.All, character.CurrentTile.tileIndex.x,
        //    character.CurrentTile.tileIndex.x, 20);

        GetComponent<PhotonView>().RPC("RPC_SetTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y,
            hMod, aMod, maxMod, turns);
    }

    public void PlaceAreaEffect(int x, int y, bool setEffect, ElementState element = ElementState.None, int healthMod = 0)
    {
        map.ChangeEffect(x, y, setEffect, element, healthMod);
    }

    public void ActivateAbilityFromGesture(GestureType type, Character character)
    {
        foreach (var ability in character.ListAbilityData)
        {
            if (ability.gestureType == type)
            {
                ability.ActivateAbility();
                break; // If ability was found, stop searching
            }
        }
    }

    void Update()
    {
        //if next round
        //calculateBuffs();
        //Check if card has been chosen
        
    }

    //public void calculateBuffs()
    //{
    //    /*for(int i = 0; i < listOfAbilities.Count; i++)
    //    {
    //        //if (BjornAbilities[i].GetType() == typeof(Poison))
    //        //{
    //            //Debug.Log("Here");
    //            //BjornAbilities[i].Apply(GetTarget());
    //        //}
    //    }*/
    //}

    //public void Tick(int nrTurns)
    //{

    //    //nrTurns -= 1;

    //   /* if (nrTurns <= 0) // Have this in AbilityManager / CharacterControl??
    //    {
    //        // End();
    //        //IsFinished = true;
    //    }*/
    //}

}

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html


