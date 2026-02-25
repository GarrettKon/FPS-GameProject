using UnityEngine;

public interface IPickup
{
    public void getWeaponStats(weaponStats weapon);

    public void getSpeedUpgrade(speedUpgrade speedItem);

    public void getJumpUpgrade(jumpUpgrade jumpItem);

    public void getDamageUpgrade(damageUpgrade dmgItem);

}
