using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Heath : NetworkBehaviour {

    [SyncVar]
    public int currentHealth = 10000;
    public int maxHealth = 10000;
	
	void Update () {
        RegenerateHealth(3);
	}

    public void RegenerateHealth(int num)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += num;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }
}
