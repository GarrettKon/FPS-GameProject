using UnityEngine;

[CreateAssetMenu]
public class damageUpgrade : ScriptableObject
{
    public GameObject dmgItem;

    [Range(2, 5)] public int damageMult;
}
