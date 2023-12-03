using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using TMPro;

public class PlayerController : MonoBehaviour
{
	#region Fields
	[Header("Movement")]
    [SerializeField] private float moveSpeed;
	[SerializeField] private Vector2 minMaxRangeHorizontal;
	[SerializeField] private Vector2 minMaxRangeVertical;
	[Header("Tilt")]
	[SerializeField] private float tiltSpeed;
    [SerializeField] private float tiltAngleHorizontal;
	[SerializeField] private float tiltAngleVertical;
	[Header("Health")]
	[SerializeField, Range(0, 100)] private int percentShowLowHealth = 30;
	[SerializeField] private GameObject lowHealthWarning;
	[SerializeField] private TMP_Text lowHealthText;
	[Header("Debuffs")]
	[SerializeField] private bool enableDebuffs = true;
	[SerializeField] private float slowMoveSpeed = 7;
	[SerializeField] private float slowDuration = 1;
	[SerializeField] private float stunDuration = 1;
	[SerializeField] private float noDmgDuration = 1;
	[SerializeField] private GameObject slowDebuff;
	[SerializeField] private GameObject stunDebuff;
	[SerializeField] private GameObject noDmgDebuff;
	[Header("Dash")]
	[SerializeField] private float dashDuration = 1;
	[SerializeField] private float dashSpeed = 100;
	[SerializeField] private ParticleSystem dashVfx;
	[SerializeField] private TrailRenderer[] dashTrails;
	[Header("References")]
	[SerializeField] private Transform playerVisual;
	[SerializeField] private ShootInput shootInput;
	[Header("Debug")]
	[SerializeField, ReadOnly] private HealthController healthController;
	[SerializeField, ReadOnly] private float defaultMoveSpeed;
	[SerializeField, ReadOnly] private float slowDebuffCounter;
	[SerializeField, ReadOnly] private float stunDebuffCounter;
	[SerializeField, ReadOnly] private float noDmgDebuffCounter;
	[SerializeField, ReadOnly] private float dashTimer;
	[SerializeField, ReadOnly] private int dashTimes;
	[SerializeField, ReadOnly] private bool isDashing;
	#endregion

	public bool HasSlowDebuff => slowDebuffCounter > 0;
	public bool HasStunDebuff => stunDebuffCounter > 0;
	public bool HasNoDmgDebuff => noDmgDebuffCounter > 0;
	public int DashTimes => dashTimes;
	public bool IsDashing => isDashing;

	#region Unity Messages
	private void Awake()
	{
		healthController = GetComponent<HealthController>();

		defaultMoveSpeed = moveSpeed;
		dashTimer = 0f;
		dashTimes = 0;

		slowDebuffCounter = 0f;
		stunDebuffCounter = 0f;
		noDmgDebuffCounter = 0f;

		slowDebuff.SetActive(false);
		stunDebuff.SetActive(false);
		noDmgDebuff.SetActive(false);

		foreach (var trail in dashTrails)
			trail.emitting = false;
	}

	private void Update()
    {
        // horizontal calculations
        float horizontal = Input.GetAxis("Horizontal");
		float xPos = horizontal * Time.deltaTime * moveSpeed;
		float zTilt = horizontal * tiltAngleHorizontal * -1;

		// vertical calculations
		float vertical = Input.GetAxis("Vertical");
		float zPos = vertical * Time.deltaTime * moveSpeed;
		float xTilt = vertical * tiltAngleVertical;

		// translate position
		transform.Translate(xPos, 0, zPos);

        // clamp position
        float clampedX = Mathf.Clamp(transform.position.x, minMaxRangeHorizontal.x, minMaxRangeHorizontal.y);
        float clampedZ = Mathf.Clamp(transform.position.z, minMaxRangeVertical.x, minMaxRangeVertical.y);
		transform.position = new Vector3(clampedX, 0, clampedZ);

		// slight tilt player (only visual to not affect movement)
		Quaternion targetAngle = Quaternion.Euler(stunDebuffCounter > 0 ? 0 : xTilt, 0, stunDebuffCounter > 0 ? 0 : zTilt); // rotate to 0 if stunned
		playerVisual.rotation = Quaternion.Lerp(playerVisual.rotation, targetAngle, Time.deltaTime * tiltSpeed);

		// mostrar warning de low health
		float healthPercent = (healthController.CurrentHealth / (float)healthController.MaxHealth) * 100;
		lowHealthWarning.SetActive(healthPercent <= percentShowLowHealth);
		if (healthPercent <= percentShowLowHealth)
			lowHealthText.text = Mathf.RoundToInt(healthPercent) + "%";

		// check debuffs
		if (slowDebuffCounter > 0)
		{
			moveSpeed = slowMoveSpeed;

			slowDebuffCounter -= Time.deltaTime;
			if (slowDebuffCounter <= 0)
			{
				moveSpeed = defaultMoveSpeed;
				slowDebuffCounter = 0f;
			}
		}

		if (stunDebuffCounter > 0)
		{
			moveSpeed = 0;

			stunDebuffCounter -= Time.deltaTime;
			if (stunDebuffCounter <= 0)
			{
				moveSpeed = defaultMoveSpeed;
				stunDebuffCounter = 0f;
			}
		}

		if (noDmgDebuffCounter > 0)
		{
			shootInput.DisableShoot = true;

			noDmgDebuffCounter -= Time.deltaTime;
			if (noDmgDebuffCounter <= 0)
			{
				moveSpeed = defaultMoveSpeed;
				shootInput.DisableShoot = false;
			}
		}

		// UI debuffs
		slowDebuff.SetActive(PlayerManager.PlayerController.HasSlowDebuff);
		stunDebuff.SetActive(PlayerManager.PlayerController.HasStunDebuff);
		noDmgDebuff.SetActive(PlayerManager.PlayerController.HasNoDmgDebuff);

		// dash
		if (dashTimes > 0 && Input.GetKeyDown(KeyCode.Space) && !isDashing)
		{
			dashTimer = dashDuration;
			moveSpeed = dashSpeed;
			isDashing = true;

			Instantiate(dashVfx, transform.position, dashVfx.transform.rotation);
            foreach (var trail in dashTrails)
				trail.emitting = true;
        }

		if (dashTimer > 0)
		{
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
			{
				moveSpeed = defaultMoveSpeed;
				isDashing = false;
				dashTimes--;

				foreach (var trail in dashTrails)
					trail.emitting = false;
			}
		}
	}
	#endregion

	public void StartSlowDebuff()
	{
		if (!enableDebuffs) return;
		slowDebuffCounter = slowDuration;
	}

	public void StartStunDebuff()
	{
		if (!enableDebuffs) return;
		stunDebuffCounter = stunDuration;
	}

	public void StartNoDmgDebuff()
	{
		if (!enableDebuffs) return;
		noDmgDebuffCounter = noDmgDuration;
	}

	public void SetupDash()
	{
		dashTimes = 2;
	}
}
