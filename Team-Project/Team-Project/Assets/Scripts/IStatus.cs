using UnityEngine;

public interface IStatus 
{
    void applyStatus(playerController.statusType status, int damageAmount, float damageRate);

    
}
