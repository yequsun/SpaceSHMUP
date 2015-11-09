﻿using UnityEngine;
using System.Collections;

public class Enemy_3 : Enemy {
	public Vector3 [] points;
	public float birthTime;
	public float lifeTime = 10;


	// Use this for initialization
	void Start () {
		points = new Vector3[3];
		points [0] = pos;
		float xMin = Util.camBounds.min.x + Main.S.enemySpawnPadding;
		float xMax = Util.camBounds.max.x - Main.S.enemySpawnPadding;

		Vector3 v;
		v = Vector3.zero;
		v.x = Random.Range (xMin, xMax);
		v.y = Random.Range (Util.camBounds.min.y, 0);
		points [1] = v;

		v = Vector3.zero;
		v.y = pos.y;
		v.x = Random.Range (xMin, xMax);
		points [2] = v;

		birthTime = Time.time;

	
	}

	public override void Move ()
	{
		float u = (Time.time - birthTime) / lifeTime;
		if (u > 1) {
			Destroy(this.gameObject);
			return;
		}
		Vector3 p01, p12;
		u = u - 0.2f * Mathf.Sin (u * Mathf.PI * 2);
		p01 = (1 - u) * points [0] + u * points [1];
		p12 = (1 - u) * points [1] + u * points [2];
		pos = (1 - u) * p01 + u * p12;

	}
	
	// Update is called once per frame
}
