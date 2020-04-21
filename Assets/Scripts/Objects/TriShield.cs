using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriShield : MonoBehaviour
{
    PhotonView photonView;
    public MiniShield[] miniShields;
    public float rotationSpeeds;
    public float shieldRespawnTime;
    public PhotonView playerPhotonView;

    private Transform playerTransform;

    private bool hasTriShieldAbility;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (miniShields != null)
        {
            for (int i = 0; i < miniShields.Length; i++)
            {
                miniShields[i].gameObject.SetActive(false);
            }
        }

    }


    private void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }


    public void SetAbility ()
    {
        hasTriShieldAbility = true;
    }

    public void Initialise()
    {
        if (hasTriShieldAbility == false)
        {
            return;
        }
        if (miniShields != null)
        {
            for (int i = 0; i < miniShields.Length; i++)
            {
                miniShields[i].isMine = true;
            }
        }
        photonView.RPC("RPC_ActivateAll", RpcTarget.All);
    }


    [PunRPC]
    void RPC_ActivateAll ()
    {
        if (miniShields != null)
        {
            for (int i = 0; i < miniShields.Length; i++)
            {
                miniShields[i].triShield = this;
                miniShields[i].index = i;
                miniShields[i].gameObject.SetActive(true);
                miniShields[i].parentsPV = playerPhotonView;
            }
        }
        playerTransform = transform.parent;
        transform.parent = null;
    }

    [PunRPC]
    void RPC_ActivateOne (bool active, int index)
    {
        miniShields[index].gameObject.SetActive(active);
    }

    void FixedUpdate()
    {
         transform.Rotate(0, rotationSpeeds * Time.fixedDeltaTime, 0);
    }

    public void ResetMiniShield (int index)
    {
        photonView.RPC("RPC_ActivateOne",RpcTarget.All,false,index);

        StartCoroutine("CoResetMiniShield", index);
    }

    IEnumerator CoResetMiniShield (int index)
    {
        yield return new WaitForSeconds(shieldRespawnTime);
        photonView.RPC("RPC_ActivateOne", RpcTarget.All, true, index);
    }
}
