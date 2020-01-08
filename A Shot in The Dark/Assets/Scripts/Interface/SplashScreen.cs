using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (CountDown());
	}

	private IEnumerator CountDown()
	{
		yield return new WaitForSeconds (5);
		//Application.LoadLevel (1);
		SceneManager.LoadScene(1);

	}

}
