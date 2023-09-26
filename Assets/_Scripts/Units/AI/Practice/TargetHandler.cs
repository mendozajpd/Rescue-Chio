using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetHandler : Transition
{
    protected Transform _origin;
    protected GameObject _target;

    protected const float maxDistance = 8f;
    protected float attackRange = 3f;

    protected float _distance => Vector2.Distance(_origin.position, _target.transform.position);

    protected TargetHandler(Transform origin, GameObject target) 
    {
        _origin = origin;
        _target = target;
    }

}

public class TargetIsFar : TargetHandler
{
    public TargetIsFar(Transform origin, GameObject target) : base(origin, target) { }

    public override bool CheckCondition() 
    {
        Debug.Log("target is :" + _target.name);

        return _target != null ? _distance > maxDistance : false;
    }
}

public class TargetIsNear : TargetHandler
{
    public TargetIsNear(Transform origin, GameObject target) : base(origin, target) { }

    public override bool CheckCondition() 
    {
        Debug.Log("target is :" + _target.name);

        return _target != null ? _distance <= maxDistance && _distance > attackRange : false;
    }
}

public class TargetIsInRange : TargetHandler
{
    public TargetIsInRange(Transform origin, GameObject target) : base(origin, target) { }

    public override bool CheckCondition() 
    {
        Debug.Log("target is :" + _target.name);

        return _target != null ? _distance <= attackRange : false;
    } 

}