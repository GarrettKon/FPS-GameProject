using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float lifeTime;

    int damage;
    ParticleSystem hitEffect;
    Rigidbody rb;



    public void SetDamage(int amount)
    {
        damage = amount;
    }

    public void SetHitEffect(ParticleSystem effect)
    {
        hitEffect = effect;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
            transform.forward = rb.linearVelocity;
    }

    void OnCollisionEnter(Collision other)
    {
        IDamage dmg = other.collider.GetComponent<IDamage>();

        if (dmg != null)
            dmg.takeDamage(damage);

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
