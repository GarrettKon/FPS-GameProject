using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float lifeTime;

    Rigidbody rigBod;

    void Start()
    {
        rigBod = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (rigBod.linearVelocity.magnitude > 0.1f)
            transform.forward = rigBod.linearVelocity;
    }

    void OnCollisionEnter(Collision other)
    {
        IDamage dmg = other.collider.GetComponent<IDamage>();
        if (dmg != null)
            dmg.takeDamage(damage);

        rigBod.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
