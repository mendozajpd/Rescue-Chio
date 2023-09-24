using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistanceToTarget : Transition
{
    private Transform _origin;
    private UnitManager _target;

    protected const float maxDistance = 8f;
    protected float attackRange = 3f;

    protected float _distance => Vector2.Distance(_origin.position, _target.transform.position);

    protected DistanceToTarget(Transform origin, UnitManager target) 
    {
        _origin = origin;
        _target = target;
    }

}

public class TargetIsFar : DistanceToTarget
{
    public TargetIsFar(Transform origin, UnitManager target) : base(origin, target) { }

    public override bool CheckCondition() => _distance > maxDistance;
}

public class TargetIsNear : DistanceToTarget
{
    public TargetIsNear(Transform origin, UnitManager target) : base(origin, target) { }

    public override bool CheckCondition() => _distance <= maxDistance && _distance > attackRange;
}

public class TargetIsInRange : DistanceToTarget
{
    public TargetIsInRange(Transform origin, UnitManager target) : base(origin, target) { }

    public override bool CheckCondition() => _distance <= attackRange;
}