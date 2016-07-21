using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	public bool SinglePlayer;
	public bool Training;

	public int Player1StartLives = 3;
	public int Player2StartLives = 3;

	public Text Player1Lives;
	public Text Player2Lives;

	public GameObject GameOver;

	private float _player1Lives;
	private float _player2Lives;

	private bool _matchEnded;

	private float _timerAfterMatch;

	// Use this for initialization
	void Start () {
		SpawnPlayers (true, SinglePlayer);
		if (Training) {
			Instantiate (Resources.Load ("Trainer"));
		}
		_player1Lives = Player1StartLives;
		_player2Lives = Player2StartLives;
		GameOver.SetActive (false);
		_matchEnded = false;
	}
	
	// Update is called once per frame
	void Update () {
		Player1Lives.text = "Player 1 Lives : " + _player1Lives;
		Player2Lives.text = "Player 2 Lives : " + _player2Lives;

		if (_player1Lives == 0 || _player2Lives == 0) {
			GameOver.SetActive (true);
			_matchEnded = true;
		}

		if (_matchEnded) {
			_timerAfterMatch += Time.deltaTime;
			if (_timerAfterMatch > 1) {
				if (Input.anyKey) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				}
			}
		}
	}

	void SpawnPlayers(bool first, bool onlyOne){

		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("SpawnPoint");

		int firstNumber = Random.Range (0, spawns.Length);
		int secondNumber;

		do {
			secondNumber = Random.Range (0, spawns.Length);
		} while (secondNumber == firstNumber);

		GameObject firstSpawn = spawns [firstNumber];
		GameObject secondSpawn = spawns [secondNumber];


		if (first) {
			GameObject player1 = (GameObject)Instantiate (Resources.Load ("Player1"), transform.position, Quaternion.identity);
			player1.name = "Player1";
			player1.GetComponent<Player> ().PlayerIdentifier = "Player1";
			if(!onlyOne){
				GameObject player2 = (GameObject)Instantiate (Resources.Load ("Player2"), transform.position, Quaternion.identity);
				player2.name = "Player2";
				player2.GetComponent<Player> ().PlayerIdentifier = "Player2";
			}
		}
		GameObject.Find ("Player1").transform.position = firstSpawn.transform.position;
		if(!onlyOne) GameObject.Find ("Player2").transform.position = secondSpawn.transform.position;
	}

	public void AddPoints(string winner){
		if(winner == "Player1"){
			_player2Lives --;
		} else {
			_player1Lives --;
		}
	}
	public void RespawnPlayer(GameObject victim){
		if ((victim.name == "Player1" && _player1Lives > 0) || (victim.name == "Player2" && _player2Lives > 0)) {
			GameObject[] spawns = GameObject.FindGameObjectsWithTag ("SpawnPoint");

			victim.transform.position = spawns [Random.Range (0, spawns.Length)].transform.position;
		}
	}
}
