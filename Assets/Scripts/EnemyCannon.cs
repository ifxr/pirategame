using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : Cannon
{
    [SerializeField]
    protected float reloadTime;
    protected float timeLastFire;

    protected override void Awake()
    {
        base.Awake();
        timeLastFire = -reloadTime;
    }

    public void LockTarget()
    {
        if (Time.time - timeLastFire < reloadTime)
        {
            return;
        }
        float bigSpeed = maxProjVelocity;
        float littleSpeed = minProjVelocity;
        float velocityStep = maxProjVelocity * 0.01f;
        bool bigLocked = false;
        bool littleLocked = false;
        while (bigSpeed > littleSpeed && ( !bigLocked || !littleLocked))
        {
            bigLocked = TrajectoryLockValid(bigSpeed);
            if (!bigLocked)
            {
                bigSpeed -= velocityStep;
            }
            littleLocked = TrajectoryLockValid(littleSpeed);
            if (!littleLocked)
            {
                littleSpeed += velocityStep;
            }
        }
        currentProjVelocity = (bigSpeed + littleSpeed) / 2;
        numShotsLoaded = multishot;
        if (bigLocked || littleLocked)
        {
            Fire();
        }
    }

    protected bool TrajectoryLockValid(float projVelocity) //just VisualizeTrajectory() without the line renderer and with an argument for initial velocity
    {
        int numsteps = 30;
        Vector3[] positions = TrajectoryPrediction.Plot(cannonball_rb, firePoint.position, transform.up * projVelocity, numsteps);

        bool hitEnemy = false;
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Physics.Linecast(positions[i], positions[i + 1], out RaycastHit newHit, ship.GetOpponentLayerMask(), QueryTriggerInteraction.Ignore);
            //Debug.DrawLine(positions[i], positions[i + 1], Color.red);
            if (newHit.collider != null)
            {
                Ship hitShip = newHit.collider.attachedRigidbody.transform.GetComponent<Ship>();
                if (hitShip != null)
                {
                    hitEnemy = true;
                    break;
                }
            }
        }
        return hitEnemy;
    }

    public override void Fire()
    {
        base.Fire();
        timeLastFire = Time.time;
    }

    protected override IEnumerator FireCoroutine()
    {
        isFiring = true;
        while (numShotsLoaded > 0)
        {
            Projectile newCannonball = Instantiate(cannonball, firePoint.position, Quaternion.identity);
            newCannonball.SetVelocity(transform.up * currentProjVelocity);
            numShotsLoaded--;
            yield return new WaitForSeconds(burstDelay);
        }
        //reset after firing
        currentProjVelocity = minProjVelocity;
        isFiring = false;
    }
}
