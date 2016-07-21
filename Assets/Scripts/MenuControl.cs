using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;


public class MenuControl : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject PlayMenu;

	public Text Player1Lobby;
	public Text Player2Lobby;

	private bool _playLobby;

	private bool _player1Ready;
	private bool _player2Ready;

	private float _timerMenuSwitch;

	private float _timer1Vibrate;
	private float _timer2Vibrate;

	private float _timerGo;

	private bool _timer3;
	private bool _timer2;
	private bool _timer1;

	// Use this for initialization
	void Start () {
		MainMenu.SetActive (true);
		PlayMenu.SetActive (false);
		_playLobby = false;

		Player1Lobby.text = "Joueur 1 - Appuyez sur A pour rejoindre la partie.";
		Player2Lobby.text = "Joueur 2 - Appuyez sur A pour rejoindre la partie.";

		_timer1Vibrate = 1.0f;
		_timer2Vibrate = 1.0f;
		_timerGo = 0;
		_timer3 = _timer2 = _timer1 = false;
	}
	
	// Update is called once per frame
	void Update () {
		_timerMenuSwitch += Time.deltaTime;
		if (_timerMenuSwitch > 0.1f){
			if (_playLobby) {
				if (!_player1Ready && Input.GetKeyDown (KeyCode.Joystick1Button0)) {
					_player1Ready = true;
					_timer1Vibrate = 0.8f;
					Player1Lobby.text = "Joueur 1 - Prêt !";
				}
				if (!_player2Ready && Input.GetKeyDown (KeyCode.Joystick2Button0)) {
					_player2Ready = true;
					_timer2Vibrate = 0.8f;
					Player2Lobby.text = "Joueur 2 - Prêt !";
				}
			}
			if (_player1Ready && _player2Ready) {
				_timerGo += Time.deltaTime;

				if (!_timer3 && _timerGo > 1.0f) {
					GameObject test = (GameObject)Instantiate (Resources.Load ("Effects/3"));
					test.transform.SetParent (GameObject.Find ("Canvas").transform, false);
					test.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
					Instantiate (Resources.Load ("Sounds/GameReadySound"));
					_timer3 = true;
				}
				if (!_timer2 && _timerGo > 2.0f) {
					GameObject test = (GameObject)Instantiate (Resources.Load ("Effects/2"));
					test.transform.SetParent(GameObject.Find("Canvas").transform, false);
					test.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
					_timer2 = true;
				}
				if (_timerGo > 2.5f) {
					_timer1Vibrate = _timer2Vibrate = 0.0f;
				}
				if (!_timer1 && _timerGo > 3.0f) {
					GameObject test = (GameObject)Instantiate (Resources.Load ("Effects/1"));
					test.transform.SetParent (GameObject.Find ("Canvas").transform, false);
					test.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
					_timer1 = true;

				}
				if (_timerGo > 4.0f) {
					MainMenu.SetActive (false);
					PlayMenu.SetActive (false);
					_timer1Vibrate = _timer2Vibrate = 50.0f;
				}
				if (_timerGo > 5.0f) {
					SceneManager.LoadScene ("Work");
				}
			}
		}
		HandleGamepadVibrate ();
	}

	void HandleGamepadVibrate(){
		_timer1Vibrate += Time.deltaTime;
		_timer2Vibrate += Time.deltaTime;
		if (_timer1Vibrate < 1.0f) {
			GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
		} else {
			GamePad.SetVibration (PlayerIndex.One, 0f, 0f);
		}
		if (_timer2Vibrate < 1.0f) {
			GamePad.SetVibration (PlayerIndex.Two, 1.0f, 1.0f);
		} else {
			GamePad.SetVibration (PlayerIndex.Two, 0f, 0f);
		}
	}

	public void GoToPlayMenu(){
		MainMenu.SetActive (false);
		PlayMenu.SetActive (true);
		_playLobby = true;
		_timerMenuSwitch = 0;
	}

	public void GoBack(){
		if (_playLobby) {
			MainMenu.SetActive (true);
			PlayMenu.SetActive (false);
			_playLobby = false;
			_timerMenuSwitch = 0;
		}
	}
}
