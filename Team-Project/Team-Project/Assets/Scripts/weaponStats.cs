using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Stats")]
public class weaponStats : ScriptableObject
{
    public enum WeaponType
    {
        WoodenBow,
        OneHandSword,
        TwoHandSword,
        OneHandAxe,
        TwoHandAxe
    }

    public WeaponType weapon;
    public GameObject weaponModel;

    [Header("Damage")]
    public int damage;
    public float attackRate;
    public float attackDistance;

    [Header("Bow Only")]
    public GameObject arrow;
    public float projectileForce;

    [Header("Visual")]
    public ParticleSystem hitEffect;

    [Header("Audio")]
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
