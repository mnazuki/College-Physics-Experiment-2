using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CentripetalForce : MonoBehaviour
{
    public Rigidbody rb;
    public Transform center;

    [Range(.1f, 100f)] public float mass = 1f;
    [Range(.1f, 100f)] public float velocity = 1f;
    [Range(.1f, 100f)] public float radius = 1f;

    public Slider massSlider, velocitySlider, radiusSlider;
    public TextMeshProUGUI massValueText, velocityValueText, radiusValueText;
    public TextMeshProUGUI forceDisplay;

    // Gravitational constant
    public float gravityStrength = 9.81f; // Earth's gravity (m/s²)

    void Start()
    {
        if (massSlider)
        {
            massSlider.value = mass;
            massSlider.onValueChanged.AddListener(value => { mass = value; UpdateUI(); });
        }
        if (velocitySlider)
        {
            velocitySlider.value = velocity;
            velocitySlider.onValueChanged.AddListener(value => { velocity = value; UpdateUI(); });
        }
        if (radiusSlider)
        {
            radiusSlider.value = radius;
            radiusSlider.onValueChanged.AddListener(value => { radius = value; UpdateUI(); });
        }

        rb.mass = mass;
        UpdateUI();
    }

    void FixedUpdate()
    {
        if (massSlider && massSlider.value != mass) massSlider.value = mass;
        if (velocitySlider && velocitySlider.value != velocity) velocitySlider.value = velocity;
        if (radiusSlider && radiusSlider.value != radius) radiusSlider.value = radius;

        rb.mass = mass;
        Vector3 directionToCenter = (center.position - rb.position).normalized;

        // Maintain circular motion (perpendicular to directionToCenter)
        Vector3 tangentialVelocity = Vector3.Cross(Vector3.up, directionToCenter) * velocity;

        rb.velocity = tangentialVelocity;

        // Centripetal force calculation: F = (m * v²) / r
        float velocitySquared = velocity * velocity;
        float forceMagnitude = (mass * velocitySquared) / radius;

        // Centripetal force towards the center of the circle
        Vector3 forceDirection = (center.position - rb.position).normalized;
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Force);

        // Apply gravity force
        Vector3 gravityForce = new Vector3(0, -gravityStrength * mass, 0); // Earth's gravity pulling downward
        rb.AddForce(gravityForce, ForceMode.Force);

        // Orbital period (T = 2 * pi * r / v)
        float period = (2 * Mathf.PI * radius) / velocity;

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log($"Period (T): {period:F2} seconds");
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (massValueText) massValueText.text = $"Mass: {mass:F2} kg";
        if (velocityValueText) velocityValueText.text = $"Velocity: {velocity:F2} m/s";
        if (radiusValueText) radiusValueText.text = $"Radius: {radius:F2} m";

        if (forceDisplay)
        {
            float velocitySquared = velocity * velocity;
            float forceMagnitude = (mass * velocitySquared) / radius;
            forceDisplay.text = $"Centripetal Force: {forceMagnitude:F2} N";
        }
    }
}
