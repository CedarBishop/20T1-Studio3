using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    PhotonView photonView;
    Rigidbody rigidbody;
    public int damage;
    public float force;
    public bool isMyProjectile;
    Vector3 _direction;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(force * transform.forward);
        StartCoroutine("DelayedDestroy");
    }


    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerCombat>())
        {
            if (isMyProjectile)
            {
                if (collision.gameObject.GetComponentInParent<PhotonView>().IsMine)
                {
                    return;
                }
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

    }

    public void SetEmissionColour (Color color)
    {
        Material material = GetComponent<MeshRenderer>().material;
        material.SetColor("_EmissionColor", color);
    }

}
