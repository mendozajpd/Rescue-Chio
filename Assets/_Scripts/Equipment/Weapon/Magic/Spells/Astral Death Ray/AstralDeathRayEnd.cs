using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayEnd : MonoBehaviour
{
    private AstralDeathRayBehavior _deathRay;


    public void SetDeathRay(AstralDeathRayBehavior deathRay)
    {
        _deathRay = deathRay;
    }

    public void SetLaserTipPosition(Vector3 laserTipPos, AstralDeathRayEnd laserTip)
    {
        laserTip.transform.position = laserTipPos;
    }
}
