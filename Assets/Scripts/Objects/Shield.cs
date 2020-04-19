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
