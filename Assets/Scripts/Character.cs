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

    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth = 100;

    public float MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return CurrentHealth; } }


    /// <summary>
    /// Amount of damage this character does with a normal basic attack
    /// </summary>
    public float BasicAttackValue { get; protected set; }
    public List<TurnBasedEffect> turnBasedEffects;
    //public TurnBasedEffect turnBasedEffect;
    public string Name;

    /// <summary>
    /// Returns true if the character is still alive
    /// </summary>
    public bool IsAlive { get; protected set; }

    /// <summary>
    /// Element of this character
    /// </summary>
    public ElementState Element { get; protected set; }
    /// <summary>
    /// Elemen this character is stronger against
    /// </summary>
    public ElementState StrongAgainst;
    /// <summary>
    /// Element this character is weaker to
    /// </summary>
    public ElementState WeakAgainst;

    /// <summary>
    /// What state this character is in
    /// </summary>
    [SerializeField] private CharacterState CurrentState;

    /// <summary>
    /// Functions to call when this character dies
    /// </summary>
    public UnityEvent deathEvent;

    public List<AbilityData> ListAbilityData = new List<AbilityData>();

    protected PhotonView photonView;
    
    public float attackMultiplier = 1f; // Decimalbaserade
    public float defenceMultiplier = 1f;

    private void Awake()
    {
        IsAlive = true;
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        //turnBasedEffect = gameObject.AddComponent<TurnBasedEffect>();
        deathEvent.AddListener(PlayerManager.Instance.UpdateCharacterLists);
        deathEvent.AddListener(PlayerManager.Instance.RPC_UpdateCharacterList);
        photonView = GetComponent<PhotonView>();
        if(photonView == null)
            Debug.LogError("MISSING PHOTONVIEW COMPONENT");
        
    }

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

    /// <summary>
    /// returns true if the character has not completed an action
    /// </summary>
    /// <returns></returns>
    public bool CanDoAction()
    {
        return CurrentState != CharacterState.ActionCompleted;
    }

    /// <summary>
    /// Compares attacking hero element vs the hero being attacked, returns bonus damage if attacker is strong against target
    /// </summary>
    /// <param name="target">Character being attacked</param>
    /// <param name="baseDamage">Base damage of the ability / auto attack</param>
    /// <param name="bonusDamageMultiplier"> How much extra damage is multiplied</param>
    /// <returns></returns>
    public float CompareElement(Character target, float baseDamage, float bonusDamageMultiplier)
    {
        if (StrongAgainst == target.Element)
        {
            Debug.Log("attack is strong against enemy character");
            return baseDamage * (bonusDamageMultiplier - 1);
        }
        return 0; //No bonus damage 
    }
    /// <summary>
    /// Compares attacking hero element vs the hero being attacked, returns bonus damage if attacker is strong against target
    /// </summary>
    /// <param name="tile">Tile to compare to</param>
    /// <param name="baseDamage">Base damage of the ability / auto attack</param>
    /// <param name="bonusDamageMultiplier"> How much extra damage is multiplied</param>
    public float CompareElement(Hextile tile, float baseDamage, float bonusDamageMultiplier)
    {
        if (Element == tile.tileType)
        {
            Debug.Log("Attacker stands in a tile and recives bonus damage");
            return baseDamage * (bonusDamageMultiplier - 1);
        }
        return 0;
    }

    public float CalculateAutoAttack(Character enemy)
    {
        float damage = BasicAttackValue * attackMultiplier + CompareElement(CurrentTile, BasicAttackValue, 2f);
        Debug.LogError(name + " auto attacks " + enemy.name + " damaging it for " + damage / enemy.defenceMultiplier + " health");
        return damage;
    }
    
    /// <summary>
    /// Sets new state for the character
    /// </summary>
    /// <param name="state">new state</param>
    public void SetState(CharacterState state) {
        CurrentState = state;
    }

    /// <summary>
    /// Modifies health and updates the healthbar
    /// </summary>
    /// <param name="amount">Positive value heals and negative value deals damage</param>
    public void ModifyHealth(float amount)
    {
        if (amount < 0) //takes damage
            currentHealth += amount / defenceMultiplier;
        else //being healed
            currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        float currentHealthPct = currentHealth / maxHealth; //Calculate current health percentage

        healthBar.SetFill(currentHealthPct); //health image 

        if(currentHealth <= 0 && IsAlive)
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
        IsAlive = false;
        CurrentTile.RemoveOccupant(); //updates tile for self
        Hexmap.Instance.UpdateTile(CurrentTile.tileIndex.x, CurrentTile.tileIndex.y); //synchronize this tile over network
        RPC_Cant_Handle_Inheritance(); //synchronize alive status over network
        deathEvent.Invoke();
        PhotonNetwork.Destroy(gameObject);
    }
    protected abstract void RPC_Cant_Handle_Inheritance();

    /// <summary>
    /// Synchronize parameters over the network
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Own player: send data to 
            stream.SendNext(currentHealth); //health
            //current tile index
            stream.SendNext(CurrentTile.tileIndex.x);
            stream.SendNext(CurrentTile.tileIndex.y);
        }
        else
        {
            //Network player, receive data
            currentHealth = (float)stream.ReceiveNext(); //health
            ModifyHealth(0); //updates healthbar 
            //current tile index
            int x = (int)stream.ReceiveNext();
            int y = (int)stream.ReceiveNext();
            CurrentTile = Hexmap.Instance.map[x, y];
        }
    }
}
