using UnityEngine;

public class lavaTrap : MonoBehaviour
{
    [SerializeField] int burnDamage;
    [SerializeField] float burnRate;
    [SerializeField] AudioSource lavaLoopSound;

    private void OnTriggerEnter(Collider other)
    {
        playerController player = other.GetComponent<playerController>();

        if (player != null)
        {
            player.applyStatus(playerController.statusType.burned, burnDamage, burnRate);

            if (lavaLoopSound != null && !lavaLoopSound.isPlaying)
                lavaLoopSound.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        playerController player = other.GetComponent<playerController>();

        if (player != null)
        {

            player.applyStatus(playerController.statusType.burned, burnDamage, burnRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lavaLoopSound != null && lavaLoopSound.isPlaying)
            lavaLoopSound.Stop();
    }
}