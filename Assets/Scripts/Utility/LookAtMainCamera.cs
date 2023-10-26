using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
	#region Fields
	private Transform lookAtTransform;
	#endregion

	#region Unity Messages
	private void Awake()
	{
		lookAtTransform = Camera.main.transform;
	}

	private void Update()
	{
		// melhoria de performance
		if (gameObject.activeSelf == false) return;

		UpdateLookAtRotation();
	}
	#endregion

	#region Private Methods
	private void UpdateLookAtRotation()
	{
		if (lookAtTransform == null)
		{
			enabled = false;
			return;
		}
		transform.rotation = Quaternion.LookRotation(lookAtTransform.forward);
	}
	#endregion
}
