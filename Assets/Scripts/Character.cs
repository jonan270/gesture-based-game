using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;
//[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
/// <summary>
/// Element of character or tile, found in Character.cs
/// </summary>
public enum ElementState
{ 
    None, Fire, Earth, Water, Wind
}

public abstract class Character : MonoBehaviour, IPunObservable
{
    public HealthBar healthBar;
    public Hextile CurrentTile { get; set; }

    //[SerializeField]
    public float currentHealth;
    //[SerializeField]
    public float maxHealth = 100;

    public float attackValue;
    public List<TurnBasedEffect> turnBasedEffects;
    //public TurnBasedEffect turnBasedEffect;
    public string Name;
    protected bool isAlive = true;

    //public Material MaterialType;
    public ElementState Element;
    public ElementState StrongAgainst, WeakAgainst; //weakagainst kanske overkill?

    public CharacterState CurrentState;

    //public AbilityManager abilityManager;

    public UnityEvent deathEvent;

    public List<AbilityData> ListAbilityData = new List<AbilityData>();

    public float attackMultiplier = 1f; // Decimalbaserade
    public float defenceMultiplier = 1f;

    //public string descriptionTextCard1;
    //public string descriptionTextCard2;
    //public string descriptionTextCard3;

    // public GameObject c1;
    //public Card c2;
    //public Card c3;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
        //turnBasedEffect = gameObject.AddComponent<TurnBasedEffect>();
    }

    //private void OnEnable()
    //{
    //    isAlive = true;
    //}

    public enum CharacterState
    {
        CanDoAction,
        LookAtCard, // "Idle" mode, Character standing still
        Walk, //walking mode
        AttackMode, // Attack mode, Character is about to perform an attack
        ActionCompleted
    }


    /// <summary>
    /// Adds a turnbased effect on this character
    /// </summary>
    /// <param name="hMod">How health is effected each turn</param>
    /// <param name="aMod">How much the characters attackvalue is multiplied by the effect</param>
    /// <param name="dMod">How much the characters defence value is multiplied by the effect</param>
    /// <param name="turns">How many turns is the effect active?</param>
    public void AddTurnBasedEffect(float hMod, float aMod, float dMod, int turns)
    {
        //turnBasedEffect = TurnBasedEffect.setTurnBased(this, hMod, aMod, maxMod, turns);
        //Debug.Log(turnBasedEffect);
        TurnBasedEffect newEffect = gameObject.AddComponent<TurnBasedEffect>();
        newEffect.setTurnBased(this, hMod, aMod, dMod, turns);
        turnBasedEffects.Add(newEffect);
    }

    public bool canDoAction()
    {
        return CurrentState != CharacterState.ActionCompleted;
    }

    /// <summary>
    /// Compares attacking hero element vs the hero being attacked
    /// </summary>
    /// <param name="enemyElement">Element of the enemy</param>
    /// <param name="baseDamage">Base damage of the ability / auto attack</param>
    /// <param name="bonusDamageMultiplier"> How much extra damage is multiplied, default value is 1</param>
    /// <returns></returns>
    public float CompareEnemyElement(ElementState enemyElement, ElementState friendlyElement, float bonusDamageMultiplier = 1f)
    {
        float multiplier = 1f;
        if (Element == friendlyElement)
        {
            Debug.Log("attack is strong against enemy character");
            multiplier *= 2;
            // Basedamage
            //return baseDamage *= bonusDamageMultiplier * attackMultiplier;

        }
        return multiplier;
        //if (enemyElement == StrongAgainst && Element == friendlyElement)

        //else if(enemyElement == WeakAgainst)
        //{
        //    Debug.Log("weak");
        //    return baseDamage -= bonusDamage;
        //}
        //return baseDamage * attackMultiplier;
    }

    public float CalculateAutoAttack(Character enemy)
    {
        float damage = attackValue * attackMultiplier * CompareEnemyElement(enemy.Element, CurrentTile.tileType);
        Debug.LogError(name + " auto attacks " + enemy.name + " damaging it for " + damage / enemy.defenceMultiplier + " health");
        return damage;
    }

    public void SetState(CharacterState state) {
        CurrentState = state;
    }

    /// <summary>
    /// Returns true if the character is still alive
    /// </summary>
    public bool IsAlive
    {
        get { return isAlive; }
    }

    /// <summary>
    /// Modifies health and updates the healthbar
    /// </summary>
    /// <param name="amount">Positive value heals and negative value deals damage</param>
    public void ModifyHealth(float amount)
    {
        if (amount < 0)
            currentHealth += amount / defenceMultiplier;
        else
            currentHealth += amount;

        float currentHealthPct = currentHealth / maxHealth; //Calculate current health percentage

        healthBar.SetFill(currentHealthPct); //health image 

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Called when character dies
    /// </summary>
    private void Die()
    {
        Debug.Log(gameObject.name + " is now dead");
        isAlive = false;
        deathEvent.Invoke();

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Own player: send data to others
            stream.SendNext(currentHealth);
            //current tile index
            stream.SendNext(CurrentTile.tileIndex.x);
            stream.SendNext(CurrentTile.tileIndex.y);
        }
        else
        {
            //Network player, receive data
            currentHealth = (float)stream.ReceiveNext();
            ModifyHealth(0); //updates healthbar 
            //current tile index
            int x = (int)stream.ReceiveNext();
            int y = (int)stream.ReceiveNext();
            CurrentTile = Hexmap.Instance.map[x, y];
        }
    }
}
