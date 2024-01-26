using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float riseSpeed = 0.1f; // The speed at which the water rises

    [SerializeField]
    private bool isRising = false; // toggle the rising water effect

    private float _currentWaterHeight = 0f;
    private float originalY;

    void Update()
    {
        if (isRising)
        {
            RiseWater();
        }
    }

    void RiseWater()
    {
        _currentWaterHeight += riseSpeed * Time.deltaTime;
        var currentTransform = transform;
        currentTransform.localScale = new Vector3(currentTransform.localScale.x, _currentWaterHeight, currentTransform.localScale.z);

        // Move the water upwards by adjusting the position
        currentTransform.position = new Vector3(currentTransform.position.x, originalY + _currentWaterHeight * 0.5f, currentTransform.position.z);
    }

    public void ToggleRisingWater()
    {
        isRising = !isRising;
        if (!isRising)
        {
            // Store the current water height when turning off
            _currentWaterHeight = transform.localScale.y;
        }
    }
}