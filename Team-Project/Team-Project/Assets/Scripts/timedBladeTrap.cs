using UnityEngine;

public class timedBladeTrap : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float activeTime;
    [SerializeField] float pauseTime;

    float timer;
    bool isActive;

    void Update()
    {
        timer += Time.deltaTime;

        if (isActive)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            if (timer >= activeTime)
            {
                isActive = false;
                timer = 0;
            }
        }
        else
        {
            if (timer >= pauseTime)
            {
                if (Random.value > 0.5f)
                    rotateSpeed *= -1f;

                isActive = true;
                timer = 0;
            }
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
