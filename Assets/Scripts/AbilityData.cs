using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityData : ScriptableObject
{
    /// <summary>
    /// Name of the ability, displayed on card
    /// </summary>
    public string abilityName;
    /// <summary>
    /// Short description of ability, displayed on card
    /// </summary>
    public string abilityDescription;

    /// <summary>
    /// How many gems it cost to cast an ability, displayed on card
    /// </summary>
    public int gemsCost;
    /// <summary>
    /// Amount of damage or healing this ability does, displayed on card
    /// </summary>
    public float powerValue;
    /// <summary>
    /// What type of ability this is, single, multi, ground, displayed on card
    /// </summary>
    public TargetType targetType;
    /// <summary>
    /// What type of element this ability is, can be displayed on card
    /// </summary>
    public ElementState abilityElement;
    /// <summary>
    /// Number of turns the ability lasts for, can be displayed on card
    /// </summary>
    public int numberOfTurns;
    /// <summary>
    /// If the ability has reached maximum number of turns
    /// </summary>
    protected bool IsFinished;
    /// <summary>
    /// What type of gesture this ability is associated to
    /// </summary>
    public GestureType gestureType;
    /// <summary>
    /// Bonues damage or healing this ability does depending on element types etc, can be displayed on card
    /// </summary>
    public float bonusPowerMultiplier = 5;
    /// <summary>
    /// Effect of this ability
    /// </summary>
    public GameObject effectPrefab; //Would be used for animate/effect for ability

    /// <summary>
    /// Wrapper function for all abilities, logic is implemented in subclass
    /// </summary>
    public abstract void ActivateAbility();

    /// <summary>
    /// Called when an ability has finished casting, resets hands and tells the gameRound manager that an action has been done. 
    /// </summary>
    protected void AbilityCompleted()
    {
        //Release character and objects from hands
        var controllers = FindObjectsOfType<CharacterSelector>();
        foreach(var controller in controllers)
        {
            controller.ReleaseCharacter();
        }
        //notify gameround that an action has been completed
        var gameRound = FindObjectOfType<GameRound>();
        gameRound.ActionTaken();
    }
}

/// <summary>
/// Type of target ability, found in AbilityData.cs
/// </summary>
public enum TargetType
{
    single,
    multi,
    ground
}
