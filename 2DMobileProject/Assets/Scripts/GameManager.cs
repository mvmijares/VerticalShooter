using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public Text pHealthText;
    public Text pScoreText;
    public Text gameTime;
    public Text pPosition;
    Player player;
    EnemyManager enemyManager;

    private void Awake() {
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Update() {
        float seconds = Mathf.Floor(Time.time % 60);
        float minutes = Mathf.Floor(Time.time / 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameTime.text = "Time : " + timeString;

        Vector3 worldPosition = transform.TransformPoint(player.transform.position);
        pPosition.text = "P Pos: X :" + worldPosition.x.ToString() + " Y :" + worldPosition.y.ToString();
    }

    private void OnEnable() {
        player.OnHealthEvent += UpdatePlayerHealth;
        enemyManager.UpdatePlayerScore += UpdatePlayerScore;
    }
    private void OnDisable() {
        player.OnHealthEvent -= UpdatePlayerHealth;
        enemyManager.UpdatePlayerScore -= UpdatePlayerScore;
    }
    void UpdatePlayerScore(EnemyManager eManager) {
        pScoreText.text = "Player Score : " + eManager.score.ToString();
    }
    void UpdatePlayerHealth(Player player) {
        pHealthText.text = "Player Health : " + player.health.ToString();
    }
}