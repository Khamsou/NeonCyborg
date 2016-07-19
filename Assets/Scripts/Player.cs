using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	
	public GameObject PlayerSprite;
	public GameObject PlayerLight;

	public GameObject Weapon;

	[Header("Attack (SECONDS)")]
	public float AttackTime;
	public float AttackCoolDown;

	[Header("Vitesse du joueur")]
	public float PlayerSpeed;

	[Header("Pénalité de garde ?")]
	public bool GuardPenalty;
	[Header("0% -> Pénalité faible, 100% -> Immobile")]
	public int GuardPenaltyPercentage;

	//Velocity
	private float _xVel;
	private float _yVel;

	private Rigidbody2D _rb;

	//Light on or ?
	private bool lightOn;

	//Attacking
	private bool _attacking;
	private float _timerAttack;

	private Animator _weaponAnim;
	private SpriteRenderer _weaponSprite;

	private string _curAnimWeapon;
	private string _nexAnimWeapon;

	void Start () {
		_rb = GetComponent<Rigidbody2D>();

		_xVel = 0;
		_yVel = 0;

		lightOn = false;

		_attacking = false;
		_timerAttack = AttackTime + AttackCoolDown;

		_weaponSprite = Weapon.GetComponent<SpriteRenderer>();
		_weaponAnim = Weapon.GetComponent<Animator>();

		_curAnimWeapon = "";
		_nexAnimWeapon = "Idle";
	}
	
	// Update is called once per frame
	void Update () {
		_xVel = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeed;
		_yVel = Input.GetAxis("Vertical") * Time.deltaTime * PlayerSpeed;

		_timerAttack += Time.deltaTime;

		lightOn = false;


		if(Input.GetButtonDown("Attack") && !_attacking && _timerAttack > (AttackTime + AttackCoolDown)){
			_attacking = true;
			_timerAttack = 0;
			_nexAnimWeapon = "HallebardeAttack";
		}

		if(_attacking){
			
			if(_timerAttack <= AttackTime){
				lightOn = true;
			} else {
				_attacking = false;
				_nexAnimWeapon = "Idle";
			}
			//CANCEL MOVEMENT
			_xVel = _yVel = 0;
		}

		_rb.velocity = new Vector2(_xVel, _yVel);

		UpdatePlayerVisibility();
		HandleWeaponAnimation();
		LolDebug();
	}

	void HandleWeaponAnimation(){
		if(_curAnimWeapon != _nexAnimWeapon){
			_curAnimWeapon = _nexAnimWeapon;
			_weaponAnim.Play(_nexAnimWeapon);
		}
	}

	void UpdatePlayerVisibility(){
		if(lightOn){
			PlayerSprite.SetActive(true);
			PlayerLight.SetActive(true);
			_weaponSprite.enabled = true;
		} else {
			PlayerSprite.SetActive(false);
			PlayerLight.SetActive(false);
			_weaponSprite.enabled = false;
		}
	}

	void LolDebug(){
		GameObject.Find("Text").GetComponent<Text>().text = "Timer attack : " + _timerAttack + "\nAttack Time : " + AttackTime  + "\nAttack cooldown : " + AttackCoolDown;
	}
}
