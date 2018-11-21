using Polarith.AI.Move;
using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// A simple tool that makes a server only AI setup possible. The idea is that the game object with the attached AI
    /// components (AIMContext and AIMBehaviours) spawns the actual agent (controller, visual representation, game
    /// logic). Then it moves itself to the position of the agent to have its perspective on the AI world.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/AI Sync")]
    [RequireComponent(typeof(AIMContext))]
    public sealed class AISync : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// A template for the agent entity (controller, visual representation, game logic) that is spawned on start.
        /// </summary>
        [Tooltip("A template for the agent entity (controller, visual representation, game logic) that is spawned " +
            "on start.")]
        public GameObject Entity;

        //--------------------------------------------------------------------------------------------------------------

        private GameObject spawnedEntity;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            // Spawn the entity
            spawnedEntity = (GameObject)Instantiate(Entity, transform.position, Quaternion.identity);
            NetworkServer.Spawn(spawnedEntity);

            AIMContext context = GetComponent<AIMContext>();

            // If you want to use this script for you own examples, you have to replace this controller with your own. 
            PhysicsController2D controller = spawnedEntity.GetComponent<PhysicsController2D>();
            controller.Context = context;
            controller.enabled = true;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (!isServer)
                return;

            // Destroy this object if the spawned entity is destroyed too
            if (spawnedEntity == null)
            {
                Destroy(gameObject);
                return;
            }

            // Sync the positions
            transform.position = spawnedEntity.transform.position;
            // This is sufficient for our examples, maybe you also need to sync the rotation and scale.
        }

        #endregion // Methods
    } // class AISync
} // namespace Polarith.AI.Package.Net
