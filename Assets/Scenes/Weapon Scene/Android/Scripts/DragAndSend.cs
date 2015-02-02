﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragAndSend : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] private int weaponNumber;
	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	//Vector2 touchPosition;
	Transform startParent;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		//startParent = transform.parent;
		//GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	#region IDragHandler implementation
	
	public void OnDrag (PointerEventData eventData)
	{
		//transform.position = touchPosition;
		transform.position = Input.mousePosition;
		Debug.Log (transform.position.y);
	}
	
	#endregion

	#endregion

	#region IEndDragHandler implementation
	
	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
			if(transform.position.y > 320){
				Debug.Log ("Send the message");
				//call RPC function here
				//RPCWrapper.RPC ("AddWeapon", RPCMode.Server, Player.id.Get (), weaponNumber);
			}

		transform.position = startPosition;
	}
	
	#endregion

	// Update is called once per frame
	/*void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			touchPosition = Input.GetTouch(0).deltaPosition;
		}

	}*/



}
