using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script sets the AI to separation based movements by calling to the
    /// functions in the Separation script
    /// </summary>
    public class SeparationAI : MonoBehaviour
    {
        /// <summary>
        /// The weight to be used in the separation calculations
        /// </summary>
        [Tooltip("The weight to be used in the separation calculations")]
        public float separationWeight = 2f;
        
        /// <summary>
        /// The weight to be used in the velocity matching calculations
        /// </summary>
        [Tooltip("The weight to be used in the velocity matching calculations")]
        public float velocityMatchWeight = 1f;

        Steering steering;
        Wander wander;
        Separation separation;
        VelocityMatching velocityMatching;
        Sensor sensor;

        void Start()
        {
            steering = GetComponent<Steering>();
            wander = GetComponent<Wander>();
            separation = GetComponent<Separation>();
            velocityMatching = GetComponent<VelocityMatching>();

            sensor = transform.Find("Sensor").GetComponent<Sensor>();
        }

        void FixedUpdate()
        {
            Vector3 accel = Vector3.zero;
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