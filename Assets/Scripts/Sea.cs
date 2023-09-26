using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Projectile proj = other.gameObject.GetComponent<Projectile>();
        if(proj != null)
        {
            proj.Hit();
        }
    }
}
