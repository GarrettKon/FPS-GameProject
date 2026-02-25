using UnityEngine;
using System.Collections;


public class damage : MonoBehaviour 
{
    enum damageType { bullet, stationary, DOT}
    [SerializeField] damageType type;
    [SerializeField] playerController.statusType statusType;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int statusDamageAmount;
    [SerializeField] float statusDamageRate;

    [SerializeField] int speed;
    [SerializeField] float destroyTime;
    [SerializeField] ParticleSystem hitEffect;

    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damageType.bullet)
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        IStatus status = other.GetComponent<IStatus>();
        if (dmg != null && type != damageType.DOT)
        {
            dmg.takeDamage(damageAmount);
            if(status != null)
            {
                status.applyStatus(statusType,statusDamageAmount,statusDamageRate);
            }
        }
        if (type == damageType.bullet)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect,transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == damageType.DOT)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
    }

   
    
}
