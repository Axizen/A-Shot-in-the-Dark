using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
    public GameObject gameModeMenuHolder;
    public GameObject creditsMenuHolder;

    public Slider[] volumeSliders;
	public Toggle[] resolutionToggles;
	public Toggle fullscreenToggle;
	public int[] screenWidths;
	int activeScreenResIndex;

	void Start() {
		activeScreenResIndex = PlayerPrefs.GetInt ("screen res index");
		bool isFullscreen = (PlayerPrefs.GetInt ("fullscreen") == 1)?true:false;

		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.sfxVolumePercent;

		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].isOn = i == activeScreenResIndex;
		}

		fullscreenToggle.isOn = isFullscreen;
	}


	public void Arcade() {
		SceneManager.LoadScene ("Game"); 
	}

    public void EndlessNights()
    {
        SceneManager.LoadScene("EndlessNights");
    }

    public void PitchBlack()
    {
        SceneManager.LoadScene("PitchBlack");
    }

    public void ALongShot()
    {
        SceneManager.LoadScene("ALongShot");
    }

    public void StreetSweep()
    {
        SceneManager.LoadScene("StreetSweep");
    }

    public void Quit() {
		Application.Quit ();
	}

	/*public void WatchSeries() {
		Application.OpenURL ("https://youtu.be/SviIeTt2_Lc?list=PLFt_AvWsXl0ctd4dgE1F8g3uec4zKNRV0");
	}*/

	public void OptionsMenu() //Show the Options Menu
    { 
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

    public void GameModeMenu() //Show the game modes
    {
        mainMenuHolder.SetActive(false);
        gameModeMenuHolder.SetActive(true);
    }

    public void CreditsMenu() //Show the credits
    {
        mainMenuHolder.SetActive(false);
        creditsMenuHolder.SetActive(true);
    }

    public void GoBack() //From Game Mode
    {
        mainMenuHolder.SetActive(true);
        gameModeMenuHolder.SetActive(false);
    }

    public void GoingBack() //From Credits
    {
        mainMenuHolder.SetActive(true);
        creditsMenuHolder.SetActive(false);
    }

    public void MainMenu() {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}

	public void SetScreenResolution(int i) {
		if (resolutionToggles [i].isOn) {
			activeScreenResIndex = i;
			float aspectRatio = 16 / 9f;
			Screen.SetResolution (screenWidths [i], (int)(screenWidths [i] / aspectRatio), false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
	}

	public void SetFullscreen(bool isFullscreen) {
		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].interactable = !isFullscreen;
		}

		if (isFullscreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		} else {
			SetScreenResolution (activeScreenResIndex);
		}

		PlayerPrefs.SetInt ("fullscreen", ((isFullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}

	public void SetMasterVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSfxVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);
	}

}
