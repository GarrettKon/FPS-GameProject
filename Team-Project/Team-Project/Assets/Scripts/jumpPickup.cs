using UnityEngine;

public class jumpPickup : MonoBehaviour
{
    [SerializeField] jumpUpgrade jumpItem;


    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null)
        {
            pickup.getJumpUpgrade(jumpItem);
            Destroy(gameObject);
        }
    }



}
