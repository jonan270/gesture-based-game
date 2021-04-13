using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedEffect : MonoBehaviour
{
    public float healthMod;
    public float defMod;
    public float attackMod;

    public int turnCount;


    /// <summary>
    /// Set the turn based effect onto character
    /// </summary>
    /// <param name="character"></param>
    /// <param name="hMod"></param>
    /// <param name="aMod"></param>
    /// <param name="dMod"></param>
    /// <param name="turns"></param>
    public void setTurnBased(Character character, float hMod, float aMod, float dMod, int turns)
    {
        //isActive = true;
        healthMod = hMod;
        attackMod = aMod;
        defMod = dMod;
        turnCount = turns;

        character.attackMultiplier *= attackMod;
        character.defenceMultiplier *= defMod;

        ApplyTurnBased(character); // Activate effect right away
    }

    /// <summary>
    /// Apply the turnbased effect on a character
    /// </summary>
    /// <param name="character">character to apply effect on</param>
    public void ApplyTurnBased(Character character)
    {
        if (IsActive())
        {
            Debug.Log("Turn based effect doing turn based things on " + character.name);
            Debug.Log("Current health: " + character.CurrentHealth + ", Defensemultiplier: " + character.defenceMultiplier);
            character.ModifyHealth(healthMod);
            turnCount--;
        }
        else
            RemoveTurnBased(character);
    }

    /// <summary>
    /// Check if this effect should be active
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        if (turnCount > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Remove this turnbased effect. TODO: Maybe rework to remove a specific turnbased ability? Works for now.
    /// </summary>
    /// <param name="character"> character to remove turnbased effect from </param>
    private void RemoveTurnBased(Character character)
    {
        character.attackMultiplier /= attackMod;
        character.defenceMultiplier /= defMod;
        foreach(var ability in character.ListAbilityData)
        {
            if(ability.isTurnbased)
            {
                ability.visualizeAbility(false);
                break; // Break after first found
            }
        }
    }
}
