using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

    public Text scoreText;
    public int score;
    public event Action<EnemyManager> UpdatePlayerScore;

    private void Update() {
        if (UpdatePlayerScore != null)
            UpdatePlayerScore(this);
    }
    public void NewEnemySpawned(Enemy enemy) {
        enemy.OnDeathEvent += OnEnemyDeath;
    }

    void OnEnemyDeath(Enemy enemy) {
        score += enemy.score;
       

        enemy.OnDeathEvent -= OnEnemyDeath;
    }
}
