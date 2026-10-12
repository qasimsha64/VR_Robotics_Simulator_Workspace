using UnityEngine;

public class Robot_Mover1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;

    [Header("Safety Sensor Settings")]
    public float safeDistance = 2.0f; // Agar obstacle is distance se kam ho toh stop kar do
    private Rigidbody rb;
    private bool isBlocked = false;

    void Start()
    {
        // Robot ka Rigidbody component fetch karna
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- 1. RAYCAST SENSOR CHECK (Eyes of the Robot) ---
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;

        // Samne ki taraf ray shoot karna
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, safeDistance))
        {
            // Agar hit hone wala object 'Wall' ya obstacle hai aur distance safe limit se kam hai
            if (hit.distance < safeDistance)
            {
                isBlocked = true;
                Debug.Log("Safety Stop! Obstacle too close at: " + hit.distance.ToString("F2") + " meters");

                // Scene view mein red line dikhane ke liye taake pata chale sensor trigger ho gaya hai
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);
            }
        }
        else
        {
            // Agar rasta saaf hai
            isBlocked = false;
            Debug.DrawRay(rayOrigin, rayDirection * safeDistance, Color.green);
        }
    }

    void FixedUpdate()
    {
        // --- 2. PHYSICS & MOVEMENT CONTROL ---
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Safety override: Agar rasta blocked hai aur user aagay jane ki koshish kar raha hai
        if (isBlocked && moveVertical > 0)
        {
            moveVertical = 0.0f; // Forward movement ko disable kar do
        }

        // Velocity calculate karna aur Rigidbody ko dena
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 targetVelocity = movement * speed;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
