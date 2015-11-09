﻿using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero S;
	public float gameRestartDelay = 2f;

	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;
	public bool ___________;
	public Bounds bounds;
	public delegate void WeaponFireDelegate ();
	public WeaponFireDelegate fireDelegate;
	public GameObject lastTriggerGo = null;
	[SerializeField]
	private float _shieldLevel = 1;

	public Weapon[] weapons;



	void Awake(){
		S = this;
		bounds = Util.CombineBoundsOfChildren (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		ClearWeapons ();
		weapons [0].SetType (WeaponType.blaster);
	
	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		bounds.center = transform.position;

		Vector3 off = Util.ScreenBoundsCheck (bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) {
			pos -= off;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler (yAxis * pitchMult, xAxis * rollMult, 0);

		if (Input.GetAxis ("Jump") == 1 && fireDelegate != null) {
			fireDelegate();
		}
	
	}

	void OnTriggerEnter(Collider other){
		GameObject go = Util.FindTaggedParent (other.gameObject);
		if (go != null) {
			if(go == lastTriggerGo){
				return;
			}
			lastTriggerGo = go;

			if(go.tag == "Enemy"){
				shieldLevel --;
				Destroy(go);
			}else if(go.tag =="PowerUp"){
				AbsorbPowerUp(go);
			}
			else{
				print ("Triggered: " + go.name);
			}
		} else {
			print ("Triggered: "+other.gameObject.name);
		}
	}

	public float shieldLevel {
		get {
			return _shieldLevel;
		}
		set{
			_shieldLevel = Mathf.Min(value, 4);
			if (value <0){
				Destroy(this.gameObject);
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}

	public void AbsorbPowerUp(GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();
		switch (pu.type) {
		case WeaponType.shield:
			shieldLevel++;
			break;
		default:
			if(pu.type == weapons[0].type){
				Weapon w = GetEmptyWeaponSlot();
				if(w!= null){
					w.SetType(pu.type);
				}
			}else{
				ClearWeapons();
				weapons[0].SetType(pu.type);
			}
			break;
		}
		pu.AbsorbedBy (this.gameObject);
	}

	Weapon GetEmptyWeaponSlot(){
		for (int i=0; i<weapons.Length; i++) {
			if(weapons[i].type == WeaponType.none){
				return weapons[i];
			}
		}
		return null;
	}

	void ClearWeapons(){
		foreach (Weapon w in weapons) {
			w.SetType(WeaponType.none);
		}
	}









}
