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
	[SerializeField] private GameObject _timerDisplay;

	private GameManager _gameManager;
	private TimeManager _timeManager;
	private Pawn _currentShip = null;

	private bool _continue = false;
	private bool _hackedDPS = false;
	private bool _wasClicked = false;
	[HideInInspector]public bool _hacking = false;
	[HideInInspector] public bool _waitingForHack = false;
	[HideInInspector] public bool _spawnAgain = false;

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
		_timerDisplay.SetActive (false);

		_gameManager.PlayerController.virusHackRefillSpeed = 0f;

		//Start Tutorial
		StartCoroutine (Tutorial ());
	}

	void StartGame()
	{
		_timeManager.enabled = true;
		_gameManager.GetComponent<UnityEngine.Playables.PlayableDirector> ().enabled = true;
		_gameManager.PlayerController.virusHackRefillSpeed = 20f;

		//show leaderboard
		_leaderboardDisplay.GetComponent<Text> ().enabled = true;
		_scorePopup.SetActive (true);
		_scoreDisplay.SetActive (true);
		_uI.SetActive (true);
		_timerDisplay.SetActive (true);

		Time.timeScale = 1f;

		gameObject.SetActive (false);
	}

	void Update()
	{
		//Press escape to skip tuto
		if (Input.GetKeyDown (KeyCode.Return))
			StartGame ();

		if(_hacking)
		{
			Ship targetShip = _gameManager.PlayerController.getTargetHack ();
			if(!targetShip)
			{
				_displayer.ToggleInfo (6, false);
				_displayer.ToggleInfo (7, false);
			}
			else
			{
				if(targetShip is DPSShip) _displayer.ToggleInfo (6, true);
				else if (targetShip is TankShip) _displayer.ToggleInfo (7, true);
			}
		}
	}

	IEnumerator Tutorial()
	{
		//Show intro
		StartCoroutine (_displayer.DisplayStep (0, true));
		yield return StartCoroutine (WaitForLeftClick ());


		//Hide intro
		yield return StartCoroutine (_displayer.DisplayStep (0, false));

		yield return new WaitForSeconds (0.2f);

		//show movement
		StartCoroutine (_displayer.DisplayStep (1, true));
	
		yield return StartCoroutine (WaitForLeftClick ());

		//Hide movement
		yield return StartCoroutine (_displayer.DisplayStep (1, false));

		yield return new WaitForSeconds (0.2f);

		//Show UI bar
		_uI.SetActive (true);

		//show info hack power
		StartCoroutine (_displayer.DisplayStep (2, true));

		//Fill up hack power
		_gameManager.PlayerController.virusHackRefillSpeed = 20f;

		yield return StartCoroutine (WaitForLeftClick (5f));
		//yield return new WaitForSeconds (5f);

		//StartCoroutine (_displayer.DisplayStep (2, false));	//hide hack power info

		while (_gameManager.PlayerController.getHackPowerRatio () < 0.99f)
			yield return null;


		//Spawn first wave of ennemies
		_spawner.SpawnWave (0, false);

		if(!_wasClicked)
		{
			yield return StartCoroutine (WaitForClick ());
			StartCoroutine (_displayer.DisplayStep (2, false));	//hide hack power info
		}

		//Wait for hack to start
		yield return StartCoroutine (WaitForRightClick ());
		_hacking = true;
		_waitingForHack = false;

		while (_hacking)
			yield return null;

		//Freeze time
		Time.timeScale = 0f;

		//Show hack results
		yield return StartCoroutine (_displayer.DisplayStep (5, true));

		yield return WaitForLeftClick ();

		yield return StartCoroutine (_displayer.DisplayStep (5, false));	//hide hack results
		int shipHacked = (_currentShip is DPSShip) ? 10 : 11;
		yield return StartCoroutine (_displayer.DisplayStep (shipHacked, true));	//show hacked ship controls

		yield return WaitForLeftClick ();

		StartCoroutine (_displayer.DisplayStep (shipHacked, false));	//hide ship controls

		Time.timeScale = 1f;

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
		yield return StartCoroutine (_displayer.DisplayStep (8, false));

		Time.timeScale = 1f;

		while (_gameManager.PlayerController.getHackPowerRatio () < 0.5f)
			yield return null;

		if (_currentShip is DPSShip)
			_spawner.SpawnWave (1, false);	//Spawn DPS
		else if (_currentShip is TankShip)
			_spawner.SpawnWave (2, false);	//spawn tank

		while (_gameManager.PlayerController.getHackPowerRatio () < 1f)
			yield return null;
		int index = (_currentShip is DPSShip) ? 2 : 1;
		_spawner.SpawnWave (index, true);

		//Show right click to hack
		_waitingForHack = true;

		//Spawn the ennemy again if killed and not hacked
		//yield return null;
		_spawnAgain = true;

		yield return StartCoroutine (WaitForRightClick ());
		_waitingForHack = false;

		//show left click to validate
		_hacking = true;

		while (_hacking)
			yield return null;

		_spawnAgain = false;	//no need to spawn again

		//Show multi info + score
		StartCoroutine (_displayer.DisplayStep (9, true));
		_scoreDisplay.SetActive (true);
		_scorePopup.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		//Freeze time
		Time.timeScale = 0f;

		yield return WaitForLeftClick ();

		yield return StartCoroutine (_displayer.DisplayStep (9, false));	//hide hack results
		shipHacked = (_currentShip is DPSShip) ? 10 : 11;
		yield return StartCoroutine (_displayer.DisplayStep (shipHacked, true));	//show hacked ship controls

		yield return WaitForLeftClick ();

		StartCoroutine (_displayer.DisplayStep (shipHacked, false));	//hide ship controls

		yield return WaitForLeftClick ();

		Time.timeScale = 1f;

		_spawner.SpawnWave (3, false);

		yield return new WaitForSeconds (5f);

		_spawner.SpawnWave (4, false);
		
		while (!(_gameManager.PlayerController.PossessedPawn is Virus))
			yield return null;

		StartCoroutine (_displayer.DisplayStep (12, true));

		_gameManager.PlayerController.virusHackRefillSpeed = 0f;
		_gameManager.ResetGameState ();

		yield return StartCoroutine (WaitForLeftClick ());

		StartCoroutine (_displayer.DisplayStep (12, false));

		yield return new WaitForSeconds (1f);

		StartGame ();
	}

	IEnumerator WaitForLeftClick()
	{
		while(!Input.GetButtonDown ("Fire"))
		{
			yield return null;
		}
	}
		
	IEnumerator WaitForLeftClick(float timer)
	{
		float elapsedTime = 0f;
		while(elapsedTime < timer)
		{
			elapsedTime += Time.unscaledDeltaTime;
			if (Input.GetButtonDown ("Fire"))
			{
				StartCoroutine (_displayer.DisplayStep (2, false));	//hide hack power info
				_wasClicked = true;
				break;
			}	
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

	IEnumerator WaitForClick()
	{
		while (!Input.GetButtonDown ("Hack") && !Input.GetButtonDown ("Fire")) 
		{
			yield return null;
		}
	}

	void HackOccured()
	{
		//_continue = true;
		_hacking = false;
		_displayer.ToggleInfo (6, false);
		_displayer.ToggleInfo (7, false);
		_currentShip = _gameManager.PlayerController.PossessedPawn;
	}
}
