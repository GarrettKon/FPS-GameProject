using UnityEngine;

public class goalObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.gameGoalReached = true;
            gameManager.instance.updateGameGoal();
        }
    }
}
