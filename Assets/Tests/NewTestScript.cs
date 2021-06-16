using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;

public class NewTestScript
{
    [UnityTest]
    public IEnumerator GameLaunchSuccessful()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel("LaunchScene");
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Launch Manager").GetComponent<GameLauncher>().Connect();
        //Ignore all import errors because I don't have the original files.
        LogAssert.Expect(LogType.Error, "Not allowed to access vertices on mesh 'PenselKlippt' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access normals on mesh 'PenselKlippt' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'PenselKlippt' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'Hilda' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'MergadKropp' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'Halsduk_test_mirror' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'Kropp' (isReadable is false; Read/Write must be enabled in import settings)");
        LogAssert.Expect(LogType.Error, "Not allowed to access uv4 on mesh 'Test_flärp_back' (isReadable is false; Read/Write must be enabled in import settings)");
        yield return new WaitForSeconds(5.0f);
        //Ensure the player's network counterpart is synced up with the local equivalent.
        GameObject localPlayer = GameObject.Find("Main Camera");
        GameObject networkPlayer = GameObject.Find("Network Player(Clone)").transform.Find("Head").gameObject;
        Assert.AreEqual(localPlayer.transform.position, networkPlayer.transform.position);
        //Ensure the player's network counterpart is synced up with the local equivalent after translation.
        localPlayer.transform.position += new Vector3(0, 0, 1);
        yield return null;
        Assert.AreEqual(localPlayer.transform.position, networkPlayer.transform.position);
        //Ensure the player has the correct amount of cards in hand.
        Assert.That(GameObject.Find("HandCards").transform.childCount == 4);
        yield return null;
    }
}
