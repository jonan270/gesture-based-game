using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Curse")]
public class Curse : AbilityData
{
    /// <summary>
    /// how large area around the middle tile should this ability affect
    /// </summary>
    public int tileRadius = 2;

    public override void ActivateAbility()
    {
        Debug.Log("Waiting for a tile to be selected");
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.chooseTile);
        PlayerManager.Instance.SubscribeToSelectTargetTile(OnSelectedTile);
    }

    private void OnSelectedTile(Hextile tile)
    {
        Debug.Log("Does something to the tile with index: " + tile.tileIndex);
        Hexmap map = FindObjectOfType<Hexmap>();
        map.affectRadius(tile.tileIndex.x, tile.tileIndex.y, tileRadius, abilityElement);
        PlayerManager.Instance.UnsubscribeFromSelectTargetTile(OnSelectedTile);
        AbilityCompleted();
    }

}
