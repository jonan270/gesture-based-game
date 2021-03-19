using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject spawnedPlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("LaunchScene");

            return;
        }
        if (playerPrefab == null)
        { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            SpawnPlayer();
        }
    }
    /// <summary>
    /// Instantiate network player at spawn location
    /// </summary>
    private void SpawnPlayer()
    {
        GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("Respawn");
        GameObject xrRig = GameObject.Find("XR Rig"); //local xr rig 
        Vector3 spawnPoint;

        if (PhotonNetwork.IsMasterClient)
        {
            spawnPoint = spawnpoints[0].transform.position;
            xrRig.transform.position = spawnPoint;
        }
        else
        {
            spawnPoint = spawnpoints[1].transform.position;
            xrRig.transform.position = spawnPoint;
            xrRig.transform.Rotate(0, 180f, 0);
        }
        spawnedPlayerPrefab = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// Called when local player leaves the room
    /// </summary>
    //
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }

    /// <summary>
    /// Called when a Photon Player got connected.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            
        }
    }

    /// <summary>
    /// Called when a Photon Player got disconnected. 
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

}
