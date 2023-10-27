using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemyStationaryMovement : Enemy
{
	[Header("Animation Movement")]
	[SerializeField] private float moveUpUntilPosZ = 40;
	[SerializeField] private float moveUpSpeed = 5;
	[SerializeField] private float startMovingOnYPosZ = 30;
	[SerializeField] private float moveYSpeed = 5;
	[SerializeField] private float rotationSpeed = 5;
	[Header("Go Away Movement")]
	[SerializeField] private float moveAwaySpeed = 5;
	[Header("Alive Time")]
	[SerializeField] private float maxAliveTime = 8;
	[Header("Debug")]
	[SerializeField, ReadOnly] private float aliveTimer;
	[SerializeField, ReadOnly] private bool isGoingAway;
	[SerializeField, ReadOnly] private float dotProduct;

	// quando é spawnado a classe mãe Enemy tem uma inicializacao
	// essa funcao é chamada apos inicializar
	protected override void AfterInitialization()
	{
		aliveTimer = 0;
		isGoingAway = false;
	}

	private void OnEnable()
	{
		aliveTimer = 0;
		isGoingAway = false;
	}

	private void Update()
	{
		// espera a initializacao da classe mãe Enemy
		if (hasInitialized == false) return;

		// pega a posiçao e direcao do spawner desconsiderando o Y
		Vector3 spawnerPos = parentEnemySpawner.transform.position;
		spawnerPos.y = 0;
		Vector3 directionToSpawner = (spawnerPos - transform.position).normalized;

		if (transform.position.z < moveUpUntilPosZ)
			AnimationMovement();
		else if (transform.position.y < 0 && isHitable == false)
		{
			// ajustar posicao em y
			float yPos = moveYSpeed * Time.deltaTime;
			transform.Translate(0, yPos, 0);
		}
		else
		{
			// rotaciona pra frente (usando o spawner como "frente" pra facilitar)
			Quaternion targetRotation = Quaternion.LookRotation(directionToSpawner);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}

		// se estiver olhando pra frente faz o enemy ser hitable
		// produto escalar retorna 1 se estiver na msm direcao
		dotProduct = Vector3.Dot(transform.forward, directionToSpawner);
		if (dotProduct >= 0.9998f)
			MakeEnemyHitable();

		if (isHitable)
		{
			// espera um tempo pra ir embora se não for destruido
			aliveTimer += Time.deltaTime;
			if (aliveTimer > maxAliveTime || WaveManager.HasStartedBossFight)
			{
				DisableShoot();
				GoAwayMovement();
			}
		}

		// caso o inimigo ta indo embora e não for destruido pelo player
		if (isGoingAway && transform.position.z <= moveRangeZ.x)
			DestroyEnemy();
	}

	private void AnimationMovement()
	{
		float zPos = moveUpSpeed * Time.deltaTime;
		float yPos = moveYSpeed * Time.deltaTime;

		if (transform.position.z > startMovingOnYPosZ && transform.position.y < 0)
			transform.Translate(0, yPos, zPos);
		else
			transform.Translate(0, 0, zPos);
	}

	private void GoAwayMovement()
	{
		isGoingAway = true;

		float zPos = moveAwaySpeed * Time.deltaTime;
		transform.Translate(0, 0, zPos);
	}
}
