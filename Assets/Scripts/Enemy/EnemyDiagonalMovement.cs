using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyDiagonalMovement : Enemy
{
	[Header("Hitable")]
	[SerializeField] private float makeHitableAfterPosZ = 50;
	[Header("Straight Movement")]
	[SerializeField] private float straightVerticalSpeed = 14;
	[SerializeField] private float moveStaightUntilPosZ = 40;
	[SerializeField] private float minStraightSpeed = 5;
	[SerializeField] private float distanceToStartTransition = 5;
	[Header("Diagonal Movement")]
	public float verticalSpeed = 7;
	public float horizontalSpeed = 4;
	[SerializeField] private float speedMultiplierBeforeBoss = 2f;
	[Header("Tilt")]
	[SerializeField] private float tiltSpeed;
	[SerializeField] private float tiltAngleHorizontal;
	[SerializeField] private float tiltAngleVertical;
	[Header("Debug")]
	[SerializeField, ReadOnly] private int toRight;

	// quando é spawnado a classe mãe Enemy tem uma inicializacao
	// essa funcao é chamada apos inicializar
	protected override void AfterInitialization()
	{
		// ao ser criado ele pode vir da direita, ou da esquerda
		 SetDirection();
	}

	private void OnEnable()
	{
		SetDirection();
	}

	private void Update()
	{
		// espera a initializacao da classe mãe Enemy
		if (hasInitialized == false) return;

		if (transform.position.z > moveStaightUntilPosZ)
			MoveStraight();
		else
			MoveDiagonal();

		if (transform.position.z <= makeHitableAfterPosZ)
			MakeEnemyHitable();

		// girar o inimigo de acordo com o movimento
		float zTilt = toRight * tiltAngleHorizontal;
		float xTilt = tiltAngleVertical * -1;
		Quaternion targetAngle = Quaternion.Euler(xTilt, 180, zTilt);
		enemyVisual.rotation = Quaternion.Lerp(enemyVisual.rotation, targetAngle, Time.deltaTime * tiltSpeed);

		// caso o inimigo não for destruido pelo player
		if (transform.position.z <= moveRangeZ.x)
			DestroyEnemy();
	}

	private void SetDirection()
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
		float xPos = 0;
		float distance = transform.position.z - moveStaightUntilPosZ;
		float actualVerticalSpeed = straightVerticalSpeed * distance.Remap(0, moveRangeZ.y, 0, 1);

		if (distance <= distanceToStartTransition)
			xPos = horizontalSpeed * Time.deltaTime * toRight;

		float zPos = 0;
		if (actualVerticalSpeed > minStraightSpeed)
			zPos = actualVerticalSpeed * Time.deltaTime;
		else
			zPos = minStraightSpeed * Time.deltaTime;

		transform.Translate(-xPos, 0, zPos);
	}

	private void MoveDiagonal()
	{
		float zPos = verticalSpeed * Time.deltaTime;
		float xPos = (horizontalSpeed * Time.deltaTime) * toRight;
		if (WaveManager.HasStartedBossFight)
			zPos *= speedMultiplierBeforeBoss;

		transform.Translate(-xPos, 0, zPos);
	}
}

