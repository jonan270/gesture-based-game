using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private static Image foregroundImage;
    public static HealthBar Create(Vector3 position, Vector3 size)
     {
         //Main health bar
         GameObject healthBarGameObject = new GameObject("HealthBar");
         healthBarGameObject.AddComponent<Canvas>();
         healthBarGameObject.transform.position = position;

         //Background
         GameObject backgroundGameObject = new GameObject("Background");
         Sprite backgroundSprite = Resources.Load("blood_red_bar", typeof(Sprite)) as Sprite;
         Image rend = backgroundGameObject.AddComponent<Image>();
         rend.sprite = backgroundSprite;
         backgroundGameObject.transform.SetParent(healthBarGameObject.transform);
         backgroundGameObject.transform.localPosition = Vector3.zero;
         backgroundGameObject.transform.localScale = size;
         backgroundGameObject.GetComponent<Image>().color = Color.grey;
        //backgroundGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.blood_red_bar;
        //backgroundGameObject.GetComponent<SpriteRenderer>().sortingOrder = 100;

        /*//Bar
        GameObject barGameObject = new GameObject("Bar");
        barGameObject.transform.SetParent(healthBarGameObject.transform);
        barGameObject.transform.localPosition = new Vector3(- size.x / 4f, 0f);
        barGameObject.transform.localScale = size;
        */

        //Foreground
        GameObject foregroundGameObject = new GameObject("Foreground");
        Sprite foregroundSprite = Resources.Load("blood_red_bar", typeof(Sprite)) as Sprite;
        foregroundImage = foregroundGameObject.AddComponent<Image>();
        foregroundImage.sprite = foregroundSprite;
        foregroundImage.GetComponent<Image>().type = Image.Type.Filled;
        foregroundImage.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;

        foregroundGameObject.transform.SetParent(healthBarGameObject.transform);
        foregroundGameObject.transform.localPosition = Vector3.zero;
        foregroundGameObject.transform.localScale = size;
        foregroundGameObject.GetComponent<Image>().color = Color.red;
        //barSpriteGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.blood_red_bar;
        //barSpriteGameObject.GetComponent<SpriteRenderer>().sortingOrder = 110; // so the sprite is on top background

        HealthBar healthBar = healthBarGameObject.AddComponent<HealthBar>();

         return healthBar;
     }

    private Transform bar;

    public void SetSize(float pct)
    {
        foregroundImage.fillAmount = pct;
        //bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    private void Awake()
    {
        bar = transform.Find("Bar");
    }



    /* private IEnumerator ChangeToPct(float pct)
     {
         float preChangePct = foregroundImage.fillAmount;
         float elapsed = 0.f;

         while (elapsed < updateSpeedSeconds)
         {
             elapsed += Time.deltaTime;
             foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds); // preChangePct is value of current healthbar, pct is the percent to be dropped
             yield return null;
         }
         foregroundImage.fillAmount = pct;
     }*/

    private void LateUpdate() //Function to make health bar always turn to the main camera (always facing the player)
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}