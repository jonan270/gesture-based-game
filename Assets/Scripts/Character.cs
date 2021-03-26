using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
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
    public string Name;
    protected bool isAlive = true;
    
    public ElementState Element;
    public ElementState StrongAgainst, WeakAgainst; //weakagainst kanske overkill?

    public CharacterState CurrentState;
    
    //public List<GameObject> cards;

    public string descriptionTextCard1;
    public string descriptionTextCard2;
    public string descriptionTextCard3;

    // public GameObject c1;
    //public Card c2;
    //public Card c3;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
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
    /// Compares attacking hero element vs hero being attacked
    /// </summary>
    /// <param name="enemyElement">Element of the enemy</param>
    /// <param name="bonusDamage">How much extra damage is added</param>
    /// <returns></returns>
    public int CompareEnemyElement(ElementState enemyElement, int bonusDamage)
    {
        if (enemyElement == StrongAgainst)
        {
            return attackValue += bonusDamage;
        }
        else if(enemyElement == WeakAgainst)
        {
            return attackValue -= bonusDamage;
        }
        return attackValue;
    }

    public void SetState(CharacterState state) {
        CurrentState = state;
    }

    public bool IsAlive //�r det en funktion f�r att kolla om karakt�ren lever eller f�r att f� veta hur mycke liv finns kvar
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
            isAlive = false;
        }
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
