using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CentripetalForce : MonoBehaviour
{
    public Rigidbody rb;
    public Transform center;

    // Exposed variables for both Inspector & Sliders
    [Range(.1f, 100f)] public float mass = 1f;
    [Range(.1f, 100f)] public float velocitySquared = 1f;
    [Range(.1f, 100f)] public float radius = 1f;

    public Slider massSlider, velocitySlider, radiusSlider;
    public TextMeshProUGUI massValueText, velocityValueText, radiusValueText;
    public TextMeshProUGUI forceDisplay;

    void Start()
    {
        // Assign slider initial values from the inspector
        if (massSlider)
        {
            massSlider.value = mass;
            massSlider.onValueChanged.AddListener(value => { mass = value; UpdateUI(); });
        }
        if (velocitySlider)
        {
            velocitySlider.value = velocitySquared;
            velocitySlider.onValueChanged.AddListener(value => { velocitySquared = value; UpdateUI(); });
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
        // Ensure UI updates even when values are changed in the Inspector
        if (massSlider && massSlider.value != mass) massSlider.value = mass;
        if (velocitySlider && velocitySlider.value != velocitySquared) velocitySlider.value = velocitySquared;
        if (radiusSlider && radiusSlider.value != radius) radiusSlider.value = radius;

        // Apply physics calculations
        rb.mass = mass;

        // Ensure the object stays at the correct radius
        Vector3 orbitDirection = (rb.position - center.position).normalized;
        rb.position = center.position + orbitDirection * radius;

        // Calculate centripetal force: F = (m * v²) / r
        float forceMagnitude = (mass * velocitySquared) / radius;

        // Apply force toward the center
        Vector3 forceDirection = (center.position - rb.position).normalized;
        float orbitalSpeed = Mathf.Sqrt(forceMagnitude / mass);
        rb.linearVelocity = Vector3.Cross(forceDirection, Vector3.up) * orbitalSpeed;
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Force);

        // Update UI every frame
        UpdateUI();
    }

    void UpdateUI()
    {
        if (massValueText) massValueText.text = $"Mass: {mass:F2} kg";
        if (velocityValueText) velocityValueText.text = $"Velocity²: {velocitySquared:F2} m²/s²";
        if (radiusValueText) radiusValueText.text = $"Radius: {radius:F2} m";

        if (forceDisplay)
        {
            float forceMagnitude = (mass * velocitySquared) / radius;
            forceDisplay.text = $"Centripetal Force: {forceMagnitude:F2} N";
        }
    }
}