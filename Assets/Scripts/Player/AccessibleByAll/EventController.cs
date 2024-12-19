using System;
using UnityEngine;
namespace Player
{
    public class EventController : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        static EventController instance;

        // Delegates
        public delegate void noValueDelegate();
        public delegate void stringDelegate(String value);
        public delegate void stringBoolDelegate(String value, bool boool);
        public delegate void boolDelegate(bool value);
        public delegate void bool5Delegate(bool value, bool value2, bool value3, bool value4, bool value5);
        public delegate bool ReturnBoolSendGameObjectDelegate(GameObject value);
        public delegate bool ReturnBoolSendFloatDelegate(float value);

        // Events
        public event noValueDelegate lose;
        public event noValueDelegate finish;
        public event noValueDelegate movePressed;
        public event noValueDelegate moveNotPressed;
        public event noValueDelegate movementNotOccuring;
        public event noValueDelegate brakePressed;
        public event bool5Delegate move;
        public event bool5Delegate brake;
        // public event boolDelegate balance;
        public event ReturnBoolSendFloatDelegate checkIfGrounded;
        public event ReturnBoolSendFloatDelegate checkIfOnRamp;
        public event ReturnBoolSendFloatDelegate checkIfOnDune;
        public event ReturnBoolSendGameObjectDelegate checkWheelOnGround;
        public event ReturnBoolSendFloatDelegate checkIsSandDuneAhead;
        public event noValueDelegate shakeInAir;
        public event noValueDelegate shakeOnRamp;
        public event noValueDelegate shakeOnDune;
        public event stringDelegate shakeOnWheel;
        // Camera
        public event stringBoolDelegate zoom;

        // Colllision
        public event noValueDelegate collidedWithCan;
        public event noValueDelegate modelCollided;

        // Helper
        public event boolDelegate rotationHelper;

        //===============================================================
        //                          Mono Methods
        //===============================================================
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //===============================================================
        //                          Move Methods
        //===============================================================

        public void MoveNotPressed()
        {
            moveNotPressed?.Invoke();
        }

        public void MovePressed()
        {
            movePressed?.Invoke();
        }

        public void Move(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            move?.Invoke(isGrounded, isOnRamp, isOnDune, isPowerUp, canRotate);
        }

        public void BrakePressed()
        {
            brakePressed?.Invoke();
        }

        public void Brake(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            brake?.Invoke(isGrounded, isOnRamp, isOnDune, isPowerUp, canRotate);
        }

        public void MovementNotOccuring()
        {
            movementNotOccuring?.Invoke();
        }

        // public void Balance(bool value)
        // {
        //     balance?.Invoke(value);
        // }
        //===============================================================
        //                          Checker Methods
        //===============================================================

        public bool CheckIfGrounded(float value = 1)
        {
            return checkIfGrounded?.Invoke(value) ?? false;
        }

        public bool CheckIfOnRamp(float value = 1)
        {
            return checkIfOnRamp?.Invoke(value) ?? false;
        }

        public bool CheckIfOnDune(float value = 1)
        {
            return checkIfOnDune?.Invoke(value) ?? false;
        }

        public bool CheckWheelOnGround(GameObject wheel)
        {
            return checkWheelOnGround?.Invoke(wheel) ?? false;
        }

        public bool CheckIfSandDuneAhead(float value)
        {
            return checkIsSandDuneAhead?.Invoke(value) ?? false;
        }

        //===============================================================
        //                          Reaction Methods
        //===============================================================

        public void ShakeInAir()
        {
            shakeInAir?.Invoke();
        }

        public void ShakeOnRamp()
        {
            shakeOnRamp?.Invoke();
        }

        public void ShakeOnDune()
        {
            shakeOnDune?.Invoke();
        }

        public void ShakeOnOneWheel(string wheel)
        {
            shakeOnWheel?.Invoke(wheel);
        }

        //===============================================================
        //                          Collision Methods
        //===============================================================

        public void ModelCollided()
        {
            modelCollided?.Invoke();
        }

        public void CollidedWithCan()
        {
            collidedWithCan?.Invoke();
        }

        //===============================================================
        //                          Other Methods
        //===============================================================

        public void RotationHelper(bool canRotate)
        {
            rotationHelper?.Invoke(canRotate);
        }

        public void Zoom(string value, bool switchYOffset)
        {
            zoom?.Invoke(value, switchYOffset);
        }

        public void Lose()
        {
            lose?.Invoke();
        }

        public void Finish()
        {
            finish?.Invoke();
        }


    }
}
