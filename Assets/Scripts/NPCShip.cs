using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NPCShip : Ship
{
    protected NavMeshAgent nav;
    [SerializeField]
    protected int gold;

    [SerializeField]
    private GoldPickup goldPickupPrefab;

    public Transform target { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        nav = GetComponent<NavMeshAgent>();
        nav.acceleration = throttleRateOfChange;
        nav.angularSpeed = turnSpeed * 180 / Mathf.PI;
        nav.speed = maxSpeed;
    }

    protected virtual void Update()
    {
        if (rb != null)
        {
            SetThrottle(rb.velocity.sqrMagnitude / Mathf.Pow(maxSpeed, 2)); //throttle exists here to know how to scale the sails
        }
    }

    public override LayerMask GetOpponentLayerMask()
    {
        return LayerMask.GetMask("Player");
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void PersueTarget()
    {
        if (target != null && nav.enabled)
        {
            nav.SetDestination(target.position);
        }
    }

    protected override void Sink()
    {
        base.Sink();
        nav.enabled = false;
    }

    protected override void Die()
    {
        if (gold > 0)
        {
            GoldPickup newGoldPickup = Instantiate(goldPickupPrefab, transform.position, Quaternion.Euler(new Vector3(0, transform.rotation.y, 0)));
            newGoldPickup.SetGold(gold);
        }
        Destroy(gameObject);
    }

    public abstract void Dock();
}
