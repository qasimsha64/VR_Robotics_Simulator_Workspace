using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class VRCommandListener : MonoBehaviour
{
    private UdpClient udpServer;
    private Thread receiveThread;
    public int listenPort = 5000;

    public string latestCommand = "IDLE";
    private bool isRunning = true; // Thread ko control karne ke liye flag

    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ListenForData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("VR Command Listener started on port: " + listenPort);
    }

    private void ListenForData()
    {
        udpServer = new UdpClient(listenPort);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // Jab tak isRunning true hai, loop chalta rahega
        while (isRunning)
        {
            try
            {
                byte[] data = udpServer.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

                latestCommand = message;
                Debug.Log("Command received from VR/Remote: " + message);
            }
            catch (SocketException)
            {
                // Yeh error tab aayega jab hum game band karein ge aur server close hoga (yeh normal hai)
                break;
            }
            catch (System.Exception e)
            {
                if (isRunning)
                {
                    Debug.LogError("Listener Error: " + e.Message);
                }
                break;
            }
        }
    }

    void Update()
    {
        if (latestCommand.Contains("MOVE_FORWARD"))
        {
            // Robot movement logic yahan aayegi
        }
    }

    void OnApplicationQuit()
    {
        // Game band hone par thread ko graceful exit dene ka tareeqa
        isRunning = false;

        if (udpServer != null)
        {
            udpServer.Close(); // Yeh .Receive() block ko khatam kar ke thread ko safely band kar dega
        }
    }
}