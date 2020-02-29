using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class GameSetup : MonoBehaviour
{
    public static GameSetup instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine("DisconnectAndLoad");
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    
}
