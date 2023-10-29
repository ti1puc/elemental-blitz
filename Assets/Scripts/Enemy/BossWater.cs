using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWater : Boss
{
	public enum Phases { Phase1, Phase2 };

	[Header("Hitable")]
	[SerializeField] private float makeHitableAfterPosZ = 50;
	[Header("Movement")]
	[SerializeField] private float verticalSpeed = 10;
	[SerializeField] private float horizontalSpeed = 10;
	[Header("Straight Movement")]
	[SerializeField] private float straightAnimationSpeed = 25;
	[SerializeField] private float straightBeforeBackSpeed = 5;
	[SerializeField] private float backSpeed = 10;
	[SerializeField] private float moveStaightUntilPosZ = 40;
	[SerializeField] private float waitSecondsToBack = .5f;
	[SerializeField] private float rotateAnimationDuration = 2f;
	[Header("Phase 1")]
	[SerializeField] private float changeSideEverySeconds = 10f;
	[SerializeField] private float shootLaserEverySeconds = 10f;
	[SerializeField] private float laserDuration = .8f;
	[SerializeField] private Laser laser;
	[Header("Phase 2")]
	[SerializeField, Range(0, 100)] private int enterPhase2LifePercent = 50;
	[SerializeField] private float changeSideEverySecondsP2 = 10f;
	[SerializeField] private float shootLaserEverySecondsP2 = 10f;
	[SerializeField] private float laserDurationP2 = .8f;
	[SerializeField] private float goBackToPosZ = 40;
	[SerializeField] private float goBackToPosZSpeed = 16;
	[SerializeField] private float sideDeathLaserEverySeconds = 20f;
	[SerializeField] private float waitStartDeathLaser = 1.5f;
	[SerializeField] private float deathLaserDuration = 5f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Phases currentPhase;
	[SerializeField, ReadOnly] private float distance;
	[SerializeField, ReadOnly] private float actualSpeedAnimation;
	[SerializeField, ReadOnly] private float timerToBack;
	[SerializeField, ReadOnly] private float timerRotateAnimation;
	[SerializeField, ReadOnly] private bool isReturning;
	[SerializeField, ReadOnly] private bool hasFinishedAnimation;
	[SerializeField, ReadOnly] private bool hasRotated;
	[SerializeField, ReadOnly] private float changeSideTimer;
	[SerializeField, ReadOnly] private float deathLaserTimer;
	[SerializeField, ReadOnly] private bool isDeathLasing;
	[SerializeField, ReadOnly] private int lastSide;
	[SerializeField, ReadOnly] private int currentSide;
	[SerializeField, ReadOnly] private float shootLaserTimer;
	[SerializeField, ReadOnly] private float healthPercentage;

	private Coroutine deathLaserCoroutine;

	protected override void AfterInitialization()
	{
		timerToBack = 0;
		timerRotateAnimation = 0;
		changeSideTimer = 0;
		currentPhase = Phases.Phase1;
		shootLaserTimer = shootLaserEverySeconds/2;
	}

	private void Update()
	{
		// espera a initializacao da classe mãe Enemy
		if (hasInitialized == false) return;

		// animacaozinha pra dar um efeito de easy out no boss
		if (!hasFinishedAnimation)
		{
			if (!isReturning)
			{
				distance = transform.position.z - moveStaightUntilPosZ;
				actualSpeedAnimation = distance - straightAnimationSpeed;
				MoveStraight(actualSpeedAnimation < straightBeforeBackSpeed ? straightBeforeBackSpeed : actualSpeedAnimation, 1);

				if (transform.position.z <= moveStaightUntilPosZ)
					isReturning = true;
			}
			else
			{
				if (timerToBack < waitSecondsToBack)
					MoveStraight(straightBeforeBackSpeed, 1);

				timerToBack += Time.deltaTime;
				if (timerToBack > waitSecondsToBack)
				{
					MoveStraight(backSpeed , -1);
					if (transform.position.z > moveStaightUntilPosZ)
						hasFinishedAnimation = true;
				}
			}
		}
		else
		{
			if (!hasRotated)
			{
				animator.SetTrigger("Rotate");
				hasRotated = true;
			}

			timerRotateAnimation += Time.deltaTime;
			if (timerRotateAnimation > rotateAnimationDuration && transform.position.z <= makeHitableAfterPosZ)
				MakeEnemyHitable();
		}

		healthPercentage = (healthController.CurrentHealth / (float)healthController.MaxHealth) * 100;
		if (healthPercentage <= enterPhase2LifePercent)
			currentPhase = Phases.Phase2;

		if (IsHitable)
		{
			switch (currentPhase)
			{
				case Phases.Phase1:
					HandleBossPhase1();
					break;
				case Phases.Phase2:
					HandleBossPhase2();
					break;
				default:
					break;
			}
		}
	}

	private void MoveStraight(float speed, int direction)
	{
		float zPos = speed * Time.deltaTime;
		transform.Translate(0, 0, zPos * direction);
	}

	private void HandleBossPhase1()
	{
		changeSideTimer += Time.deltaTime;
		if (changeSideTimer > changeSideEverySeconds)
		{
			currentSide = Random.Range(0, 3);
			if (currentSide == lastSide)
			{
				currentSide++;
				if (currentSide > 2)
					currentSide = 0;
			}

			lastSide = currentSide;
			changeSideTimer = 0;
		}

		switch (currentSide)
		{
			case 0:
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), horizontalSpeed * Time.deltaTime);
				break;
			case 1:
				transform.position += transform.right * (horizontalSpeed * Time.deltaTime);
				break;
			case 2:
				transform.position -= transform.right * (horizontalSpeed * Time.deltaTime);
				break;
			default:
				break;
		}

		if (transform.position.x < moveRangeX.x)
			transform.position = new Vector3(moveRangeX.x, transform.position.y, transform.position.z);

		if (transform.position.x > moveRangeX.y)
			transform.position = new Vector3(moveRangeX.y, transform.position.y, transform.position.z);

		shootLaserTimer += Time.deltaTime;
		if (shootLaserTimer > shootLaserEverySeconds)
		{
			laser.ChargeLaserToShoot();
			StartCoroutine(DisableLaser(laserDuration));
			shootLaserTimer = 0;
		}
	}

	private IEnumerator DisableLaser(float duration)
	{
		yield return new WaitForSeconds(duration + laser.ChargeTime);
		laser.StopLaser();
		shootLaserTimer = 0;
	}

	private void HandleBossPhase2()
	{
		deathLaserTimer += Time.deltaTime;
		if (deathLaserTimer > sideDeathLaserEverySeconds)
		{
			if (transform.position.z < goBackToPosZ)
				MoveStraight(goBackToPosZSpeed, -1);

			if (transform.position.z >= goBackToPosZ && deathLaserCoroutine == null)
				deathLaserCoroutine = StartCoroutine(DeathLaserCoroutine());

			//if (currentSide == 1)
			//	animator.SetTrigger("SideLeft");
			//else
			//	animator.SetTrigger("SideRight");

			isDeathLasing = true;
		}

		if (isDeathLasing) return;

		if (!isDeathLasing && transform.position.z > moveStaightUntilPosZ)
			MoveStraight(goBackToPosZSpeed, 1);

		changeSideTimer += Time.deltaTime;
		if (changeSideTimer > changeSideEverySecondsP2)
		{
			currentSide = Random.Range(0, 3);
			if (currentSide == lastSide)
			{
				currentSide++;
				if (currentSide > 2)
					currentSide = 0;
			}

			animator.SetTrigger("Rotate");

			lastSide = currentSide;
			changeSideTimer = 0;
		}

		switch (currentSide)
		{
			case 0:
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), horizontalSpeed * Time.deltaTime);
				break;
			case 1:
				transform.position += transform.right * (horizontalSpeed * Time.deltaTime);
				break;
			case 2:
				transform.position -= transform.right * (horizontalSpeed * Time.deltaTime);
				break;
			default:
				break;
		}

		if (transform.position.x < moveRangeX.x)
			transform.position = new Vector3(moveRangeX.x, transform.position.y, transform.position.z);

		if (transform.position.x > moveRangeX.y)
			transform.position = new Vector3(moveRangeX.y, transform.position.y, transform.position.z);

		shootLaserTimer += Time.deltaTime;
		if (shootLaserTimer > shootLaserEverySecondsP2)
		{
			laser.ChargeLaserToShoot();
			StartCoroutine(DisableLaser(laserDurationP2));
			shootLaserTimer = 0;
		}
	}

	private IEnumerator DeathLaserCoroutine()
	{
		laser.IsDeathLaser = true;
		WaveManager.BlinkIndicator(currentSide);

		yield return new WaitForSeconds(waitStartDeathLaser);
		laser.ChargeLaserToShoot();

		yield return new WaitForSeconds(deathLaserDuration + laser.ChargeTime);
		laser.StopLaser();
		laser.IsDeathLaser = false;

		deathLaserTimer = 0;
		isDeathLasing = false;
		deathLaserCoroutine = null;
	}
}
