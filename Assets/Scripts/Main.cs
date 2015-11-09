﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static public Main S;
	public static Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemySpawnPadding = 1.5f;
	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] {WeaponType.blaster,WeaponType.blaster,WeaponType.spread, WeaponType.shield};
	public bool _____________;
	public WeaponType[] activeWeaponTypes;
	public float enemySpawnRate;

	// Use this for initialization

	void Awake(){
		S = this;
		Util.SetCameraBounds (this.camera);
		enemySpawnRate = 1f / enemySpawnPerSecond;
		Invoke ("SpawnEnemy", enemySpawnRate);

		W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions){
			W_DEFS[def.type] = def;
		}

	}

	public static WeaponDefinition GetWeaponDefinition(WeaponType wt){
		if (W_DEFS.ContainsKey (wt)) {
			return W_DEFS[wt];
		}
		return new WeaponDefinition ();
	}


	void Start () {
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i=0; i<weaponDefinitions.Length; i++) {
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void SpawnEnemy(){
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;
		Vector3 pos = Vector3.zero;
		float xMin = Util.camBounds.min.x + enemySpawnPadding;
		float xMax = Util.camBounds.max.x + enemySpawnPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = Util.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;
		Invoke ("SpawnEnemy", enemySpawnRate);

	}

	public void Restart(){
		Application.LoadLevel ("main");
	}

	public void DelayedRestart (float delay){
		Invoke ("Restart", delay);
	}

	public void ShipDestroyed(Enemy e){
		if (Random.value <= e.powerUpDropChance) {
			int ndx = Random.Range(0,powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency[ndx];

			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp>();
			pu.SetType(puType);
			pu.transform.position = e.transform.position;
		
		}

	}
}