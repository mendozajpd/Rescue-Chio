using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetHandler : Transition
{
    protected Transform _origin;
    protected GameObject _target;

    protected float _maxDistance;
    protected float attackRange = 1.5f;

    protected float _distance => Vector2.Distance(_origin.position, _target.transform.position);

    protected TargetHandler(Transform origin, GameObject target, float aggroDistance) 
    {
        _origin = origin;
        _target = target;
        _maxDistance = aggroDistance;
    }

}

public class TargetIsFar : TargetHandler
{
    public TargetIsFar(Transform origin, GameObject target, float aggroDistance) : base(origin, target, aggroDistance) { }

    public override bool CheckCondition() 
    {
        //if (_target != null) Debug.Log("target is :" + _target.name);

        if (_target == null)
        {
            //Debug.Log("There is no target");
            return false;
        }
        else
        {
            //Debug.Log("Target is far");
            return _distance > _maxDistance;
        }

    }
}

public class TargetIsNear : TargetHandler
{
    public TargetIsNear(Transform origin, GameObject target, float aggroDistance) : base(origin, target, aggroDistance) { }

    public override bool CheckCondition() 
    {
        //if (_target != null) Debug.Log("target is :" + _target.name);
        if (_target == null)
        {
            Debug.Log("There is no target");
            return false;
        }
        else
        {
            //Debug.Log("Target is near");
            return _distance <= _maxDistance && _distance > attackRange;
        }

    }
}

public class TargetIsInRange : TargetHandler
{
    public TargetIsInRange(Transform origin, GameObject target, float aggroDistance) : base(origin, target, aggroDistance) { }

    public override bool CheckCondition()
    {
        if (_target == null) 
        {
            //Debug.Log("There is no target");
            return false;
        } else
        {
            //Debug.Log("Target is in range");
            return _distance <= attackRange;
        }


    } 

}