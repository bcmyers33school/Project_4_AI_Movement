using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script is for an alignment movement type AI and calls the functions
    /// from the Alignment script. It also uses the Wander script for randomness in the
    /// movements. VelocityMatching script is used to match the velocities of other AI
    /// in the group for a consistent grouping pattern. The sensor script is used
    /// to sense the other AI in the group.
    /// </summary>
    public class AlignmentAI : MonoBehaviour
    {
        float separationWeight = 2f;
        public float velocityMatchWeight = 1f;

        AIMovement group;
        Steering steering;
        Wander wander;
        VelocityMatching velocityMatching;
        Sensor sensor;
        Alignment direction;

        void Start()
        {
            steering = GetComponent<Steering>();
            wander = GetComponent<Wander>();
            velocityMatching = GetComponent<VelocityMatching>();
            direction = GetComponent<Alignment>();
            sensor = transform.Find("Sensor").GetComponent<Sensor>();
        }

        void FixedUpdate()
        {
            Vector3 accel = Vector3.zero;
            accel += direction.GetSteering(sensor.targets);
            accel += velocityMatching.GetSteering(sensor.targets) * velocityMatchWeight;

            if (accel.magnitude < 0.05f)
            {
                accel = wander.GetSteering();
            }
            
            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}