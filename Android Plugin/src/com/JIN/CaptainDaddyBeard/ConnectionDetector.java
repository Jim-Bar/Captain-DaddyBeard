package com.JIN.CaptainDaddyBeard;

import java.io.IOException;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.util.Log;

public class ConnectionDetector extends BroadcastReceiver 
{
	// Automatically called when the wifi state change.
	@Override
	public void onReceive(Context context, Intent intent)
	{
		NetworkInfo info = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);
	      if(info != null) {
	         if(info.isConnected()) {
	        	 Log.i("CaptainDaddyBeard", "ServiceConnector : Connected to the hotspot");
		            connectToServer();
	         }
	      }
	}
	
	static public void connectToServer()
	{
		// Launch the task in background (because network operations are forbidden on the UI thread).
		Thread networkThread = new Thread(new Runnable() {
		    @Override
		    public void run() {
		        try {
		        	UnityBridge.connectToServer();
		        } catch (IOException e) {
		        	Log.e("CaptainDaddyBeard", "ServiceConnector : IOException :");
		            e.printStackTrace();
		        }
		    }
		});

		networkThread.start();
	}
}
