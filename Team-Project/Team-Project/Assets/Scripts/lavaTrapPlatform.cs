using UnityEngine;
using System.Collections;

public class lavaTrapPlatform : MonoBehaviour
{
    [SerializeField] GameObject floorTop;
    [SerializeField] AudioClip breakClip;
    [SerializeField] float disappearDelay = 0.5f;

    bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isTriggered)
            return;

        PlayBreakingSound();
        StartCoroutine(DisappearPlatform());
    }

    void PlayBreakingSound()
    {
        if (breakClip != null)
            AudioSource.PlayClipAtPoint(breakClip, transform.position);
    }

    IEnumerator DisappearPlatform()
    {
        isTriggered = true;

        yield return new WaitForSeconds(disappearDelay);

        floorTop.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        floorTop.SetActive(true);
        isTriggered = false;
    }
}
