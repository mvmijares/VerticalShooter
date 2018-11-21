using Polarith.AI.Move;
using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// A script that attaches the player gameobject to an <see cref="AIMEnvironment"/> when a player connects to the
    /// game.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Environment Adapter")]
    public sealed class EnvironmentAdapter : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The name of player environment where the player is assigned too on StartLocalPlayer. Note, that it is better
        /// to use tags here, since we cannot do this in a plugin we have to stick to this brute force solution.
        /// </summary>
        [Tooltip("This tag is used to find the player environment on StartLocalPlayer.")]
        public string EnvironmentName = "PlayerEnvironment";

        #endregion // Fields

        /// <summary>
        /// Adds the player reference to the environment given by the <see cref="EnvironmentName"/>.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            GameObject obj = GameObject.Find(EnvironmentName);
            if (obj != null)
            {
                AIMEnvironment env = obj.GetComponent<AIMEnvironment>();
                if (env != null)
                    env.GameObjects.Add(gameObject);
                else
                    Debug.LogWarning(gameObject.name + " " + "EnvironmentAdapter: Environment does not exist.");
            }
        }
    } // class EnvironmentAdapter
} // namespace Polarith.AI.Package.Net
