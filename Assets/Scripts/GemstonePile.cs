using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemstonePile : MonoBehaviour
{
    public Hextile gemTile;

    public int amountGems;
    [SerializeField]
    public GameObject prefabLargePile;
    [SerializeField]
    public GameObject prefabSmallPile;

    void Start()
    {
        
    }

    public void removeGemstonePile(GameObject ob)
    {
        if(ob != null)
            Destroy(ob);
    }

    public void setSize(int amGems)
    {
        amountGems = amGems;

        if (amountGems <= 5)
        {
            prefabSmallPile.SetActive(true);
        }
        else if (amountGems > 5)
            prefabLargePile.SetActive(true);

        
    }
}
