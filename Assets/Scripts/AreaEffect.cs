using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    /// <summary>
    /// Effects may have an element. For example Water traps deal extra damage to Fire characters
    /// </summary>
    public ElementState TrapElement{get; private set;}
    /// <summary>
    /// Healthmodifier adds or removes health from the character
    /// </summary>
    public float healthModifier;
    /// <summary>
    /// Is this effect active in the game?
    /// </summary>
    public bool isActivated = false;
    
    /// <summary>
    /// Set values of the effect
    /// </summary>
    /// <param name="eState"></param>
    /// <param name="hMod"></param>
    public void SetEffect(ElementState eState, float hMod)
    {
        isActivated = true;
        TrapElement = eState;
        healthModifier = hMod;
    }

    /// <summary>
    /// Sets this effect to inactive and removes healthModifier.
    /// </summary>
    public void Remove()
    {
        isActivated = false;
        healthModifier = 0;
    }

    /// <summary>
    /// Checks for elements(TODO) and applies the effect to a given character.
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public float ApplyEffect(Character character)
    {
        // TODO: Check for element types.

        // Check that health does not exceed maxhealth.
        if (healthModifier + character.currentHealth > character.maxHealth)
            return character.maxHealth;
        else
            return healthModifier;
    }
}
