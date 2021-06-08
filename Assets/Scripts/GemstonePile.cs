using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemstonePile : MonoBehaviour
{
    public Hextile gemTile;

    public int amountGems;
    //[SerializeField]
    //public GameObject prefabLargePile;
    //[SerializeField]
    //public GameObject prefabSmallPile;

    void Start()
    {
        
    }

    public void RemoveGemstonePile(GameObject ob)
    {
        if(ob != null)
            Destroy(ob);
    }

    /// <summary>
    /// Set up the pile.
    /// </summary>
    /// <param name="amGems">Amount of gems in the pile.</param>
    public void InitializePile(int amGems)
    {
        amountGems = amGems;
        gemTile = transform.parent.gameObject.GetComponent<Hextile>();

        //if (amountGems <= 5)
        //{
        //    prefabSmallPile.SetActive(true);
        //}
        //else if (amountGems > 5)
        //    prefabLargePile.SetActive(true);
    }
}
