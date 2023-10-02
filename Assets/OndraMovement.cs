using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndraMovement : MonoBehaviour
{
    [Header("Rotation phase")]
    [SerializeField]
    float maxRotationSpeed;
    float currentRotationSpeed = 0;
    [SerializeField]
    float acceleration;


    [Header("SettingPower phase")]
    [SerializeField]
    Transform arrow;

    private const float minPower = 1;
    private const float maxPower = 5;
    private float currentPower = 1;
    [SerializeField]
    private float growingSpeed = 5;
    private bool isGrowing = true;

    [Header("Throwing phase")]
    [SerializeField]
    private Rigidbody hammer;

    [SerializeField]
    private float powerMultiplier = 500;

    private ThrowPhase currentPhase = ThrowPhase.Rotating;

    void Update()
    {
        if(currentPhase == ThrowPhase.Rotating) {
            OndraRotate();
        } else if (currentPhase == ThrowPhase.SettingPower) {
            SettingPower();
        } else {
            //Thrown();
        }

        ProcessInput();
    }

    private void ProcessInput() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentPhase == ThrowPhase.Rotating) {
                currentPhase = ThrowPhase.SettingPower;
            } else if (currentPhase == ThrowPhase.SettingPower) {
                currentPhase = ThrowPhase.Thrown;
                Thrown();
            }
        }
    }

    private void Thrown() {
        hammer.isKinematic = false;
        hammer.transform.SetParent(null);

        hammer.AddForce(Vector3.up * powerMultiplier * currentPower);
    }

    private void SettingPower() {
        if(isGrowing) {
            currentPower += growingSpeed * Time.deltaTime;
            if(currentPower > maxPower) {
                isGrowing = false;
            }
        } else {
            currentPower -= growingSpeed * Time.deltaTime;
            if(currentPower < minPower) {
                isGrowing = true;
            }
        }
        arrow.localScale = Vector3.one * currentPower;
    }

    private void OndraRotate() {

        if(currentRotationSpeed < maxRotationSpeed) {
            currentRotationSpeed += acceleration;
        }

        transform.Rotate(Vector3.up * currentRotationSpeed * Time.deltaTime);
    }
}

enum ThrowPhase
{
    Rotating,
    SettingPower,
    Thrown
}