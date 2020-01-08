using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public AudioClip splashScreenTheme;
    public AudioClip menuTheme;
    public AudioClip arcadeTheme;
    public AudioClip endlessNightsTheme;
    public AudioClip pitchBlackTheme;
    public AudioClip aLongShotTheme;
    public AudioClip streetSweepTheme;


    string sceneName;

	void Start() {
		OnLevelWasLoaded (0);
	}


	void OnLevelWasLoaded(int sceneIndex) {
		string newSceneName = SceneManager.GetActiveScene ().name;
		if (newSceneName != sceneName) {
			sceneName = newSceneName;
			Invoke ("PlayMusic", .2f);
		}
	}

	public void PlayMusic() {
		AudioClip clipToPlay = null;

		if (sceneName == "Menu") {
			clipToPlay = menuTheme;
		} else if (sceneName == "Game") {
			clipToPlay = arcadeTheme;
		} else if (sceneName == "EndlessNights")
        {
            clipToPlay = endlessNightsTheme;
        } else if (sceneName == "PitchBlack")
        {
            clipToPlay = pitchBlackTheme;
        } else if (sceneName == "ALongShot")
        {
            clipToPlay = aLongShotTheme;
        } else if (sceneName == "StreetSweep")
        {
            clipToPlay = streetSweepTheme;
        }
        else if (sceneName == "SplashScreen")
        {
            clipToPlay = splashScreenTheme;
        }

        if (clipToPlay != null) {
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
	}

    public void StopTheMusic()
    {
        AudioClip clipToStop = null;

        if (sceneName == "Menu")
        {
            clipToStop = menuTheme;
        }
        else if (sceneName == "Game")
        {
            clipToStop = arcadeTheme;
        }
        else if (sceneName == "EndlessNights")
        {
            clipToStop = endlessNightsTheme;
        }
        else if (sceneName == "PitchBlack")
        {
            clipToStop = pitchBlackTheme;
        }
        else if (sceneName == "ALongShot")
        {
            clipToStop = aLongShotTheme;
        }
        else if (sceneName == "StreetSweep")
        {
            clipToStop = streetSweepTheme;
        }
        else if (sceneName == "SplashScreen")
        {
            clipToStop = splashScreenTheme;
        }

        if (clipToStop != null)
        {
            AudioManager.instance.StopMusic(clipToStop, 2);
            Invoke("StopMusic", clipToStop.length);
        }
    }

}
