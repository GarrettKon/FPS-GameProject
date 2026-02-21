using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Stats")]
public class weaponStats : ScriptableObject
{
    public enum WeaponType
    {
        Bow,
        OneHandSword,
        TwoHandSword,
        OneHandAxe,
        TwoHandAxe
    }

    public WeaponType weaponType;

    [Header("Damage")]
    public int damage;
    public float attackRate;
    public float attackDistance;

    [Header("Bow Only")]
    public float projectileForce;

    [Header("Visual")]
    public GameObject weaponModel;
    public ParticleSystem hitEffect;

    [Header("Audio")]
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
