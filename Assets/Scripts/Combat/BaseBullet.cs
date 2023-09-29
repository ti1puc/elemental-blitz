using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : PoolableObject
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float moveSpeed;
	[SerializeField] private float maxDistance = 50;
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] private float distanceFromSpawn;
	[SerializeField, ReadOnly] private Vector3 initialPosition;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void OnEnable()
	{
		// hold and update initial position every time bullet obj is enabled
		initialPosition = transform.position;
	}

	private void Update()
	{
		float zPos = moveSpeed * Time.deltaTime;
		transform.Translate(0, 0, zPos);

		// if bullet goes too far from obj destroy it
		distanceFromSpawn = Vector3.Distance(initialPosition, transform.position);
		if (distanceFromSpawn > maxDistance)
			DestroyPoolable();
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
