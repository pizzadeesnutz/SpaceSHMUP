using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public static Main S;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemySpawnPadding = 1.5f;
	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] 
	{ WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };
	public bool ________________;
	public float enemySpawnRate;
	public WeaponType[] activeWeaponTypes;

	void Awake(){
		S = this;
		Utils.SetCameraBounds (this.GetComponent<Camera>());
		enemySpawnRate = 1f / enemySpawnPerSecond;
		Invoke ("SpawnEnemy", enemySpawnRate);
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			W_DEFS [def.type] = def;
		}//end of foreach
	}//end of Awake()

	static public WeaponDefinition GetWeaponDefention(WeaponType wt){
		if(W_DEFS.ContainsKey(wt)){
			return(W_DEFS[wt]);
		}//end of if
			return(new WeaponDefinition());
	}//GetWeaponDefention(WeaponType wt)

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
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i = 0; i < weaponDefinitions.Length; i++) {
			activeWeaponTypes [i] = weaponDefinitions [i].type;
		}//end of for loop
	}//end of Start()

	void Update () {
	
	}//end of Update()

	public void DelayedRestart(float delay){
		Invoke ("Restart", delay);
	}//end of DelayedRestart(float delay)

	public void Restart(){
		SceneManager.LoadScene ("_Scene_0");
	}//end of Restart()

	public void ShipDestroyed(Enemy e){
		if (Random.value <= e.powerUpDropChance) {
			int ndx = Random.Range (0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency [ndx];
			GameObject go = Instantiate (prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();
			pu.SetType (puType);
			pu.transform.position = e.transform.position;
		}//end of if
	}//end of ShipDestroyed(Enemy e)
}