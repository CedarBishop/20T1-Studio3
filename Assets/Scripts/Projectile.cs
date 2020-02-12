using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    PhotonView photonView;
    Rigidbody2D rigidbody;
    public int damage;
    public float force;
    public bool isMyProjectile;
    public long id; 
    Vector2 _direction;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(force * transform.right);

    }


    IEnumerator DelyedDestroy()
    {
        yield return new WaitForSeconds(3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerCombat>())
        {
            if (isMyProjectile)
            {
                return;
            }

            if (collision.gameObject.GetComponentInParent<PhotonView>())
            {
                if (collision.gameObject.GetComponentInParent<PhotonView>().IsMine)
                {
                    collision.gameObject.GetComponentInParent<PlayerCombat>().TakeDamage(damage);                    

                    print("hit by enemy");
                  
                }
            }
        }


        Destroy(gameObject);
        photonView.RPC("RPC_ProjectileByID", RpcTarget.Others, id);

    }

    [PunRPC]
    void RPC_ProjectileByID(long id)
    {
        foreach (var item in FindObjectsOfType<Projectile>())
        {
            if (item.id == id)
            {
                Destroy(item.gameObject);
                return;
            }
        }
    }

}
