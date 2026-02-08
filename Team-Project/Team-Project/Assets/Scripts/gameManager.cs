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
    public Image healthBar;
    public GameObject playerDamageFlash;

    public GameObject player;
    //TODO playerScript Pending Miyu's script


    //TODO HP bar + Damage Flash Matt

    int enemyCountNumber;
    public bool isPaused;
    float timeScaleOrig;

    //will be set to true in another script when player enters collider for the goal Miguel
    public bool gameGoalReached;
    GameObject goalObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        gameGoalReached = false;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //TODO pause menu code Garrett
    }

    public void updateGameGoal(int amount)
    {
        enemyCountNumber += amount;
        enemyCountText.text = enemyCountNumber.ToString("F0");

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
}
