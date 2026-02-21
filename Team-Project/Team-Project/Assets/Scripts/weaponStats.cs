using UnityEngine;

public class weaponStats : ScriptableObject
{
    public enum WeaponType
    {
        Bow,
        Melee
    }

    public WeaponType weaponType;
    public int damage;
    public float shootForce;
    public GameObject weaponModel;

    [Range(1, 10)] public int shootDamage;
    [Range(15, 1000)] public int shootDist;
    [Range(0.1f, 2f)] public float shootRate;
    [Range(5, 50)] public int arrowMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
