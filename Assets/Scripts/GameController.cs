using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController = null;

	public GameObject gameOverUI;
	bool gameOver;

	void Awake(){
		if(gameController != null){
			Destroy(gameObject);
		} else {
			gameController = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start(){
		gameOverUI.SetActive(false);
	}

	void Update(){
		if (gameOver && Input.GetMouseButton(0)){
			RestartScene();
		}
	}

	public void PlayerDeath(){
		Debug.Log("Ae");
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

	public void LoadScene(int scene){
		ResetUI();
		// if (scene == 0){
		// 	menuUI.SetActive(true);
		// }
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	public void RestartScene(){
		LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void ResetUI(){
		gameOverUI.SetActive(false);
	}
}
