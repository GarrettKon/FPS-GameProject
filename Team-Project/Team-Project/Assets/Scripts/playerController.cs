using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int baseSpeed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int crouchMod;
    [SerializeField] Transform playerCamera;
    [SerializeField] float crouchCameraOffset;
    [SerializeField] float crouchLerpSpeed;
    [SerializeField] float standLerpSpeed;
    [SerializeField] float crouchHeight;
    [SerializeField] float standHeight;

    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float shootForce;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    [SerializeField] float invulnDuration;
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackUpForce;

    int jumpCount;
    int HPOrig;
    int weaponListPos;
    int currentAmmo;
    float shootTimer;

    bool isCrouching;
    bool isStandingUp;
    bool isSprinting;

    bool isInvulnerable;

    Vector3 moveDir;
    Vector3 playerVeloc;
    Vector3 playerCenterOrig;
    Vector3 cameraStartPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        cameraStartPos = playerCamera.localPosition;
        standHeight = controller.height;
        playerCenterOrig = controller.center;
        baseSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        crouch();
        crouchVisual();
        standUpLerp();
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVeloc = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        speed = baseSpeed;

        if (isCrouching)
            speed /= crouchMod;

        if (isSprinting)
            speed *= sprintMod;

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVeloc * Time.deltaTime);
        playerVeloc.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && weaponList.Count > 0 && shootTimer >= shootRate)
        {
            attack();
        }

        selectWeapon();
        reload();
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVeloc.y = jumpSpeed;
            jumpCount++;
            isCrouching = false;
            isStandingUp = true;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
            isSprinting = true;

        if (Input.GetButtonUp("Sprint"))
            isSprinting = false;
    }

    void crouch()
    {
        if (isStandingUp)
            return;

        if (!Input.GetButtonDown("Crouch"))
            return;

        bool wantToCrouch = !isCrouching;

        if (wantToCrouch)
        {
            isCrouching = true;
            isStandingUp = false;

            controller.height = crouchHeight;
            controller.center = new Vector3(
                controller.center.x,
                crouchHeight / 2f,
                controller.center.z
            );
        }
        else
        {
            Vector3 rayStart = transform.position + Vector3.up * controller.height;
            float rayDistance = standHeight - controller.height;

            if (Physics.Raycast(rayStart, Vector3.up, rayDistance))
                return;

            isCrouching = false;
            isStandingUp = true;
        }
    }


    void crouchVisual()
    {
        Vector3 targetPos = cameraStartPos;

        if (isCrouching)
        {
            targetPos.y -= crouchCameraOffset;
        }

        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetPos,
            crouchLerpSpeed * Time.deltaTime
        );
    }

    void standUpLerp()
    {
        if (!isStandingUp)
            return;

        controller.height = Mathf.Lerp(
            controller.height,
            standHeight,
            standLerpSpeed * Time.deltaTime
        );

        controller.center = Vector3.Lerp(
            controller.center,
            playerCenterOrig,
            standLerpSpeed * Time.deltaTime
        );

        if (Mathf.Abs(controller.height - standHeight) < 0.01f)
        {
            controller.height = standHeight;
            controller.center = playerCenterOrig;
            isStandingUp = false;
        }
    }

    void attack()
    {
        shootTimer = 0;

        weaponStats current = weaponList[weaponListPos];

        if (current.weaponType == weaponStats.WeaponType.Bow)
        {
            FireArrow();
        }
        else
        {
            MeleeAttack();
        }
    }

    void FireArrow()
    {
        weaponStats current = weaponList[weaponListPos];

        if (currentAmmo <= 0)
            return;

        currentAmmo--;

        GameObject arrow = Instantiate(
            arrowPrefab,
            firePoint.position,
            Quaternion.LookRotation(Camera.main.transform.forward)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(Camera.main.transform.forward * current.shootForce, ForceMode.Impulse);
    }

    void reload()
    {
        if (Input.GetButtonDown("Reload") && weaponList.Count > 0)
        {
            weaponStats current = weaponList[weaponListPos];

            if (current.weaponType == weaponStats.WeaponType.Bow)
            {
                currentAmmo = current.arrowMax;
            }
        }
    }

    void MeleeAttack()
    {
        weaponStats current = weaponList[weaponListPos];

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out hit,
                            current.shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(current.damage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        if (isInvulnerable)
            return;

        HP -= amount;
        updatePlayerUI();

        StartCoroutine(flashScreen());
        StartCoroutine(Invulnerability());

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void knockback(Vector3 knockbackPos)
    {
        Vector3 knockDir = (transform.position - knockbackPos).normalized;

        knockDir.y = 0;

        playerVeloc += knockDir * knockbackForce;
        playerVeloc.y = knockbackUpForce;
    }

    IEnumerator flashScreen()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnDuration);
        isInvulnerable = false;
    }

    public void updatePlayerUI()
    {
        gameManager.instance.healthBar.fillAmount = (float)HP / HPOrig;
    }

    public void getWeaponStats(weaponStats weapon)
    {
        weaponList.Add(weapon);
        weaponListPos = weaponList.Count - 1;

        changeWeapon();
    }

    void changeWeapon()
    {
        weaponStats current = weaponList[weaponListPos];

        shootDamage = current.shootDamage;
        shootDist = current.shootDist;
        shootRate = current.shootRate;

        if (weaponList[weaponListPos].weaponType == weaponStats.WeaponType.Bow)
        {
            currentAmmo = weaponList[weaponListPos].arrowMax;
        }

        weaponModel.GetComponent<MeshFilter>().sharedMesh =
            current.weaponModel.GetComponent<MeshFilter>().sharedMesh;

        weaponModel.GetComponent<MeshRenderer>().sharedMaterial =
            current.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponListPos < weaponList.Count - 1)
        {
            weaponListPos++;
            changeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponListPos > 0)
        {
            weaponListPos--;
            changeWeapon();
        }
    }
}