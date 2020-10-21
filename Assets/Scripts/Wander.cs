using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This class allows the AI to move in random directions and is the basis of 
    /// any good game AI character. This script can be used with other scripts for
    /// more complex movement AI.
    /// </summary>
    [RequireComponent(typeof(Steering))]
    public class Wander : MonoBehaviour
    {
        /// <summary>
        /// AI direction radius
        /// </summary>
        [Tooltip("AI direction radius")]
        public float wanderingRadius = 1.0f;

        /// <summary>
        /// Distance to wander before changing direction
        /// </summary>
        [Tooltip("Distance to wander before changing direction")]
        public float wanderingDistance = 2.5f;
        
        /// <summary>
        /// Max random displacement per second
        /// </summary>
        [Tooltip("Max random displacement per second")]
        public float randomDisplacement = 40f;

        Vector3 randomPositionTarget;
        Steering steering;

        void Awake()
        {
            steering = GetComponent<Steering>();
        }

        void Start()
        {
            var theta = Random.value * 2 * Mathf.PI;
            randomPositionTarget = new Vector3(wanderingRadius * Mathf.Cos(theta), 0f, wanderingRadius * Mathf.Sin(theta));
        }

        /// <summary>
        /// Gets the random directions for the ai to move
        /// </summary>
        /// <returns>targetPosition</returns>
        public Vector3 GetSteering()
        {
            // AI random displacement for each time frame
            var randomness = randomDisplacement * Time.deltaTime;
            // Adds a random vector for random movements, this is what makes it more AI like
            randomPositionTarget += new Vector3(Random.Range(-1f, 1f) * randomness, 0f, Random.Range(-1f, 1f) * randomness);
            randomPositionTarget.Normalize();
            randomPositionTarget *= wanderingRadius;
            Vector3 targetPosition = transform.position + transform.right * wanderingDistance + randomPositionTarget;
            
            // Returns the random new position for the AI to move to
            return steering.Seek(targetPosition);
        }

    }
}