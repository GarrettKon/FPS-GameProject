using UnityEngine;

public class trapDamage : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;
    [SerializeField] timedBladeTrap trapController;

    private void OnTriggerEnter(Collider other)
    {
        if (!trapController.IsActive())
            return;

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
                dmg.takeDamage(damageAmount);
        }
    }
}
