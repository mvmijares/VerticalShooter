using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnDefinition {
    public GameObject enemy; // enemy Prefab
    public int health; // enemy health;
    public int numEnemy; // number of enemies
    public int bulletDamage;
    public int score; //value to give to player when enemy dies
    public float spawnTime; // when to spawn enemies
    public float spawnDelay; //delay between each enemy spawn
    public float speed; //enemy speed;
    public bool canHover;
    public float hoverTime;
    public float hoverDistance; // Distance in between points to hover.
}
//Class that deals with spawning and passing values for enemy information.
[System.Serializable]
public class EnemySpawn : MonoBehaviour {

    public SpawnDefinition spawnDefinition;
   
    bool canSpawn;
    bool spawn;
    float currSpawnTime;
    int enemiesSpawned;

    EditorPath path; 

    private EnemyManager enemyManager;

    private void Start() {
        path = GetComponent<EditorPath>();
        enemiesSpawned = 0;
        spawn = true;
        enemyManager = FindObjectOfType<EnemyManager>();

        if (!spawnDefinition.canHover)
            spawnDefinition.hoverTime = 0.0f;
    }

    void Update() {
        if (spawnDefinition != null) {
            if(Time.time > spawnDefinition.spawnTime) {
                if(enemiesSpawned < spawnDefinition.numEnemy)
                    SpawnEnemy();
            }
        }
    }
    //Spawning Enemies
    void SpawnEnemy() {
        if (spawn) {
            GameObject clone = Instantiate(spawnDefinition.enemy, path.path[0].position, spawnDefinition.enemy.transform.rotation);
            Enemy enemy = clone.GetComponent<Enemy>();

            //Event handler for manager
            enemyManager.NewEnemySpawned(CreateSpawnDefinitions(enemy));

            enemiesSpawned++;
            spawn = false;
        }
        if (!spawn) {
            currSpawnTime += Time.deltaTime;
            if(currSpawnTime >= spawnDefinition.spawnDelay) {
                spawn = true;
                currSpawnTime = 0.0f;
            }
        }
    }
    //Information for Enemy
    Enemy CreateSpawnDefinitions(Enemy enemy) {
  
        enemy.speed = spawnDefinition.speed;
        enemy.score = 100;
        enemy.canHover = spawnDefinition.canHover;
        enemy.SetPath(path.path);

        if (spawnDefinition.canHover) {
            enemy.hoverTime = spawnDefinition.hoverTime;
        }
        return enemy;
    }
   
}
