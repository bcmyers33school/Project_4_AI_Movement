using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script allows an AI to pursue another AI while remaining a specified
    /// distance away from the target
    /// </summary>
    [RequireComponent(typeof(Steering))]
    public class OffsetPursue : MonoBehaviour
    {
        /// <summary>
        /// The maximum time the pursuing AI will predict the targets future movements
        /// </summary>
        [Tooltip("The maximum time the pursuing AI will predict the targets future movements")]
        public float maxPrediction = 1f;

        AIMovement rigbod;
        Steering steering;

        void Awake()
        {
            rigbod = GetComponent<AIMovement>();
            steering = GetComponent<Steering>();
        }

        public Vector3 GetSteering(AIMovement target, Vector3 offset, out Vector3 targetPos)
        {
            var offsetPos = target.Position + target.Transform.TransformDirection(offset);
            
            // Calculates the distance to the specified point away from the target
            var dist = offsetPos - transform.position;
            var distance = dist.magnitude;
            
            var speed = rigbod.Velocity.magnitude;

            float prediction;
            if (speed <= distance / maxPrediction)
            {
                prediction = maxPrediction;
            }
            else
            {
                prediction = distance / speed;
            }

            targetPos = offsetPos + target.Velocity * prediction;

            return steering.Arrive(targetPos);
        }
    }
}