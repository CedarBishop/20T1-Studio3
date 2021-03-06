﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetup : MonoBehaviour
{
    PhotonView photonView;
    public GameObject character;
    public int characterValue;
    public TriShield triShield;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private AbilitiesManager abilitiesManager;
    public int roomNumber;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        abilitiesManager = GetComponent<AbilitiesManager>();
        
        if (int.TryParse(PhotonNetwork.NickName, out roomNumber))
        {
            print("Room number parsed " + roomNumber);
        }
        transform.position = LevelManager.instance.spawnPoints[roomNumber - 1].position;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.instance.selectedCharacter);
        }



    }

    [PunRPC]
    void RPC_AddCharacter(int characterNum)
    {        
        GameManager.instance.ClearWinText();
        character = Instantiate(PlayerInfo.instance.allCharacters[characterNum], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
        print(transform.position);
        character.name = character.name + " Model";
        abilitiesManager.InitCharacterMaterials(character);
    }

    public void Die()
    {
        DisableControls();
        photonView.RPC("RPC_Die", RpcTarget.Others, photonView.ViewID);
        Destroy(character);
    }

    [PunRPC]
    void RPC_Die(int id)
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < photonViews.Length; i++)
        {
            if (photonView.ViewID == id)
            {
                Destroy(character);

            }
        }
    }


    [PunRPC]
    void RPC_ResetStats()
    {
        AvatarSetup[] avatarSetups = FindObjectsOfType<AvatarSetup>();
        if (avatarSetups != null)
        {
            for (int i = 0; i < avatarSetups.Length; i++)
            {
                if (avatarSetups[i].GetComponent<PhotonView>().IsMine)
                {
                    avatarSetups[i].ResetStats();
                }
            }
        }
    }

    public void ResetStats()
    {
        print("Player " + roomNumber + " Reseting Stats");
        print(LevelManager.instance.spawnPoints[roomNumber - 1].position);
        transform.position = LevelManager.instance.spawnPoints[roomNumber - 1].position;
        playerCombat.ResetHealth();
    }

    public void DisableControls()
    {
        playerCombat.enabled = false;
        playerMovement.enabled = false;
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        print("Disable Controls");
    }

    public void StartNewRound()
    {
        playerCombat.enabled = true;
        playerMovement.enabled = true;
        triShield.Initialise();
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ResetStats", RpcTarget.All);
            if (character == null)
            {
                photonView.RPC("RPC_AddCharacter", RpcTarget.All, PlayerInfo.instance.selectedCharacter);
            }
        }
    }
}
