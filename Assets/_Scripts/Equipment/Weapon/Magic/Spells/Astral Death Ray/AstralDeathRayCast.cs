using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralDeathRayCast : MonoBehaviour
{
    private AstralDeathRayBehavior _deathRay;

    RaycastHit2D hit;

    private void Awake()
    {
        _deathRay = GetComponentInParent<AstralDeathRayBehavior>();
        _createRaycast();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(hit) DebugHit(hit);
    }

    private void _createRaycast()
    {
        hit = Physics2D.Raycast(transform.position, Vector3.up, _deathRay.LaserDistance, GetIgnoreLayerMask("AllyProjectiles","Player"));
    }

    private void DebugHit(RaycastHit2D hit)
    {
        Debug.Log("hit:" + hit.collider.name);
    }

    private int GetIgnoreLayerMask(params string[] layerNamesToIgnore)
    {
        int layerMask = 0;

        foreach (string layerName in layerNamesToIgnore)
        {
            int layer = LayerMask.NameToLayer(layerName);
            layerMask |= 1 << layer;
        }

        return ~layerMask;
    }
}
