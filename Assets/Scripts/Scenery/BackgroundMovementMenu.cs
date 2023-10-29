using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovementMenu : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float resetWhenZPosition;
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] private Vector3 initialPosition;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void Awake()
	{
		initialPosition = transform.localPosition;
	}

	private void Update()
	{
		// background goes backward
		float zPos = moveSpeed * Time.deltaTime;
		transform.localPosition += transform.forward * zPos;

		// if past a certain point, resets position
		if (transform.localPosition.z <= resetWhenZPosition)
			transform.localPosition = initialPosition;
	}

	private void OnValidate()
	{
		if (resetWhenZPosition > 0)
			resetWhenZPosition = -resetWhenZPosition;
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
