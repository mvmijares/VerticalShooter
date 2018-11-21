using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// A simple controller for the enemies cannon. It decides which player to target and when to shoot. Furthermore,
    /// you can add some randomness to the enemies aiming skills.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Enemy Cannon Controller")]
    public sealed class EnemyCannonController : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// A template for the projectile that is being spawned when the cannon shoots.
        /// </summary>
        [Tooltip("A template for the projectile that is being spawned when the cannon shoots.")]
        public GameObject BulletPrefab;

        /// <summary>
        /// The tag of the player objects. All objects with this tag are potential targets.
        /// </summary>
        [Tooltip("The tag of the player objects. All objects with this tag are potential targets.")]
        public string PlayerTag = "Player";

        /// <summary>
        /// The range of the weapon, it is only fired if the target is closer than this distance.
        /// </summary>
        [Tooltip("The range of the weapon, it is only fired if the target is closer than this distance.")]
        public float MaxDistance = 50.0f;

        /// <summary>
        /// Determines how fast the weapon can reload.
        /// </summary>
        [Tooltip("Determines how fast the weapon can reload.")]
        public float Interval = 0.5f;

        /// <summary>
        /// Influences the frequency of when the weapon is fired. E.g. a value of 1 means that the enemy may wait up to
        /// 1 additional second before it fires again.
        /// </summary>
        [Tooltip("Influences the frequency of when the weapon is fired. E.g. a value of 1 means that the enemy may " +
            "wait up to 1 additional second before it fires again.")]
        public float Randomness;

        /// <summary>
        /// A random angle offset is applied to the perfect shot direction between 0 and spread. 0 = perfect aim 180 =
        /// pure random. (Spread is in degrees)
        /// </summary>
        [Tooltip("A random angle offset is applied to the perfect shot direction between 0 and spread. 0 = perfect " +
            "aim 180 = pure random. (Spread is in degrees)")]
        public float Spread = 0.0f;

        /// <summary>
        /// The traveling speed of the spawned bullet.
        /// </summary>
        [Tooltip("The traveling speed of the spawned bullet.")]
        public float BulletSpeed = 10.0f;

        //--------------------------------------------------------------------------------------------------------------

        private GameObject[] players;

        private float currentTime;
        private float delay;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Update()
        {
            // The server decides when a bullet is fired
            if (isClient && !isServer)
                return;

            // Find the players
            // A quick and dirty method of finding all active player. There are better ways of doing this.
            players = GameObject.FindGameObjectsWithTag(PlayerTag);

            if (players == null || players.Length == 0)
                return;

            // Determine the closest player
            int closest = 0;
            float closestMag = (players[closest].transform.position - transform.position).sqrMagnitude;
            float magnitude = 0.0f;
            for (int i = 1; i < players.Length; i++)
            {
                magnitude = (players[i].transform.position - transform.position).sqrMagnitude;
                if (magnitude < closestMag)
                {
                    closest = i;
                    closestMag = magnitude;
                }
            }

            // If the weapon is reloaded -> fire
            if (currentTime >= Interval + delay)
            {
                // Get target direction and check distance
                Vector3 dir = players[closest].transform.position - transform.position;
                if (dir.sqrMagnitude > MaxDistance * MaxDistance)
                    return;
                dir = Quaternion.Euler(0.0f, 0.0f, Random.value * Spread * 2.0f - Spread) * dir;
                dir.Normalize();

                // Spawn the bullet
                GameObject bullet = (GameObject)Instantiate(BulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = dir * BulletSpeed;
                NetworkServer.Spawn(bullet);
                Destroy(bullet, 4.0f);

                // Set values for next action
                delay = Random.value * Randomness;
                currentTime = 0.0f;
            }

            currentTime += Time.deltaTime;
        }

        #endregion // Methods
    } // class EnemyCannonController
} // namespace Polarith.AI.Package.Net
