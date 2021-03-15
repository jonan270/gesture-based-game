using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "New Character", menuName = "Character")]

public abstract class Character : MonoBehaviour
{
    public HealthBar healthBar;

    [SerializeField]
    protected float currentHealth;
    [SerializeField]
    protected float maxHealth = 100;

    public int attackValue;
    public string Name;
    protected bool isAlive;
    
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

    public enum ElementState
    {
        Fire, Earth, Water, Wind
    }

    public enum CharacterState
    {
        LookAtCard, // "Idle" mode, Character standing still
        Walk, //walking mode
        AttackMode // Attack mode, Character is about to perform an attack
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

    public bool CheckHealth() //är det en funktion för att kolla om karaktären lever eller för att få veta hur mycke liv finns kvar
    {
        if (currentHealth <= 0) //Check if still alive
        {
            isAlive = false;
        }
        else
            isAlive = true;

        return isAlive;
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
    }
}
