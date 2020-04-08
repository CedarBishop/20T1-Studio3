using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropMine : MonoBehaviour
{
    public float timeBeforeExplode;
    public ParticleSystem explosionParticle;
    public int damage;
    public int roomNumber;
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        object[] data = photonView.InstantiationData;

        roomNumber = (int)data[0];

        StartCoroutine("CoExplode"); // start timer before this mine explodes
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerCombat>())             // Check if colliders has player combat component
        {
            PlayerCombat player = other.GetComponentInParent<PlayerCombat>();
            if (player.roomNumber != roomNumber) // check if that room number of that player does not equal the number of the player who placed this mine
            {
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    player.TakeDamage(damage, false); // if so, the damage that player and play explosion particle
                    photonView.RPC("RPC_Explosion", RpcTarget.All); ;
                }
            }
        }
    }

    IEnumerator CoExplode ()
    {
        yield return new WaitForSeconds(timeBeforeExplode);

        Explosion();
    }

    [PunRPC]
    void RPC_Explosion()
    {
        Explosion();
    }

    void Explosion ()
    {
        if (explosionParticle != null)
        {
            SoundManager.instance.PlaySFX("MineExplosion");
            ParticleSystem particle = Instantiate(explosionParticle, transform.position,Quaternion.identity);
            particle.Play();
            Destroy(particle, 1);
        }
        Destroy(gameObject);
    }


}
