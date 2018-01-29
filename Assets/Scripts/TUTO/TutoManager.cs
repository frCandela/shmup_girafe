using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//1. spawn virus + introduction ("Welcome to BUG.exe...")

//2. spawn 2 ennemies + UI bar + explanations

//3. Hack 1st ennemy + kill other -> "kill ennemies to fill your hack power bar"
//(controls for dps/tank explained)

//4. New wave : hack 2nd ennemy -> "Hack ennemies to increase your combo multiplier"
//(controls for dps/tank explained)

//5. New wave : kill the player -> "Upon destruction of the ennemy you're hacking, you're ejected back to virus form, get ready..."

//6. Start countdown + game.

public class TutoManager : MonoBehaviour 
{
	[SerializeField] private GameObject _manager;
	[SerializeField] private TutorialUIDisplayer _displayer;
	[SerializeField] private TutoEnnemySpawner _spawner;
	[SerializeField] private GameObject _leaderboardDisplay;
	[SerializeField] private GameObject _uI;
	[SerializeField] private GameObject _scoreDisplay;
	[SerializeField] private GameObject _scorePopup;

	private GameManager _gameManager;
	private TimeManager _timeManager;
	private Pawn _currentShip;

	private bool _continue = false;
	private bool _hackedDPS = false;

	void Start()
	{
		_gameManager = _manager.GetComponent<GameManager> ();
		_timeManager = _manager.GetComponent<TimeManager> ();

		_gameManager.PlayerController.onHack.AddListener (HackOccured);

		//hide UI
		_leaderboardDisplay.GetComponent<Text> ().enabled = false;
		_uI.SetActive (false);
		_scoreDisplay.SetActive (false);
		_scorePopup.SetActive (false);

		_currentShip = GameManager.instance.PlayerController.PossessedPawn;
		//Modify virus ship
		_gameManager.PlayerController.virusHackRefillSpeed = 0f;

		//Start Tutorial
		StartCoroutine (Tutorial ());
	}

	void StartGame()
	{
		_timeManager.enabled = true;
		_gameManager.GetComponent<UnityEngine.Playables.PlayableDirector> ().enabled = true;

		//show leaderboard
		_leaderboardDisplay.GetComponent<Text> ().enabled = true;
		_scorePopup.SetActive (true);
	}

	void Update()
	{
		//Press escape to skip tuto
		if (Input.GetKeyDown (KeyCode.Return))
			StartGame ();
	}

	IEnumerator Tutorial()
	{
		//Show intro
		StartCoroutine (_displayer.DisplayStep (0, true));
		yield return StartCoroutine (WaitForLeftClick ());

		//Hide intro, show movement
		yield return StartCoroutine (_displayer.DisplayStep (0, false));
		StartCoroutine (_displayer.DisplayStep (1, true));

		yield return StartCoroutine (WaitForLeftClick ());

		//Hide movement
		yield return StartCoroutine (_displayer.DisplayStep (1, false));

		//Show UI bar + intro to hack power
		_uI.SetActive (true);
		StartCoroutine (_displayer.DisplayStep (2, true));

		yield return StartCoroutine (WaitForLeftClick ());

		//Fill up hack power
		GameManager.instance.PlayerController.virusHackRefillSpeed = 50f;
		//Spawn first wave of ennemies
		_spawner.SpawnWave (0);

		StartCoroutine (_displayer.DisplayStep (2, false));	//hide hack power info
		yield return new WaitForSeconds (2f);

		//Show right click to hack
		StartCoroutine (_displayer.DisplayStep (3, true));

		yield return StartCoroutine (WaitForRightClick ());

		//show left click to validate
		StartCoroutine (_displayer.DisplayStep (3, false));
		StartCoroutine (_displayer.DisplayStep (4, true));

		while (!_continue)
			yield return null;
		_continue = false;

		StartCoroutine (_displayer.DisplayStep (4, false));

		//Freeze time
		Time.timeScale = 0f;

		//Show hack results
		yield return StartCoroutine (_displayer.DisplayStep (5, true));

		yield return WaitForLeftClick ();
		StartCoroutine (_displayer.DisplayStep (5, false));	//hide hack results
		if (_currentShip is DPSShip) 
		{
			yield return StartCoroutine (_displayer.DisplayStep (6, true));	//show dps controls
			_hackedDPS = true;
		}
		else if(_currentShip is TankShip)
			yield return StartCoroutine (_displayer.DisplayStep (7, true));	//show tank controls

		yield return WaitForLeftClick ();

		Time.timeScale = 1f;
		if (_hackedDPS)
			StartCoroutine (_displayer.DisplayStep (6, false));
		else
			StartCoroutine (_displayer.DisplayStep (7, false));

		_spawner.SpawnWave (1);

		//Wait for ennemy killed
		while(_gameManager.PlayerController.getHackPowerRatio () == 0)
		{
			yield return null;
		}

		yield return new WaitForSeconds (0.5f);
		Time.timeScale = 0f;
		GameManager.instance.MainCameraController.trauma = 0f;
		//show "kill ennemies to get hack power"
		StartCoroutine (_displayer.DisplayStep (8, true));

		yield return StartCoroutine (WaitForLeftClick ());

		Time.timeScale = 1f;

		while (_gameManager.PlayerController.getHackPowerRatio () < 0.5f)
			yield return null;

		_spawner.SpawnWave (2);

		while (_gameManager.PlayerController.getHackPowerRatio () < 1f)
			yield return null;
		yield return StartCoroutine (_displayer.DisplayStep (8, false));
		_spawner.SpawnWave (3);

		//Show right click to hack
		StartCoroutine (_displayer.DisplayStep (3, true));

		yield return StartCoroutine (WaitForRightClick ());

		//show left click to validate
		StartCoroutine (_displayer.DisplayStep (3, false));
		StartCoroutine (_displayer.DisplayStep (4, true));

		while (!_continue)
			yield return null;
		_continue = false;

		StartCoroutine (_displayer.DisplayStep (4, false));

		//Show multi info
		StartCoroutine (_displayer.DisplayStep (9, true));
		yield return new WaitForSeconds (0.2f);
		//Freeze time
		Time.timeScale = 0f;

		yield return WaitForLeftClick ();

		Time.timeScale = 1f;
		StartCoroutine (_displayer.DisplayStep (9, false));
	}

	IEnumerator WaitForLeftClick()
	{
		while(!Input.GetButtonDown ("Fire"))
		{
			yield return null;
		}
	}

	IEnumerator WaitForRightClick()
	{
		while (!Input.GetButtonDown ("Hack")) 
		{
			yield return null;
		}
	}

	void HackOccured()
	{
		_continue = true;
		_currentShip = _gameManager.PlayerController.PossessedPawn;
		print ("Now controlling a "+_currentShip.ToString ());
	}
}
