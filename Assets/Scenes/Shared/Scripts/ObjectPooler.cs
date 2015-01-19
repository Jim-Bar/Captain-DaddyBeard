using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Create a pool of pre-allocated objects in order to avoid instantiation and garbage collection while the game runs.
 * 
 * HOW TO USE THIS CLASS :
 * 
 * 1 - Create an instance of it in the script which will be responsible for the creation of objects. Like this :
 * public class SomeClassName : MonoBehaviour {
 *   [SerializeField] private ObjectPooler pool; // Declare the pool.
 * 
 *   // Other stuff...
 * }
 * 
 * 2 - New fields (for pool configuration) have appeared in the editor (see the inspector, in the script 'SomeClassName' section).
 * Particularly it is there where the prefab which will be cloned is defined (field 'Pooled Object').
 * 
 * 3 - Instead of calling 'Instantiate (...)', call 'pool.GetPooledObject (...)'.
 * And Instead of calling 'Destroy (...)', call 'ObjectPooler.ReleasePooledObject (...)'.
 * 
 * 
 * Notes :
 * - You can create several instances of this class.
 * - The pool is instantiated the first time 'GetPooledObject ()' is called, which can happen in the middle of the game.
 * To avoid this behaviour, call 'Initialize ()' earlier (on scene loading for example). 'Initialize ()' will then instantiate the objects.
 * - This class never check if 'Instantiate ()' succeed or failed.
 * - The object "Pool Stock" will retain all unused objects as children, and for all instances (shared between instances).
 */
[System.Serializable]
public class ObjectPooler {

	// All unused pooled object will be children of this object.
	private static GameObject poolStock = null;
	
	[SerializeField] [Tooltip("Prefab to clone")] private GameObject pooledObject = null; // Prefab of the object to create.
	[SerializeField] [Tooltip("Number of objects initially created")] [Range(0, 1000)] private int pooledAmount = 10; // Initial size of the pool.
	[SerializeField] [Tooltip("Is the pool allowed to create new objects ?")] private bool willGrow = false; // Will the pool grow if needed ?
	[SerializeField] [Tooltip("Display warnings when there is no more objects available and the pool cannot grow")] private bool logUnavailability = true;
	[SerializeField] [Tooltip("Display pratical information such as object instantiations, etc...")] private bool verbose = true;

	private bool initialized = false; // The pool is initialized on the first call to GetPooledObject().
	private List<GameObject> pooledObjects = null; // The objects.

	/*
	 * Call this method instead of 'Instantiate (...)'.
	 * Return an object or null in case of fail. You should ALWAYS check that the object returned is not null.
	 * You have to provide a parent to the object (most often it will be 'this.transform'). The position and rotation are optional (prefab's ones will be used).
	 * If there is no more objects available in the pool but if the pool can grow, the method will instantiate a new object and will add it to the pool.
	 * If there is no more objects available in the pool and if the pool is not allowed to grow, the method return null.
	 */
	public GameObject GetPooledObject (Transform parent) { return GetPooledObject (parent, pooledObject.transform.position); }
	public GameObject GetPooledObject (Transform parent, Vector3 position) { return GetPooledObject (parent, position, pooledObject.transform.rotation); }
	public GameObject GetPooledObject (Transform parent, Vector3 position, Quaternion rotation) {
		// Initialize the pool the first time.
		if (!initialized)
			Initialize ();
		
		// Search for a available pooled object.
		for (int i = 0; i < pooledObjects.Count; i++)
			if (!pooledObjects[i].activeInHierarchy)
				return InitializePooledObject (pooledObjects[i], parent, position, rotation);
		
		// There is no pooled object available but the pool can grow.
		if (willGrow)
		{
			if (verbose)
				Debug.Log (GetType().Name + " : Pool grows of one object '" + pooledObject.name + "'.");
			GameObject obj = MonoBehaviour.Instantiate(pooledObject) as GameObject;
			pooledObjects.Add(obj);
			return InitializePooledObject (obj, parent, position, rotation);
		}
		
		// There is no pooled object available and the pool is not allowed grow.
		if (logUnavailability)
			Debug.LogWarning (GetType().Name + " : No pooled object '" + pooledObject.name + "' available.");
		return null;
	}

	/*
	 * Call this method instead of 'Destroy (...)'.
	 * Simply make an object available for future use.
	 * Note that this is a static method, so the correct call is 'ObjectPooler.ReleasePooledObject (...)'.
	 */
	public static void ReleasePooledObject (GameObject obj) {
		// Check that the pool stock hierarchy does not change (happens when the application terminates, causing errors).
		if (poolStock != null && !poolStock.activeSelf)
			obj.transform.SetParent (poolStock.transform);
		obj.SetActive (false); // Make the object available.
	}

	/*
	 * Instantiate the pool stock object.
	 * Instantiate the objects according to the initial number of objects and make thme available.
	 */
	public void Initialize () {
		// Create the pool stock object (which keeps the unused objects as children).
		if (poolStock == null)
			poolStock = new GameObject ("Unused pooled objects");

		// Allocate the pool.
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = MonoBehaviour.Instantiate(pooledObject) as GameObject;
			obj.transform.SetParent (poolStock.transform); // Put the object in pool stock hierarchy.
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}

		// Inform that the pool has just been instantiated (as this can happen in the middle of the game; see the header notes for details).
		if (verbose)
			Debug.Log (GetType().Name + " : Pooled objects '" + pooledObject.name + "' have just been instantiated.");
	}

	/*
	 * Simply initialize an object and return it.
	 */
	private GameObject InitializePooledObject (GameObject obj, Transform parent, Vector3 position, Quaternion rotation) {
		initialized = true; // Mark the pool as initialized.
		obj.transform.parent = parent;
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.SetActive (true);
		
		return obj;
	}
}
