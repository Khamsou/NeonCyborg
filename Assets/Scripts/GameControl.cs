using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	public bool SinglePlayer;
	public bool Training;

	public int Player1StartLives = 3;
	public int Player2StartLives = 3;

	public Text Player1Lives;
	public Text Player2Lives;

	private float _player1Lives;
	private float _player2Lives;

	// Use this for initialization
	void Start () {
		SpawnPlayers (true, SinglePlayer);
		if (Training) {
			Instantiate (Resources.Load ("Trainer"));
		}
		_player1Lives = Player1StartLives;
		_player2Lives = Player2StartLives;
	}
	
	// Update is called once per frame
	void Update () {
		Player1Lives.text = "Player 1 Lives : " + _player1Lives;
		Player2Lives.text = "Player 2 Lives : " + _player2Lives;
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
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("SpawnPoint");

		victim.transform.position = spawns [Random.Range (0, spawns.Length)].transform.position;
	}
}
