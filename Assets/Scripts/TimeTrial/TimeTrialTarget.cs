using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialTarget : MonoBehaviour
{
    public MeshRenderer indicator;
    public MeshRenderer targetMesh;

    public Material targetActivatedMaterial;
    public Material targetDeactivatedMaterial;

    public GameObject minimapIndicator;

    private Material indicatorActiveMat;
    private Material indicatorDeactivatedMat;
    private bool isActive;

    private BoxCollider collider;


    public void Activate ()
    {
        isActive = true;
        collider.enabled = isActive;
        indicator.material = indicatorActiveMat;
        targetMesh.material = targetActivatedMaterial;
        minimapIndicator.layer = 10;
    }

    public void Activate(Material activeMat, Material deactivatedMat)
    {
        collider = GetComponent<BoxCollider>();
        indicatorActiveMat = activeMat;
        indicatorDeactivatedMat = deactivatedMat;
        Activate();
    }

    public void Deactivate ()
    {
        isActive = false;
        collider.enabled = isActive;
        indicator.material = indicatorDeactivatedMat;
        targetMesh.material = targetDeactivatedMaterial;
        TimeTrialManager.instance.TargetHit();
        minimapIndicator.layer = 11;
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
