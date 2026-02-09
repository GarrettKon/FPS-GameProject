using UnityEngine;

public class foundKey : MonoBehaviour
{

    [SerializeField] GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetActive(false);
            gameObject.SetActive(false);
            //todo Pop up noting the door is now open
        }
    }
}
