﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public Vector2 rotMinMax = new Vector2(15,90);
	public Vector2 driftMinMax = new Vector2(.25f,2);
	public float lifeTime = 6f;
	public float fadeTime = 4f;
	public bool ________________;
	public WeaponType type;
	public GameObject cube;
	public TextMesh letter;
	public Vector3 rotPerSecond;
	public float birthTime;



	void Awake(){
		cube = transform.Find ("Cube").gameObject;
		letter = GetComponent<TextMesh> ();

		Vector3 vel = Random.onUnitSphere;
		vel.z = 0;
		vel.Normalize();
		vel *= Random.Range (driftMinMax.x, driftMinMax.y);
		rigidbody.velocity = vel;

		transform.rotation = Quaternion.identity;

		rotPerSecond = new Vector3 (Random.Range(rotMinMax.x,rotMinMax.y),
		                            Random.Range(rotMinMax.x,rotMinMax.y),
		                            Random.Range(rotMinMax.x,rotMinMax.y));
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
		birthTime = Time.time;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);
		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
		if (u >= 1) {
			Destroy(this.gameObject);
			return;
		}
		if (u > 0) {
			Color c = cube.renderer.material.color;
			c.a = 1f-u;
			cube.renderer.material.color = c;
			c = letter.color;
			c.a = 1f-(u*0.5f);
			letter.color = c;

		}
	
	}

	public void SetType(WeaponType wt){
		WeaponDefinition def = Main.GetWeaponDefinition (wt);
		cube.renderer.material.color = def.color;
		letter.text = def.letter;
		type = wt;
	}

	public void AbsorbedBy(GameObject target){
		Destroy (this.gameObject);
	}

	void CheckOffscreen(){
		if (Util.ScreenBoundsCheck (cube.collider.bounds, BoundsTest.offScreen) != Vector3.zero) {
			Destroy(this.gameObject);
		}
	}

















}
