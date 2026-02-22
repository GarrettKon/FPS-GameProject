using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage, IStatus, IPickup
{
    [SerializeField] CharacterController controller;
    public enum statusType { none, poisoned, burned, shocked };
    public statusType status;
    bool isDamaging;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int baseSpeed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] int shockMod;

    [SerializeField] int crouchMod;
    [SerializeField] Transform playerCamera;
    [SerializeField] float crouchCameraOffset;
    [SerializeField] float crouchLerpSpeed;
    [SerializeField] float standLerpSpeed;
    [SerializeField] float crouchHeight;
    [SerializeField] float standHeight;

    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackUpForce;

    [SerializeField] float statusEndTime;


    int jumpCount;
    int HPOrig;
    float statusTimer;

    bool isCrouching;
    bool isStandingUp;
    bool isSprinting;

    Vector3 moveDir;
    Vector3 playerVeloc;
    Vector3 playerCenterOrig;
    Vector3 cameraStartPos;

    int statusAmount;
    float statusRate;

    public weaponController weaponController;

    void Start()
    {
        HPOrig = HP;
        cameraStartPos = playerCamera.localPosition;
        standHeight = controller.height;
        playerCenterOrig = controller.center;
        baseSpeed = speed;
        status = statusType.none;
    }

    void Update()
    {
        movement();
        sprint();
        crouch();
        crouchVisual();
        standUpLerp();
        endStatus();
        handleStatus();
    }

    void movement()
    {
        if (controller.isGrounded && playerVeloc.y < 0)
        {
            jumpCount = 0;
            playerVeloc.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        speed = baseSpeed;

        if (status == statusType.shocked)
            speed /= shockMod;

        if (isCrouching)
            speed /= crouchMod;

        if (isSprinting)
            speed *= sprintMod;

        playerVeloc.y -= gravity * Time.deltaTime;

        jump();

        Vector3 finalMove = move * speed + playerVeloc;

        controller.Move(finalMove * Time.deltaTime);
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
            controller.center = new Vector3(controller.center.x, crouchHeight / 2f, controller.center.z);
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();

        StartCoroutine(flashScreen());

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }
    public void takeDamageStatus(int amount)
    {
        HP -= amount;

        updatePlayerUI();
        StartCoroutine(flashScreen());
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

    public void updatePlayerUI()
    {
        gameManager.instance.healthBar.fillAmount = (float)HP / HPOrig;
    }

    public void applyStatus(statusType stat, int damageAmount, float damageRate)
    {
        if (status == stat || status == statusType.none)
        {
            statusTimer = 0;
            status = stat;
            gameManager.instance.statusFlash(status);
            statusAmount = damageAmount;
            statusRate = damageRate;
        }
    }

    void endStatus()
    {
        if (statusTimer >= statusEndTime)
        {
            status = statusType.none;
            gameManager.instance.burnStatusScreen.SetActive(false);
            gameManager.instance.poisonStatusScreen.SetActive(false);
            gameManager.instance.shockStatusScreen.SetActive(false);
            statusTimer = 0;
        }
    }

    void handleStatus()
    {
        if (status == statusType.none)
            return;

        statusTimer += Time.deltaTime;

        if (!isDamaging && status != statusType.shocked)
        {
            StartCoroutine(statusDamage(statusAmount, statusRate));
        }

        endStatus();
    }

    IEnumerator statusDamage(int statusDamageAmount, float statusDamageRate)
    {
        isDamaging = true;
        takeDamageStatus(statusDamageAmount);
        yield return new WaitForSeconds(statusDamageRate);
        isDamaging = false;
    }

    public void getWeaponStats(weaponStats weapon)
    {
        if (weaponController != null)
            weaponController.addWeapon(weapon);
    }
}