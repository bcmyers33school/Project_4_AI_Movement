using UnityEngine;
using System.Collections.Generic;

namespace AIMovement
{
    /// <summary>
    /// This script allows the AI to sense other AI's. It works by placing an empty child
    /// object on the AI, adding this script to the empty child object, and adding a mesh
    /// collider to the child object. The mesh collider does not physically interact
    /// with anything, and instead is basically a field that can sense objects inside of it.
    /// </summary>
    public class Sensor : MonoBehaviour
    {
        HashSet<AIMovement> _targets = new HashSet<AIMovement>();

        public HashSet<AIMovement> targets
        {
            get
            {
                _targets.RemoveWhere(IsNull);
                return _targets;
            }
        }

        void TryToAdd(Component other)
        {
            AIMovement rigbod = other.GetComponent<AIMovement>();
            if (rigbod != null)
            {
                _targets.Add(rigbod);
            }
        }

        void TryToRemove(Component other)
        {
            AIMovement rigbod = other.GetComponent<AIMovement>();
            if (rigbod != null)
            {
                _targets.Remove(rigbod);
            }
        }

        void OnTriggerExit(Collider other)
        {
            TryToRemove(other);
        }
        
        void OnTriggerEnter(Collider other)
        {
            TryToAdd(other);
        }
        
        static bool IsNull(AIMovement ai)
        {
            return (ai == null || ai.Equals(null));
        }

    }
}