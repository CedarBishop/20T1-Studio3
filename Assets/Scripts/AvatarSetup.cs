using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetup : MonoBehaviour
{
    PhotonView photonView;
    public GameObject character;
    public int characterValue;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    public int roomNumber;
    //  public CameraController cameraPrefab;


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (int.TryParse(PhotonNetwork.NickName, out roomNumber))
        {
            print("Room number parsed " + roomNumber);
        }
        transform.position = LevelManager.instance.spawnPoints[roomNumber - 1].position;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.selectedCharacter);
        }
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        //Instantiate(cameraPrefab,transform);
    }

    [PunRPC]
    void RPC_AddCharacter(int characterNum)
    {
        UIManager.instance.ClearWinText();
        characterValue = characterNum;
        character = Instantiate(PlayerInfo.playerInfo.allCharacters[characterValue], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform);
    }

    public void Die()
    {
        DisableControls();
        photonView.RPC("RPC_Die", RpcTarget.Others, photonView.ViewID);
        Destroy(character);
      //  StartCoroutine("DelayRespawn");
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

    public void Respawn()
    {
        print("Respawn");
        playerCombat.enabled = true;
        playerMovement.enabled = true;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ResetStats", RpcTarget.All);
            print("Add Character");
            photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.selectedCharacter);
        }
    }

    IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(5);
        Respawn();
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
        UIManager.instance.StartRoundTimer();
    }

    public void DisableControls()
    {
        playerCombat.enabled = false;
        playerMovement.enabled = false;
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
    }

    public void StartNewRound()
    {
        playerCombat.enabled = true;
        playerMovement.enabled = true;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ResetStats", RpcTarget.All);
            if (character == null)
            {
                photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.selectedCharacter);
            }
        }
    }
}
