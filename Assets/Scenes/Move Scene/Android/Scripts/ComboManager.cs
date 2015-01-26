using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ComboManager : MonoBehaviour {

	[Tooltip("Prefab of Arrow Up")]
	[SerializeField] private GameObject ArrowUp = null;

	private string ArrowTag = null;
	private GameObject Arrow;
	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered
	private List<Vector3> touchPositions = new List<Vector3>(); //store all the touch positions in list


	// Use this for initialization
	void Start () {

		if (ArrowUp == null)
			Debug.LogError (GetType ().Name + " : The field 'ArrowUp' is empty");

		RPCWrapper.RegisterMethod(ComboTask);
		dragDistance = Screen.height*20/100; //dragDistance is 20% height of the screen 
	
	}

	public void ComboTask(string comboTag) {

		Debug.Log("combo sent");

		ArrowTag = comboTag;

		Debug.Log(ArrowTag);

		if (ArrowTag.Equals ("ArrowUp")) {
			Debug.Log ("instantiate arrow ");
			Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0 );
			Arrow = Instantiate (ArrowUp, pos, Quaternion.identity) as GameObject;

		}

	}
	
	// Update is called once per frame
	void Update () {

		foreach (Touch touch in Input.touches)  //use loop to detect more than one swipe
		{ 	
			if (touch.phase == TouchPhase.Moved) //add the touches to list as the swipe is being made
			{
				touchPositions.Add(touch.position);
			}
			
			if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
			{

				fp =  touchPositions[0]; //get first touch position from the list of touches
				lp =  touchPositions[touchPositions.Count-1]; //last touch position 
				
				//Check if drag distance is greater than 20% of the screen height
				if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
				{//It's a drag
					//check if the drag is vertical or horizontal 
					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{   //If the horizontal movement is greater than the vertical movement...
						if ((lp.x>fp.x))  //If the movement was to the right)
						{   //Right swipe
							Debug.Log("Right Swipe");
						}
						else
						{   //Left swipe
							Debug.Log("Left Swipe"); 
						}
					}
					else
					{   //the vertical movement is greater than the horizontal movement
						if (lp.y>fp.y)  //If the movement was up
						{   //Up swipe
							Debug.Log("Up Swipe"); 
							if(ArrowTag.Equals("ArrowUp")) {
								Destroy(Arrow);
								ArrowTag = null;

							}
						}
						else
						{   //Down swipe
							Debug.Log("Down Swipe");
						}
					}
				} 
			}
			else
			{   //It's a tap as the drag distance is less than 20% of the screen height
				
			}
		}
	
	}
}
