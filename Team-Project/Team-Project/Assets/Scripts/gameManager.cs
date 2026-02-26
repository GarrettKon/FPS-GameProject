using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text keyFoundText;
    [SerializeField] TMP_Text doorOpenText;
    public Image healthBar;
    public GameObject playerDamageFlash;
    public GameObject burnStatusScreen;
    public GameObject poisonStatusScreen;
    public GameObject shockStatusScreen;
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;



    int enemyCountNumber;
    public bool isPaused;
    float timeScaleOrig;

    //will be set to true in another script when player enters collider for the goal Miguel
    public bool gameGoalReached;
    public bool keyFound;
    GameObject goalObject;

    Coroutine keyFlashRoutine;
    Coroutine doorFlashRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        gameGoalReached = false;
        keyFound = false;
        updateKeyFound();

        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnPause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;

    }

    public void updateGameGoal()
    {
        if (gameGoalReached)
        {
            youWin();
        }
    }

    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);

    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);

    }

    public void statusFlash(playerController.statusType type)
    {
        if (type == playerController.statusType.burned)
        {
            burnStatusScreen.SetActive(true);
        }
        if (type == playerController.statusType.poisoned)
        {
            poisonStatusScreen.SetActive(true);
        }
        if (type == playerController.statusType.shocked)
        {
            shockStatusScreen.SetActive(true);
        }
    }

    public void updateEnemyCount(int amount)
    {
        enemyCountNumber += amount;
        enemyCountText.text = enemyCountNumber.ToString("F0");
    }

    public void updateKeyFound()
    {
        if (!keyFound)
            return;

        if (keyFlashRoutine != null)
            StopCoroutine(keyFlashRoutine);

        keyFlashRoutine = StartCoroutine(FlashKeyFound());
    }

    IEnumerator FlashKeyFound()
    {
        keyFoundText.gameObject.SetActive(true);

        Color c = keyFoundText.color;
        c.a = 1f;
        keyFoundText.color = c;

        yield return new WaitForSeconds(1.5f);

        keyFoundText.gameObject.SetActive(false);

        keyFlashRoutine = null;
    }

    public void flashDoorOpen()
    {
        if (!keyFound)
            return;

        if (doorFlashRoutine != null)
            StopCoroutine(doorFlashRoutine);

        doorFlashRoutine = StartCoroutine(FlashDoorOpen());
    }

    IEnumerator FlashDoorOpen()
    {
        doorOpenText.gameObject.SetActive(true);

        float timer = 0f;
        float duration = 1.5f;
        float speed = 6f;

        Color c = doorOpenText.color;

        while (timer < duration)
        {
            c.a = Mathf.Abs(Mathf.Sin(timer * speed));
            doorOpenText.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        doorOpenText.gameObject.SetActive(false);

        c.a = 1f;
        doorOpenText.color = c;

        doorFlashRoutine = null;
    }
}
