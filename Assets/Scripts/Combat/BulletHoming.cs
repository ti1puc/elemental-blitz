using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : BulletBase
{
	[Header("Homing")]
	[SerializeField] private float maxHomingDistance = 10;
	[SerializeField, ReadOnly] private bool disableHoming;

	private Vector3 directionPlayer;

	protected override void OnEnable()
	{
		initialPosition = transform.position;
		disableHoming = false;
	}

	protected override void Update()
	{
		float zPos = moveSpeed * Time.deltaTime;
		float yPos = moveSpeedUpCorrection * Time.deltaTime;

		float distanceFromPlayer = Vector3.Distance(PlayerManager.Player.transform.position, transform.position);
		if (disableHoming == false)
			directionPlayer = (PlayerManager.Player.transform.position - transform.position).normalized;

		if (distanceFromPlayer > maxHomingDistance && disableHoming == false)
		{
			transform.position += directionPlayer * zPos;
		}
		else
		{
			disableHoming = true;

			transform.position += directionPlayer * zPos;
			// clamp Y em 0 pra bala não sair do cenario
			transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -200f, 0), transform.position.z);
		}

		// if bullet goes too far from obj destroy it
		distanceFromSpawn = Vector3.Distance(initialPosition, transform.position);
		if (distanceFromSpawn > maxDistance)
			DestroyBullet();
	}
}
