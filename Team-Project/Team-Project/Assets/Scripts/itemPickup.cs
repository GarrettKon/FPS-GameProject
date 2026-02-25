using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] speedUpgrade speedItem;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null)
        {
            pickup.getSpeedUpgrade(speedItem); 
            Destroy(gameObject);
        }
    }

   
}
