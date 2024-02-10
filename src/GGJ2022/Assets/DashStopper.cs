using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStopper : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    
    public void StopDashing()
    {
        if(PlayerMovement != null) PlayerMovement.StopDashing();
    }
}
