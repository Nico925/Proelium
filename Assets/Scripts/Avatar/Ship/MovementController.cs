using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        MovementControllerConfig MovementConfig
        {
            get { return ship.Avatar.AvatarData.shipConfig.movementConfig; }
        }

        Ship ship;
        Rigidbody rigid;
        Vector3 fullTorque;
        #region Rotation fields
        Vector3 proj;
        Quaternion targetRotation;
        Quaternion deltaRotation;
        Vector3 deltaAngles;
        Vector3 worldDeltaAngles;

        Vector3 rollProj;
        Quaternion rollTargetRotation;
        Quaternion rollDeltaRotation;
        Vector3 rollDeltaAngles;
        Vector3 rollWorldDeltaAngles;
        #endregion

        #region API
        public void Init(Ship _ship, Rigidbody _rigid)
        {
            ship = _ship;
            rigid = _rigid;
        }
        /// <summary>
        /// Self orient and propel toward _target
        /// </summary>
        /// <param name="_target">Provide direction of acceleration and magnitude of the pulse</param>
        public void Move(Vector3 _target)
        {
            rigid.AddForce(_target * MovementConfig.MovmentSpeed, ForceMode.Force);
            if (_target == Vector3.zero)
                _target = transform.forward;
            

            fullTorque = Yaw(_target, Vector3.up);
            //if ((int)ship.Avatar.PlayerId == 3)
                //Debug.Log(fullTorque);
            float roll = 0;
            if (fullTorque.y < -100)
                roll = 1;
            else if (fullTorque.y > 100)
                roll = -1;
             ship.Model.transform.DOLocalRotate(new Vector3(ship.Model.transform.rotation.x, ship.Model.transform.rotation.y, roll * 30), 0.2f);
            //Vector3 rollTarget = Vector3.Cross(fullTorque, transform.forward) * Mathf.Sin(Vector3.Angle(Vector3.up,));
            //fullTorque += Roll(rollTarget, transform.forward);
            rigid.AddTorque(fullTorque, ForceMode.Force);
        }
        #endregion

        
        Vector3 Roll(Vector3 _target, Vector3 _normal)
        {
            // Compute target rotation (align rigidybody's up direction to the normal vector)

            rollProj = Vector3.ProjectOnPlane(_target, _normal);
            rollTargetRotation = Quaternion.LookRotation(rollProj, _normal);

            rollDeltaRotation = Quaternion.Inverse(transform.rotation) * rollTargetRotation;
            rollDeltaAngles = GetRelativeAngles(rollDeltaRotation.eulerAngles);
            rollWorldDeltaAngles = transform.TransformDirection(rollDeltaAngles);

            Vector3 appliedTorque = MovementConfig.RotationSpeed * rollWorldDeltaAngles - MovementConfig.RotationSpeed * 10 * rigid.angularVelocity;
            return appliedTorque;
        }

        /// <summary>
        /// Yaw the gameObj toward _target onto plane defined by _normal
        /// </summary>
        /// <param name="_target">Target to head to</param>
        /// <param name="_normal">Plane on wich apply the rotation</param>
        /// <returns>Applied torque</returns>
        Vector3 Yaw(Vector3 _target, Vector3 _normal)
        {
            // Compute target rotation (align rigidybody's up direction to the normal vector)

            proj = Vector3.ProjectOnPlane(_target, _normal);
            targetRotation = Quaternion.LookRotation(proj, _normal);

            deltaRotation = Quaternion.Inverse(transform.rotation) * targetRotation;
            deltaAngles = GetRelativeAngles(deltaRotation.eulerAngles);
            worldDeltaAngles = transform.TransformDirection(deltaAngles);
            Vector3 appliedTorque = MovementConfig.RotationSpeed * worldDeltaAngles - MovementConfig.RotationSpeed * 10 * rigid.angularVelocity;
            return appliedTorque;
        }

        // Convert angles above 180 degrees into negative/relative angles
        Vector3 GetRelativeAngles(Vector3 angles)
        {
            Vector3 relativeAngles = angles;
            if (relativeAngles.x > 180f)
                relativeAngles.x -= 360f;
            if (relativeAngles.y > 180f)
                relativeAngles.y -= 360f;
            if (relativeAngles.z > 180f)
                relativeAngles.z -= 360f;

            return relativeAngles;
        }
    }

    [Serializable]
    public class MovementControllerConfig
    {
        public float RotationSpeed;
        public float MovmentSpeed;
    }
}
