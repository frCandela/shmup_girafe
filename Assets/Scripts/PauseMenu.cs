using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	void Update()
	{
		//Press escape to go to menu
		if (Input.GetKeyDown (KeyCode.Escape))
			BackToMenu ();
	}

	public void BackToMenu()
	{
		
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Title");
	}
}
