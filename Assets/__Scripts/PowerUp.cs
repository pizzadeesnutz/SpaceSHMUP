using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
	public Vector2 rotMinMax = new Vector2(15,90);
	public Vector2 driftMinMax = new Vector2 (.25f, 2);
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
		vel.Normalize ();
		vel *= Random.Range (driftMinMax.x, driftMinMax.y);
		GetComponent<Rigidbody> ().velocity = vel;
		transform.rotation = Quaternion.identity;
		rotPerSecond = new Vector3 (Random.Range (rotMinMax.x, rotMinMax.y), Random.Range (rotMinMax.x, rotMinMax.y), Random.Range (rotMinMax.x, rotMinMax.y));
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
		birthTime = Time.time;
	}//end of Awake

	void Start () {
	
	}//end of Start()

	void Update () {
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);
		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}//end of if
		if (u > 0) {
			Color c = cube.GetComponent<Renderer> ().material.color;
			c.a = 1f - u;
			cube.GetComponent<Renderer> ().material.color = c;
			c = letter.color;
			c.a = 1f - (u * 0.5f);
			letter.color = c;
		}//end of if
	}//end of Update()

	public void SetType(WeaponType wt){
		WeaponDefinition def = Main.GetWeaponDefention (wt);
		cube.GetComponent<Renderer> ().material.color = def.color;
		letter.text = def.letter;
		type = wt;
	}//end of SetType(WeaponType wt)

	public void AbsorbedBy(GameObject target){
		Destroy (this.gameObject);
	}//end AbsorbedBy(GameObject target)

	void CheckOffscreen(){
		if (Utils.ScreenBoundsCheck (cube.GetComponent<Collider> ().bounds, BoundsTest.offScreen) != Vector3.zero) {
			Destroy (this.gameObject);
		}//end of if
	}//end CheckOffscreen()
}
