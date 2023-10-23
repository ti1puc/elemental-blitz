using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundMovement : Enemy
{
	[Header("Hitable")]
	[SerializeField] private float makeHitableAfterPosZ = 50;
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 14;
	[SerializeField] private float stopOnPosZ = 40;
	[SerializeField] private float stopForSeconds = 2;
	[Header("Animation Movement")]
	[SerializeField] private float straightVerticalSpeed = 20;
	[SerializeField] private float moveStaightUntilPosZ = 40;
	[SerializeField] private float minStraightSpeed = 5;
	[Header("Debug")]
	[SerializeField, ReadOnly] private int toRight;
	[SerializeField, ReadOnly] private bool hasStopped;
	[SerializeField, ReadOnly] private float stoppedTimer;

	// quando é spawnado a classe mãe Enemy tem uma inicializacao
	// essa funcao é chamada apos inicializar
	protected override void AfterInitialization()
	{
		// ao ser criado ele pode vir da direita, ou da esquerda
		Direction();
		stoppedTimer = 0;
		hasStopped = false;
	}

	private void OnEnable()
	{
		Direction();
		stoppedTimer = 0;
		hasStopped = false;
	}

	private void Update()
	{
		// espera a initializacao da classe mãe Enemy
		if (hasInitialized == false) return;

		if (transform.position.z < stopOnPosZ && hasStopped == false)
		{
			stoppedTimer += Time.deltaTime;
			if (stoppedTimer > stopForSeconds)
			{
				stoppedTimer = 0;
				hasStopped = true;
			}
			return;
		}

		if (transform.position.z > moveStaightUntilPosZ)
			MoveStraight();
		else
			MoveForward();

		if (transform.position.z <= makeHitableAfterPosZ)
			MakeEnemyHitable();

		// caso o inimigo não for destruido pelo player
		if (transform.position.z <= moveRangeZ.x)
			DestroyEnemy();
	}

	private void Direction()
	{
		Vector3 currentPos = transform.position;
		toRight = Random.Range(0, 2);

		if (toRight == 1)
		{
			//toRight = true;
			currentPos.x = moveRangeX.x;
		}
		else
		{
			// toRight = false;
			currentPos.x = moveRangeX.y;
			toRight = -1;
		}

		currentPos.z = moveRangeZ.y;
		transform.position = currentPos;
	}

	private void MoveStraight()
	{
		float distance = transform.position.z - moveStaightUntilPosZ;
		float actualVerticalSpeed = straightVerticalSpeed * distance.Remap(0, moveRangeZ.y, 0, 1);

		float zPos = 0;
		if (actualVerticalSpeed > minStraightSpeed)
			zPos = actualVerticalSpeed * Time.deltaTime;
		else
			zPos = minStraightSpeed * Time.deltaTime;

		transform.Translate(0, 0, zPos);
	}

	private void MoveForward()
	{
		float zPos = moveSpeed * Time.deltaTime;
		transform.Translate(0, 0, zPos);
	}
}
