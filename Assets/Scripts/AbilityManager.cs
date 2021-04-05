using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager ManagerInstance { get; private set; }
    private List<GameObject> CharacterList = new List<GameObject>();
    private Hexmap map;

    void Start()
    {
        map = FindObjectOfType<Hexmap>();
        CharacterList = GameObject.Find("Game Manager").GetComponent<CharacterControl>().listOfCharacters;
        ManagerInstance = this;
    }


    [PunRPC]
    void RPC_AffectHealth(int x, int y, int amount)
    {
        map.hexTiles[x, y].occupant.ModifyHealth(amount);
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
        GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y, -damage);
    }

    public void HealCharacter(int x, int y, int amount)
    {
        //GetComponent<PhotonView>().RPC("RPC_AffectHealth", RpcTarget.Others, x, y,
        //    PlayerManager.Instance.selectedCharacter.GetComponent<Character>().attackValue);
        map.hexTiles[x, y].occupant.ModifyHealth(amount);
    }

    public void ActivateAbilityFromGesture(GestureType type, Character character)
    {
        foreach (var ability in character.ListAbilityData)
        {
            if (ability.gestureType == type)
                ability.ActivateAbility();
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


