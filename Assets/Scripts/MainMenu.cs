﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene(1);
	}

	public void Options(){
		Debug.Log("NO OPTIONS MATE");
	}

	public void ExitGame(){
		Application.Quit();
	}
}