using UnityEngine;

public class weaponPickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pik = other.GetComponent<IPickup>();

        if (pik != null)
        {
            pik.getWeaponStats(weapon);
            Destroy(gameObject);
        }
    }
}
