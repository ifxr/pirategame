using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShip : Ship
{
    private PlayerCannon starboardCannon, portCannon;
    private float turnInput;
    private bool boostInput;

    [SerializeField]
    protected float boostStrength, boostCooldown, repairPercentRate, damageRepairDelay, cannonPitch, minCannonPitch, maxCannonPitch, pitchRate;
    [HideInInspector]
    public int gold;

    private float timeLastDamage = 0;
    private float timeLastBoost = 0;

    public UpgradeableStat sails, rudder, hull, loader, gunpowder, boostStat, armor;

    private Transform rolledSails, unfurledSails;

    public bool isInHideout = false;

    public static AudioClip playerHit;
    static AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        starboardCannon = transform.Find("StarboardCannon").GetComponent<PlayerCannon>();
        portCannon = transform.Find("PortCannon").GetComponent<PlayerCannon>();

        gold = 0;
        timeLastBoost = -boostCooldown;
        gameObject.layer = LayerMask.NameToLayer("Player");

        sails = new SpeedStat(this);
        rudder = new TurnSpeedStat(this);
        hull = new HealthStat(this);
        loader = new MultishotStat(this);
        gunpowder = new ChargeRateStat(this);
        boostStat = new BoostStrengthStat(this);
        armor = new RammingArmorStat(this);

        UpdateCannonPitch(5);

        rolledSails = transform.Find("playerRolledSail");
        unfurledSails = transform.Find("playerUnfurledSail");

        SetThrottle(0);

        anim.enabled = false;
    }

    private void Start()
    {
        rolledSails.GetChild(0).GetComponent<Renderer>().material.color = GameManager.Instance.playerColor;
        unfurledSails.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = GameManager.Instance.playerColor;
        unfurledSails.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = GameManager.Instance.playerColor;

    }
    private void Update()
    {
        AdjustThrottle(Input.GetAxisRaw("Throttle"));
        turnInput = Input.GetAxis("Turning");

        if (Input.GetButtonDown("StarboardCannon"))
        {
            StartCoroutine(starboardCannon.Charge());
        }
        else if (Input.GetButtonUp("StarboardCannon"))
        {
            starboardCannon.Fire();
        }
        if (Input.GetButtonDown("PortCannon"))
        {
            StartCoroutine(portCannon.Charge());
        }
        else if (Input.GetButtonUp("PortCannon"))
        {
            portCannon.Fire();
        }

        boostInput = Input.GetButton("Boost");

        float pitchInput = Input.GetAxisRaw("PitchCannons");
        cannonPitch = Mathf.Clamp(cannonPitch + pitchInput * (maxCannonPitch * pitchRate) * Time.deltaTime, minCannonPitch, maxCannonPitch);
        UpdateCannonPitch(cannonPitch);
        if (Time.time - timeLastDamage > damageRepairDelay && rb.velocity.sqrMagnitude < 0.01 && rb.angularVelocity.sqrMagnitude < 0.01)
        {
            Heal(repairPercentRate * maxHealth * Time.deltaTime);
        }
    }

    private void UpdateCannonPitch(float pitch)
    {
        starboardCannon.transform.localRotation = Quaternion.Euler(Vector3.forward * (-90 + pitch));
        portCannon.transform.localRotation = Quaternion.Euler(Vector3.forward * (90 - pitch));
    }

    protected override void SetThrottle(float newThrottle)
    {
        base.SetThrottle(newThrottle);
        rolledSails.localScale = new Vector3(1, 1 - throttle, 1 - throttle);
        unfurledSails.localScale = new Vector3(1, throttle, throttle);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //old direct velocity change movement
        //rb.velocity = transform.forward * throttle * maxSpeed;
        //rb.angularVelocity = Vector3.up * turnInput * turnSpeed

        if (rb != null)
        {
            //force movement system
            rb.AddForce(transform.forward * ( Mathf.Clamp(throttle * maxSpeed - new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude, 0, maxSpeed)), ForceMode.VelocityChange);
            rb.AddTorque(Vector3.up * turnInput * (turnSpeed - Mathf.Abs(Vector3.Dot(rb.angularVelocity, Vector3.up))), ForceMode.VelocityChange);

            //boost
            if (boostInput && Time.time - timeLastBoost > boostCooldown)
            {
                rb.AddForce(transform.forward * (maxSpeed * boostStrength), ForceMode.VelocityChange);
                timeLastBoost = Time.time;
                boostInput = false;
            }
        }
    }

    public override void Damage(float damage)
    {
        playerHit = Resources.Load<AudioClip>("PlayerHit");
        source = GetComponent<AudioSource>();
        source.Play();
        base.Damage(damage);
        timeLastDamage = Time.time;
    }

    public float GetBoostChargePercent()
    {
        return Mathf.Clamp((Time.time - timeLastBoost) / boostCooldown, 0, 1);
    }

    public void SetMaxSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public void SetTurnSpeed(float newTurnSpeed)
    {
        turnSpeed = newTurnSpeed;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        float currentHealthPercent = currentHealth / maxHealth;
        maxHealth = newMaxHealth;
        currentHealth = currentHealthPercent * newMaxHealth;
    }

    public void SetChargeRate(float newChargeRate)
    {
        starboardCannon.chargeRate = newChargeRate;
        portCannon.chargeRate = newChargeRate;
    }

    public void SetMultishot(int newMultishot)
    {
        starboardCannon.multishot = newMultishot;
        portCannon.multishot = newMultishot;
    }

    public void SetBoostStrength(float newBoostStrength, float newBoostCooldown)
    {
        boostStrength = newBoostStrength;
        boostCooldown = newBoostCooldown;
    }

    public void SetRammingArmor(float newRammingArmor)
    {
        rammingArmor = newRammingArmor;
    }

    public float GetPickupGoldMultiplier()
    {
        return 1 + Mathf.Floor(gold/1000) * 0.1f;
    }

    protected void Heal(float amountToHeal)
    {
        if (amountToHeal < 0)
        {
            Debug.LogWarning("Input amountToHeal was negative. Amount should be positive!");
        }
        currentHealth = Mathf.Clamp(currentHealth + amountToHeal, 0, maxHealth);
    }

    public override LayerMask GetOpponentLayerMask()
    {
        return ~(1 << gameObject.layer);
    }

    private void AdjustThrottle(float throttleInputAxis)
    {
        SetThrottle(throttle + throttleInputAxis * throttleRateOfChange * Time.deltaTime);
    }

    protected override void Sink()
    {
        anim.enabled = true;
        base.Sink();
    }

    protected override void Die()
    {
        GameManager.Instance.Lose();
        gameObject.SetActive(false);
    }
}
