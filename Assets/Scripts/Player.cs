using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	[Header("DEBUG")]
	public bool DebugPlayer = false;

	[Header("Linking")]
	public GameObject PlayerSprite;
	public GameObject PlayerLightParent;
	public GameObject PlayerLight;
	public GameObject OrientationRoot;
	public GameObject Weapon;
	public GameObject Guard;

	[Header("Main Config")]
	public string PlayerIdentifier = "Player1";
	public float StartingAngle = 90;
	public float InputOrientationDetect = 0.2f;

	[Header("Attack (SECONDS)")]
	public float AttackTime;
	public float AttackCoolDown;

	[Header("Vitesse du joueur")]
	public float PlayerSpeed;

	[Header("Pénalité de garde ?")]
	public bool GuardPenalty;
	[Header("0% -> Pénalité faible, 100% -> Immobile")]
	public int GuardPenaltyPercentage;
	[Header("Stamina garde")]
	public float StaminaRegen;
	public float StaminaUse;

	//Velocity
	private float _xVel;
	private float _yVel;

	//Control
	private float _controlTimer;
	private float _controlCooldown;

	//View
	private float _xView;
	private float _yView;

	//Left Stick Direction
	private float _lAngle;

	//Right Stick Direction
	private float _rAngle;

	//Stamina Guard
	private float _staminaGuard;
	private float _staminaRegen;

	//RigidBody
	private Rigidbody2D _rb;

	//Light on or ?
	private bool _lightOn;

	//Attacking
	private bool _attacking;
	private float _timerAttack;
	private bool _left;

	//Guard
	private bool _guarding;

	private Animator _weaponAnim;
	private SpriteRenderer _weaponSprite;

	private string _curAnimWeapon;
	private string _nexAnimWeapon;

	void Start () {
		_rb = GetComponent<Rigidbody2D>();

		_xVel = 0;
		_yVel = 0;

		_xView = 0;
		_yView = 0;

		_lAngle = StartingAngle;
		OrientationRoot.transform.localEulerAngles = new Vector3 (0, 0, _lAngle);

		_lightOn = false;

		_attacking = false;
		_timerAttack = AttackTime + AttackCoolDown;

		_guarding = false;

		_weaponSprite = Weapon.GetComponent<SpriteRenderer>();
		_weaponAnim = Weapon.GetComponent<Animator>();

		_curAnimWeapon = "";
		_nexAnimWeapon = "Idle";

		_controlCooldown = 0.2f;
		_controlTimer = _controlCooldown;

		_staminaGuard = 100;
		_staminaRegen = StaminaRegen;

		_left = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Inputs
		//Left stick
		_xVel = Input.GetAxis(PlayerIdentifier + "Horizontal") * Time.deltaTime * PlayerSpeed;
		_yVel = Input.GetAxis(PlayerIdentifier + "Vertical") * Time.deltaTime * PlayerSpeed;

		//Right stick
		_xView = Input.GetAxis (PlayerIdentifier + "HorizontalView");
		_yView = Input.GetAxis (PlayerIdentifier + "VerticalView");

		//Angle calculation
		_lAngle = Mathf.Atan2 (_yVel, _xVel) * Mathf.Rad2Deg;
		_rAngle = Mathf.Atan2 (_yView, _xView) * Mathf.Rad2Deg;

		//Lights
		PlayerLight.GetComponent<Light> ().intensity = (150 - _staminaGuard) / 20;
		PlayerLight.GetComponent<Light> ().range = (250 - _staminaGuard) / 20;

		//Apply rotation if NOT ATTACKING
		if (!_attacking){
			if (_xVel > InputOrientationDetect || _yVel > InputOrientationDetect || _xVel < -InputOrientationDetect || _yVel < -InputOrientationDetect) {
				OrientationRoot.transform.localEulerAngles = new Vector3 (0, 0, _lAngle);
			}
			if (_xView > InputOrientationDetect || _yView > InputOrientationDetect || _xView < -InputOrientationDetect || _yView < -InputOrientationDetect) {
				OrientationRoot.transform.localEulerAngles = new Vector3 (0, 0, _rAngle);
			}
		}

		//Timers
		_timerAttack += Time.deltaTime;
		_controlTimer += Time.deltaTime;

		//Visual
		_lightOn = false;

		_guarding = false;
		//ATTACKING
		if (_controlTimer > _controlCooldown && !_guarding && !_attacking && _timerAttack > (AttackTime + AttackCoolDown)) {
			if (Input.GetButtonDown (PlayerIdentifier + "Attack")) {
				_attacking = true;
				_timerAttack = 0;
				if (_left) {
					_nexAnimWeapon = "SwordAttack";
				} else {
					_nexAnimWeapon = "SwordAttack2";
				}
				Instantiate(Resources.Load("Sounds/SwordSwoosh" + Random.Range(1, 5) + "Sound"));
				_left = !_left;
			}
		}

		//Guard
		if (_controlTimer > _controlCooldown && !_attacking && Input.GetButton (PlayerIdentifier + "Guard")) {
			_guarding = true;
		}

		//Guarding
		if (_guarding == true) {
			if (_staminaGuard >= 0) {
				_staminaGuard -= StaminaUse * Time.deltaTime;

				if (_staminaGuard < 20) {
					PlayerLight.transform.position = new Vector3 (PlayerLightParent.transform.position.x + Random.Range (-0.5f, 0.5f), PlayerLightParent.transform.position.y + Random.Range (-0.5f, 0.5f), PlayerLightParent.transform.position.z);
				} else {
					PlayerLight.transform.position = PlayerLightParent.transform.position;
				}
			} else {
				_guarding = false;
				Instantiate (Resources.Load ("Sounds/StunSound"));
				Instantiate (Resources.Load ("Effects/StunEffect"), PlayerLight.transform.position, Quaternion.identity);
				_xVel = _yVel = 0;
				_rb.velocity = new Vector2(_xVel, _yVel);
				Stun (0.8f);
			}
			_lightOn = true;
			if (GuardPenalty) {
				_xVel = _xVel * ((100 - GuardPenaltyPercentage) / 100f);
				_yVel = _yVel * ((100 - GuardPenaltyPercentage) / 100f);
			}
			_nexAnimWeapon = "SwordGuard";
		} else {
			if (_staminaGuard < 100) {
				_staminaGuard += _staminaRegen * Time.deltaTime;
			} else {
				_staminaGuard = 100;
			}
			if (_attacking) {
				if (_timerAttack <= AttackTime) {
					_lightOn = true;
				} else {
					_attacking = false;
				}
				//CANCEL MOVEMENT
				_xVel = _yVel = 0;
			} else {
				_nexAnimWeapon = "Idle";
			}
		}

		if(_controlTimer > _controlCooldown) _rb.velocity = new Vector2(_xVel, _yVel);

		//Controlling colliders
		Guard.GetComponent<Collider2D>().enabled = _guarding;
		Weapon.GetComponent<Collider2D> ().enabled = _attacking;

		if(!DebugPlayer) UpdatePlayerVisibility();
		HandleWeaponAnimation();

	}

	public void CancelAttack(){
		_attacking = false;
		_timerAttack = AttackTime;
	}

	public void Repulse(Vector2 origin, float force){
		Vector2 forceApplied = (new Vector2 (transform.position.x, transform.position.y) - origin) * force;
		_rb.AddForceAtPosition (forceApplied, origin);
	}

	public void Stun(float duration){
		_controlTimer = 0;
		_controlCooldown = duration;
	}

	public void ConsumeStamina(float amount){
		_staminaGuard -= amount;
	}

	public void Reset(){
		_controlCooldown = 0.2f;
		_controlTimer = _controlCooldown;
		_staminaGuard = 100f;
		_lightOn = false;
	}

	public float GetStamina(){
		return _staminaGuard;
	}

	void HandleWeaponAnimation(){
		if(_curAnimWeapon != _nexAnimWeapon){
			_curAnimWeapon = _nexAnimWeapon;
			_weaponAnim.Play(_nexAnimWeapon);
		}
	}

	void UpdatePlayerVisibility(){
		if(_lightOn || _controlTimer < _controlCooldown){
			PlayerLight.SetActive(true);
			_weaponSprite.enabled = true;
		} else {
			PlayerLight.SetActive(false);
			_weaponSprite.enabled = false;
		}
	}		
}
