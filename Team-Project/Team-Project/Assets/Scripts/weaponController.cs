using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponController : MonoBehaviour
{
    public Transform weaponModel;

    List<weaponStats> weaponList = new List<weaponStats>();
    int weaponListPos = 0;

    GameObject currentWeaponObject;
    Transform firePoint;

    void Update()
    {
        selectWeapon();
    }

    public void Attack()
    {
        if (weaponList.Count == 0) return;

        weaponStats current = weaponList[weaponListPos];

        if (current.weapon == weaponStats.WeaponType.WoodenBow)
            fireArrow(current);
        else
            meleeAttack(current);
    }

    void fireArrow(weaponStats current)
    {
        if (firePoint == null || current.arrow == null)
            return;

        GameObject arrow =
            Instantiate(current.arrow, firePoint.position, firePoint.rotation);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        if (rb != null)
            rb.AddForce(firePoint.forward * current.projectileForce, ForceMode.Impulse);
    }

    void meleeAttack(weaponStats current)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, current.attackDistance))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
                dmg.takeDamage(current.damage);

            PlayWeaponEffect(current);
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
        if (current.shootSound.Length > 0)
        {
            AudioSource.PlayClipAtPoint(current.shootSound[Random.Range(0, current.shootSound.Length)], transform.position, current.shootSoundVol);
        }
    }

    void PlayWeaponEffect(weaponStats current)
    {
        if (current.hitEffect != null)
        {
            Instantiate(current.hitEffect, transform.position, Quaternion.identity);
        }
    }
}
