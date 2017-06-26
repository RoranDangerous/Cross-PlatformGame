using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {
	public GameObject spawn;
	public GameObject spawnPoint;
	// Use this for initialization
	void Start () {
		spawnPoint = spawn.transform.GetChild(Random.Range (0, spawn.transform.childCount)).gameObject;
	}
}
