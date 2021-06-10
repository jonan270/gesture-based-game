using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemstonePile : MonoBehaviour
{
    public Hextile gemTile;
    [SerializeField]
    private GameObject gem;
    [SerializeField]
    private GameObject textObject;

    public int amountGems;

    [SerializeField]
    private float rotationSpeed;
    //[SerializeField]
    //public GameObject prefabLargePile;
    //[SerializeField]
    //public GameObject prefabSmallPile;

    void Start()
    {
        
    }

    private void Update()
    {
        gem.transform.rotation = Quaternion.Slerp(gem.transform.rotation, gem.transform.rotation * Quaternion.Euler(0, 0, 90), Time.deltaTime * rotationSpeed);
        textObject.transform.LookAt(Camera.main.transform);
        textObject.transform.Rotate(0, 180, 0);
    }

    public void RemoveGemstonePile(GameObject ob)
    {
        if (ob != null)
        {
            amountGems = 0; //Purely for the synchronisation function.
            Destroy(ob);
        }
    }

    /// <summary>
    /// Set up the pile.
    /// </summary>
    /// <param name="amGems">Amount of gems in the pile.</param>
    public void InitializePile(int amGems)
    {
        amountGems = amGems;
        gemTile = transform.parent.gameObject.GetComponent<Hextile>();
        textObject.GetComponent<TextMeshProUGUI>().text = "x" + amGems;
        gem.transform.rotation *= Quaternion.Euler(0, 0, Random.Range(0, 44.9f));

        //if (amountGems <= 5)
        //{
        //    prefabSmallPile.SetActive(true);
        //}
        //else if (amountGems > 5)
        //    prefabLargePile.SetActive(true);
    }
}
