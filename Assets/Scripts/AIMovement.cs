using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace AIMovement
{
    /// <summary>
    /// The main class used in all the scripts, it is a wrapper class
    /// </summary>
    public class AIMovement : MonoBehaviour
    {
        // Initializes the game objects collider
        SphereCollider col;
        
        [System.NonSerialized]
        public Vector3 movementPlane = Vector3.up;

        Rigidbody rigBod;

        /// <summary>
        /// Returns the radius of the current game objects collider
        /// </summary>
        public float Radius
        {
            get
            {
                return Mathf.Max(rigBod.transform.localScale.x, rigBod.transform.localScale.y, rigBod.transform.localScale.z) * col.radius;
            }
        }


        /// <summary>
        /// Calls the setup function on awake to find the current objects collider
        /// and rigidbody.
        /// </summary>
        void Awake()
        {
            SetUp();
        }

        /// <summary>
        /// Sets up the AIMovement script so that it knows its underlying collider and rigidbody
        /// </summary>
        public void SetUp()
        {
            SetUpRigidbody();
            SetUpCollider();
        }

        /// <summary>
        /// Gets the current objects rigidbody and assigns it
        /// </summary>
        void SetUpRigidbody()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                this.rigBod = rb;
            }
        }

        /// <summary>
        /// Gets the current objects collider and assigns it to the object
        /// </summary>
        void SetUpCollider()
        {
            SphereCollider col = rigBod.GetComponent<SphereCollider>();
            
            if (col != null)
            { 
                this.col = col;
            }

        }

        /// <summary>
        /// Gets the position of the current game object to be used
        /// </summary>
        public Vector3 Position
        {
            get
            { 
                return new Vector3(rigBod.position.x, 0, rigBod.position.z);
            }
        }
        
        /// <summary>
        /// Gets the position of the current game objects collider
        /// </summary>
        public Vector3 ColliderPosition
        {
            get
            {
                return Transform.TransformPoint(col.center) + rigBod.position - Transform.position;
            }
        }
        
        /// <summary>
        /// Transforms the current game objects rotation
        /// </summary>
        public Quaternion Rotation
        {
            // Gets the rotation of the rigidbody
            get
            {
                return rigBod.rotation;
            }

            // Sets the new rotation of the rigidbody
            set
            {
                rigBod.MoveRotation(value);
            }
        }
        
        /// <summary>
        /// Returns the velocity to be used in the script
        /// </summary>
        public Vector3 Velocity
        {
            // Gets the velocity
            get
            {
                Vector3 velocity = rigBod.velocity;
                velocity.y = 0;
                float magnitude = Vector3.ProjectOnPlane(rigBod.velocity, movementPlane).magnitude;
                return velocity.normalized * magnitude;
            }

            // Sets the velocity
            set
            {
                value.y = rigBod.velocity.y;
                rigBod.velocity = value;
                
            }
        }

        /// <summary>
        /// Transforms the current game object
        /// </summary>
        public Transform Transform
        {
            get
            {
                return rigBod.transform;
            }
        }

        /// <summary>
        /// Converts the vector of the game object to exclude the Y component
        /// </summary>
        public Vector3 ConvertVector(Vector3 v)
        {
            return v;
        }
        
        /// <summary>
        /// calls the fixedupdate function
        /// </summary>
        void Start()
        {
            FixedUpdate();
        }
        
        void FixedUpdate()
        {

        }
    }
}