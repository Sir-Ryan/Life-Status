using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

	public void Emotions() 
	{
		SceneManager.LoadScene("Emotions");
	}

	public void Activities()
	{
		SceneManager.LoadScene("Activities");
	}

	public void Back()
	{
		SceneManager.LoadScene("Menu");
	}
}
