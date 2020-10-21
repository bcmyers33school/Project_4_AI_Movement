using UnityEngine;

namespace AIMovement
{
    /// <summary>
    /// This class calls the wander script and updates the screen. It is for a purely
    /// wandering AI.
    /// </summary>
    public class WanderAI : MonoBehaviour
    {
        Steering steering;
        Wander wander;

        void Start()
        {
            steering = GetComponent<Steering>();
            wander = GetComponent<Wander>();
        }
        
        /// <summary>
        /// Calls the functions from the Wander script and updates the game
        /// </summary>
        void FixedUpdate()
        {
            Vector3 accel = wander.GetSteering();
            steering.Steer(accel);
            steering.LookWhereGoing();
        }
    }
}