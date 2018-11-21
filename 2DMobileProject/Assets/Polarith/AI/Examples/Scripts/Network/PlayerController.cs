using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// A networked version of a simple player controller. With this you can move, shoot and boost.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Player Controller")]
    public sealed class PlayerController : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The maximum speed at which the player can travel.
        /// </summary>
        [Tooltip("The maximum speed at which the player can travel.")]
        public float MaxSpeed = 3.0f;

        /// <summary>
        /// The accelartion used to reach the target velocity.
        /// </summary>
        [Tooltip("The accelartion used to reach the target velocity.")]
        public float Acceleration = 1.0f;

        /// <summary>
        /// The maximum available torque.
        /// </summary>
        [Tooltip("The maximum available torque.")]
        public float Torque = 2.0f;

        /// <summary>
        /// The multiplier for the boost effect. It is applied to both MaxSpeed and Acceleration.
        /// </summary>
        [Tooltip("The multiplier for the boost effect. It is applied to both MaxSpeed and Acceleration.")]
        public float BoostMultiplier = 2.0f;

        /// <summary>
        /// The duration of the boost effect in seconds.
        /// </summary>
        [Tooltip("The duration of the boost effect in seconds.")]
        public float BoostDuration = 0.5f;

        /// <summary>
        /// The cooldown in seconds for the boost effect.
        [Tooltip("The cooldown in seconds for the boost effect.")]
        public float BoostCooldown = 3.0f;

        /// <summary>
        /// The key for moving up.
        /// </summary>
        [Tooltip("The key for moving up.")]
        public KeyCode MoveUp = KeyCode.W;

        /// <summary>
        /// The key for moving down.
        /// </summary>
        [Tooltip("The key for moving down.")]
        public KeyCode MoveDown = KeyCode.S;

        /// <summary>
        /// The key for moving left.
        /// </summary>
        [Tooltip("The key for moving left.")]
        public KeyCode MoveLeft = KeyCode.A;

        /// <summary>
        /// The key for moving right.
        /// </summary>
        [Tooltip("The key for moving right.")]
        public KeyCode MoveRight = KeyCode.D;

        /// <summary>
        /// The key for activating the boost mode.
        /// </summary>
        [Tooltip("The key for activating the boost mode.")]
        public KeyCode Boost = KeyCode.Space;

        /// <summary>
        /// A reference to the gameobject representing the local player. E.g. a Sprite or a Mesh.
        /// </summary>
        [Tooltip("A reference to the gameobject representing the local player. E.g. a Sprite or a Mesh.")]
        public GameObject SelfVis;

        /// <summary>
        /// A reference to the gameobject representing the other players who joined the game. E.g. a Sprite or a Mesh.
        /// </summary>
        [Tooltip("A reference to the gameobject representing the other players who joined the game. E.g. a Sprite or " +
            "a Mesh.")]
        public GameObject OtherVis;

        /// <summary>
        /// The template for the bullets the player can shoot.
        /// </summary>
        [Tooltip("The template for the bullets the player can shoot.")]
        public GameObject BulletPrefab;

        /// <summary>
        /// The velocity of the bullets.
        /// </summary>
        [Tooltip("The velocity of the bullets.")]
        public float BulletSpeed = 10.0f;

        //--------------------------------------------------------------------------------------------------------------

        private Rigidbody2D body;
        private Vector3 position;

        private float boostTimer;
        private float boost = 1.0f;
        private float fireDelayTime = 0.0f;
        private int acc;
        private int strafe;
        private bool isBoost;

        #endregion // Fields

        #region Methods ================================================================================================

        /// <summary>
        /// Activates the <see cref="SelfVis"/> object and deactivates the <see cref="OtherVis"/> object. Such that the
        /// local player has the right visual representation.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            if (SelfVis != null)
                SelfVis.SetActive(true);
            if (OtherVis != null)
                OtherVis.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            // Pre-cache the rigidbody
            body = GetComponent<Rigidbody2D>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            // Only update for the local playyer
            if (!isLocalPlayer)
                return;

            // Fire
            if (Input.GetMouseButton(0) && fireDelayTime >= 0.2f)
            {
                // Get the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane xy = new Plane(Vector3.forward, new Vector3(0.0f, 0.0f, 0.0f));
                float distance;
                xy.Raycast(ray, out distance);
                position = ray.GetPoint(distance);

                // Shoot the bullet
                CmdFire((position - transform.position).normalized);
                fireDelayTime = 0.0f;
            }

            // Thruster control
            if (Input.GetKey(MoveUp))
                acc = 1;
            else if (Input.GetKey(MoveDown))
                acc = -1;
            else if (!Input.GetKey(MoveUp) && !Input.GetKey(MoveDown))
                acc = 0;

            // Strafe control
            if (Input.GetKey(MoveLeft))
                strafe = 1;
            else if (Input.GetKey(MoveRight))
                strafe = -1;
            else if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight))
                strafe = 0;

            // Boost
            if (Input.GetKey(Boost))
            {
                if (!isBoost && boostTimer >= BoostCooldown)
                {
                    boost = BoostMultiplier;
                    isBoost = true;
                    boostTimer = 0.0f;
                }
            }
            if (isBoost)
            {
                if (boostTimer >= BoostDuration)
                {
                    isBoost = false;
                    boost = 1.0f;
                    boostTimer = 0.0f;
                }
            }

            // Update cooldown timers
            boostTimer += Time.deltaTime;
            fireDelayTime += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            // Apply Force and Torque to rigidbody
            if (acc > 0)
                body.AddForce(Vector3.up * Acceleration * boost);

            if (acc < 0)
                body.AddForce(Vector3.up * -Acceleration * boost);

            if (strafe > 0)
                body.AddForce(Vector3.right * -Acceleration * boost);

            if (strafe < 0)
                body.AddForce(Vector3.right * Acceleration * boost);

            // Limit velocity
            float speed = body.velocity.magnitude;
            if (speed >= MaxSpeed * boost)
                body.velocity *= (MaxSpeed * boost) / speed;

            // Rotate to mouse
            Vector3 diff = position - transform.position;
            diff.Normalize();
            float angle = Vector3.Angle(transform.up, diff);
            if (angle >= 10.0f)
            {
                Vector3 tmpPos = Quaternion.Inverse(transform.rotation) * transform.position;
                Vector3 tmpPos2 = Quaternion.Inverse(transform.rotation) * position;

                if (tmpPos.x > tmpPos2.x)
                    body.AddTorque(angle * Torque);
                else
                    body.AddTorque(-angle * Torque);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        [Command]
        private void CmdFire(Vector3 dir)
        {
            GameObject bullet = (GameObject)Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            bullet.GetComponent<Rigidbody2D>().velocity = dir * BulletSpeed;

            NetworkServer.Spawn(bullet);

            Destroy(bullet, 4.0f);
        }

        #endregion // Methods
    } // class PlayerController
} // namespace Polarith.AI.Package.Net
