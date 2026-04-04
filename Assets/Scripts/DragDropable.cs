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

	private readonly float throwForceMultiplier = 0.35f;
	private readonly float grabDistance = 0.5f;

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

	private Queue<Vector3> lastScreenPos = new Queue<Vector3>();
	private readonly uint queueCapacity = 5;

	private float getMouseSpeed()
	{
		int numItems = Mathf.Max(lastScreenPos.Count - 1, 1);
		Vector3 sum = new Vector3();
		while (lastScreenPos.Count > 1)
		{
			sum += lastScreenPos.Dequeue();
		}

		return Mathf.Abs((sum / numItems).magnitude);
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
		Rigidbody rb = GetComponent<Rigidbody>();
		isDragging = true;

		// Move object towards camera when picked up
		Vector3 cameraPos = camera.transform.position;
		transform.position = (WorldPos + cameraPos) * grabDistance;

		lastScreenPos = new Queue<Vector3>();
		
		// grab
		rb.useGravity = false;
		while(isDragging)
		{
			// dragging
			transform.position = WorldPos;
			yield return new WaitForFixedUpdate();
			if (lastScreenPos.Count >= queueCapacity)
				lastScreenPos.Dequeue();
			lastScreenPos.Enqueue(curScreenPos);
			yield return null;
		}
		// drop
		yield return new WaitForFixedUpdate();
		rb.useGravity = true;
		Vector3 flingDirection = WorldPos - cameraPos;
		Vector3 flingForce = flingDirection * (throwForceMultiplier * Mathf.Clamp(getMouseSpeed()/10, 0, 10));
		rb.AddForce(flingForce, ForceMode.Impulse);


	}
}
