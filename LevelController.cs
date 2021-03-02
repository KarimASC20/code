using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public void Lvl1(){
		SceneManager.LoadScene("Lvl_1");
	}
	public void Lvl2(){
		SceneManager.LoadScene("Lvl_2");
	}
	public void Lvl3(){
		SceneManager.LoadScene("Lvl_3");
	}
	public void BackToMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}