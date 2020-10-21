using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This script calls to the OffsetPursue
    /// </summary>
    public class OffsetPursueAI : MonoBehaviour
    {
        /// <summary>
        /// Target to pursue
        /// </summary>
        [Tooltip("Target to pursue")]
        public AIMovement target;

        /// <summary>
        /// Distance to stay away from target
        /// </summary>
        [Tooltip("Distance to stay away from target")]
        public Vector3 offsetDistance;
        
        /// <summary>
        /// Distance to look
        /// </summary>
        [Tooltip("Distance to look")]
        public float lookDistance = 2f;

        Steering steering;
        OffsetPursue offsetPursue;
        Separation separation;
        Sensor sensor;

        void Start()
        {
            steering = GetComponent<Steering>();
            offsetPursue = GetComponent<OffsetPursue>();
            separation = GetComponent<Separation>();
            sensor = transform.Find("Sensor").GetComponent<Sensor>();
        }

        void LateUpdate()
        {
            Vector3 targetPos;
            Vector3 offsetAccel = offsetPursue.GetSteering(target, offsetDistance, out targetPos);
            Vector3 seperationAccel = separation.GetSteering(sensor.targets);
            steering.Steer(offsetAccel + seperationAccel);

            if (Vector3.Distance(transform.position, targetPos) > lookDistance)
            {
                steering.LookWhereGoing();
            }
            else
            {
                steering.LookAtDirection(target.Rotation);
            }
        }
    }
}