using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{

    private bool travelling = false; // Is the projectile currently travelling?

    private float speed = 0.5f; // Serialized variable to control speed of projectiles
    private Vector3 projectileHeight = new Vector3(0, 1f, 0);

    private float startTime; // At what time does the projectile start travelling?
    private float journeyLength; // Length of journey for projectile

    private GameObject particleObj;

    // Lerp between start and end
    private Vector3 startTarget;
    private Vector3 endTarget;

    private float projectileDamage;
    private Character projectileTarget;

    public void CreateProjectile(float projectileDmg, Character user, Character projectileTarg, GestureType gesture)
    {
        projectileDamage = projectileDmg;
        projectileTarget = projectileTarg;

        startTarget = user.transform.position; //+ projectileHeight;
        endTarget = projectileTarg.transform.position + projectileHeight;

        GameObject projectilePrefab = user.GetEffectFromGesture(gesture);


        particleObj = Instantiate(projectilePrefab, user.transform.position, Quaternion.identity);
        //particleObj.transform.localScale = user.transform.localScale;
        particleObj.transform.LookAt(endTarget);

        startTime = Time.time;

        //particleObj.transform.localEulerAngles = new Vector3(0, 0, 0);
        projectileDamage = projectileDmg;

        travelling = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (travelling)
            LerpProjectile();
    }

    /// <summary>
    /// Lerp projectile
    /// </summary>
    private void LerpProjectile()
    {
        //Debug.Log("Particle Obj: " + particleObj);

        journeyLength = Vector3.Distance(startTarget, endTarget);
        //Debug.Log("journeyLength is: " + journeyLength);
        //projectileObj.transform.position = to;
        float distanceCovered = (Time.time - startTime);
        //Debug.Log("distanceCovered is: " + distanceCovered);
        float fraction = speed * distanceCovered / journeyLength;

        //Debug.Log("Fraction is: " + fraction);

        particleObj.transform.position = Vector3.Lerp(startTarget, endTarget, fraction);
        if (fraction > 0.99)
        {
            AbilityManager.ManagerInstance.DamageCharacter(projectileTarget, projectileDamage);
            travelling = false;
            Destroy(particleObj);
            Destroy(this);
        }
    }
}
