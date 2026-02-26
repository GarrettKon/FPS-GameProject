using UnityEngine;

public class damagePickup : MonoBehaviour
{
    [SerializeField] damageUpgrade dmgItem;


    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null)
        {
            pickup.getDamageUpgrade(dmgItem);
            Destroy(gameObject);
        }
    }



}