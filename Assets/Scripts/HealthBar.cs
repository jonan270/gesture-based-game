using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbar;

    [SerializeField] public Image green;
    [SerializeField] public Image red;


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

    public void setColor(bool master)
    {
        if (master)
        {
            healthbar = red;
            red.enabled = true;
            green.enabled = false;
        }
        else if(!master)
        {
            healthbar = green;
            green.enabled = true;
            red.enabled = false;
            
        }
    }

}