using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

	public bool devMode;

	public Wave[] waves;
	public Enemy enemy;

	LivingEntity playerEntity;
	Transform playerT;
    public ParticleSystem playerSpawnEffect;
    public ParticleSystem enemySpawnEffect;

    Wave currentWave;
	int currentWaveNumber;

	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;

	MapGenerator map;
    GameUI gameUI;

    [Tooltip("How much time should we check inbetween updates if the player is camping or not.")]
    public float timeBetweenCampingChecks = 2;
    [Tooltip("The distance between the player's old position and current position before the player is considered camping.")]
    public float campThresholdDistance = 1.5f; 
	float nextCampCheckTime;
	Vector3 campPositionOld;
	bool isCamping;

	bool isDisabled;

	public event System.Action<int> OnNewWave;

	void Start() {
		playerEntity = FindObjectOfType<Player> ();//The player reference
		playerT = playerEntity.transform; //The player's transform reference

		nextCampCheckTime = timeBetweenCampingChecks + Time.time;
		campPositionOld = playerT.position;
		playerEntity.OnDeath += OnPlayerDeath;

		map = FindObjectOfType<MapGenerator> ();
        gameUI = FindObjectOfType<GameUI>();
		NextWave ();
	}

	void Update() {
		if (!isDisabled) {
			if (Time.time > nextCampCheckTime) {
				nextCampCheckTime = Time.time + timeBetweenCampingChecks; //Check how long the player has been in one place

				isCamping = (Vector3.Distance (playerT.position, campPositionOld) < campThresholdDistance); 
				campPositionOld = playerT.position; //Set the camping position to the player's current position
			}

			if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime) {
				enemiesRemainingToSpawn--;
				nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

				StartCoroutine ("SpawnEnemy");
			}
           
		}

		if (devMode) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				StopCoroutine("SpawnEnemy");
				foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
					GameObject.Destroy(enemy.gameObject);
				}
				NextWave();
			}
		}
	}

	IEnumerator SpawnEnemy() {
		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform spawnTile = map.GetRandomOpenTile ();
		if (isCamping) { //If the player is camping in one spot
			spawnTile = map.GetTileFromPosition(playerT.position); //Spawn an enemy on the player's position
		}
		Material tileMat = spawnTile.GetComponent<Renderer> ().material;
		Color initialColour = Color.white;
		Color flashColour = Color.red;
        AudioManager.instance.PlaySound("Enemy Spawn", transform.position);
        Destroy(Instantiate(enemySpawnEffect.gameObject, spawnTile.position + Vector3.up, Quaternion.identity) as GameObject, 2);
        float spawnTimer = 0;

		while (spawnTimer < spawnDelay) {

			tileMat.color = Color.Lerp(initialColour,flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

			spawnTimer += Time.deltaTime;
			yield return null;
		}

        
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;

        spawnedEnemy.OnDeath += OnEnemyDeath;
		spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
	}

	void OnPlayerDeath() {
		isDisabled = true;
	}

	void OnEnemyDeath() {
		enemiesRemainingAlive --;

		if (enemiesRemainingAlive == 0) {
			NextWave();
		}
	}

	void ResetPlayerPosition() {
        playerT.position = map.GetTileFromPosition (Vector3.zero).position + Vector3.up * 3;
        AudioManager.instance.PlaySound("Player Spawn", transform.position);
        Destroy(Instantiate(playerSpawnEffect.gameObject, playerT.position, Quaternion.identity) as GameObject, 2);

    }

	void NextWave() {
		if (currentWaveNumber > 0) {
			AudioManager.instance.PlaySound2D ("Level Complete");
		}
		currentWaveNumber ++;

		if (currentWaveNumber - 1 < waves.Length) { //If the current wave number is less than the max number of waves
			currentWave = waves [currentWaveNumber - 1]; //set the current wave to the new wave number

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;

			if (OnNewWave != null) {
				OnNewWave(currentWaveNumber);
			}
			ResetPlayerPosition();
		}

        if (currentWaveNumber - 1 == waves.Length && enemiesRemainingAlive == 0) //If we're on the last wave and there are no more enemies
        {
            //The player has won the game
            gameUI.YouWon();
            //SceneManager.LoadScene("Menu");
        }




    }

	[System.Serializable]
	public class Wave {
		public bool infinite;
		public int enemyCount;
		public float timeBetweenSpawns;

		public float moveSpeed;
		public int hitsToKillPlayer;
		public float enemyHealth;
		public Color skinColour;
	}

}
