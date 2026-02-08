using UnityEngine;

public class playerControlller : MonoBehaviour
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
    [SerializeField] float crouchHeight;
    [SerializeField] float standHeight;

    int jumpCount;
    int HPOrig;
    int speedOrig;

    bool isCrouching;

    Vector3 moveDir;
    Vector3 playerVeloc;
    Vector3 playerCenterOrig;
    Vector3 cameraStartPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraStartPos = playerCamera.localPosition;
        standHeight = controller.height;
        playerCenterOrig = controller.center;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        crouch();
        crouchVisual();
    }

    void movement()
    {
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

    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVeloc.y = jumpSpeed;
            jumpCount++;
            isCrouching = false;
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
        if (!Input.GetButtonDown("Crouch"))
            return;

        bool wantToCrouch = !isCrouching;

        if (!wantToCrouch)
        {
            Vector3 rayStart = transform.position + Vector3.up * controller.height;
            float rayDistance = standHeight - controller.height;

            if (Physics.Raycast(rayStart, Vector3.up, rayDistance))
                return;
        }

        isCrouching = wantToCrouch;

        if (isCrouching)
        {
            speed /= crouchMod;
            controller.height = crouchHeight;
            controller.center = new Vector3(
                controller.center.x,
                crouchHeight / 2f,
                controller.center.z
            );
        }
        else
        {
            speed *= crouchMod;
            controller.height = standHeight;
            controller.center = playerCenterOrig;
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

}