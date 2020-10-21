using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    /// <summary>
    /// This script allows the AI in a group to match velocities to create a uniform group
    /// </summary>
    [RequireComponent(typeof(Steering))]
    public class VelocityMatching : MonoBehaviour
    {
        /// <summary>
        /// The facing rotation angle to be used in calculations
        /// </summary>
        [Tooltip("The facing rotation angle to be used in calculations")]
        public float cosine = 90;
        
        /// <summary>
        /// The max acceleration speed
        /// </summary>
        [Tooltip("The max acceleration speed")]
        public float maxAcceleration = 6f;

        /// <summary>
        /// The time it should take to reach the desired speeds so it's not instantaneous
        /// </summary>
        float timeToDesiredSpeed = 0.15f;
        
        /// <summary>
        /// The facing rotation value
        /// </summary>
        float cosineValue;

        AIMovement rigbod;
        Steering steering;

        void Awake()
        {
            // Converts the cosine value into radians for use in calculations
            cosineValue = Mathf.Cos(cosine * Mathf.Deg2Rad);
            
            rigbod = GetComponent<AIMovement>();
            steering = GetComponent<Steering>();
        }

        public Vector3 GetSteering(ICollection<AIMovement> targets)
        {
            Vector3 accel = Vector3.zero;
            int count = 0;

            foreach (AIMovement ai in targets)
            {
                if (steering.IsFacing(ai.Position, cosineValue))
                {
                    Vector3 a = ai.Velocity - rigbod.Velocity;
                    a /= timeToDesiredSpeed;
                    accel += a;
                    count++;
                }
            }

            if (count > 0)
            {
                accel /= count;

                if (accel.magnitude > maxAcceleration)
                {
                    accel = accel.normalized * maxAcceleration;
                }
            }
            return accel;
        }
    }
}