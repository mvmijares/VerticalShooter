using System.Collections.Generic;
using UnityEngine;

namespace Polarith.AI.Package.Net
{
    /// <summary>
    /// Add damage to the <see cref="EntityInfo"/> of the other collider. It also spawns nice particle effects.
    /// <para/>
    /// Note, this is just a script used for our example scenes and therefore not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Examples/Network/Contact Damage")]
    public sealed class ContactDamage : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The amount of damage that applied to the other <see cref="EntityInfo"/>. A positive value means damage, a
        /// negative value would heal the other entity.
        /// </summary>
        [Tooltip("The amount of damage that applied to the othe EntityInfo. A positive value means damage, a " +
            "negative value would heal the other entity.")]
        public float Amount;

        /// <summary>
        /// Only other colliders with this material are considered as valid targets. Using a material instead of the
        /// collision matrix is a workaround. We have to do this because we cannot work with Tags and Layers for a
        /// package project.
        /// </summary>
        [Tooltip("Only other colliders with this material are considered as valid targets. Using a material instead " +
            "of the collision matrix is a workaround. We have to do this because we cannot work with Tags and Layers " +
            "for a package project.")]
        public List<PhysicsMaterial2D> TargetMaterials;

        /// <summary>
        /// A template that is spawned when the collider hit something, e.g. a particle system.
        /// </summary>
        [Tooltip("A template that is spawned when the collider hit something, e.g. a particle system.")]
        public GameObject HitParticles;

        /// <summary>
        /// A template that is spawned when the other <see cref="EntityInfo"/> is destroyed, e.g. a particle system.
        /// </summary>
        [Tooltip("A template that is spawned when the other EntityInfo is destroyed, e.g. a particle system.")]
        public GameObject Deathparticles;

        #endregion // Fields

        #region Methods ================================================================================================

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Layer Check
            if (TargetMaterials.Contains(collision.sharedMaterial))
            {
                Collider2D triggerObject = collision;

                // Get and process the EntityInfo if available
                EntityInfo info = triggerObject.GetComponent<EntityInfo>();
                if (info != null)
                {
                    if (info.CurrentHitpoints - Amount <= 0f)
                    {
                        if (Deathparticles != null)
                        {
                            GameObject particles = Instantiate(Deathparticles);
                            particles.transform.position = transform.position;
                        }
                    }
                    info.TakeDamge(Amount);
                }

                // Spawn HitParticles
                if (HitParticles != null)
                {
                    GameObject particles = Instantiate(HitParticles);
                    particles.transform.position = transform.position;
                }

                // Destroy the projectile
                Destroy(gameObject);
            }
        }

        #endregion // Methods
    } // class ContactDamage
} // namespace Polarith.AI.Package.Net
