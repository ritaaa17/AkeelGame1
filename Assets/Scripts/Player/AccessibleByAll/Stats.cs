using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class Stats : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Game Components
        GameController gameController;
        public GameController GameController { get => gameController; }

        // Script Components
        EventController eventController;

        // Can
        [Header("Can")]
        [SerializeField] float powerUpModifier = 2;
        public float PowerUpModifier { get => powerUpModifier; }
        [SerializeField] float canPowerupTime = 5f;
        public float CanPowerupTime { get => canPowerupTime; }

        // Speed
        [Header("Speed")]
        [SerializeField] float slowDownForceRamp = 300; // Slow down for shaking on ramp
        public float SlowDownForceOnRamp { get => slowDownForceRamp; }
        [SerializeField] float slowDownForceDune = 100; // Slow down for shaking on dune
        public float SlowDownForceOnDune { get => slowDownForceDune; }
        [SerializeField] float deceleration = 1000;
        public float Deceleration { get => deceleration; }
        [SerializeField] float timeToStartDeceleration = 0.5f;
        public float TimeToStartDeceleration { get => timeToStartDeceleration; }
        [SerializeField] float brakeToMoveForceModifier = 1000;
        public float BrakeToMoveForceModifier { get => brakeToMoveForceModifier; }
        // [SerializeField] float speed = 1;
        // public float Speed { get => speed; }
        // [SerializeField] float maxYVelocity = 10;
        // public float MaxYVelocity { get => maxYVelocity; }
        // [SerializeField] float maxXVelocity = 2;
        // public float MaxXVelocity { get => maxXVelocity; }

        // Wheels
        [Header("Wheels")]
        [SerializeField] float acceleration = 1500000f;
        public float Acceleration { get => acceleration; }
        [SerializeField] float maxSpeed = 1500000f;
        public float MaxSpeed { get => maxSpeed; }
        [SerializeField] float maxMotorTorque = 10000; // Adjust as needed
        public float MaxMotorTorque { get => maxMotorTorque; }
        [SerializeField] float timeToResetAcceleraction = 0.5f; // Adjust as needed
        public float TimeToResetAcceleraction { get => timeToResetAcceleraction; }
        [SerializeField] float brakeModifier = 4;
        public float BrakeModifier { get => brakeModifier; }

        // Dune Wheels
        [Header("Dune Wheels")]
        [SerializeField] float duneAcceleration = 1500000f;
        public float DuneAcceleration { get => duneAcceleration; }
        [SerializeField] float duneMaxSpeed = 1500000f;
        public float DuneMaxSpeed { get => duneMaxSpeed; }
        [SerializeField] float duneMaxMotorTorque = 10000; // Adjust as needed
        public float DuneMaxMotorTorque { get => duneMaxMotorTorque; }
        [SerializeField] float duneTimeToResetAcceleraction = 0.5f; // Adjust as needed
        public float DuneTimeToResetAcceleraction { get => duneTimeToResetAcceleraction; }
        [SerializeField] float duneBrakeModifier = 4;
        public float DuneBrakeModifier { get => duneBrakeModifier; }


        // Rotation
        [Header("Rotation")]
        [SerializeField] float maxRotationZPositive = 90;
        public float MaxRotationZPositive { get => maxRotationZPositive; }
        [SerializeField] float maxRotationZNegative = -90;
        public float MaxRotationZNegative { get => maxRotationZNegative; }


        // Balancing
        [Header("Balancing")]
        [SerializeField] float xCenterOfMass = 0;
        public float XCenterOfMass { get => xCenterOfMass; }
        [SerializeField] float yCenterOfMass = 0;
        public float YCenterOfMass { get => yCenterOfMass; }
        [SerializeField] float timeToResetCenterOfMass = 1;
        public float TimeToResetCenterOfMass { get => timeToResetCenterOfMass; }
        [SerializeField] float balanceForce = 1;
        public float BalanceForce { get => balanceForce; }
        [SerializeField] float balanceModifier = 10;
        public float BalanceModifier { get => balanceModifier; }

        // In Air Uncontrolled Balance Force
        [Header("In Air Uncontrolled Balance")]
        [SerializeField] float uncontrolledBalanceForce = 10;
        public float UncontrolledBalanceForce { get => uncontrolledBalanceForce; }
        [SerializeField] float timeToStopShaking = 0.5f;
        public float TimeToStopShaking { get => timeToStopShaking; }


        // Layers
        [Header("Layers")]
        [SerializeField] LayerMask groundLayer;
        public LayerMask GroundLayer { get => groundLayer; }
        [SerializeField] LayerMask rampLayer;
        public LayerMask RampLayer { get => rampLayer; }
        [SerializeField] LayerMask duneLayer;
        public LayerMask DuneLayer { get => duneLayer; }

        //model size
        [Header("Model")]
        [SerializeField] GameObject model;
        public GameObject Model { get => model; }
        [SerializeField] float modelSize;
        public float ModelSize { get => modelSize; }

        // Wheels
        [Header("Wheels")]
        [SerializeField] GameObject frontWheel;
        public GameObject FrontWheel { get => frontWheel; }
        [SerializeField] GameObject backWheel;
        public GameObject BackWheel { get => backWheel; }

        // Camera
        [Header("Camera")]
        [SerializeField] float distanceToZoomOutForDune = 300;
        public float DistanceToZoomOutForDune { get => distanceToZoomOutForDune; }

        // Rotation Helper
        [Header("Rotation Helper")]
        [SerializeField] float angularVelocityNegative = -80;
        public float AngularVelocityNegative { get => angularVelocityNegative; }
        [SerializeField] float angularVelocityPositive = 80;
        public float AngularVelocityPositive { get => angularVelocityPositive; }

        //===============================================================
        //                          Mono Methods
        //===============================================================
        void Awake()
        {
            gameController = FindAnyObjectByType<GameController>();
            eventController = GetComponent<EventController>();
        }

        void Start()
        {
            modelSize = GetModelSize();
        }

        void FixedUpdate()
        {
        }

        //===============================================================
        //                              Methods
        //===============================================================

        public float GetModelSize()
        {

            return model.GetComponent<Renderer>().bounds.size.x ;

        }

        public float GetWheelSize()
        {
            return (frontWheel.GetComponent<Renderer>().bounds.size.y / 2) + 0.1f;
        }


    }
}