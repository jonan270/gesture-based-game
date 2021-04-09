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

    /// <summary>
    /// Amount of damage this character does with a normal basic attack
    /// </summary>
    public int BasicAttackValue { get; protected set; }
    //public List<TurnBasedEffect> turnBasedEffects;
    public TurnBasedEffect turnBasedEffect;
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

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        IsAlive = true;
        turnBasedEffect = gameObject.AddComponent<TurnBasedEffect>();
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

    public void AddTurnBasedEffect(int hMod, float aMod, float maxMod, int turns)
    {
        //turnBasedEffect = TurnBasedEffect.setTurnBased(this, hMod, aMod, maxMod, turns);
        Debug.Log(turnBasedEffect);
        turnBasedEffect.setTurnBased(this, hMod, aMod, maxMod, turns);
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
    public void ModifyHealth(int amount)
    {
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
