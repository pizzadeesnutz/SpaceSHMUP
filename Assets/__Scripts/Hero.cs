using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero		S;
	public float gameRestartDelay = 2f;

	public float	speed = 30;
	public float	rollMult = -45;
	public float  	pitchMult=30;

	[SerializeField]
	private float	_shieldLevel=1;
	public Weapon[] weapons;

	public bool	_____________________;
	public Bounds bounds;
	public delegate void WeaponFireDelegate ();
	public WeaponFireDelegate fireDelegate;

	public GameObject lastTriggerGo = null;

	void Awake(){
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);

	}//end of Awake()

	void Start () {
		ClearWeapons ();
		weapons [0].SetType (WeaponType.blaster);
	}//end of Start()
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;
		
		bounds.center = transform.position;
		
		// constrain to screen
		Vector3 off = Utils.ScreenBoundsCheck(bounds,BoundsTest.onScreen);
		if (off != Vector3.zero) {  // we need to move ship back on screen
			pos -= off;
			transform.position = pos;
		}
		
		// rotate the ship to make it feel more dynamic
		transform.rotation =Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult,0);

		if (Input.GetAxis ("Jump") == 1 && fireDelegate != null) {
			fireDelegate ();
		}//end of if
	}

	void OnTriggerEnter(Collider other){
		GameObject go = Utils.FindTaggedParent (other.gameObject);
		if (go != null) {
			if (go == lastTriggerGo) {
				return;
			}//end of nested if
			lastTriggerGo = go;
			if (go.tag == "Enemy") {
				shieldLevel--;
				Destroy (go);
			}//end of nested if
			else if(go.tag == "PowerUp"){
				AbsorbPowerUp (go);
			}//end of else if
			else {
				print ("Trigered by: " + go.name);
			}//end of else
		}//end of if
		else {
			print ("Triggered: " + other.gameObject);
		}//end of else
	}//end of OnTriggerEnter

	public float shieldLevel{
		get{
			return(_shieldLevel);
		}//end of get
		set{
			_shieldLevel = Mathf.Min (value, 4);
			if (value < 0) {
				Destroy (this.gameObject);
				Main.S.DelayedRestart (gameRestartDelay);
			}//end of if
		}//end of set
	}//end of shieldLevel

	public void AbsorbPowerUp(GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();
		switch (pu.type) {
		case WeaponType.shield:
			shieldLevel++;
			break;
		default: 
			if (pu.type == weapons [0].type) {
				Weapon w = GetEmptyWeaponSlot ();
				if (w != null) {
					w.SetType (pu.type);
				}//end of nested if
			}//end of if
			else {
				ClearWeapons ();
				weapons [0].SetType (pu.type);
			}//end of else
			break;
		}//end of switch
		pu.AbsorbedBy(this.gameObject);
	}//end of AbsorbPowerUp(GameObject go)

	Weapon GetEmptyWeaponSlot(){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i].type == WeaponType.none) {
				return(weapons [i]);
			}//end of if
		}//end of for
		return(null);
	}//GetEmptyWeaponSlot()

	void ClearWeapons(){
		foreach(Weapon w in weapons){
			w.SetType (WeaponType.none);
		}//end of foreach
	}//end of ClearWeapons()
}