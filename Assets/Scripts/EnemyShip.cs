using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : NPCShip
{
    public float aggroRadius, targetRadius, trailRadius, fireHalfAngle;
    protected State currentState;
    protected EnemyCannon[] cannons;

    public static AudioClip cannonShot;
    static AudioSource source;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        cannons = GetComponentsInChildren<EnemyCannon>(true);
        currentState = new PatrolState(this);
    }

    protected override void Update()
    {
        currentState = currentState.StateUpdate();
        base.Update();
    }

    public virtual void SetStopDistance(float distance)
    {
        nav.stoppingDistance = distance;
    }

    public void PersueTarget(float targetDistance)
    {
        if (target != null)
        {
            nav.SetDestination(target.position + (transform.position - target.position).normalized * targetDistance);
        }
    }

    public override void Dock()
    {
        currentState.StateDock();
    }

    protected override void Sink()
    {
        base.Sink();
        currentState = new SunkState(this);
    }

    public virtual void Attack()
    {
        foreach(EnemyCannon c in cannons)
        {
            //if (!c.isCharging)
            //{
            //    StartCoroutine(c.Charge());
            //}
            c.LockTarget();
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.EnemyGone(this);
    }

    protected abstract class State
    {
        protected EnemyShip ship;

        public State(EnemyShip _ship)
        {
            ship = _ship;
        }
        
        public virtual void StateDock()
        {

        }
        public bool HasLOS(Transform _target, float radius)
        {
            if ((_target.position - ship.transform.position).sqrMagnitude < Mathf.Pow(radius, 2))
            {
                Physics.Linecast(ship.transform.position, _target.position, out RaycastHit hitInfo, 1 << 8, QueryTriggerInteraction.Ignore);
                if (hitInfo.collider == null)
                {
                    return true;
                }
            }
            return false;
        }
        public abstract State StateUpdate();
    }
    protected class PatrolState : State
    {
        private List<Port> patrolPorts = new List<Port>();
        public PatrolState(EnemyShip _ship) : base(_ship)
        {
            List<Port> portList = new List<Port>(GameManager.Instance.ports);
            portList.RemoveAll(port => (port.transform.position - ship.transform.position).sqrMagnitude < 0.01); //if the ship is already at a port, remove it from the possible ports
            for (int i = 0; i < Random.Range(1, portList.Count); i++) //add a random number of ports to patrol
            {
                Port newPort = portList[Random.Range(0, portList.Count)];
                patrolPorts.Add(newPort);
                portList.Remove(newPort);
            }

            ship.SetStopDistance(0);
        }
        public override State StateUpdate()
        {
            ship.SetTarget(patrolPorts[0].transform);
            ship.PersueTarget();

            Transform player = GameManager.Instance.player.transform;
            if(HasLOS(player, ship.aggroRadius))
            {
                return new AggroState(ship, player);
            }
            else
            {
                return this;
            }
        }

        public override void StateDock()
        {
            base.StateDock();
            if(patrolPorts.Count > 1)
            {
                patrolPorts.RemoveAt(0);
            }
            else
            {
                Destroy(ship.gameObject);
            }
        }
    }

    protected class AggroState : State
    {
        Transform target;
        public AggroState(EnemyShip _ship, Transform _target) : base(_ship)
        {
            target = _target;
            ship.SetTarget(target);
            
        }
        public override State StateUpdate()
        {
            if (HasLOS(target, ship.trailRadius))
            {
                ship.PersueTarget(ship.targetRadius);
                ship.rb.AddTorque(Vector3.up * Vector3.SignedAngle(ship.transform.forward, target.transform.position - ship.transform.position, Vector3.up) / 180 * (ship.turnSpeed - Mathf.Abs(Vector3.Dot(ship.rb.angularVelocity, Vector3.up))) * 1f);
                
                if (Vector3.Angle(ship.transform.forward, target.transform.position - ship.transform.position) < ship.fireHalfAngle)
                {
                    ship.Attack();
                }    
                return this;
            }
            else
            {
                return new PatrolState(ship);
            }
        }
    }

    protected class SunkState : State
    {

        public SunkState(EnemyShip _ship) : base(_ship)
        {
            
        }

        public override State StateUpdate()
        {
            return this;
        }
    }
}
