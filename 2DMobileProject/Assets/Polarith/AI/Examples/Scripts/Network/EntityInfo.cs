using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// Contains information about the Hitpoints of an entity and how it should behave on death. The actual hitpoints
    /// are synced.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Entity Info")]
    public sealed class EntityInfo : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The maximum and thus initial hitpoints.
        /// </summary>
        [Tooltip("The maximum and thus initial hitpoints.")]
        public float MaxHitpoints = 3f;

        /// <summary>
        /// If <c>true</c>, the objects respawns at a random position instead of being destroyed.
        /// </summary>
        [Tooltip("If true, the objects respawns at a random position instead of being destroyed.")]
        public bool RespawnOnDeath;

        /// <summary>
        /// A list of possible spawnpoints.
        /// </summary>
        public List<Vector3> SpawnPoints = new List<Vector3>();

        //--------------------------------------------------------------------------------------------------------------

        [SyncVar]
        private float hitpoints = 3f;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Get the current hitpoints.
        /// </summary>
        public float CurrentHitpoints
        {
            get { return hitpoints; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        /// <summary>
        /// Applies the given amount of damage to the current hitpoints. If the hitpoints drop below 0, the entity dies.
        /// </summary>
        /// <param name="amount">This amount is substracted from the <see cref="CurrentHitpoints"/>.</param>
        public void TakeDamge(float amount)
        {
            // Server only
            if (!isServer)
                return;

            hitpoints -= amount;

            // Check death condition
            if (hitpoints <= 0.0f)
            {
                if (RespawnOnDeath)
                    RpcRespawn();
                else
                    Destroy(gameObject);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [ClientRpc]
        private void RpcRespawn()
        {
            if (isLocalPlayer)
            {
                // Set random position
                int randomIndex = (int)Random.Range(0.0f, SpawnPoints.Count);
                if (randomIndex > SpawnPoints.Count - 1)
                    randomIndex = SpawnPoints.Count - 1;
                transform.position = SpawnPoints[randomIndex];

                hitpoints = MaxHitpoints;
            }
        }

        #endregion // Methods
    } //  class EntityInfo
} // namespace Polarith.AI.Package.Net
