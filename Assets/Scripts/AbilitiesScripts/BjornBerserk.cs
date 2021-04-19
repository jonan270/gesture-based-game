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
        int x = me.CurrentTile.tileIndex.x;
        int y = me.CurrentTile.tileIndex.y;
        //visualizeAbility(true);

        // Multiply attack by 2, divide defence by 2, active for 3 turns
        //me.activeEffect = effectPrefab;
        //PlayerManager.Instance.
        Character target = PlayerManager.Instance.GetCharacterAt(x, y);
        //target.activeEffect = effectPrefab;

        AbilityManager.ManagerInstance.ActivateTurnBasedAbility(target, 0f, 2f, 0.5f, 3, gestureType);
        //Debug.Log("Starting");
        //Debug.Log(me.turnBasedEffects[0].attackMod);
        
        AbilityCompleted();
    }
}
