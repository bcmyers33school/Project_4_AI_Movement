using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    /// <summary>
    /// This script is to align the flocking AI's in a certain direction and can
    /// be used with other scripts for more complicated movement patterns or just
    /// the AlignmentAI script.
    /// </summary>
    [RequireComponent(typeof(AIMovement))]
    public class Alignment : MonoBehaviour
    {
        AIMovement rigbod;

        void Awake()
        {
            rigbod = GetComponent<AIMovement>();
        }

        public Vector3 GetSteering(ICollection<AIMovement> targets)
        {
            Vector3 accel = Vector3.zero;
            
            foreach (AIMovement ai in targets)
            {
                // Gets the direction to the fleeFrom
                var dir = rigbod.ColliderPosition - ai.ColliderPosition;
                // Distance to fleeFrom
                var colPos = rigbod.ColliderPosition.magnitude;
                float length = dir.magnitude;
                if (colPos < length)
                {
                    dir = rigbod.ConvertVector(dir);
                    dir.Normalize();
                    accel = dir;
                }
                else
                {
                   accel = Vector3.zero;
                }
            }
            return accel;
        }
    }
}