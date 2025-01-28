using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class AI_car : MonoBehaviour
{
    [Header("Path Following")]
    public Transform path;
    public float maxSteerAngle = 45f;
    public float waypointThreshold = 1f;

    [Header("Driving Settings")]
    public float acceleration = 300f;
    public float maxSpeed = 50f;
    public float brakeForce = 500f;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public float sideSensorOffset = 0.5f;
    public float sensorAngle = 30f;
    public Vector3 sensorOffset = new Vector3(0f, 0.5f, 1f);

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;

    [Header("Wheel Transforms")]
    public Transform frontLeftTransform, frontRightTransform, rearLeftTransform, rearRightTransform;

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private bool isAvoiding = false;
    private bool isBlocked = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Initialize waypoints from the path
        waypoints = new List<Transform>();
        foreach (Transform t in path.GetComponentsInChildren<Transform>())
        {
            if (t != path.transform)
                waypoints.Add(t);
        }
    }

    private void FixedUpdate()
    {
        SensorCheck();
       
            ApplyBrakes();
       
            ApplySteering();
            Drive();
            CheckWaypoint();
        
        UpdateWheels();
    }

    private void SensorCheck()
    {
        isAvoiding = false;
        isBlocked = false;

        // Base sensor start position
        Vector3 sensorStartPosition = transform.position + transform.forward * sensorOffset.z + transform.up * sensorOffset.y;

        // Front sensor
        bool frontBlocked = Physics.Raycast(sensorStartPosition, transform.forward, out RaycastHit front, sensorLength);

        // Right front sensor
        bool rightBlocked = Physics.Raycast(sensorStartPosition + transform.right * sideSensorOffset, transform.forward, out RaycastHit left, sensorLength);

        // Left front sensor
        bool leftBlocked = Physics.Raycast(sensorStartPosition - transform.right * sideSensorOffset, transform.forward, out RaycastHit right, sensorLength);

        // Right angled sensor
        bool rightAngledBlocked = Physics.Raycast(sensorStartPosition + transform.right * sideSensorOffset, Quaternion.Euler(0, sensorAngle, 0) * transform.forward, sensorLength);

        // Left angled sensor
        bool leftAngledBlocked = Physics.Raycast(sensorStartPosition - transform.right * sideSensorOffset, Quaternion.Euler(0, -sensorAngle, 0) * transform.forward, sensorLength);

        // Debug rays for visualization
        Debug.DrawRay(sensorStartPosition, transform.forward * sensorLength, Color.blue); // Front
        Debug.DrawRay(sensorStartPosition + transform.right * sideSensorOffset, transform.forward * sensorLength, Color.yellow); // Right
        Debug.DrawRay(sensorStartPosition - transform.right * sideSensorOffset, transform.forward * sensorLength, Color.yellow); // Left
        Debug.DrawRay(sensorStartPosition + transform.right * sideSensorOffset, Quaternion.Euler(0, sensorAngle, 0) * transform.forward * sensorLength, Color.cyan); // Right angled
        Debug.DrawRay(sensorStartPosition - transform.right * sideSensorOffset, Quaternion.Euler(0, -sensorAngle, 0) * transform.forward * sensorLength, Color.cyan); // Left angled


        //if (frontBlocked)
        //{
        //    isBlocked = true; 
        //}
        if ((frontBlocked && front.collider != null && front.collider.CompareTag("MainCar")) ||
        (rightBlocked && right.collider != null && right.collider.CompareTag("MainCar")) ||
        (leftBlocked && left.collider != null && left.collider.CompareTag("MainCar")))
        {
            isBlocked = true;
            Debug.Log("blocked");
        }
        else
        {
            isBlocked = false;

            if (rightBlocked || rightAngledBlocked)
            {
                isAvoiding = true;
                ApplyAvoidance(-1f); // Steer left
            }

            if (leftBlocked || leftAngledBlocked)
            {
                isAvoiding = true;
                ApplyAvoidance(1f); // Steer right
            }
        }
    }


    private void ApplyAvoidance(float multiplier)
    {
        frontLeftWheel.steerAngle = maxSteerAngle * multiplier;
        frontRightWheel.steerAngle = maxSteerAngle * multiplier;
    }

    private void ApplySteering()
    {
        if (isAvoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steerAngle = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;
    }

    private void Drive()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rearLeftWheel.motorTorque = acceleration;
            rearRightWheel.motorTorque = acceleration;
        }
        else
        {
            rearLeftWheel.motorTorque = 0f;
            rearRightWheel.motorTorque = 0f;
        }
    }

    private void CheckWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    private void ApplyBrakes()
    {
        if (isBlocked)
        {
            frontLeftWheel.brakeTorque = brakeForce;
            frontRightWheel.brakeTorque = brakeForce;
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;

            rearLeftWheel.motorTorque = 0f;
            rearRightWheel.motorTorque = 0f;
        }
        else
        {
           
            frontLeftWheel.brakeTorque = 0f;
            frontRightWheel.brakeTorque = 0f;
            rearLeftWheel.brakeTorque = 0f;
            rearRightWheel.brakeTorque = 0f;
        }
    }

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheel, frontLeftTransform);
        UpdateWheel(frontRightWheel, frontRightTransform);
        UpdateWheel(rearLeftWheel, rearLeftTransform);
        UpdateWheel(rearRightWheel, rearRightTransform);
    }

    private void UpdateWheel(WheelCollider collider, Transform transform)
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        transform.position = position;
        transform.rotation = rotation;
    }
}
