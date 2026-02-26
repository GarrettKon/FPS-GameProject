using UnityEngine;

[CreateAssetMenu]

public class speedUpgrade : ScriptableObject
{
    public GameObject speedItem;

    [Range(1, 4)] public int speedMult;
}
