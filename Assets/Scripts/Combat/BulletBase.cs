using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] protected int damage = 1;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected float moveSpeedUpCorrection;
	[SerializeField] protected float maxDistance = 50;
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] protected float distanceFromSpawn;
	[SerializeField, ReadOnly] protected Vector3 initialPosition;

	public int Damage => damage;

	#region Unity Messages
	protected virtual void OnEnable()
	{
		// hold and update initial position every time bullet obj is enabled
		initialPosition = transform.position;
    }

	protected virtual void Update()
	{
		float zPos = moveSpeed * Time.deltaTime;
		float yPos = moveSpeedUpCorrection * Time.deltaTime;

		if (transform.position.y < 0)
			transform.Translate(0, yPos, zPos);
		else
			transform.Translate(0, 0, zPos);

		// if bullet goes too far from obj destroy it
		distanceFromSpawn = Vector3.Distance(initialPosition, transform.position);
		if (distanceFromSpawn > maxDistance)
			DestroyBullet();
	}
    #endregion

    #region Public Methods
	public void DestroyBullet()
	{
		ObjectPoolManager.DespawnGameObject(gameObject);
	}

    #endregion

    #region Private Methods
    #endregion
}
