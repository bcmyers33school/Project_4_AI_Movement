using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    /// <summary>
    /// This class sets the minimum amount of distance between each AI in the group
    /// </summary>
    [RequireComponent(typeof(AIMovement))]
    public class Separation : MonoBehaviour
    {
        /// <summary>
        /// The maximum acceleration for separation
        /// </summary>
        [Tooltip(" The maximum acceleration for separation")]
        public float maxSeparationAcceleration = 25;

        /// <summary>
        /// Minimum amount of space between each AI in the group
        /// </summary>
        [Tooltip("Minimum amount of space between each AI in the group")]
        public float minimumSeparationDistance = 1f;

        AIMovement rigbod;

        void Awake()
        {
            rigbod = GetComponent<AIMovement>();
        }

        public Vector3 GetSteering(ICollection<AIMovement> targets)
        {
            Vector3 acceleration = Vector3.zero;

            foreach (AIMovement ai in targets)
            {
                // Direction and distance to each ai in the group
                Vector3 direction = rigbod.ColliderPosition - ai.ColliderPosition;
                float dist = direction.magnitude;

                if (dist < minimumSeparationDistance)
                {
                    // The strength to increase acceleration by
                    var strength = maxSeparationAcceleration * (minimumSeparationDistance - dist) / (minimumSeparationDistance - rigbod.Radius - ai.Radius);
                    
                    // Converts the distance vector into a usable format
                    direction = rigbod.ConvertVector(direction);
                    direction.Normalize();
                    acceleration = direction * strength;
                }
                else
                {
                    acceleration = Vector3.zero;
                }
            }
            return acceleration;
        }
    }
}