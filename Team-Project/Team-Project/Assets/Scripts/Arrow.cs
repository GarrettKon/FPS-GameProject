using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float lifeTime;

    Rigidbody rb;

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

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
