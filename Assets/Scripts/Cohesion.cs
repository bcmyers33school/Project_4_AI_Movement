using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    /// <summary>
    /// This script is to make the AI group together based on weight.
    /// </summary>
    [RequireComponent(typeof(Steering))]
    public class Cohesion : MonoBehaviour
    {
        public float cosine = 120f;

        float cosineValue;

        Steering steering;

        void Awake()
        {
            cosineValue = Mathf.Cos(cosine * Mathf.Deg2Rad);
            steering = GetComponent<Steering>();
        }

        public Vector3 GetSteering(ICollection<AIMovement> targets)
        {
            Vector3 centerMass = Vector3.zero;
            int count = 0;

            // Sum of positions of AI in front of the current AI 
            foreach (AIMovement AI in targets)
            {
                /*
                 * Insures that only the positions of the AI in front of where the
                 * current AI is facing are used in the calculation
                 */
                if (steering.IsFacing(AI.Position, cosineValue))
                {
                    centerMass += AI.Position;
                    count++;
                }
            }

            if (count == 0)
            {
                return Vector3.zero;
            }
            else
            {
                centerMass /= count;
                return steering.Arrive(centerMass);
            }
        }
    }
}