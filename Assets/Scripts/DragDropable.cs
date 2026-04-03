/* DragDropable.cs
 * Based on a script by Game-Dev Mammad
 * https://www.youtube.com/watch?v=zo1dkYfIJVg
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropable : MonoBehaviour 
{
	[SerializeField] private InputAction press, screenPos;

	private float grabDistance = 0.5f;

	private Vector3 curScreenPos;

	Camera camera;
	private bool isDragging;

	private Vector3 WorldPos
	{
		get
		{
			float z = camera.WorldToScreenPoint(transform.position).z;
			return (camera.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z)));
		}
	}
	private bool isClickedOn
	{
		get
		{
			Ray ray = camera.ScreenPointToRay(curScreenPos);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				return hit.transform == transform;
			}
			return false;
		}
	}
	private void Awake() 
	{
		camera = Camera.main;
		screenPos.Enable();
		press.Enable();
		screenPos.performed += context => { curScreenPos = context.ReadValue<Vector2>(); };
		press.performed += _ => { if(isClickedOn) StartCoroutine(Drag()); };
		press.canceled += _ => { isDragging = false; };

	}

	private IEnumerator Drag()
	{
		isDragging = true;

		// Move object towards camera when picked up
		Vector3 cameraPos = camera.transform.position;
		transform.position = (WorldPos + cameraPos) * grabDistance;
		
		// grab
		GetComponent<Rigidbody>().useGravity = false;
		while(isDragging)
		{
			// dragging
			transform.position = WorldPos;
			Debug.Log("WorldPos: " + WorldPos.ToString());
			yield return null;
		}
		// drop
		GetComponent<Rigidbody>().useGravity = true;


	}
}
