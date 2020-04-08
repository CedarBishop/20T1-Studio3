using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialTarget : MonoBehaviour
{
    public GameObject indicator;
    
    private Material indicatorActiveMat;
    private Material indicatorDeactivatedMat;
    private bool isActive;

    public void Activate ()
    {
        isActive = true;
    }

    public void Activate(Material activeMat, Material deactivatedMat)
    {
        isActive = true;
    }

    public void Deactivate ()
    {
        isActive = false;
        TimeTrialManager.instance.TargetHit();
    }

    public bool CheckIfActive ()
    {
        return isActive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            Deactivate();
        }
    }


}
