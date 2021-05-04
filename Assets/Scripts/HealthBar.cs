using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbar;


    /// <summary>
    /// Fill healthbar with % amount
    /// </summary>
    /// <param name="pct"></param>
    public void SetFill(float pct)
    {
        healthbar.fillAmount = pct;
    }

    private void LateUpdate()
    {
        //Function to make health bar always turn to the main camera (always facing the player)
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}