using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : BulletBase
{
	[Header("Homing")]
	[SerializeField] private bool goFowardFirst;
	[SerializeField, ShowIf("goFowardFirst")] private float distanceForward;
	[SerializeField] private float maxHomingDistance = 10;
	[SerializeField, ReadOnly] private bool disableHoming;
	[SerializeField, ReadOnly] private float forwardDistance;

	private Vector3 directionPlayer;

	protected override void OnEnable()
	{
		initialPosition = transform.position;
		disableHoming = false;
		forwardDistance = 0;
	}

	protected override void Update()
	{
		if (goFowardFirst)
		{
			if (forwardDistance < distanceForward)
			{
				GoForward();
				forwardDistance += Time.deltaTime;
			}
			else
				Homing();
		}
		else
			Homing();

		// if bullet goes too far from obj destroy it
		distanceFromSpawn = Vector3.Distance(initialPosition, transform.position);
		if (distanceFromSpawn > maxDistance)
			DestroyBullet();
	}

	private void Homing()
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
	}

	private void GoForward()
	{
		float zPos = moveSpeed * Time.deltaTime;
		transform.Translate(0, 0, zPos);
	}
}
