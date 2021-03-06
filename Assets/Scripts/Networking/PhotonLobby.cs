﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby photonLobby;
    public GameObject battleButton;
    public GameObject cancelButton;
    public PhotonRoom room;
    public string roomNumberString;

    private void Awake()
    {
        if (photonLobby == null)
        {
            photonLobby = this;
        }
        else if (photonLobby != this)
        {
            Destroy(gameObject);
        }
        Instantiate(room);
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            battleButton.SetActive(true);
            cancelButton.SetActive(false);
        }
        else
        {
            battleButton.SetActive(false);
            cancelButton.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
        }
       
    }

    public override void OnConnectedToMaster ()
    {
        print("Player has connected to master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked ()
    {
        SoundManager.instance.PlaySFX("Button");
        if (string.IsNullOrEmpty(roomNumberString))
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.JoinRoom(roomNumberString);
        }
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("failed to join random room");
        PhotonRoom.instance.CreateRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonRoom.instance.CreateRoom();
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Failed to create a new room");
        PhotonRoom.instance.CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        SoundManager.instance.PlaySFX("Button");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }   
}
