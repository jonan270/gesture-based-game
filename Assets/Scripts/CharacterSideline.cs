using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterSideline : MonoBehaviour
{
    public static CharacterSideline Instance;

    public Transform sideLineStartPlayer1, sideLineStartPlayer2;

    [SerializeField] private Vector3 offset;

    private int counter = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddSidelineCharcater(GameObject prefab)
    {
        Vector3 charRotation = PhotonNetwork.IsMasterClient ? new Vector3(0, -90, 0) : new Vector3(0, 90, 0);
        var obj = PhotonNetwork.Instantiate(prefab.name, GetNextSpawnPoint(), Quaternion.Euler(charRotation));
        obj.transform.parent = transform;
        ++counter;
    }

    private Vector3 GetNextSpawnPoint()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return sideLineStartPlayer1.position + offset * counter;
        }
        else
        {
            return sideLineStartPlayer2.position - offset * counter;
        }
    }
}
