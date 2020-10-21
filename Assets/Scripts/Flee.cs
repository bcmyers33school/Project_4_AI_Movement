using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script sets an AI to flee from another specified AI
    /// </summary>
    [RequireComponent(typeof(AIMovement))]
    public class Flee : MonoBehaviour
    {
        /// <summary>
        /// Distance from fleeFrom when the AI begins to flee
        /// </summary>
        [Tooltip("Distance from fleeFrom when the AI begins to flee")]
        public float panicDist = 6f;
        
        /// <summary>
        /// The fleeing AI's max speed
        /// </summary>
        public float maxSpeed = 15f;
        
        AIMovement rigbod;

        void Awake()
        {
            rigbod = GetComponent<AIMovement>();
        }

        public Vector3 GetSteering(Vector3 targetAIPosition)
        {
            Vector3 accel = transform.position - targetAIPosition;

            /*
             * This statement makes the fleeing AI stop if he is a specified distance
             * away from the pursuing AI and returns a speed of 0
            */
            if (accel.magnitude > panicDist)
            {
                rigbod.Velocity = Vector3.zero;
                return Vector3.zero;
            }

            return GiveMaxAccel(accel);
        }

        /// <summary>
        /// Returns the max speed for the fleeing AI
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        Vector3 GiveMaxAccel(Vector3 speed)
        {
            speed.Normalize();

            /* Accelerate to the fleeFrom */
            speed *= maxSpeed;

            return speed;
        }
    }
}