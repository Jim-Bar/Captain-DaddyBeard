using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Get all RPC calls in one place.
 * The types supported are :
 * void (nothing), int, bool, float, string, Vector3, Quaternion, object and object[].
 * 
 * HOW TO USE THIS CLASS :
 * - Create the method as it was an RPC method (but without the tag [RPC]).
 * - Register the method using RegisterMethod() :
 *   RPCWrapper.RegisterMethod (HerePutYourMethod);
 * - Call it from the network using RPC() (like you would do using the standard RPC method) :
 * 	 RPCWrapper.RPC (string methodToCall, RPCMode receivers, type arg);
 * 
 * IF THE DELEGATE FOR YOUR RPC METHOD DOES NOT EXIST :
 * - You can create it : add a delegate which match your method signature.
 * - Add a dictionary of this delegate (do not forget to allocate it in AllocateDictionaries()).
 * - Create the corresponding overload of RegisterMethod().
 * - Idem for RPC().
 * - Create the correct Receive_****() method.
 * - Finally, fill the OnLevelWasLoaded() method.
 * 
 * This class will raise exceptions if methods not registered are called.
 * This class removes the RPCMode bug.
 */

[RequireComponent (typeof (NetworkView))]
public class RPCWrapper : MonoBehaviour {

	// Types of the delegates.
	public delegate void TargetMethod_void ();
	public delegate void TargetMethod_int (int arg);
	public delegate void TargetMethod_bool (bool arg);
	public delegate void TargetMethod_float (float arg);
	public delegate void TargetMethod_string (string arg);
	public delegate void TargetMethod_Vector3 (Vector3 arg);
	public delegate void TargetMethod_Quaternion (Quaternion arg);
	public delegate void TargetMethod_int_int (int arg1, int arg2);
	public delegate void TargetMethod_int_Vector3 (int arg1, Vector3 arg2);
	public delegate void TargetMethod_int_int_int (int arg1, int arg2, int arg3);
	
	// Dictionaries that contain all the registered methods (sorted by type).
	private static Dictionary<string, TargetMethod_void> methods_void;
	private static Dictionary<string, TargetMethod_int> methods_int;
	private static Dictionary<string, TargetMethod_bool> methods_bool;
	private static Dictionary<string, TargetMethod_float> methods_float;
	private static Dictionary<string, TargetMethod_string> methods_string;
	private static Dictionary<string, TargetMethod_Vector3> methods_Vector3;
	private static Dictionary<string, TargetMethod_Quaternion> methods_Quaternion;
	private static Dictionary<string, TargetMethod_int_int> methods_int_int;
	private static Dictionary<string, TargetMethod_int_Vector3> methods_int_Vector3;
	private static Dictionary<string, TargetMethod_int_int_int> methods_int_int_int;

	/*
	 * When 'true', let exceptions propagate to the top when methods are not registered.
	 * WARNING : When this option is 'true', ne message will be output when an RPC call fail !
	 */
	[Tooltip("Does the RPCWrapper output RPC fails ?")]
	[SerializeField] private bool ignoreErrors = false;

	// Put the network view in cache (faster than reference networkView directly).
	private static new NetworkView networkView = null;
	
	private void Awake () {
		// Get a reference towards the network view.
		if (networkView == null)
		{
			networkView = GetComponent<NetworkView> ();
			if (networkView == null)
				Debug.LogError (GetType ().Name + " : Not found network view");
			else
			{
				DontDestroyOnLoad (transform.gameObject);
				AllocateDictionaries ();
			}
		}
		else
			Debug.LogWarning (GetType ().Name + " : Multiple instances of " + GetType ().Name + " detected");
	}
	
	// Create the dictionaries of delegates.
	private void AllocateDictionaries () {
		methods_void = new Dictionary<string, TargetMethod_void> ();
		methods_int = new Dictionary<string, TargetMethod_int> ();
		methods_bool = new Dictionary<string, TargetMethod_bool> ();
		methods_float = new Dictionary<string, TargetMethod_float> ();
		methods_string = new Dictionary<string, TargetMethod_string> ();
		methods_Vector3 = new Dictionary<string, TargetMethod_Vector3> ();
		methods_Quaternion = new Dictionary<string, TargetMethod_Quaternion> ();
		methods_int_int = new Dictionary<string, TargetMethod_int_int> ();
		methods_int_Vector3 = new Dictionary<string, TargetMethod_int_Vector3> ();
		methods_int_int_int = new Dictionary<string, TargetMethod_int_int_int> ();
	}
	
	// Register a method.
	public static void RegisterMethod (TargetMethod_void method) { methods_void[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_int method) { methods_int[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_bool method) { methods_bool[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_float method) { methods_float[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_string method) { methods_string[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_Vector3 method) { methods_Vector3[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_Quaternion method) { methods_Quaternion[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_int_int method) { methods_int_int[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_int_Vector3 method) { methods_int_Vector3[method.Method.Name] = method; }
	public static void RegisterMethod (TargetMethod_int_int_int method) { methods_int_int_int[method.Method.Name] = method; }
	
	// Perform an RPC.
	public static void RPC (string methodName, RPCMode receivers) { networkView.RPC ("Receive_void", receivers, methodName); }
	public static void RPC (string methodName, RPCMode receivers, int arg) { networkView.RPC ("Receive_int", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, bool arg) { networkView.RPC ("Receive_bool", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, float arg) { networkView.RPC ("Receive_float", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, string arg) { networkView.RPC ("Receive_string", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, Vector3 arg) { networkView.RPC ("Receive_Vector3", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, Quaternion arg) { networkView.RPC ("Receive_Quaternion", receivers, methodName, arg); }
	public static void RPC (string methodName, RPCMode receivers, int arg1, int arg2) { networkView.RPC ("Receive_int_int", receivers, methodName, arg1, arg2); }
	public static void RPC (string methodName, RPCMode receivers, int arg1, Vector3 arg2) { networkView.RPC ("Receive_int_Vector3", receivers, methodName, arg1, arg2); }
	public static void RPC (string methodName, RPCMode receivers, int arg1, int arg2, int arg3) { networkView.RPC ("Receive_int_int_int", receivers, methodName, arg1, arg2, arg3); }
	
	// Receive an RPC and call the targetted method (will raise an exception if there is no such method registered).
	[RPC] private void Receive_void (string methodName) { if (methods_void.ContainsKey(methodName) || !ignoreErrors) methods_void[methodName] (); }
	[RPC] private void Receive_int (string methodName, int arg) { if (methods_int.ContainsKey(methodName) || !ignoreErrors) methods_int[methodName] (arg); }
	[RPC] private void Receive_bool (string methodName, bool arg) { if (methods_bool.ContainsKey(methodName) || !ignoreErrors) methods_bool[methodName] (arg); }
	[RPC] private void Receive_float (string methodName, float arg) { if (methods_float.ContainsKey(methodName) || !ignoreErrors) methods_float[methodName] (arg); }
	[RPC] private void Receive_string (string methodName, string arg) { if (methods_string.ContainsKey(methodName) || !ignoreErrors) methods_string[methodName] (arg); }
	[RPC] private void Receive_Vector3 (string methodName, Vector3 arg) { if (methods_Vector3.ContainsKey(methodName) || !ignoreErrors) methods_Vector3[methodName] (arg); }
	[RPC] private void Receive_Quaternion (string methodName, Quaternion arg) { if (methods_Quaternion.ContainsKey(methodName) || !ignoreErrors) methods_Quaternion[methodName] (arg); }
	[RPC] private void Receive_int_int (string methodName, int arg1, int arg2) { if (methods_int_int.ContainsKey(methodName) || !ignoreErrors) methods_int_int[methodName] (arg1, arg2); }
	[RPC] private void Receive_int_Vector3 (string methodName, int arg1, Vector3 arg2) { if (methods_int_Vector3.ContainsKey(methodName) || !ignoreErrors) methods_int_Vector3[methodName] (arg1, arg2); }
	[RPC] private void Receive_int_int_int (string methodName, int arg1, int arg2, int arg3) { if (methods_int_int_int.ContainsKey(methodName) || !ignoreErrors) methods_int_int_int[methodName] (arg1, arg2, arg3); }

	// Clear methods registered each time a new scene is loaded.
	private void OnLevelWasLoaded (int level) {
		methods_void.Clear ();
		methods_int.Clear ();
		methods_bool.Clear ();
		methods_float.Clear ();
		methods_string.Clear ();
		methods_Vector3.Clear ();
		methods_Quaternion.Clear ();
		methods_int_int.Clear ();
		methods_int_Vector3.Clear ();
		methods_int_int_int.Clear ();
	}
}
