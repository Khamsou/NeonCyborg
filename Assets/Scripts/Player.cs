using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	[Header("DEBUG")]
	public bool DebugPlayer = false;

	[Header("Linking")]
	public GameObject PlayerSprite;
	public GameObject PlayerLight;
	public GameObject PlayerWeaponLight;
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

	//Velocity
	private float _xVel;
	private float _yVel;

	//View
	private float _xView;
	private float _yView;

	private float _angle;

	private Rigidbody2D _rb;

	//Light on or ?
	private bool lightOn;

	//Attacking
	private bool _attacking;
	private float _timerAttack;

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

		_angle = StartingAngle;
		OrientationRoot.transform.localEulerAngles = new Vector3 (0, 0, _angle);

		lightOn = false;

		_attacking = false;
		_timerAttack = AttackTime + AttackCoolDown;

		_guarding = false;

		_weaponSprite = Weapon.GetComponent<SpriteRenderer>();
		_weaponAnim = Weapon.GetComponent<Animator>();

		_curAnimWeapon = "";
		_nexAnimWeapon = "Idle";
	}
	
	// Update is called once per frame
	void Update () {

		_xVel = Input.GetAxis(PlayerIdentifier + "Horizontal") * Time.deltaTime * PlayerSpeed;
		_yVel = Input.GetAxis(PlayerIdentifier + "Vertical") * Time.deltaTime * PlayerSpeed;

		_xView = Input.GetAxis (PlayerIdentifier + "HorizontalView");
		_yView = Input.GetAxis (PlayerIdentifier + "VerticalView");

		_angle = Mathf.Atan2 (_yView, _xView) * Mathf.Rad2Deg;

		if (!_attacking && (_xView > InputOrientationDetect || _yView > InputOrientationDetect || _xView < -InputOrientationDetect || _yView < -InputOrientationDetect)) {
			OrientationRoot.transform.localEulerAngles = new Vector3 (0, 0, _angle);
		}

		_timerAttack += Time.deltaTime;

		lightOn = false;
		_guarding = false;


		if (!_guarding && !_attacking && _timerAttack > (AttackTime + AttackCoolDown)) {
			if (Input.GetButtonDown (PlayerIdentifier + "Attack")) {
				_attacking = true;
				_timerAttack = 0;
				_nexAnimWeapon = "HallebardeAttack";
			}
		}
		if (!_attacking && Input.GetButton (PlayerIdentifier + "Guard")) {
			_guarding = true;
		}

		if (_guarding == true) {
			lightOn = true;
			if (GuardPenalty) {
				_xVel = _xVel * ((100 - GuardPenaltyPercentage) / 100f);
				_yVel = _yVel * ((100 - GuardPenaltyPercentage) / 100f);
			}
			_nexAnimWeapon = "HallebardeGuard";
		} else {
			if (_attacking) {
				if (_timerAttack <= AttackTime) {
					lightOn = true;
				} else {
					_attacking = false;
				}
				//CANCEL MOVEMENT
				_xVel = _yVel = 0;
			} else {
				_nexAnimWeapon = "Idle";
			}
		}

		_rb.velocity = new Vector2(_xVel, _yVel);

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
			PlayerWeaponLight.SetActive(true);
			_weaponSprite.enabled = true;
		} else {
			PlayerSprite.SetActive(false);
			PlayerLight.SetActive(false);
			PlayerWeaponLight.SetActive(false);
			_weaponSprite.enabled = false;
		}
	}		
}
