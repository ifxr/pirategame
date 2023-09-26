using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeableStat
{
    public int lvl { get; private set; }
    public int tmplvl { get; private set; }

    protected PlayerShip ship;
    public UpgradeableStat(PlayerShip _ship)
    {
        lvl = 0;
        tmplvl = lvl;
        ship = _ship;
        SetShipStat();
    }
    public abstract int CostFunction(int level);
    public virtual void ConfirmLevelUp()
    {
        lvl = tmplvl;
    }

    protected abstract void SetShipStat();

    public void CancelLevelUp(out int totalRefund)
    {
        totalRefund = 0;
        for (int i = tmplvl; i > lvl; i--)
        {
            totalRefund += CostFunction(i);
        }
        tmplvl = lvl;
    }

    public void TempLevelUp()
    {
        tmplvl++;
    }
    public void TempLevelDown()
    {
        tmplvl--;
    }
}

public class SpeedStat : UpgradeableStat
{
    public SpeedStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetMaxSpeed(MovementSpeedFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }
    public override int CostFunction(int level)
    {
        return 250 * level;
    }
    protected float MovementSpeedFunction(int level)
    {
        return 2.5f + 0.5f * level;
    }
}

public class TurnSpeedStat : UpgradeableStat
{
    public TurnSpeedStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetTurnSpeed(TurnSpeedFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected float TurnSpeedFunction(int level)
    {
        return 0.75f + 0.25f * level;
    }
}

public class HealthStat : UpgradeableStat
{
    public HealthStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetMaxHealth(MaxHealthFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected float MaxHealthFunction(int level)
    {
        return 5f + 3f * level;
    }
}

public class MultishotStat : UpgradeableStat
{
    public MultishotStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetMultishot(MultishotFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected int MultishotFunction(int level)
    {
        return 1 + 1 * level;
    }
}

public class ChargeRateStat : UpgradeableStat
{
    public ChargeRateStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetChargeRate(ChargeRateFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected float ChargeRateFunction(int level)
    {
        return 0.375f + 0.2f * level;
    }
}

public class BoostStrengthStat : UpgradeableStat
{
    public BoostStrengthStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetBoostStrength(BoostStrengthFunction(lvl), BoostCooldownFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected float BoostStrengthFunction(int level)
    {
        return 2.5f + 0.3f * (level);
    }

    protected float BoostCooldownFunction(int level)
    {
        return 5 * (Mathf.Pow(0.9f, level));
    }
}

public class RammingArmorStat : UpgradeableStat
{
    public RammingArmorStat(PlayerShip _ship) : base(_ship)
    {

    }

    protected override void SetShipStat()
    {
        ship.SetRammingArmor(RammingArmorFunction(lvl));
    }

    public override void ConfirmLevelUp()
    {
        base.ConfirmLevelUp();
        SetShipStat();
    }

    public override int CostFunction(int level)
    {
        return 250 * level;
    }

    protected float RammingArmorFunction(int level)
    {
        return 0.15f * Mathf.Pow(1.15f, level);
    }
}