using Polarith.AI.Move;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// Spawns enemies at given positions in a given interval. The number of active enemies at the same time is limited.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Enemy Spawner")]
    public sealed class EnemySpawner : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The template for the spawned object.
        /// </summary>
        [Tooltip("The template for the spawned object.")]
        public GameObject Spawnable;

        /// <summary>
        /// Limits how many objects can be active at the same time.
        /// </summary>
        [Tooltip("Limits how many objects can be active at the same time.")]
        public int MaximumEnemies = 7;

        /// <summary>
        /// Contains possible spawn points.
        /// </summary>
        [Tooltip("Contains possible spawn points.")]
        public List<GameObject> SpawnPoints = new List<GameObject>();

        /// <summary>
        /// Spawned objects are added to this environment such that Polarith.AI can perceiver the objects.
        /// </summary>
        [Tooltip("Spawned objects are added to this environment such that Polarith.AI can perceiver the objects.")]
        public AIMEnvironment Environment;

        /// <summary>
        /// A possible empty object which acts as the spawned objects parent. This is used to organize the objects.
        /// </summary>
        [Tooltip("A possible empty object which acts as the spawned objects parent. This is used to organize the " +
            "objects.")]
        public GameObject EnemyParent;

        /// <summary>
        /// The tag of the <see cref="EnemyParent"/>, if not assigned, the tag is used to find the object at start.
        /// </summary>
        [Tooltip("The tag of the EnemyParent, if not assigned the, tag is used to find the object at start.")]
        public string EnemyParentTag = "EnemyPool";

        /// <summary>
        /// The spawn delay in seconds.
        /// </summary>
        [Tooltip("The spawn delay in seconds.")]
        public float Delay = 0.5f;

        //--------------------------------------------------------------------------------------------------------------

        private float currentTime = 0.0f;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            if (EnemyParent == null)
                EnemyParent = GameObject.FindWithTag(EnemyParentTag);
            UpdateEnvironment();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            UpdateEnvironment();

            // Only spawn on server
            if (!isServer)
                return;

            // Check if the SpawnPoints list is valid
            if (SpawnPoints.Count == 0)
            {
                Debug.LogError(gameObject.name + ", " + "EnemySpawner: SpawnPoints need at least one entry.");
                return;
            }

            // If the time has come, spawn the object
            if (currentTime >= Delay)
            {
                if (Environment.GameObjects.Count < MaximumEnemies)
                {
                    int randomIndex = (int)Random.Range(0.0f, SpawnPoints.Count);
                    if (randomIndex > SpawnPoints.Count - 1)
                        randomIndex = SpawnPoints.Count - 1;

                    GameObject enemy = (GameObject)Instantiate(
                        Spawnable,
                        SpawnPoints[randomIndex].transform.position,
                        Quaternion.identity);

                    enemy.transform.parent = EnemyParent.transform;

                    NetworkServer.Spawn(enemy);
                }

                currentTime = 0.0f;
            }

            currentTime += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void UpdateEnvironment()
        {
            // Quick and dirty update of the environment references.
            Environment.GameObjects.Clear();
            for (int i = 0; i < EnemyParent.transform.childCount; i++)
                Environment.GameObjects.Add(EnemyParent.transform.GetChild(i).gameObject);
        }

        #endregion // Methods
    } // class EnemySpawner
} // namespace Polarith.AI.Package.Net
