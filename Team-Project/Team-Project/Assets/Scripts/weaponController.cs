using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponController : MonoBehaviour
{

    [SerializeField] float swingAngle;
    [SerializeField] float swingSpeed;

    public Transform weaponModel;

    List<weaponStats> weaponList = new List<weaponStats>();
    int weaponListPos = 0;

    GameObject currentWeaponObject;
    Transform firePoint;

    bool isSwinging;

    void Update()
    {
        selectWeapon();
    }

    public void Attack()
    {
        if (weaponList.Count == 0) return;

        weaponStats current = weaponList[weaponListPos];

        PlayWeaponSound(current);

        if (current.weapon == weaponStats.WeaponType.WoodenBow)
            fireArrow(current);
        else
            if (!isSwinging)
                StartCoroutine(SwingWeapon(current));
    }

    void fireArrow(weaponStats current)
    {
        if (firePoint == null || current.arrow == null)
            return;

        GameObject arrowObj =
            Instantiate(current.arrow, firePoint.position, firePoint.rotation);

        Rigidbody rb = arrowObj.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = firePoint.forward * current.projectileForce;

        Arrow arrowScript = arrowObj.GetComponent<Arrow>();

        if (arrowScript != null)
        {
            arrowScript.SetDamage(current.damage);
            arrowScript.SetHitEffect(current.hitEffect);
        }
    }

    void meleeAttack(weaponStats current)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, current.attackDistance))
        {
            IDamage dmg = hit.collider.GetComponentInParent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(current.damage);

                if (current.hitEffect != null)
                    Instantiate(current.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void changeWeapon(weaponStats current)
    {
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponObject = Instantiate(current.weaponModel, weaponModel);

        if (current.weapon == weaponStats.WeaponType.WoodenBow)
            firePoint = currentWeaponObject.transform.Find("Fire Point");
        else
            firePoint = null;

        currentWeaponObject.SetActive(true);
    }

    public void addWeapon(weaponStats weapon)
    {
        weaponList.Add(weapon);

        if (weaponList.Count == 1)
            changeWeapon(weaponList[0]);
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponListPos < weaponList.Count - 1)
        {
            weaponListPos++;
            changeWeapon(weaponList[weaponListPos]);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponListPos > 0)
        {
            weaponListPos--;
            changeWeapon(weaponList[weaponListPos]);
        }
    }

    void PlayWeaponSound(weaponStats current)
{
    if (current.shootSound != null && current.shootSound.Length > 0)
    {
        AudioSource.PlayClipAtPoint(
            current.shootSound[Random.Range(0, current.shootSound.Length)],
            weaponModel.position,
            current.shootSoundVol
        );
    }
}

    public void PlayWeaponEffect(weaponStats current)
    {
        if (current.hitEffect != null)
        {
            Instantiate(current.hitEffect, transform.position, Quaternion.identity);
        }
    }

    IEnumerator SwingWeapon(weaponStats current)
    {
        isSwinging = true;

        Quaternion startRot = currentWeaponObject.transform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, -swingAngle, 0);

        float swing = 0;

        while (swing < 1)
        {
            swing += Time.deltaTime * swingSpeed;
            currentWeaponObject.transform.localRotation =
                Quaternion.Lerp(startRot, endRot, swing);
            yield return null;
        }

        swing = 0;

        while (swing < 1)
        {
            swing += Time.deltaTime * swingSpeed;
            currentWeaponObject.transform.localRotation =
                Quaternion.Lerp(endRot, startRot, swing);
            yield return null;
        }

        currentWeaponObject.transform.localRotation = startRot;

        meleeAttack(current);

        isSwinging = false;
    }
}
