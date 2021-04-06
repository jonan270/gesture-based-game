using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager ManagerInstance { get; private set; }
    private List<Character> turnBasedEffected;
    private Hexmap map;


    [SerializeField] private PhotonView photonView;

    void Start()
    {
        if(photonView == null)
        {
            Debug.LogError("Missing photonView reference!");
        }
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
        map.map[x, y].occupant.ModifyHealth(amount);
    }


    [PunRPC]
    void RPC_ApplyTurnBased(int x, int y)
    {
        Character occupant = map.map[x, y].occupant;
        occupant.turnBasedEffect.ApplyTurnBased(occupant);
    }

    [PunRPC]
    void RPC_SetTurnBased(int x, int y, int hMod, float aMod, float maxMod, int turns)
    {
        map.map[x, y].occupant.AddTurnBasedEffect(hMod, aMod, maxMod, turns);
    }

    public void TurnBasedTick(Character character)
    {
        if(character.turnBasedEffect && character.turnBasedEffect.isActive)
        {
            photonView.RPC("RPC_ApplyTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x,
                character.CurrentTile.tileIndex.y);
        }
    }

    /// <summary>
    /// Damages a character 
    /// </summary>
    /// <param name="target">Character to damage</param>
    /// <param name="damage">Amount of damage to deal, assumes finalized damage</param>
    public void DamageCharacter(Character target, int damage)
    {
        int x = target.CurrentTile.tileIndex.x;
        int y = target.CurrentTile.tileIndex.y;
        //Character attacker = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        //Character target = map.hexTiles[x, y].occupant;
        //int bonusAttackDmg = 5;
        //int damage = attacker.CompareEnemyElement(target.Element, attacker.attackValue, bonusAttackDmg);
        photonView.RPC("RPC_AffectHealth", RpcTarget.Others, x, y, -damage);
    }
    public void DamageCharacter(int x, int y, ElementState attackerState)
    {
        //Character attacker = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        //Character target = map.hexTiles[x, y].occupant;
        //int bonusAttackDmg = 5;
        //int damage = attacker.CompareEnemyElement(target.Element, attacker.attackValue, bonusAttackDmg);
        photonView.RPC("RPC_AffectHealth", RpcTarget.Others, x, y, 0);
    }

    public void HealCharacter(int x, int y, int amount)
    {
        //GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y,
        //    PlayerManager.Instance.selectedCharacter.GetComponent<Character>().attackValue);
        map.map[x, y].occupant.ModifyHealth(amount);
    }

    public void ActivateTurnBasedAbility(Character character, int hMod, float aMod, float maxMod, int turns)
    {
        //Debug.Log(character.CurrentTile.tileIndex.x + character.CurrentTile.tileIndex.y + hMod + aMod + maxMod + turns);
        //GetComponent<PhotonView>().RPC("RPC_test", RpcTarget.All);

        //GetComponent<PhotonView>().RPC("RPC_DamageCharacter", RpcTarget.All, character.CurrentTile.tileIndex.x,
        //    character.CurrentTile.tileIndex.x, 20);

        photonView.RPC("RPC_SetTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y,
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


