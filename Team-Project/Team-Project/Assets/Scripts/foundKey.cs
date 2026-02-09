using UnityEngine;

public class foundKey : MonoBehaviour
{

    [SerializeField] GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.keyFound = true;
            gameManager.instance.updateKeyFound();
            door.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
