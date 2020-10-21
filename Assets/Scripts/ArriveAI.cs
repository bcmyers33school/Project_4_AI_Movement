using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script allows for AI to go to a specific location in game.
    /// </summary>
    public class ArriveAI : MonoBehaviour
    {

        /// <summary>
        /// Position for the AI to go to
        /// </summary>
        [Tooltip("Position for the AI to go to")]
        public Vector3 targetPosition;

        Steering steering;

        void Start()
        {
            steering = GetComponent<Steering>();
        }

        void FixedUpdate()
        {
            // Calls to the Steering script for the arrive function.
            Vector3 accel = steering.Arrive(targetPosition);

            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}