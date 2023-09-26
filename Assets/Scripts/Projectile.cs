using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private TrailRenderer tr;
    private MeshRenderer mr;

    [SerializeField]
    private float damage;

    public static AudioClip cannonShot;
    static AudioSource source;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        mr = GetComponent<MeshRenderer>();
        cannonShot = Resources.Load<AudioClip>("CannonShot");
        source = GetComponent<AudioSource>();
        source.Play();

    }

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    private void Update()
    {
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void Hit()
    {
        tr.emitting = false;
        tr.autodestruct = true;
        mr.forceRenderingOff = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ship shipCollision = collision.gameObject.GetComponent<Ship>();
        if(shipCollision != null)
        {
            shipCollision.Damage(damage);
        }
        Hit();
    }
}
