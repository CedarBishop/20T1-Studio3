using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Photon.Pun.PhotonView parentsPV;


    private void OnTriggerEnter(Collider other)
    {
        if (parentsPV.IsMine == false)
        {
            if (other.TryGetComponent<Projectile>(out Projectile proj))
            {
                if (proj.isMyProjectile)
                {
                    Destroy(proj.gameObject);
                }
            }
            return;
        }

        if (other.TryGetComponent<Projectile>(out Projectile projectile))
        {
            if (projectile.isMyProjectile == false)
            {
                BlockedProjectile(projectile.gameObject);
            }
        }
    }

    protected virtual void BlockedProjectile (GameObject projectile)
    {
        Destroy(projectile);
    }
}
