using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : PoolableObject
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float moveSpeed;
	//[Header("References")]
	private Vector3 initialPosition;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void Start()
	{
		//initialPosition = transform.position;
	}

	private void Update()
	{
		float zPos = moveSpeed * Time.deltaTime;
		transform.Translate(0, 0, zPos);
	}
	#endregion

	#region Public Methods
	public override void ResetPosition()
	{
		//transform.position = initialPosition;
	}

	public void DestroyBullet()
	{
		killAction?.Invoke(this);
	}
	#endregion

	#region Private Methods
	#endregion
}
