using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager ManagerInstance { get; private set; }
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

    /// <summary>
    /// ActivateTurnBasedAbility()
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="amount"></param>
    [PunRPC]
    void RPC_AffectHealth(int x, int y, float amount)
    {
        map.map[x, y].occupant.ModifyHealth(amount);
    }


    [PunRPC]
    void RPC_ApplyTurnBased(int x, int y)
    {
        //Character occupant = map.hexTiles[x, y].occupant;
        //occupant.turnBasedEffect.ApplyTurnBased(occupant);
        foreach (var effect in map.map[x, y].occupant.turnBasedEffects)
        {
            if (effect.IsActive())
                effect.ApplyTurnBased(map.map[x, y].occupant);
            else
                map.map[x, y].occupant.turnBasedEffects.Remove(effect);
        }
    }

    /// <summary>
    /// See ActivateTurnBasedAbility()
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="hMod"></param>
    /// <param name="aMod"></param>
    /// <param name="maxMod"></param>
    /// <param name="turns"></param>
    [PunRPC]
    void RPC_SetTurnBased(int x, int y, float hMod, float aMod, float maxMod, int turns)
    {
        map.map[x, y].occupant.AddTurnBasedEffect(hMod, aMod, maxMod, turns);
    }

    /// <summary>
    /// Activates a TurnBasedAbility for a given character. Calls to sepperate RPC function.
    /// 
    /// turns sets for how long the effect is active.
    /// hMod sets how health is affected each turn. aMod is a percentagebased attackincrease.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hMod"></param>
    /// <param name="aMod"></param>
    /// <param name="maxMod"></param>
    /// <param name="turns"></param>
    public void ActivateTurnBasedAbility(Character character, int hMod, float aMod, float maxMod, int turns)
    {
        //Debug.Log(character.CurrentTile.tileIndex.x + character.CurrentTile.tileIndex.y + hMod + aMod + maxMod + turns);
        //GetComponent<PhotonView>().RPC("RPC_test", RpcTarget.All);

        //GetComponent<PhotonView>().RPC("RPC_DamageCharacter", RpcTarget.All, character.CurrentTile.tileIndex.x,
        //    character.CurrentTile.tileIndex.x, 20);

        photonView.RPC("RPC_SetTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y,
            hMod, aMod, maxMod, turns);
    }

    /// <summary>
    /// Call at the beginning of each turn to apply turnbased effects to character
    /// </summary>
    /// <param name="character"></param>
    public void TurnBasedTick(Character character)
    {
        foreach(var effect in character.turnBasedEffects)
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
    public void DamageCharacter(Character target, float damage)
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


    /// <summary>
    /// Heal a character at tile x,y for amount.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="amount"></param>
    public void HealCharacter(int x, int y, int amount)
    {
        //GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y,
        //    PlayerManager.Instance.selectedCharacter.GetComponent<Character>().attackValue);
        map.map[x, y].occupant.ModifyHealth(amount);
    }

    /// <summary>
    /// Places an AreaEffect at coordinates x,y if setEffect is true. If false it removes the AreaEffect from the tile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="setEffect"></param>
    /// <param name="element"></param>
    /// <param name="healthMod"></param>
    public void PlaceAreaEffect(int x, int y, bool setEffect, ElementState element = ElementState.None, float healthMod = 0)
    {
        map.ChangeEffect(x, y, setEffect, element, healthMod);
    }

    /// <summary>
    /// Takes abbility of Gesturetype from character and avtivates it
    /// </summary>
    /// <param name="type"></param>
    /// <param name="character"></param>
    public void ActivateAbilityFromGesture(GestureType type, Character character)
    {
        foreach (var ability in character.ListAbilityData)
        {
            if (ability.gestureType == type)
            {
                ability.ActivateAbility();
                Debug.Log("Ability activated is: " + ability.abilityName);
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

}

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html


