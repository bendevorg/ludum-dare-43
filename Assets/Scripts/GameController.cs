using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public GameObject gameOverUI;
	public GameObject levelClearedUI;
	public GameObject congratulationsUI;
	public Text scoreTextUI;

	int ropesLost;
	int segmentsLost;
	int segmentsLostAtCurrentLevel;

	bool gameOver;
	bool levelCleared;
	bool gameCleared;

	void Awake(){
		if(gameController != null){
			Destroy(gameObject);
		} else {
			gameController = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start(){
		ResetUI();
	}

	void Update(){
		if (Input.GetMouseButton(0)){
			if (gameOver) {
				RestartScene();
			}	else if (levelCleared) {
				NextLevel();
			} else if (gameCleared) { 
				ResetGame();
			}
		}
	}

	void LateUpdate() {
		scoreTextUI.text = "You left " + segmentsLost + " mindus behind";
	}

	public void AddSegmentsLost(int _segmentsLost) {
		segmentsLost += _segmentsLost;
		segmentsLostAtCurrentLevel += _segmentsLost;
		ropesLost++;
	}

	public void PlayerDeath(){
		GameOver();
	}

	void GameOver(){
		gameOver = true;
		gameOverUI.SetActive(true);
	}

	void PauseGame(){
		Time.timeScale = 1 - Time.timeScale;
		//	pauseUI.SetActive(!pauseUI.activeSelf);
	}

	public void LevelCleared() {
			Time.timeScale = 0;
		if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings) {
			gameCleared = true;
			congratulationsUI.SetActive(true);
		} else {
			levelCleared = true;
			levelClearedUI.SetActive(true);
		}
	}

	void NextLevel() {
		levelCleared = false;
		segmentsLostAtCurrentLevel = 0;
		LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	void LoadScene(int scene){
		ResetUI();
		// if (scene == 0){
		// 	menuUI.SetActive(true);
		// }
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	void RestartScene(){
		gameOver = false;
		segmentsLost -= segmentsLostAtCurrentLevel;
		segmentsLostAtCurrentLevel = 0;
		LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void ResetGame() {
		gameCleared = false;
		segmentsLost = 0;
		segmentsLostAtCurrentLevel = 0;
		LoadScene(0);
	}

	void ResetUI(){
		gameOverUI.SetActive(false);
		levelClearedUI.SetActive(false);
		congratulationsUI.SetActive(false);
	}
}
