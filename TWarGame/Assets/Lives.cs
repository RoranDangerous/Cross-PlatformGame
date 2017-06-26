using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour {

	private int lives = 3;

	public bool DecreaseLives()
	{
		print (lives);
		lives -= 1;
		return lives <= 0;
	}
}
