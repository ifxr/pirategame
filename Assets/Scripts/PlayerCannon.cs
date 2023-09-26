using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : Cannon
{
    protected LineRenderer lrCharge, lrMax;

    [HideInInspector]
    public bool isCharging = false;
    
    protected override void Awake()
    {
        base.Awake();
        lrCharge = GetComponent<LineRenderer>();
        lrCharge.positionCount = 0;
        lrMax = firePoint.GetComponent<LineRenderer>();
        lrMax.positionCount = 0;

    }
        
    public virtual IEnumerator Charge()
    {
        if (!isFiring)
        {
            isCharging = true;
            numShotsLoaded = 1;
            while (isCharging)
            {
                if (Mathf.Abs(currentProjVelocity - maxProjVelocity) < 0.01 && numShotsLoaded < multishot)
                {
                    currentProjVelocity = minProjVelocity;
                    numShotsLoaded++;
                }
                currentProjVelocity = Mathf.Clamp(currentProjVelocity + chargeRate * maxProjVelocity * Time.deltaTime, minProjVelocity, maxProjVelocity);
                VisualizeTrajectory(currentProjVelocity, lrCharge);
                if(numShotsLoaded > 1) { VisualizeTrajectory(maxProjVelocity, lrMax); }
                yield return null;
            }
        }
    }

    protected bool VisualizeTrajectory(float velocity, LineRenderer lr)
    {
        int maxsteps = 100;
        int numsteps = maxsteps;
        Vector3[] positions = TrajectoryPrediction.Plot(cannonball_rb, firePoint.position, transform.up * velocity, maxsteps);

        bool hitEnemy = false;
        for(int i = 0; i < positions.Length - 1; i++)
        {
            Physics.Linecast(positions[i], positions[i + 1], out RaycastHit checkForEnemies, ship.GetOpponentLayerMask(), QueryTriggerInteraction.Ignore);
            Physics.Linecast(positions[i], positions[i + 1], out RaycastHit checkForHit, ~0 ,QueryTriggerInteraction.Collide);
            if (checkForEnemies.collider != null)
            {
                Ship hitShip = null;
                if (checkForEnemies.collider.attachedRigidbody != null)
                {
                    hitShip = checkForEnemies.collider.attachedRigidbody.transform.GetComponent<Ship>();
                }
                if(hitShip != null)
                {
                    hitEnemy = true;
                }
            }
            if(checkForHit.collider != null)
            {
                numsteps = i + 1;
                positions[i] = checkForHit.point;
                break;
            }
        }
        if (hitEnemy)
        {
            lr.startColor = Color.red;
            lr.endColor = Color.red;
        }
        else
        {
            lr.startColor = Color.white;
            lr.endColor = Color.white;
        }
        lr.positionCount = numsteps;
        lr.SetPositions(positions);
        return hitEnemy;
    }

    private void DeleteTrajectory()
    {
        lrCharge.positionCount = 0;
        lrMax.positionCount = 0;
    }

    public override void Fire()
    {
        isCharging = false;
        DeleteTrajectory();
        base.Fire();
    }

    protected override IEnumerator FireCoroutine()
    {
        isFiring = true;
        while (numShotsLoaded > 1)
        {
            Projectile newCannonball = Instantiate(cannonball, firePoint.position, Quaternion.identity);
            newCannonball.SetVelocity(transform.up * maxProjVelocity);
            numShotsLoaded--;
            yield return new WaitForSeconds(burstDelay);
        }
        Projectile finalCannonball = Instantiate(cannonball, firePoint.position, Quaternion.identity);
        finalCannonball.SetVelocity(transform.up * currentProjVelocity);
        numShotsLoaded--;

        //reset after firing
        currentProjVelocity = minProjVelocity;
        isFiring = false;
    }
}
