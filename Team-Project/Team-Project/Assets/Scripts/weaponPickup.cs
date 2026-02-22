using System.Collections.Generic;
using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    public List<weaponStats> weaponList;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pik = other.GetComponent<IPickup>();

        if (pik != null && !weaponList.Contains(weapon))
        {
            pik.getWeaponStats(weapon);
            Destroy(gameObject);
        }
    }
}