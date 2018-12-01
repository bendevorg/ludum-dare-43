using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public GameObject gameOverUI;
	public GameObject levelClearedUI;
	bool gameOver;
	bool levelCleared;

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
			}
		}
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
		levelCleared = true;
		levelClearedUI.SetActive(true);
	}

	void NextLevel() {
		levelCleared = false;
		LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LoadScene(int scene){
		ResetUI();
		// if (scene == 0){
		// 	menuUI.SetActive(true);
		// }
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	public void RestartScene(){
		gameOver = false;
		LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void ResetUI(){
		gameOverUI.SetActive(false);
		levelClearedUI.SetActive(false);
	}
}
