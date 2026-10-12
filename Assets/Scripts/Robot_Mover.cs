using UnityEngine;

public class Robot_Mover : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;

    void Start()
    {
        // Script start hote hi robot ka Rigidbody component utha lo
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // Physics calculations ke liye FixedUpdate use hota hai
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Transform.Translate ki jagah Rigidbody velocity use karna taake colliders rok sakein
        Vector3 targetVelocity = movement * speed;
        rb.linearVelocity= new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
