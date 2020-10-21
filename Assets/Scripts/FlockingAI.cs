using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script combines cohesion, separation, and alignment to create a flocking
    /// movement behaviour in the AI group
    /// </summary>
    public class FlockingAI : MonoBehaviour
    {
        /// <summary>
        /// The weight of AI cohesion
        /// </summary>
        [Tooltip("The weight of AI cohesion")]
        public float cohesionWeight = 1.5f;
        
        /// <summary>
        /// The weight of AI separation
        /// </summary>
        [Tooltip("The weight of AI separation")]
        public float separationWeight = 2f;
        
        /// <summary>
        /// The weight of AI velocity matching
        /// </summary>
        [Tooltip("The weight of AI velocity matching")]
        public float velocityMatchWeight = 1f;

        Steering steering;
        Wander wander;
        Cohesion cohesion;
        Separation separation;
        VelocityMatching velocityMatching;
        Alignment direction;
        Sensor sensor;

        void Start()
        {
            steering = GetComponent<Steering>();
            wander = GetComponent<Wander>();
            cohesion = GetComponent<Cohesion>();
            separation = GetComponent<Separation>();
            velocityMatching = GetComponent<VelocityMatching>();
            direction = GetComponent<Alignment>();
            sensor = transform.Find("Sensor").GetComponent<Sensor>();
        }

        void FixedUpdate()
        {
            Vector3 accel = Vector3.zero;

            accel += separation.GetSteering(sensor.targets) * separationWeight;
            if (accel != Vector3.zero)
            {
                
                accel += cohesion.GetSteering(sensor.targets) * cohesionWeight;
            }
            accel += velocityMatching.GetSteering(sensor.targets) * 
            velocityMatchWeight;

            accel += direction.GetSteering(sensor.targets);
            
            if (accel.magnitude < 0.005f)
            {
                accel = wander.GetSteering();
            }

            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}