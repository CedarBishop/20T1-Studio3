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


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered,PlayerInfo.playerInfo.selectedCharacter);
        }
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    [PunRPC]
    void RPC_AddCharacter(int characterNum)
    {
        characterValue = characterNum;
        character = Instantiate(PlayerInfo.playerInfo.allCharacters[characterValue],transform.position, transform.rotation, transform);
    }

    public void Die ()
    {
        playerCombat.enabled = false;
        playerMovement.enabled = false;
        photonView.RPC("RPC_Die",RpcTarget.Others,photonView.ViewID);
        Destroy(character);
    }

    [PunRPC]
    void RPC_Die (int id)
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < photonViews.Length; i++)
        {
            if (photonView.ViewID == id)
            {
                Destroy(character);

                StartCoroutine("DelayRespawn");
            }
        }
    }

    public void Respawn()
    {
        playerCombat.enabled = true;
        playerMovement.enabled = true;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.selectedCharacter);
        }
    }

    IEnumerator DelayRespawn ()
    {
        yield return new WaitForSeconds(5);
        Respawn();
    }
}
