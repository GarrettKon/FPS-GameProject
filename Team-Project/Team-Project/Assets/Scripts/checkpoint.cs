using System.Collections;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(showPop());
        }
    }

    IEnumerator showPop()
    {
        gameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameManager.instance.checkpointPopup.SetActive(false);

    }
}
