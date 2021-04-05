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

    public int attackValue;
    //public List<TurnBasedEffect> turnBasedEffects;
    public TurnBasedEffect turnBasedEffect;
    public string Name;
    protected bool isAlive = true;

    //public Material MaterialType;
    public ElementState Element;
    public ElementState StrongAgainst, WeakAgainst; //weakagainst kanske overkill?

    public CharacterState CurrentState;

    //public AbilityManager abilityManager;

    public UnityEvent deathEvent;

    public List<AbilityData> ListAbilityData = new List<AbilityData>();

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
        turnBasedEffect = gameObject.AddComponent<TurnBasedEffect>();
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

    public void AddTurnBasedEffect(int hMod, float aMod, float maxMod, int turns)
    {
        //turnBasedEffect = TurnBasedEffect.setTurnBased(this, hMod, aMod, maxMod, turns);
        Debug.Log(turnBasedEffect);
        turnBasedEffect.setTurnBased(this, hMod, aMod, maxMod, turns);
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
    /// <param name="bonusDamage">How much extra damage is added</param>
    /// <returns></returns>
    public int CompareEnemyElement(ElementState enemyElement, int baseDamage, int bonusDamage)
    {
        if (enemyElement == StrongAgainst)
        {
            Debug.Log("attack is strong against enemy character");
            return baseDamage += bonusDamage;
            
        }
        //else if(enemyElement == WeakAgainst)
        //{
        //    Debug.Log("weak");
        //    return baseDamage -= bonusDamage;
        //}
        return baseDamage;
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
    public void ModifyHealth(int amount)
    {
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
        }
        else
        {
            //Network player, receive data
            currentHealth = (float)stream.ReceiveNext();
            ModifyHealth(0); //updates healthbar 
        }
    }
}
