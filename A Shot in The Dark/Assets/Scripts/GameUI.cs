using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public int levelToLoad;
	public GameObject allUI;
	public Image fadePlane;
	public GameObject gameOverUI;
    public GameObject youWinUI;

    public RectTransform newWaveBanner;
    public RectTransform healthBar;
    public Text newWaveTitle;
	public Text newWaveEnemyCount;
	public Text scoreUI;
    public Text BestScoreUI;
	public Text gameOverScoreUI;
    public Text youWinScoreUI;
    

	Spawner spawner;
	Player player;

	void Start () {
		allUI.SetActive (true);
		player = FindObjectOfType<Player> ();
		player.OnDeath += OnGameOver;
        BestScoreUI.text = "Best " + SaveDataHandler.saveDataHandler.highscoreLevel[Application.loadedLevel].ToString("D6");
    }

	void Awake() {
		spawner = FindObjectOfType<Spawner> ();
		spawner.OnNewWave += OnNewWave;
	}

	void Update() {
		scoreUI.text = ScoreKeeper.score.ToString("D6");

		float healthPercent = 0;
		if (player != null) {
			healthPercent = player.health / player.startingHealth;
		}
		healthBar.localScale = new Vector3 (healthPercent, 1, 1);


	}

	void OnNewWave(int waveNumber) {
		string[] numbers = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifthteen", "Sixteen", "Seventeen", "Eightteen", "Nineteen", "Twenty", "Twentyone", "Twentytwo", "Twentythree", "Twentyfour", "Twentyfive" };
		newWaveTitle.text = "- Wave " + numbers [waveNumber - 1] + " -";
		string enemyCountString = ((spawner.waves [waveNumber - 1].infinite) ? "Infinite" : spawner.waves [waveNumber - 1].enemyCount + "");
		newWaveEnemyCount.text = "Enemies: " + enemyCountString;

		StopCoroutine ("AnimateNewWaveBanner");
		StartCoroutine ("AnimateNewWaveBanner");
	}
		
	void OnGameOver() {
        AudioManager.instance.PlaySound2D("Game Over");
        RecordHighScore();
        
        Cursor.visible = true;
		StartCoroutine(Fade (Color.clear, new Color(0,0,0,.95f),1));
		gameOverScoreUI.text = scoreUI.text;
		scoreUI.gameObject.SetActive (false);
        //Invoke("StopTheMusic", .2f);
        healthBar.transform.parent.gameObject.SetActive (false);
        gameOverUI.SetActive (true);
    }

   public void YouWon() {
        AudioManager.instance.PlaySound2D("Victory");
        RecordHighScore();

        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, .95f), 1));
        youWinScoreUI.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        //Invoke("StopTheMusic", .2f);
        healthBar.transform.parent.gameObject.SetActive(false);
        youWinUI.SetActive(true);
    }

	IEnumerator AnimateNewWaveBanner() {

		float delayTime = 1.5f;
		float speed = 3f;
		float animatePercent = 0;
		int dir = 1;

		float endDelayTime = Time.time + 1 / speed + delayTime;

		while (animatePercent >= 0) {
			animatePercent += Time.deltaTime * speed * dir;

			if (animatePercent >= 1) {
				animatePercent = 1;
				if (Time.time > endDelayTime) {
					dir = -1;
				}
			}

			newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp (-170, 45, animatePercent);
			yield return null;
		}

	}
		
	IEnumerator Fade(Color from, Color to, float time) {
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp(from,to,percent);
			yield return null;
		}
	}

	// UI Input
	public void StartNewGame() {
        //SceneManager.LoadScene ("Game");
        SceneManager.LoadScene(levelToLoad);
    }

	public void ReturnToMainMenu() {
		SceneManager.LoadScene ("Menu");
	}

    public void RecordHighScore()
    {

        if (SaveDataHandler.saveDataHandler.highscoreLevel[Application.loadedLevel] < ScoreKeeper.score)
        {
            SaveDataHandler.saveDataHandler.highscoreLevel[Application.loadedLevel] = ScoreKeeper.score;
            SaveDataHandler.saveDataHandler.Save();
        }
    }
}
