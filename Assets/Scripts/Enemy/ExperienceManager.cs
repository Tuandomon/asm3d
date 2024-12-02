using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    private PlayerExperience playerExperience;

    private void Start()
    {
        // Lấy tham chiếu tới PlayerExperience trên cùng một GameObject
        playerExperience = GetComponent<PlayerExperience>();
    }

    public void AddExperience(int amount)
    {
        if (playerExperience != null)
        {
            playerExperience.AddExperience(amount);
        }
    }
}

