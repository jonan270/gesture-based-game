using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DrinkMead")]
public class DrinkMead : AbilityData
{
    ParticleSystem particleSystem;

    private void Awake()
    {
        //particleSystem = effectPrefab.GetComponent<ParticleSystem>();
        //particleSystem = effectPrefab.GetComponent<ParticleSystem>();
    }

    public override void ActivateAbility()
    {
        //Debug.Log("Before: " + particleSystem.isPlaying);
        //particleSystem.Play();
        //Debug.Log("After: " + particleSystem.isPlaying);

        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        float currentAtk = me.attackMultiplier;

        currentAtk = 1f;
        // Multiply attack by 2, divide health by 2, active for 3 turns
        AbilityManager.ManagerInstance.ActivateTurnBasedAbility(me, 0, 2f, 0.5f, 2, gestureType);
        AbilityCompleted();
    }
}
