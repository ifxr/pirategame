using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Ship : MonoBehaviour
{
    protected Rigidbody rb;
    protected Animator anim;

    [SerializeField]
    protected float maxHealth, maxSpeed, throttleRateOfChange, turnSpeed, rammingArmor;
    protected float currentHealth;

    protected float throttle { get; private set; }

    public static AudioClip enemyHit, death;
    static AudioSource source;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        throttle = 0;
    }

    protected virtual void FixedUpdate()
    {
        if (rb != null)
        {
            float verticalRestoringForce = 100;
            float restoringTorque = 5f;
            //vertical restoring force (to keep ship on water)
            rb.AddForce(Vector3.up * verticalRestoringForce * Mathf.Clamp(0 - transform.position.y, -1, 1), ForceMode.Acceleration);
            //angular restoring force (to keep ship upright)
            rb.AddRelativeTorque(restoringTorque * new Vector3(Vector3.Dot(transform.forward, Vector3.up), 0, -Vector3.Dot(transform.right, Vector3.up)));
        }
    }

    public abstract LayerMask GetOpponentLayerMask();
    

    protected virtual void SetThrottle(float newThrottle)
    {
        throttle = Mathf.Clamp(newThrottle, 0f, 1);
    }

    public float GetCurrentHealthPercent()
    {
        return Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    public virtual void Damage(float damage)
    {
        if(damage < 0)
        {
            Debug.LogWarning("Input damage was negative. Damage should be positive!");
        }
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Sink();
        }
        if(currentHealth > 0 && damage >= 1)
        {
            enemyHit = Resources.Load<AudioClip>("EnemyHit");
            source = GetComponent<AudioSource>();
            source.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Time.timeSinceLevelLoad < 0.5f) //so ships don't damage each other in the initial spawn-in
        {
            return;
        }
        float damagePerVelocity = 0.75f;
        if(collision.rigidbody == null)
        {
            return;
        }
        if (collision.rigidbody.gameObject.GetComponent<Ship>() != null || collision.rigidbody.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            float relativeVelocity = (rb.velocity - collision.rigidbody.velocity).magnitude;

            Damage(relativeVelocity * (1 - rammingArmor) * damagePerVelocity);
        }
    }

    protected virtual void Sink()
    {
        death = Resources.Load<AudioClip>("Death");
        source = GetComponent<AudioSource>();
        source.Play();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Destroy(rb);
        anim.SetTrigger("Sink");
    }

    protected abstract void Die();
}
