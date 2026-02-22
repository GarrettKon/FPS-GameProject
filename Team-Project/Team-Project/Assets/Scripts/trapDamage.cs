using UnityEngine;

public class trapDamage : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float standDamageInterval;
    [SerializeField] timedBladeTrap trapController;

    float standTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTrapActive())
            return;

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            playerController player = other.GetComponent<playerController>();

            if (dmg != null)
                dmg.takeDamage(damageAmount);

            if (player != null)
                player.knockback(transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsTrapActive())
            return;

        if (!other.CompareTag("Player"))
            return;

        standTimer += Time.deltaTime;

        if (standTimer >= standDamageInterval)
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
                dmg.takeDamage(damageAmount);

            standTimer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            standTimer = 0;
    }

    private bool IsTrapActive()
    {
        if (trapController == null)
            return true;

        return trapController.IsActive();
    }
}
