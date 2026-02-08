using UnityEngine;

public class playerControlller : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int crouchMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    int jumpCount;
    int HPOrig;
    int speedOrig;

    bool isCrouching;

    Vector3 moveDir;
    Vector3 playerVeloc;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        crouch();
    }

    void movement()
    {
        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVeloc = Vector3.zero;
        }

        moveDir =  Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
                
        controller.Move(playerVeloc * Time.deltaTime);
        playerVeloc.y -= gravity * Time.deltaTime;

    }

    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVeloc.y = jumpSpeed;
            jumpCount++;
        }
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                speed /= crouchMod;
            }
            else
            {
                speed *= crouchMod;
            }
        }
    }
}
