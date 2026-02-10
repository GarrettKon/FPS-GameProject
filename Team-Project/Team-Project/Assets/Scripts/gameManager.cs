using TMPro;
using UnityEngine;
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
    public Image healthBar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public playerController playerScript;



    int enemyCountNumber;
    public bool isPaused;
    float timeScaleOrig;

    //will be set to true in another script when player enters collider for the goal Miguel
    public bool gameGoalReached;
    public bool keyFound;
    GameObject goalObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        gameGoalReached = false;
        keyFound = false;
        timeScaleOrig = Time.timeScale;

        updateKeyFound();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
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
        Cursor.lockState = CursorLockMode.None;
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

    public void updateEnemyCount(int amount)
    {
        enemyCountNumber -= amount;
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

    Coroutine keyFlashRoutine;

    IEnumerator FlashKeyFound()
    {
        keyFoundText.gameObject.SetActive(true);

        float timer = 0f;
        float duration = 1.5f;
        float speed = 6f;

        Color c = keyFoundText.color;

        while (timer < duration)
        {
            c.a = Mathf.Abs(Mathf.Sin(timer * speed));
            keyFoundText.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        keyFoundText.gameObject.SetActive(false);
        c.a = 1f;
        keyFoundText.color = c;

        keyFlashRoutine = null;
    }
}
