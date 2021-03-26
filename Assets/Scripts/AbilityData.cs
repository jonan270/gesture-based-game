using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityData : ScriptableObject
{
    //public int baseDamage;
    public string abilityName;
    public string abilityDescription;

    public int gemsCost;
    public int amount;
    public TargetType targType;

    public int numberOfTurns;
    public bool IsFinished;

    public string gestureType;

    public static int extraPowerElement = 5;

    public GameObject effectPrefab; //Would be used for animate/effect for ability

    //public abstract void TriggerAbility();
    public abstract void OnHit(GameObject target, GameObject attacker);

}
/*
public abstract class dmgOrHealAbility : AbilityData // Berserk, Trap, Multishot, Heal, Cleave
{
    //public abstract void OnHit(Character target, Character attacker);
}

public abstract class affectEnvironment : AbilityData // Curse
{
    //public abstract void changeTiles(Hextile tile);
}

public abstract class Buff : AbilityData // Poison, Ring of Attack, Drink Mead
{

   // public abstract void Apply(Character t);


}*/

public enum TargetType
{
    SINGLE,
    MULTI
}
