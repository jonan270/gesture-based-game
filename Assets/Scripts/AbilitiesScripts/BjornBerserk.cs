using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BjornBerserk")]
public class BjornBerserk : AbilityData
{
    
    private void Start()
    {
        isTurnbased = true;
    }

    public override void ActivateAbility()
    {
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        visualizeAbility(true);

        // Multiply attack by 2, divide defence by 2, active for 3 turns
        AbilityManager.ManagerInstance.ActivateTurnBasedAbility(me, 0f, 2f, 0.5f, 3);
        Debug.Log("Starting");
        Debug.Log(me.turnBasedEffects[0].attackMod);
        
        AbilityCompleted();
    }
}
