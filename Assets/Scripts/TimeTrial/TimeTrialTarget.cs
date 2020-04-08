using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialTarget : MonoBehaviour
{
    public MeshRenderer indicator;
    public MeshRenderer targetMesh;

    public Material targetActivatedMaterial;
    public Material targetDeactivatedMaterial;
    
    private Material indicatorActiveMat;
    private Material indicatorDeactivatedMat;
    private bool isActive;


    public void Activate ()
    {
        isActive = true;
        indicator.material = indicatorActiveMat;
        targetMesh.material = targetActivatedMaterial;
    }

    public void Activate(Material activeMat, Material deactivatedMat)
    {
        targetActivatedMaterial = activeMat;
        targetDeactivatedMaterial = deactivatedMat;
        Activate();
    }

    public void Deactivate ()
    {
        isActive = false;
        indicator.material = indicatorDeactivatedMat;
        targetMesh.material = targetDeactivatedMaterial;
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
