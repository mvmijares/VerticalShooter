using Polarith.AI.Move;
using Polarith.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// An alternative physics controller that works only on the server. Basically the same implementation as <see
    /// cref="AIMPhysicsController2D"/>.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Physics Controller 2D")]
    public sealed class PhysicsController2D : NetworkBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// Determines the base value of the applied force for rotating the character towards the decided direction.
        /// This value is highly dependent on the <see cref="Rigidbody2D.angularDrag"/>, <see cref="Rigidbody2D.mass"/>
        /// and the <see cref="PhysicsMaterial2D"/> used by the involved <see cref="Collider2D"/> instances.
        /// <para/>
        /// For the default value, you may use a <see cref="Rigidbody2D"/> configuration with mass = 1, angular drag =
        /// 10 and a default collider material.
        /// </summary>
        [Tooltip("Determines the base value of the applied force for rotating the character towards the decided " +
            "direction. The default value of 0.05 is suitable for a rigidbody (2D) angular drag of 10, whereby the " +
            "mass = 1 and a default 'PhysicsMaterial2D' should be used.")]
        public float Torque = 0.05f;

        /// <summary>
        /// Determines the base value specifying how fast the character moves. This value is highly dependent on the
        /// <see cref="Rigidbody.drag"/>, <see cref="Rigidbody2D.mass"/> and the <see cref="PhysicsMaterial2D"/> used by
        /// the involved <see cref="Collider2D"/> instances.
        /// <para/>
        /// For the default value, you may use a <see cref="Rigidbody2D"/> configuration with mass = 1, drag = 5 and a
        /// default collider material.
        /// </summary>
        [Tooltip("Determines the base value specifying how fast the character moves. This value is highly dependent " +
            "on the 'Rigidbody2D.drag', 'Rigidbody2D.mass' and the 'PhysicsMaterial2D' used by the involved collider " +
            "instances.\n\nFor the default value, you may use a 2D rigidbody configuration with mass = 1, drag = 5 " +
            "and a default collider material.")]
        public float Speed = 10f;

        /// <summary>
        /// If set equal to or greater than 0, the evaluated AI decision value is multiplied to the <see cref="Speed"/>.
        /// </summary>
        [Tooltip("If set equal to or greater than 0, the evaluated AI decision value is multiplied to the 'Speed'.")]
        public int ObjectiveAsSpeed = -1;

        /// <summary>
        /// The <see cref="AIMContext"/> which provides the next movement direction that is applied to the <see
        /// cref="Body2D"/>.
        /// </summary>
        [Tooltip("The 'AIMContext' which provides the next movement direction that is applied to the rigidbody.")]
        public AIMContext Context;

        /// <summary>
        /// The <see cref="Rigidbody"/> which is manipulated by this controller.
        /// </summary>
        [Tooltip("The rigidbody which is manipulated by this controller.")]
        public Rigidbody2D Body2D;

        //--------------------------------------------------------------------------------------------------------------

        private Vector3 up, cross;
        private float angleDiff, velocity;

        #endregion // Fields

        #region Methods ================================================================================================

        private void OnEnable()
        {
            if (Body2D == null)
                Body2D = gameObject.GetComponentInChildren<Rigidbody2D>();
            if (Context == null)
                Context = gameObject.GetComponentInChildren<AIMContext>();

            // Disable if the setup is invalid.
            if (Body2D == null || Context == null)
                enabled = false;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            // Server only
            if (!isServer)
                return;

            // Find out the relation of the current movement direction to the decided direction
            up = transform.up;
            angleDiff = Vector3.Angle(up, Context.DecidedDirection);
            cross = Vector3.Cross(up, Context.DecidedDirection);

            // Do not let the cross.z be close to zero, otherwise, the character stuck when the decision direction is
            // anti-parallel
            if (!Mathf2.Approximately(Context.DecidedDirection.sqrMagnitude, 0f) &&
                Mathf2.Approximately(cross.z, 0f))
            {
                cross.z = Mathf.Sign(Random.Range(-1f, 1f));
            }

            // Orient towards decision direction using torque
            Body2D.AddTorque(cross.z * Torque * angleDiff);

            // Translate along oriented direction using the force (which may be with you, of course)
            if (ObjectiveAsSpeed >= 0 && ObjectiveAsSpeed < Context.DecidedValues.Count)
            {
                velocity = Context.DecidedValues[ObjectiveAsSpeed] * Speed;
                velocity = velocity > Speed ? Speed : velocity;
            }
            else
            {
                velocity = Speed;
            }

            Body2D.AddForce(velocity * up);
        }

        #endregion // Methods
    } // class PhysicsController2D
} // namespace Polarith.AI.Package.Net
