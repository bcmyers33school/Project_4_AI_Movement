using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script is assigned to AI with specifically cohesion based movements
    /// and calls to the Cohesion script. It uses a sensor to detect the AI in the
    /// group and to determine calculations.
    /// </summary>
    public class CohesionAI : MonoBehaviour
    {
        /// <summary>
        /// The weight of the AI cohesion movement calculations
        /// </summary>
        [Tooltip("The weight of the AI cohesion movement calculations")]
        public float cohesionWeight = 1f;
        
        /// <summary>
        /// The weight of the AI separation movement calculations
        /// </summary>
        [Tooltip("The weight of the AI separation movement calculations")]
        public float separationWeight = 2f;
        
        /// <summary>
        /// The wieght of the AI to be used in velocity based movement calculations
        /// </summary>
        [Tooltip("The wieght of the AI to be used in velocity based movement calculations")]
        public float velocityMatchWeight = 1f;

        Steering steering;
        Wander wander;
        Cohesion cohesion;
        Separation separation;
        VelocityMatching velocityMatching;
        Sensor sensor;

        void Start()
        {
            steering = GetComponent<Steering>();
            wander = GetComponent<Wander>();
            cohesion = GetComponent<Cohesion>();
            separation = GetComponent<Separation>();
            velocityMatching = GetComponent<VelocityMatching>();
            sensor = transform.Find("Sensor").GetComponent<Sensor>();
        }

        void FixedUpdate()
        {
            Vector3 accel = Vector3.zero;
            accel += cohesion.GetSteering(sensor.targets) * cohesionWeight;
            accel += separation.GetSteering(sensor.targets) * separationWeight;
            accel += velocityMatching.GetSteering(sensor.targets) * velocityMatchWeight;

            if (accel.magnitude < 0.005f)
            {
                accel = wander.GetSteering();
            }

            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}