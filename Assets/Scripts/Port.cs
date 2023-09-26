using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Port : MonoBehaviour
{
    private SphereCollider zone;

    private void Awake()
    {
        zone = GetComponent<SphereCollider>();
        zone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            NPCShip npc = other.attachedRigidbody.gameObject.GetComponent<NPCShip>();
            if (npc != null && npc.target != null && npc.target.Equals(transform))
            {
                npc.Dock();
            }
        }
    }

    public bool isClearToSpawn()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, zone.radius);
        foreach(Collider c in colls)
        {
            if (c.attachedRigidbody != null) {
                if (c.attachedRigidbody.gameObject.GetComponent<Ship>())
                {
                    return false;
                }
            }
        }
        return true;
    }
}
