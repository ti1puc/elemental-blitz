using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovementMenu : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float resetWhenZPos = -207.33f;
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] private Vector3 initialPosition;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void Awake()
	{
		initialPosition = transform.position;
	}

	private void Update()
	{
		// background goes backward
		float zPos = moveSpeed * Time.deltaTime;
		transform.position += transform.right * zPos;

		// if past a certain point, resets position
		if (transform.position.z <= resetWhenZPos)
			transform.position = initialPosition;
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
