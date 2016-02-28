using UnityEngine;
using System.Collections;

public enum WeaponType{
	none,
	blaster,
	spread,
	phaser,
	missile,
	laser,
	shield
}//end of enum WeaponType

[System.Serializable]
public class WeaponDefinition{
	public WeaponType type = WeaponType.none;
	public string letter;
	public Color color = Color.white;
	public GameObject projectilePrefab;
	public Color projectileColor = Color.white;
	public float damageOnHit = 0;
	public float continuousDamage = 0;
	public float delayBetweenShots = 0;
	public float velocity = 20;
}//end of WeaponDefintion

public class Weapon : MonoBehaviour {
	static public Transform PROJECTILE_ANCHOR;
	public bool ________________;
	[SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition def;
	public GameObject collar;
	public float lastShot;

	void Awake(){
		collar = transform.Find ("Collar").gameObject;
	}//end of Awake()

	void Start () {
		SetType (_type);

		if (PROJECTILE_ANCHOR == null) {
			GameObject go = new GameObject("_Projectile_Anchor");
			PROJECTILE_ANCHOR = go.transform;
		}//end of if
		GameObject parentGO = transform.parent.gameObject;
		if (parentGO.tag == "Hero") {
			Hero.S.fireDelegate += Fire;
		}//end of if
	}//end of Start()


	public WeaponType type{
		get{
			return(_type);
		}//end of get
		set{
			SetType (value);
		}//end of set
	}//end of type

	public void SetType(WeaponType wt){
		_type = wt;
		if (type == WeaponType.none) {
			this.gameObject.SetActive (false);
			return;
		}//end of if
		else {
			this.gameObject.SetActive (true);
		}//end of else
		def = Main.GetWeaponDefention(_type);
		collar.GetComponent<Renderer> ().material.color = def.color;
		lastShot = 0;
	}//end of SetType(WeaponType wt)


	public void Fire(){
		if (!gameObject.activeInHierarchy)
			return;
		if (Time.time - lastShot < def.delayBetweenShots) {
			return;
		}//end of if
		Projectile p;
		switch (type) {
		case WeaponType.blaster:
			p = MakeProjectile ();
			p.GetComponent<Rigidbody> ().velocity = Vector3.up * def.velocity;
			break;

		case WeaponType.spread:
			p = MakeProjectile ();
			p.GetComponent<Rigidbody> ().velocity = Vector3.up * def.velocity;
			p = MakeProjectile ();
			p.GetComponent<Rigidbody> ().velocity = new Vector3 (-.2f, 0.9f, 0) * def.velocity;
			p = MakeProjectile ();
			p.GetComponent<Rigidbody> ().velocity = new Vector3 (.2f, 0.9f, 0) * def.velocity;
			break;
		}//end of switch
	}//end of fire

	public Projectile MakeProjectile(){
		GameObject go = Instantiate (def.projectilePrefab) as GameObject;
		if (transform.parent.gameObject.tag == "Hero") {
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer ("ProjectileHero");
		}//end of if
		else {
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer ("ProjectileEnemy");
		}//end of else
		go.transform.position = collar.transform.position;
		go.transform.parent = PROJECTILE_ANCHOR;
		Projectile p = go.GetComponent<Projectile> ();
		p.type = type;
		lastShot = Time.time;
		return (p);
	}//end of MakeProjectile

	void Update () {
	
	}//end of Update()
}
