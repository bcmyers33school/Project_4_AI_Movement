using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    [RequireComponent(typeof(AIMovement))]
    public class Steering : MonoBehaviour
    {
        /// <summary>
        /// The maximum acceleration of the character
        /// </summary>
        [Tooltip("The max acceleration of the character")]
        public float maximumAcceleration = 15f;
        
        /// <summary>
        /// The length of time to get to the desired speed
        /// </summary>
        [Tooltip("The length of time to get to the desired speed")]
        public float timeToAttainTargetSpeed = 0.1f;
        
        /// <summary>
        /// How close to the desired radius does the character begin to slow down
        /// </summary>
        [Tooltip("How close to the desired radius does the character begin to slow down")]
        public float slowRadius = 1f;
        
        /// <summary>
        /// How fast the character turns
        /// </summary>
        [Tooltip("How fast the character turns")]
        public float turningSpeed = 25f;
        
        /// <summary>
        /// The maximum velocity of the character
        /// </summary>
        [Tooltip("The max velocity of the character")]
        public float maximumVelocity = 4f;

        /// <summary>
        /// How far away from the desired location is considered to be close enough
        /// </summary>
        [Tooltip("How far away from the desired radius is considered to be close enough")]
        public float targetRadius = 0.001f;

        /// <summary>
        /// Look Smoothing Controls
        /// </summary>
        [Tooltip("Whether look smoothing is on or off")]
        public bool smoothing = true;
        
        /// <summary>
        /// Number of samples for smoothing controls
        /// </summary>
        [Tooltip("Number of samples to use for look smoothing if it is on")]
        public int numSamplesForSmoothing = 5;
        
        // Gets the velocity smoothing samples
        Queue<Vector3> velocitySmoothingSamples = new Queue<Vector3>();
        
        AIMovement rigbod;
        
        /// <summary>
        /// Gets the AIMovement script and sets it to rigidbody on awake
        /// </summary>
        void Awake()
        {
            rigbod = GetComponent<AIMovement>();
        }

        /// <summary>
        /// Updates the velocity of the current game object
        /// </summary>
        public void Steer(Vector3 velocity)
        {
            rigbod.Velocity += velocity * Time.deltaTime;

            if (rigbod.Velocity.magnitude > maximumVelocity)
            {
                rigbod.Velocity = rigbod.Velocity.normalized * maximumVelocity;
            }
        }
        
        /// <summary>
        /// Makes the ai look at the direction it is going
        /// </summary>
        public void LookWhereGoing()
        {
            Vector3 direction = rigbod.Velocity;

            if (smoothing)
            {
                if (velocitySmoothingSamples.Count == numSamplesForSmoothing)
                {
                    velocitySmoothingSamples.Dequeue();
                }

                velocitySmoothingSamples.Enqueue(rigbod.Velocity);
                direction = Vector3.zero;

                foreach (Vector3 velocity in velocitySmoothingSamples)
                {
                    direction += velocity;
                }

                direction /= velocitySmoothingSamples.Count;
            }
            // Calls the LookAtDirection Function to look at the direction it is going
            LookAtDirection(direction);
        }
        
        /// <summary>
        /// This is the function to change the AI's orientation to where it is going
        /// </summary>
        /// <param name="direction"></param>
        public void LookAtDirection(Vector3 direction)
        {
            direction.Normalize();
            
            if (direction.sqrMagnitude > 0.001f)
            {
                var toRotation = -1 * (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
                var rotation = Mathf.LerpAngle(rigbod.Rotation.eulerAngles.y, toRotation, Time.deltaTime * turningSpeed);
                rigbod.Rotation = Quaternion.Euler(0, rotation, 0);
            }
        }

        /// <summary>
        /// The rotation to look at in the direction it is going
        /// </summary>
        /// <param name="toRotation"></param>
        public void LookAtDirection(Quaternion toRotation)
        {
            LookAtDirection(toRotation.eulerAngles.y);
        }
        
        /// <summary>
        /// Float of rotation to look at
        /// </summary>
        /// <param name="toRotation"></param>
        public void LookAtDirection(float toRotation)
        {
            float rotation = Mathf.LerpAngle(rigbod.Rotation.eulerAngles.y, toRotation, Time.deltaTime * turningSpeed);
            rigbod.Rotation = Quaternion.Euler(0, rotation, 0);

        }
        
        /// <summary>
        /// Makes sure the AI is facing the fleeFrom
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cosineValue"></param>
        /// <returns></returns>
        public bool IsFacing(Vector3 target, float cosineValue)
        {
            Vector3 facing = transform.right.normalized;
            Vector3 directionToTarget = (target - transform.position);
            directionToTarget.Normalize();

            return Vector3.Dot(facing, directionToTarget) >= cosineValue;
        }
        /// <summary>
        /// Returns the acceleration to reach the current game objects desired position
        /// </summary>
        public Vector3 Seek(Vector3 targetPosition, float maxAccel)
        {
            Vector3 accel = rigbod.ConvertVector(targetPosition - transform.position);
            accel.Normalize();
            accel *= maxAccel;

            return accel;
        }

        /// <summary>
        /// Seeks the targets position
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public Vector3 Seek(Vector3 targetPosition)
        {
            return Seek(targetPosition, maximumAcceleration);
        }

        /// <summary>
        /// Arrival position of the AI
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public Vector3 Arrive(Vector3 targetPosition)
        {
            targetPosition = rigbod.ConvertVector(targetPosition);
            Vector3 targetAcceleration = targetPosition - rigbod.Position;
            float distance = targetAcceleration.magnitude;

            if (distance < targetRadius)
            {
                rigbod.Velocity = Vector3.zero;
                return Vector3.zero;
            }

            float targetSpeed;
            if (distance > slowRadius)
            {
                targetSpeed = maximumVelocity;
            }
            else
            {
                targetSpeed = maximumVelocity * (distance / slowRadius);
            }

            targetAcceleration.Normalize();
            targetAcceleration *= targetSpeed;
            Vector3 acceleration = targetAcceleration - rigbod.Velocity;
            acceleration *= 1 / timeToAttainTargetSpeed;

            if (acceleration.magnitude > maximumAcceleration)
            {
                acceleration.Normalize();
                acceleration *= maximumAcceleration;
            }
            return acceleration;
        }
    }
}