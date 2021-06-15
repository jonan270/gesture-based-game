using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;

public class NewTestScript
{
    [UnityTest]
    public IEnumerator NewTestScriptSimplePasses()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel("Andreas_sprint_2");
        LogAssert.Expect(LogType.Error, "Can not Instantiate before the client joined/created a room. State: ConnectedToMasterServer");
        yield return new WaitForSeconds(5.0f);
    }
}
