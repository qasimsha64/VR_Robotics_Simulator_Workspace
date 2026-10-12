using UnityEngine;
using System.Text;
using System.Net.Sockets;

public class RobotTelemetryPublisher : MonoBehaviour
{
    [System.Serializable]
    public class RobotData
    {
        public string robotId;
        public float posX;
        public float posY;
        public float posZ;
        public float obstacleDistance;
        public bool isBlocked;
    }

    public string robotId = "Robot_Alpha_01";
    public string targetIP = "127.0.0.1"; // Localhost (apna hi computer ya VR headset ka IP)
    public int targetPort = 5000;         // Network port number

    private UdpClient udpClient;

    void Start()
    {
        // UDP Client initialize karna network data bhejne ke liye
        udpClient = new UdpClient();
    }

    void Update()
    {
        // Spacebar dabane par telemetry network par broadcast ho gi
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendTelemetryOverNetwork();
        }
    }

    void SendTelemetryOverNetwork()
    {
        // 1. Data Object fill karna
        RobotData data = new RobotData();
        data.robotId = this.robotId;
        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.posZ = transform.position.z;
        // (Raycast distance values yahan add kar sakte hain)

        // 2. JSON mein convert karna
        string jsonString = JsonUtility.ToJson(data);

        // 3. JSON string ko bytes mein convert karke network par bhejna
        byte[] sendBytes = Encoding.UTF8.GetBytes(jsonString);
        try
        {
            udpClient.Send(sendBytes, sendBytes.Length, targetIP, targetPort);
            Debug.Log("Telemetry sent over network to IP: " + targetIP + ":" + targetPort);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Network Error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        // Jab game band ho toh socket close kar do
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}