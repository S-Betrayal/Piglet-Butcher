using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    public PlayerController playerController;
    public void Attack()
    {
        playerController.Attack();
    }
}
