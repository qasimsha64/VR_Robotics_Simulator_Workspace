using UnityEngine;

public class Robot_Raycast : MonoBehaviour
{
    public float sensorRange = 5.0f; // Sensor kitni door tak dekh sakta hai

    void Update()
    {
        // Robot ki position se aagay ki taraf ray shuru hoti hai
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        RaycastHit hit;

        // Raycast shoot karna aur check karna ke kya raste mein koi obstacle hai
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, sensorRange))
        {
            // Agar raste mein kuch aa gaya, toh uska fasla (distance) pata lagao
            float distanceToObstacle = hit.distance;
            Debug.Log("Obstacle detected! Distance: " + distanceToObstacle.ToString("F2") + " meters");

            // Scene view mein red line dikhane ke liye (Debugging)
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);
        }
        else
        {
            // Agar raste mein kuch nahi hai, toh green line dikhao
            Debug.DrawRay(rayOrigin, rayDirection * sensorRange, Color.green);
        }
    }
}
