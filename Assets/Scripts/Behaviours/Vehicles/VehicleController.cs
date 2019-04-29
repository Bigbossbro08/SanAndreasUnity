﻿using UnityEngine;
using SanAndreasUnity.Net;

namespace SanAndreasUnity.Behaviours.Vehicles
{
    [RequireComponent(typeof(Vehicle))]
    public class VehicleController : MonoBehaviour
    {
        private Vehicle m_vehicle;
        

        private void Awake()
        {
            m_vehicle = GetComponent<Vehicle>();
        }

        private void Update()
        {
            
            var driverSeat = m_vehicle.DriverSeat;

            if (null == driverSeat || null == driverSeat.OccupyingPed)
            {
                if (NetStatus.IsServer)
                    this.ResetInput();
                return;
            }
            
            if (driverSeat.OccupyingPed != Ped.Instance)
                return;
            
            // local ped is occupying driver seat

            this.ResetInput();

			if (!GameManager.CanPlayerReadInput()) return;

            var accel = Input.GetAxis("Vertical");
            var brake = Input.GetButton("Brake") ? 1.0f : 0.0f;
            var speed = Vector3.Dot(m_vehicle.Velocity, m_vehicle.transform.forward);

            if (speed * accel < 0f)
            {
                brake = Mathf.Max(brake, 0.75f);
                accel = 0f;
            }

            m_vehicle.Accelerator = accel;
            m_vehicle.Steering = Input.GetAxis("Horizontal");
            m_vehicle.Braking = brake;
        }

        void ResetInput()
        {
            m_vehicle.Accelerator = 0;
            m_vehicle.Steering = 0;
            m_vehicle.Braking = 0;
        }
    }
}