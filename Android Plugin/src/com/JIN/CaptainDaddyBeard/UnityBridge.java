package com.JIN.CaptainDaddyBeard;

import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;

import android.content.Context;
import android.util.Log;

// Connection to wifi hotspot.
import android.net.wifi.WifiConfiguration;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;

// Connection to server (UDP).
import java.io.IOException;
import java.net.DatagramSocket;
import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.SocketTimeoutException;

public class UnityBridge extends UnityPlayerActivity
{
	static private String callbackObject, callbackMethod;
	static private int clientPort;
	
	// Just connect to the wifi hotspot (note that the OS sometimes connect on itself, when it already knows the wifi hotspot).
	static public void connectToWifiHotspot(Context context, String ssid, String key, String object, String method, int port)
    {		
		// Initialize arguments for later.
		callbackObject = object;
		callbackMethod = method;
		clientPort = port;
		
		// Check the device is not already connected to the hotspot.
    	WifiManager wifiManager = (WifiManager) context.getSystemService(WIFI_SERVICE);
    	WifiInfo wifiInfo = wifiManager.getConnectionInfo();
    	if (!wifiInfo.getSSID().equals("\"" + ssid + "\"")) // getSSID returns the SSID with double quotes.
    	{
    		Log.i("CaptainDaddyBeard", "ServiceConnector : Connecting to wifi hotspot (ssid = \"" + ssid + "\", key = \"" + key + "\")");
    		
			// Configure wifi.
	    	WifiConfiguration wifiConfig = new WifiConfiguration();
	    	wifiConfig.SSID = String.format("\"%s\"", ssid);
	    	wifiConfig.preSharedKey = String.format("\"%s\"", key);
	
	    	// Connect.
	    	int netId = wifiManager.addNetwork(wifiConfig);
	    	wifiManager.disconnect();
	    	wifiManager.enableNetwork(netId, true);
	    	wifiManager.reconnect();
    	}
    	else
    	{
    		Log.i("CaptainDaddyBeard", "ServiceConnector : Already connected to wifi hotspot (ssid = \"" + ssid + "\", key = \"" + key + "\")");
    		ConnectionDetector.connectToServer(); // In case the device is already connected, skip connection to hotspot.
    	}
    }
	
	/*
	 * Broadcast over all the network to ask the server its IP address. 
	 * Then receive the IP address. Reiterate the operation ten times if it fails.
	 * After that, send the IP address to Unity (the IP will be 0.0.0.0 if it has failed).
	 */
	static public void connectToServer() throws IOException
	{
		DatagramSocket client_socket = new DatagramSocket(clientPort);
        InetAddress IPAddress =  InetAddress.getByName("255.255.255.255"); // Broadcast to locate server.

        String sendString = "ip?";
        byte[] send_data = new byte[32];
        byte[] receiveData = new byte[32];
       	send_data = sendString.getBytes();

        DatagramPacket send_packet = new DatagramPacket(send_data, sendString.length(), IPAddress, 51976);
        try {
        	client_socket.send(send_packet);
        } catch (IOException e) {
        	Log.e("CaptainDaddyBeard", "ServiceConnector : Unable to send data to the server");
        }
   		
   		client_socket.setSoTimeout(1000); // 1 second timeout.
   		int attemptsLeft = 10; // Try ten times and quit trying if all attempts failed.
   		boolean ipReceived = false;
   		while(attemptsLeft-- > 0 && !ipReceived)
   		{
   			DatagramPacket receivePacket = new DatagramPacket(receiveData, receiveData.length);
   		    try {
   		    	client_socket.receive(receivePacket);
   		    } catch (SocketTimeoutException e) { // Timeout case.
   		        // Resend the request.
   		    	Log.w("CaptainDaddyBeard", "ServiceConnector : Receive socket timeout");
   		    	client_socket.send(send_packet);
   		        continue;
   		    }
   		    
   		    // Check received data if we actually received something.
   		    String receiveString = new String(receiveData);
   		    if (receiveString.startsWith("ip:"))
   		    {
   		    	// Get IP address and remove the '/' character.
   		    	String ipAddress = receivePacket.getAddress().toString().substring(1);
   	   		    Log.i("CaptainDaddyBeard", "ServiceConnector : Server IP address received : " + ipAddress);
   		    	sendMessageInUiThread(ipAddress);
   		    	
   		    	ipReceived = true;
   		    	break; // Stop the loop in case of success.
   		    }
   		}
   		
   		client_socket.close();
   		if (!ipReceived) // In case of fail.
   			sendMessageInUiThread("0.0.0.0");
	}
	
	// Send a message to Unity using the UI thread. This function should be called outside of the UI thread.
	static private void sendMessageInUiThread(String ipAddress)
	{
		if (ipAddress != null) // Avoid null exception (when the app run in background for some reason).
		{
			class RunnableArg implements Runnable { // Create a runnable that handle arguments.
	    		String ipAddress;
	    		RunnableArg(String ip) { ipAddress = ip; }
	    		public void run() {
	    			UnityPlayer.UnitySendMessage(callbackObject, callbackMethod, ipAddress);
	    		}
	    	}
	    	// Run UnitySendMessage on the main thread (since Unity library is only loaded on the main thread).
	    	RunnableArg runUnitySendMessage = new RunnableArg(ipAddress);
	    	if (UnityPlayer.currentActivity != null) // Avoid null exception too (see upwards).
	    		UnityPlayer.currentActivity.runOnUiThread(runUnitySendMessage);
		}
	}
}
