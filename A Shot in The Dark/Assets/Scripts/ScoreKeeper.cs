using UnityEngine;
using System.Collections;


public class ScoreKeeper : MonoBehaviour {

	public static int score { get; private set; }

    public static int points { get; private set; }

    [Tooltip("Add points to score earned by distance the enemy was killed from.")]
    public bool enableDistanceScoring; 

	float lastEnemyKillTime;
	int streakCount;
	float streakExpiryTime = 1;

    void Start() {
        FloatingScoreController.Initialize();
        Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<Player> ().OnDeath += OnPlayerDeath;
	}

	void OnEnemyKilled() {
		if (Time.time < lastEnemyKillTime + streakExpiryTime) {
            streakCount++;
        } else {
			streakCount = 0;
		}

		lastEnemyKillTime = Time.time;

          if (enableDistanceScoring)
        {
            CalculateScoreDistance();
        }
        else
        {
            points = 3 + 2 * streakCount;
            score += points;
            //score += 3 + 2 * streakCount;
            //Pop up the streak count where the player is
            if (streakCount >= 1)
            {
                FloatingScoreController.CreateFloatingScoreText("x" + streakCount.ToString(), GameObject.FindGameObjectWithTag("Player").transform);

            }
            //Pop up the floating score where the enemy was killed
            FloatingScoreController.CreateFloatingScoreText("+" + points.ToString(), GameObject.FindGameObjectWithTag("Enemy").transform);
        }
	}

	void OnPlayerDeath() {

		Enemy.OnDeathStatic -= OnEnemyKilled;
	}

    void CalculateScoreDistance()
{
        //Calculate how much points the player gets based on the distance the enemy was killed
        float dist = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, GameObject.FindGameObjectWithTag("Enemy").transform.position);
        //print(dist);
        int distRound = Mathf.CeilToInt(dist); //The distance score rounded to an int
        points = (3 + 2 * streakCount) + distRound;
        score += points;
        //score += (3 + 2 * streakCount) + distRound;
        /*Streak count*/
        if (streakCount >= 1)
        {
            FloatingScoreController.CreateFloatingScoreText("x" + streakCount.ToString(), GameObject.FindGameObjectWithTag("Player").transform);

        }
        /*points scored*/
        FloatingScoreController.CreateFloatingScoreText("+" + points.ToString(), GameObject.FindGameObjectWithTag("Enemy").transform); 
    }

}
