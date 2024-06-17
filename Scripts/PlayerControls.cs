using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ships move up and down based upon player input")] 
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float xRange = 10f;
    [SerializeField] float yRange = 7f;

    [Header("Laser gun array")]
    [SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float controlRollFactor = -20f;

    float yThrow;
    float xThrow;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * moveSpeed * Time.deltaTime;
        float rawXPose = transform.localPosition.x + xOffset;
        float clampedXPose = Mathf.Clamp(rawXPose, -xRange, xRange);

        float yOffset = yThrow * moveSpeed * Time.deltaTime;
        float rawYPose = transform.localPosition.y + yOffset;
        float clampedYPose = Mathf.Clamp(rawYPose, -yRange, yRange);

        transform.localPosition = new Vector3(
            clampedXPose,
            clampedYPose,
            transform.localPosition.z);
    }
}
