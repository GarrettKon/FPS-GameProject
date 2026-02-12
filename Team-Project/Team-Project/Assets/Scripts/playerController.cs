using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
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

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    int jumpCount;
    int HPOrig;
    int speedOrig;

    float shootTimer;

    bool isCrouching;
    bool isStandingUp;

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
        speedOrig = speed;
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
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVeloc * Time.deltaTime);
        playerVeloc.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && shootTimer >= shootRate)
            shoot();
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
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
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

            speed = speedOrig / crouchMod;

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
            speed = speedOrig;
            isStandingUp = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        //TODO waiting on damage HP & flash material Matt
        updatePlayerUI();
        StartCoroutine(flashScreen());

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
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
}