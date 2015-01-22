using UnityEngine;
using System.Collections;
using System.Diagnostics; // Process management.
using System.Security.Principal; // Detection of admin privileges.

// Includes for the UDP part of the server.
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System.Net;
using System.Net.Sockets;
using System.Text;
#endif

/*
 * Service Connector is used by both server (PC) and client (tablet).
 * /!\ ONLY ONE INSTANCE OF THIS CLASS SHOULD BE USED AT THE SAME TIME /!\
 * 
 * Server services are :
 * - Create a wifi hotspot.
 * - Accept connections from clients.
 * 
 * Client services are :
 * - Connect to the wifi hotspot.
 * - Connect to the server.
 * 
 * HOW TO USE THIS CLASS :
 * 1 - Create an object in the scene (it must last until the game ends. It can be the camera).
 * 2 - Attach this script to the object.
 * That's all. But if you want to be noticed when the connections are effective, then do the following :
 * 3 - Create a method that will be called when a client connect to the game (must be public, return void and takes an int (clients number) if it is on the server side, nothing otherwise).
 * 4 - At the beginning of the game, for the server side, call (just replace 'myDelegate' by the name of your method) :
 * ServiceConnector.ProvideDelegate (new ServiceConnector.ClientConnected (myDelegate));
 * 4bis - And for the client side :
 * ServiceConnector.ProvideDelegate (new ServiceConnector.ConnectedToServer (myDelegate));
 * And that's it !
 * 
 * Do not forget the JAR file, Android manifest and package identifier.
 * 
 */
public class ServiceConnector : MonoBehaviour {

	// Two differents ports in order to avoid conflict between IP communication and actual game.
	private static int connectionPort; // Used for IP discovering.
	private static int standardPort; // Used when the client know the server IP (the game in general).

	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
	// Debugging purpose : allow another program to keep the wifi hotspot online (do not recreate it each time the game is launched).
	[Tooltip("Will the game create the wifi hotspot (it could be already created by another program) ?")]
	[SerializeField] private bool createWifiHotspot = true;
	#endif

	// Functions called when a client connects to the server (server side : ClientConnected, client side : ConnectedToServer).
	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
	public delegate void ClientConnected (int numClients);
	private static ClientConnected delegateMethod = null;
	public static void ProvideDelegate(ClientConnected method) { delegateMethod = method; }
	#elif UNITY_ANDROID
	public delegate void ConnectedToServer ();
	private static ConnectedToServer delegateMethod = null;
	public static void ProvideDelegate(ConnectedToServer method) { delegateMethod = method; }
	#endif

	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		Connect ();
	}

	private void Connect () {
		string ssid = "Daddy_Beard_Server", key = "the_captain";
		connectionPort = 51976;
		standardPort = 51977;

		// Server side.
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		if (createWifiHotspot)
			StartWifiHotspot (ssid, key); // Creates the wifi hotspot. This may take several seconds, so be patient on the client side.
		else
			UnityEngine.Debug.Log ("Service Connector : Wifi hotspot creation skipped.");
		StartCoroutine (CommunicateServerIP ()); // Creates the server in a coroutine to avoid freezing the game.
		Network.InitializeServer (2, standardPort, false);

		// Client side.
		#elif UNITY_ANDROID
		ConnectToWifiHotspot (ssid, key, name); // Connect to the wifi hotspot. This may take several seconds, so ConnectToServer() is likely to fail.

		// Unknow side.
		#else
		UnityEngine.Debug.LogError ("Service Connector : usable only on Windows or Android.");

		#endif
	}

	/*
	 * Server (Windows) side.
	 */
	#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

	void OnPlayerConnected () {
		UnityEngine.Debug.Log ("ServiceConnector : Client connected.");
		if (delegateMethod != null) // Tell that a client has just connected (if there is a delegate).
			delegateMethod (Network.connections.Length);
	}

	void OnDestroy () {
		Network.Disconnect ();
		if (createWifiHotspot)
			StopWifiHotspot (); // Destroys the wifi hotspot.
	}
	
	/*
	 * Create a wifi hotspot which has 'ssid' for name and 'key' for password (without sharing any Internet connection) reacheable by a tablet device.
	 * The 'Stop ()' method MUST be called when the program ends.
	 * Requirements : 'ssid' and 'key' not null, 'key' with 8 to 63 ASCII characters , process has administrator privileges.
	 * Return 0 on success, 1 if no admin mode, 2 if 'ssid' or 'key' is null, 3 if the number of characters in the key is incorrect.
	 */
	private static int StartWifiHotspot (string ssid, string key) {
		// Prerequisite : admin privileges and arguments validity.
		if (IsAdmin ()) {
			if (ssid != null && key != null) {
				if (8 <= key.Length || key.Length <= 63) {
					// Step 1 : configure the wifi hotspot.
					Process wifiProcess = new Process ();
					InitializeRouter (wifiProcess);
					wifiProcess.StartInfo.Arguments = "wlan set hostednetwork mode=allow ssid=\"" + ssid + "\" key=\"" + key + "\"";
					using (Process process = Process.Start(wifiProcess.StartInfo)) {
						process.WaitForExit();
					}
					
					// Step 2 : start the wifi hotspot.
					wifiProcess.StartInfo.Arguments = "wlan start hostednetwork";
					UnityEngine.Debug.Log ("ServiceConnector : Wifi hotspot started.");
					using (Process process = Process.Start(wifiProcess.StartInfo)) {
						process.WaitForExit();
					}
				}
				else {
					UnityEngine.Debug.LogError ("ServiceConnector : Wifi hotspot : the key must be a string with 8 to 63 ASCII characters.");
					return 3;
				}
			}
			else {
				UnityEngine.Debug.LogError ("ServiceConnector : Wifi hotspot : the SSID or key is null.");
				return 2;
			}
		}
		else {
			UnityEngine.Debug.LogError ("ServiceConnector : Wifi hotspot : need administrator privileges to start !");
			return 1;
		}
		
		return 0;
	}

	/*
	 * Destroy the wifi hotspot.
	 * MUST be called when the program ends.
	 */
	private static void StopWifiHotspot () {
		// Step 3 : stop the wifi hotspot.
		Process wifiProcess = new Process ();
		InitializeRouter (wifiProcess);
		wifiProcess.StartInfo.Arguments = "wlan stop hostednetwork";
		UnityEngine.Debug.Log ("ServiceConnector : Wifi hotspot stopped.");
		using (Process process = Process.Start(wifiProcess.StartInfo)) {
			process.WaitForExit();
		}
	}

	// Initialize wifi hotspot attributes.
	private static void InitializeRouter (Process wifiProcess) {
		wifiProcess.StartInfo.UseShellExecute = false;
		wifiProcess.StartInfo.CreateNoWindow = true;
		wifiProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		//wifiProcess.StartInfo.FileName = "netsh";
		wifiProcess.StartInfo.FileName = "C:\\Windows\\System32\\netsh";
	}
	
	// Return true if the current process has admin privileges.
	private static bool IsAdmin ()
	{
		return UacHelper.IsProcessElevated;
	}

	/*
	 * Wait for clients to ask the server IP address.
	 * This method is blocking so should be called from a coroutine.
	 * This method never returns to allow numerous devices to connect one after another (imagine the situation where three friends with three devices play one after another).
	 */
	private static IEnumerator CommunicateServerIP ()
	{
		int recv;
		byte[] data = new byte[32];
		IPEndPoint ipep = new IPEndPoint(IPAddress.Any, connectionPort);
		
		Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		newsock.Bind(ipep);
		
		IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
		EndPoint Remote = (EndPoint)(sender);
		
		while (true)
		{
			while (newsock.Available > 0) // Read all the data received.
			{
				recv = newsock.ReceiveFrom(data, ref Remote);
				if (Encoding.ASCII.GetString(data, 0, recv).Equals("ip?"))
				{
					UnityEngine.Debug.Log ("ServiceConnector : IP Communication : Communicate IP to " + Remote.ToString());
					string sendString = "ip:";
					data = Encoding.ASCII.GetBytes(sendString);
					newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
				}
			}
			
			yield return new WaitForSeconds (0.2f); // Check again in 0.2 second.
		}
	}


	/*
	 * Client (Android) side.
	 */
	#elif UNITY_ANDROID

	void OnConnectedToServer () {
		UnityEngine.Debug.Log ("ServiceConnector : Connected to server.");
		if (delegateMethod != null) // Tell that we have just connected to the server (if there is a delegate).
			delegateMethod ();
	}

	void OnDestroy () {
		Network.Disconnect ();
	}

	/*
	 * Connect to a wifi hotspot created by the server.
	 * Also transmit the callback parameters (object, method and port) used when the server IP address has been found.
	 */
	private static void ConnectToWifiHotspot (string ssid, string key, string objectName) {
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext");
		
		AndroidJavaClass androidClass = new AndroidJavaClass("com.JIN.CaptainDaddyBeard.UnityBridge");
		object[] args = new object[] { app, ssid, key, objectName, "ConnectToServer", connectionPort };

		// Do not remove the commented lines, they could be useful a day or another.
		//activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
		//                                                       {
			androidClass.CallStatic("connectToWifiHotspot", args);
		//}));
	}

	/*
	 * Called by the Java plugin when the server IP address has been found or when the IP research failed (in this case 'ip' is "0.0.0.0").
	 * The IP address research is automatically launched when the Android OS connects to the hotspot and warns the application (see the Java plugin).
	 * Never add 'static' to this function (as UnitySendMessage will not work nor ouput errors).
	 * As the device can sometimes connect by itself automatically to the wifi hotspot (because it is known), this method can be called twice.
	 * That why we check that we are not already connected.
	 */
	public void ConnectToServer (string ip) {
		if (Network.peerType == NetworkPeerType.Disconnected)
			Network.Connect (ip, standardPort);
	}

	#endif
}
