using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cannon : MonoBehaviour
{
    [SerializeField]
    public Projectile cannonball;

    public float minProjVelocity, maxProjVelocity, chargeRate, burstDelay;
    public int multishot;

    protected int numShotsLoaded;
    protected float currentProjVelocity;

    protected Transform firePoint;

    protected Rigidbody cannonball_rb;
    protected Ship ship;

    [HideInInspector]
    public bool isFiring = false;

    protected virtual void Awake()
    {
        firePoint = transform.GetChild(0);
        cannonball_rb = cannonball.GetComponent<Rigidbody>();
        currentProjVelocity = minProjVelocity;
        ship = transform.parent.GetComponent<Ship>();
        numShotsLoaded = 0;
    }

    public virtual void Fire()
    {
        StartCoroutine(FireCoroutine());
    }

    protected abstract IEnumerator FireCoroutine();
}
