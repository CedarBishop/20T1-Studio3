using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniShield : Shield
{
    [HideInInspector] public int index;
    [HideInInspector] public TriShield triShield;
    [HideInInspector] public bool isMine;


    protected override void BlockedProjectile(GameObject projectile)
    {
        Destroy(projectile);
        triShield.ResetMiniShield(index);
    }
}
