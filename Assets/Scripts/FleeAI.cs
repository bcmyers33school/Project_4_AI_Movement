using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script is used on the fleeing AI to call to the fleeing script and
    /// update the game
    /// </summary>
    public class FleeAI : MonoBehaviour
    {
        /// <summary>
        /// The AI to flee from
        /// </summary>
        [Tooltip("The AI to flee from")]
        public Transform fleeFrom;

        Steering steering;
        Flee flee;

        void Start()
        {
            steering = GetComponent<Steering>();
            flee = GetComponent<Flee>();
        }

        void FixedUpdate()
        {
            Vector3 accel = flee.GetSteering(fleeFrom.position);
            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}