using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemstonePile : MonoBehaviour
{
    public Hextile gemTile;

    public int amountGems;

    public GameObject prefabLargePile;
    public GameObject prefabSmallPile;

    void Start()
    {
        amountGems = Random.Range(1, 10);

        if(amountGems < 5)
        {
            Instantiate(prefabSmallPile, gemTile.Position, Quaternion.identity);
        }
        else
        {
            Instantiate(prefabLargePile, gemTile.Position, Quaternion.identity);
        }
    }

    public void removeGemstonePile(GameObject ob)
    {
        if(ob != null)
            Destroy(ob);
    }
}
