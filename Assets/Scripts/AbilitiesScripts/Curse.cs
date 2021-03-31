using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Curse")]
public class Curse : AbilityData
{
    public override void OnHit(GameObject target, GameObject attacker)
    {
        //GameObject.
            Vector3 center = target.GetComponent<Hextile>().Position;
            float radius = 3;

            //Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (Collider col in Physics.OverlapSphere(center, radius))
            {
                if (col.tag == "Hextile")
                {
                    col.gameObject.GetComponent<Hextile>().makeType(ElementState.Fire);
                }
            }
        
    }
}
