using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityData : ScriptableObject
{
    //public int baseDamage;
    public int abilityValue;
    public int numberOfTargets;
    public int duration;

    public bool areaAttack;
    public bool healAbility;
    public bool buffAbility;

    public GameObject effectPrefab; //Would be used for animate/effect for ability

}
