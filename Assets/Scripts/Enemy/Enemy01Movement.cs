using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy01Movement : Enemy
{
	#region Fields
	[Header("Movement")]
	public float moveSpeed = 3;
	public float horizontalSpd = 4;
	public float spawnX = 22;
	public float spawnZ = 42;
	public float maxZ = -30;
	[Header("Tilt")]
	[SerializeField] private float tiltSpeed;
	[SerializeField] private float tiltAngleHorizontal;
	[SerializeField] private float tiltAngleVertical;
	[Header("Debug")]
	[SerializeField, ReadOnly] private int toRight;

	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	public void OnEnable()
	{
		// ao ser criado ele pode vir da direita, ou da esquerda
		 Direction();
	}

	private void Update()
	{
		float zPos = moveSpeed * Time.deltaTime;

		float xPos = (horizontalSpd * Time.deltaTime) * toRight;
		transform.Translate(xPos, 0, -zPos);

		// girar o inimigo de acordo com o movimento
		float zTilt = toRight * tiltAngleHorizontal;
		float xTilt = tiltAngleVertical * -1;
		Quaternion targetAngle = Quaternion.Euler(xTilt, 180, zTilt);
		enemyVisual.rotation = Quaternion.Lerp(enemyVisual.rotation, targetAngle, Time.deltaTime * tiltSpeed);

		// caso o inimigo não for destruido pelo player
		if (transform.position.z <= maxZ)
			DestroyEnemy();
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void Direction()
	{
		Vector3 currentPos = transform.position;
		toRight = Random.Range(0, 2);

		if (toRight == 1)
		{
			//toRight = true;
			currentPos.x = -spawnX;
		}
		else
		{
			// toRight = false;
			currentPos.x = spawnX;
			toRight = -1;
		}

		currentPos.z = spawnZ;
		transform.position = currentPos;
	}

	#endregion
}

