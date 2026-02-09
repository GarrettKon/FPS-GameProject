using TMPro;
using UnityEngine;
using UnityEngine.UI;



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
        //TODO pause menu code Garrett
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
        //you win menu code Garrett

    }

    public void youLose()
    {
        //you lose menu code Garrett

    }

    public void updateEnemyCount(int amount)
    {
        enemyCountNumber -= amount;
        enemyCountText.text = enemyCountNumber.ToString("F0");
    }

    public void updateKeyFound()
    {
        keyFoundText.text = keyFound.ToString();
    }
}
