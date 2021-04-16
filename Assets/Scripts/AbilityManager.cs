using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager ManagerInstance { get; private set; }
    public List<Character> turnBasedEffected; // characters effected by a turnbased effect.

    private Hexmap map;

    private GameObject projectileObj;
    private bool travelling = false;

    // ** Projectiles like fireball ***
    [SerializeField]
    private float speed = 100f; // Serialized variable to control speed of projectiles
    private float startTime; // At what time does the projectile start travelling?
    private float journeyLength; // Length of journey for projectile

    // Lerp between start and end
    private Vector3 startTarget;
    private Vector3 endTarget;

    private float projectileDamage;
    private Character projectileTarget;
    // ********************************

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

    void Update()
    {
        if (travelling)
            LerpProjectile();
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
        Character target = PlayerManager.Instance.GetCharacterAt(x, y);
        //Debug.LogError("character " + map.map[x, y].occupant.name + " takes " + amount + " damage");
        //map.map[x, y].occupant.ModifyHealth(amount);
        Debug.LogError("character " + target.name + " takes " + amount + " damage");
        target.ModifyHealth(amount);
    }


    /// <summary>
    /// Network function for casting a projectile at target
    /// </summary>
    /// <param name="userX"> X position of ability user </param>
    /// <param name="userY"> Y position of ability user </param>
    /// <param name="targetX"> X position of ability target </param>
    /// <param name="targetY"> Y position of ability target </param>
    /// <param name="hitDamage"> Damage to apply when projectile connects </param>
    [PunRPC]
    void RPC_CastProjectile(int userX, int userY, int targetX, int targetY, float hitDamage)
    {
        Character target = PlayerManager.Instance.GetCharacterAt(targetX, targetY);
        projectileTarget = target;

        Character user = PlayerManager.Instance.GetCharacterAt(userX, userY);

        startTarget = user.transform.position;
        endTarget = target.transform.position;

        //Debug.Log("User: " + user);
        GameObject projectile = user.ListAbilityData[2].effectPrefab; // TODO: Find a better way to find projectiles?

        projectileObj = Instantiate(projectile, user.transform.position, Quaternion.identity);
        projectileObj.transform.localScale = user.transform.localScale;

        startTime = Time.time;

        //visualEffect.transform.SetParent(parent, true);
        projectileObj.transform.localEulerAngles = new Vector3(0, 0, 0);
        //LerpProjectile(projectileObj, user.transform.position, target.transform.position);
        projectileDamage = hitDamage;
        travelling = true;
    }

    /// <summary>
    /// Lerp projectile
    /// </summary>
    private void LerpProjectile()
    {
        journeyLength = Vector3.Distance(startTarget, endTarget);
        //Debug.Log("journeyLength is: " + journeyLength);
        //projectileObj.transform.position = to;
        float distanceCovered = (Time.time - startTime) * speed;
        //Debug.Log("distanceCovered is: " + distanceCovered);
        float fraction = speed * distanceCovered / journeyLength;

        //Debug.Log("Fraction is: " + fraction);

        projectileObj.transform.position = Vector3.Lerp(startTarget, endTarget, fraction);
        if (fraction > 0.99)
        {
            DamageCharacter(projectileTarget, projectileDamage);
            travelling = false;
            Destroy(projectileObj);
        }
    }

    /// <summary>
    /// Call this outside class to cast a projectile
    /// </summary>
    /// <param name="user"> Character that uses ability </param>
    /// <param name="target"> Character that gets hit by ability </param>
    /// <param name="hitDamage"> Damage to apply </param>
    public void CastProjectile(Character user, Character target, float hitDamage)
    {
        photonView.RPC("RPC_CastProjectile", RpcTarget.All, user.CurrentTile.tileIndex.x, user.CurrentTile.tileIndex.y,
            target.CurrentTile.tileIndex.x, target.CurrentTile.tileIndex.y, hitDamage);
    }


    /// <summary>
    /// Network RPC to apply all turnbased effects to all characters at a specific tile
    /// </summary>
    /// <param name="x"> x index of character tile </param>
    /// <param name="y"> y index of character tile </param>
    [PunRPC]
    void RPC_ApplyTurnBased(int x, int y)
    {
        Character target = PlayerManager.Instance.GetCharacterAt(x, y);
        if(target) // Safety check
        {
            foreach (var effect in target.turnBasedEffects)
            {
                if (effect.IsActive())
                    effect.ApplyTurnBased(target);
                else
                {
                    effect.RemoveTurnBased(target);
                    target.turnBasedEffects.Remove(effect);
                }
            }
        }
    }

    /// <summary>
    /// kallas på när en ny runda har börjat
    /// </summary>
    public void ApplyTurnBasedEffects()
    {
        //loopar igenom listan av affected characters
        foreach(var character in turnBasedEffected)
        {
            TurnBasedTick(character);
        }
        //alla karaktärer som tillhör sig själv 
        //applicera effekten igen 
        //tex. modify health eller w/e
    }
    
    /// <summary>
    /// See ActivateTurnBasedAbility()
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="hMod"></param>
    /// <param name="aMod"></param>
    /// <param name="dMod"></param>
    /// <param name="turns"></param>
    [PunRPC]
    void RPC_SetTurnBased(int x, int y, float hMod, float aMod, float dMod, int turns, int userX, int userY)
    {
        Character target = PlayerManager.Instance.GetCharacterAt(x, y);
        Character user = PlayerManager.Instance.GetCharacterAt(userX, userY);

        Debug.Log("Setting ability from" + user.name + " on " + target.name);
        turnBasedEffected.Add(target); // Add to list of effected characters

        target.activeEffect = user.ListAbilityData[0].effectPrefab; // Turnbased abilities should be placed in slot 0

        //GameObject visualEffect = target.ListAbilityData[0].effectPrefab;
        target.AddTurnBasedEffect(hMod, aMod, dMod, turns);
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
    /// <param name="dMod"></param>
    /// <param name="turns"></param>
    public void ActivateTurnBasedAbility(Character character, float hMod, float aMod, float dMod, int turns, Character user = null)
    {
        Debug.Log("Activating turnbased ability on " + character.name + " for " + turns + " turns");
        //GetComponent<PhotonView>().RPC("RPC_DamageCharacter", RpcTarget.All, character.CurrentTile.tileIndex.x,
        //    character.CurrentTile.tileIndex.x, 20);
        int targetX = character.CurrentTile.tileIndex.x;
        int targetY = character.CurrentTile.tileIndex.y;
        int userX;
        int userY;

        if (!user)
        {
           userX = targetX;
           userY = targetY;
        }
        else
        {
            userX = user.CurrentTile.tileIndex.x;
            userY = user.CurrentTile.tileIndex.y;
        }
        photonView.RPC("RPC_SetTurnBased", RpcTarget.All, character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y,
            hMod, aMod, dMod, turns, userX, userY);
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
        photonView.RPC("RPC_AffectHealth", RpcTarget.Others, x, y, -damage);
    }
    public void DamageCharacter(int x, int y, float damage)
    {
        //Character attacker = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();
        //Character target = map.hexTiles[x, y].occupant;
        //int bonusAttackDmg = 5;
        //int damage = attacker.CompareEnemyElement(target.Element, attacker.attackValue, bonusAttackDmg);
        photonView.RPC("RPC_AffectHealth", RpcTarget.Others, x, y, -damage);
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
                break; // If ability was found, stop searching
            }
        }
    }
}

    //Maybe do abilities like this: https://answers.unity.com/questions/1727492/spells-and-abilities-system.html


