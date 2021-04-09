using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BjornBerserk")]
public class BjornBerserk : AbilityData
{

    public override void ActivateAbility()
    {
        Debug.Log("BJORN ANGRY");
        Character me = PlayerManager.Instance.selectedCharacter.GetComponent<Character>();

        // Multiply attack by 2, divide health by 2, active for 3 turns
        //me.AddTurnBasedEffect(0, 2f, 0.5f, 3);
        AbilityManager.ManagerInstance.ActivateTurnBasedAbility(me, 0, 2f, 0.5f, 3);
        //AbilityManager.ManagerInstance.DamageCharacter(me.CurrentTile.tileIndex.x, me.CurrentTile.tileIndex.y);
        //AbilityCompleted();
    }
}
