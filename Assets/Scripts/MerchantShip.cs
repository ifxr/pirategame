using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantShip : NPCShip
{
    private void Start()
    {
        FindTargetPort();
    }

    protected override void Update()
    {
        PersueTarget();
        base.Update();
    }

    private void FindTargetPort()
    {
        Port targetPort = GameManager.Instance.ports[Random.Range(0, GameManager.Instance.ports.Count)];
        while((targetPort.transform.position - transform.position).sqrMagnitude < 0.01) //if target port is the same as the one it is already at, find another one
        {
            targetPort = GameManager.Instance.ports[Random.Range(0, GameManager.Instance.ports.Count)];
        }
        SetTarget(targetPort.transform);
    }

    public override void Dock()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.Instance.MerchantGone(this);
    }
}
