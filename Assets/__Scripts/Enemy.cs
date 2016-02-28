﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float		speed = 10f;
	public float		fireRate = 0.3f;
	public float		health = 10;
	public int			score = 100;
	public int 			showDamageForFrames = 2;
	public float 		powerUpDropChance = .25f;
	public bool _______________;
	public Color[] 		originalColors;
	public Material[] 	materials;
	public int 			remainingDamageFrames = 0;
	public Bounds 		bounds;
	public Vector3 		boundsCenterOffset;

	void Start () {
	
	}//end of start

	void Update () {
		Move ();
		if (remainingDamageFrames > 0) {
			remainingDamageFrames--;
			if (remainingDamageFrames == 0) {
				UnShowDamage ();
			}//end of nested if
		}//end of if
	}//end of Update

	public virtual void Move(){
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}//end of Move()

	public Vector3 pos{
		get{
			return (this.transform.position);
		}
		set{
			this.transform.position = value;
		}
	}//end of pos

	void Awake(){
		materials = Utils.GetAllMaterials (gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors [i] = materials [i].color;
		}//end of for loop
		InvokeRepeating ("CheckOffscreen", 0f, 2f);
	}//end of Awake()

	void CheckOffscreen(){
		if (bounds.size == Vector3.zero) {
			bounds = Utils.CombineBoundsOfChildren (this.gameObject);
			boundsCenterOffset = bounds.center - transform.position;
		}//end of if
		bounds.center = transform.position + boundsCenterOffset;
		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.offScreen);
		if (off != Vector3.zero) {
			if (off.y < 0) {
				Destroy (this.gameObject);
			}//end of nested if
		}//end of if
	}//end of CheckOffScreen()

	void OnCollisionEnter(Collision coll){
		GameObject other = coll.gameObject;
		switch (other.tag) {
		case "ProjectileHero":
			Projectile p = other.GetComponent<Projectile> ();
			bounds.center = transform.position + boundsCenterOffset;
			if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck (bounds, BoundsTest.offScreen) != Vector3.zero) {
				Destroy (other);
				break;
			}//end of if
			health -= Main.W_DEFS [p.type].damageOnHit;
			ShowDamage ();
			if (health <= 0) {
				Main.S.ShipDestroyed (this);
				Destroy (this.gameObject);
			}//end of if
			Destroy (other);
			break;
		}//end of switch
	}//end of OnCollisionEnter(Collision coll)

	void ShowDamage(){
		foreach (Material m in materials) {
			m.color = Color.red;
		}//end of foreach
		remainingDamageFrames = showDamageForFrames;
	}//end of ShowDamage()

	void UnShowDamage(){
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}//end of for
	}//UnShowDamage()
}//end of Enemy Script