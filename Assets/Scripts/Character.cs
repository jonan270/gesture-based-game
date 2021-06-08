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
    public Animator anim;
    [SerializeField] private GameObject sideLineCharacter;

    public HealthBar healthBar;
    public Hextile CurrentTile { get; set; }

    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth = 100;

    public float MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } }


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


    public Material MaterialType;

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

    public PhotonView photonView { get; protected set; }
    
    public float attackMultiplier = 1f; // Decimalbaserade
    public float defenceMultiplier = 1f;

    public GameObject activeEffect;

    protected virtual void Awake()
    {
        IsAlive = true;
    }

    protected virtual void Start()
    {
        attackMultiplier = 1f;
        //defenceMultiplier = 1f;
        currentHealth = maxHealth;
        //turnBasedEffect = gameObject.AddComponent<TurnBasedEffect>();
        deathEvent.AddListener(PlayerManager.Instance.UpdateCharacterLists);
        deathEvent.AddListener(PlayerManager.Instance.RPC_UpdateCharacterList);
        photonView = GetComponent<PhotonView>();
        if(photonView == null)
            Debug.LogError("MISSING PHOTONVIEW COMPONENT");

        if(photonView.IsMine)
        {
            GetComponentInChildren<Light>().enabled = true;
        }

    }

    public enum CharacterState
    {
        CanDoAction, //idle 
        PickedUp, //playr is holding character
        Dead, //character has died
        Walking, //character is walking
        ActionCompleted, //idle

        TakeDamage // Character is damaged

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
        TurnBasedEffect newEffect = gameObject.AddComponent<TurnBasedEffect>();
        newEffect.visualEffect = activeEffect;
        newEffect.setTurnBased(this, hMod, aMod, dMod, turns);
        turnBasedEffects.Add(newEffect);
    }

    /// <summary>
    /// Finds the effectPrefab of the ability of a certain GestureType
    /// </summary>
    /// <param name="type">Type of gesture</param>
    /// <returns></returns>
    public GameObject GetEffectFromGesture(GestureType type)
    {
        foreach(var ability in ListAbilityData)
        {
            if(ability.gestureType == type)
            {
                return ability.effectPrefab;
            }
        }
        //Below should never happen
        Debug.Log("ABILITY OF TYPE " + type + " IS MISSING ON" + name);
        return null;
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
        //Debug.LogError(name + " auto attacks " + enemy.name + " damaging it for " + damage / enemy.defenceMultiplier + " health");
        return damage;
    }
    
    /// <summary>
    /// Sets new state for the character also play correct animation
    /// </summary>
    /// <param name="state">new state</param>
    public void SetState(CharacterState state) {
        CurrentState = state;

        if(CurrentState == CharacterState.CanDoAction)
        {
            GetComponentInChildren<Light>().enabled = true;
        }
        if (CurrentState == CharacterState.ActionCompleted || CurrentState == CharacterState.PickedUp)
        {
            GetComponentInChildren<Light>().enabled = false;
        }
        if(CurrentState == CharacterState.CanDoAction || CurrentState == CharacterState.ActionCompleted)
        {
            //anim.Play("Idle");
            anim.SetBool("Idle", true);
            anim.SetBool("Walking", false);
        }
        if (CurrentState == CharacterState.Walking)
        {
            anim.SetBool("Walking", true);
            anim.SetBool("Idle", false);

        }
        //anim.Play("Run");
        if (CurrentState == CharacterState.Dead)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Dead", true);

        }
        //anim.Play("Die");

        Debug.Log(name + " state is " + CurrentState);
    }

    /// <summary>
    /// Modifies health and updates the healthbar
    /// </summary>
    /// <param name="amount">Positive value heals and negative value deals damage</param> //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public void ModifyHealth(float amount)
    {
  
     
        // Takes damage
        if (amount < 0)
        {
            Debug.LogError(name + " takes " + amount / defenceMultiplier + " amount of damage");
            currentHealth += amount / defenceMultiplier;
        }

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
        else
        {
            // Try to trigger oof animation here....?
            anim.SetBool("Oof", true);
            anim.SetBool("Idle", false);
            StartCoroutine(animationWaiter());
        }
    }

    /// <summary>
    /// Called when character dies
    /// </summary>
    private void Die()
    {
        StartCoroutine(waiter());
        // S�tta en key? Allm�n animations Key??

    }

    IEnumerator animationWaiter()
    {
        yield return new WaitForSeconds(4);
        anim.SetBool("Oof", false);
        anim.SetBool("Idle", true);

    }

    public void Attack()
    {
        anim.SetBool("Attack", true);
        anim.SetBool("Walking", false);
        StartCoroutine(AttackAnimationWaiter());
    }

    IEnumerator AttackAnimationWaiter()
    {
        yield return new WaitForSeconds(4);
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
    }

    IEnumerator waiter()
    {

        Debug.Log(gameObject.name + " is now dead");
        IsAlive = false;
        CurrentTile.RemoveOccupant(); //updates tile for self
        Hexmap.Instance.UpdateTile(CurrentTile.tileIndex.x, CurrentTile.tileIndex.y); //synchronize this tile over network
        RPC_Cant_Handle_Inheritance(); //synchronize alive status over network
        SetState(CharacterState.Dead);
        deathEvent.Invoke();
        yield return new WaitForSeconds(4);
        CharacterSideline.Instance.AddSidelineCharcater(sideLineCharacter);
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
            stream.SendNext(defenceMultiplier);
            //current tile index
            stream.SendNext(CurrentTile.tileIndex.x);
            stream.SendNext(CurrentTile.tileIndex.y);
            stream.SendNext(anim.GetBool("Walking"));
            stream.SendNext(anim.GetBool("Idle"));
            stream.SendNext(anim.GetBool("Dead"));

        }
        else
        {
            //Network player, receive data
            currentHealth = (float)stream.ReceiveNext(); //health
            defenceMultiplier = (float)stream.ReceiveNext(); // Defence multiplier
            ModifyHealth(0); //updates healthbar 
            //current tile index
            int x = (int)stream.ReceiveNext();
            int y = (int)stream.ReceiveNext();
            CurrentTile = Hexmap.Instance.map[x, y];

            anim.SetBool("Walking", (bool)stream.ReceiveNext());
            anim.SetBool("Idle", (bool)stream.ReceiveNext());
            anim.SetBool("Dead", (bool)stream.ReceiveNext());

        }
    }

    public void AttemptGatheringGemstones()
    {
        Debug.Log("Attempting to gather gemstones...");
        GemstonePile pile = CurrentTile.GetComponentInChildren<GemstonePile>();
        if (pile)
        {
            PlayerManager.Instance.ModifyGemstones(pile.amountGems);
            pile.RemoveGemstonePile(pile.gameObject);
            Debug.Log("Gemstones gathered and removed!");
        }
        else Debug.LogError("No gemstones on this tile!");
    }
}
