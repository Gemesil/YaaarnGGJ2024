using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public float minBlinkInterval = 1f; // Minimum time between blinks
    public float maxBlinkInterval = 5f; // Maximum time between blinks
    public float blinkDuration = 0.3f;  // Duration the light stays off during blink

    private Light lightComponent;
    private float nextBlinkTime;
    private bool isBlinking;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogError("BlinkingLight script requires a Light component on the same GameObject.");
            enabled = false;
            return;
        }

        SetNextBlinkTime();
    }

    void Update()
    {
        // Check if it's time to blink
        if (Time.time >= nextBlinkTime)
        {
            // Start the blink
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        if (!isBlinking)
        {
            isBlinking = true;

            // Turn off the light
            lightComponent.enabled = false;

            // Wait for the specified duration
            yield return new WaitForSeconds(blinkDuration);

            // Turn on the light
            lightComponent.enabled = true;

            // Set the next blink time
            SetNextBlinkTime();

            isBlinking = false;
        }
    }

    void SetNextBlinkTime()
    {
        // Calculate the next blink time within the specified interval
        nextBlinkTime = Time.time + Random.Range(minBlinkInterval, maxBlinkInterval);
    }
}