using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[SerializeField]
	private WeaponType _type;

	public WeaponType type{
		get{
			return (_type);
		}//end of get
		set{
			SetType (value);
		}//end of set
	} //end of type

	void Start () {
	
	}//end of Start()

	void Update () {
	
	}//end of Update()

	void Awake(){
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
	}//end of Awake()

	public void SetType(WeaponType eType){
		_type = eType;
		WeaponDefinition def = Main.GetWeaponDefention (_type);
		GetComponent<Renderer> ().material.color = def.projectileColor;
	}//SetType(WeaponType eType)

	void CheckOffscreen(){
		if(Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds,BoundsTest.offScreen) != Vector3.zero){
			Destroy(this.gameObject);
		}//end of if
	}//end of CheckOffscreen
}
