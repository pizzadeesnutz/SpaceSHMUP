﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public static Main S;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemySpawnPadding = 1.5f;
	public bool ________________;
	public float enemySpawnRate;

	void Awake(){
		S = this;
		Utils.SetCameraBounds (this.GetComponent<Camera>());
		enemySpawnRate = 1f / enemySpawnPerSecond;
		Invoke ("SpawnEnemy", enemySpawnRate);
	}//end of Awake()

	public void SpawnEnemy(){
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x + enemySpawnPadding;
		float xMax = Utils.camBounds.max.x + enemySpawnPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;
		Invoke ("SpawnEnemy", enemySpawnRate);
	}//end of SpawnEnemy()

	void Start () {
	
	}//end of Start()

	void Update () {
	
	}//end of Update()
}
