using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Laser : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private int laserDamage = 10;
	[SerializeField] private float raycastMaxDistance = 100;
	[SerializeField] private float offsetWhenHit = 2;
	[Header("Laser Charge")]
	[SerializeField] private float laserChargeDuration = 1;
	[SerializeField] private float laserChargeScaling = 1.1f;
	[SerializeField] private float waitDisableChargeVfx = .5f;
	[Header("Death Laser")]
	[SerializeField] private float defaultLineWidth = 3f;
	[SerializeField] private float deathLaserLineWidth = 12f;
	[SerializeField] private float defaulRaycastRadius = 3f;
	[SerializeField] private float deathLaserRaycastRadius = 12f;
	[Header("References")]
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private GameObject startVfx;
	[SerializeField] private GameObject endVfx;
	[SerializeField] private ParticleSystem laserChargeVfx;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Vector3 initialChargeScale;
	[SerializeField, ReadOnly] private bool isLaserCharging;
	[SerializeField, ReadOnly] private float laserChargeTimer;
	[SerializeField, ReadOnly] private bool isLaserEnabled;
	[SerializeField, ReadOnly] private List<ParticleSystem> particles = new List<ParticleSystem>();

	public float ChargeTime => laserChargeDuration;
	public float LineWidth => IsDeathLaser ? deathLaserLineWidth : defaultLineWidth;
	public float RaycastRadius => IsDeathLaser ? deathLaserRaycastRadius : defaulRaycastRadius;
	public int Damage => IsDeathLaser ? 999 : laserDamage;
	public bool IsDeathLaser { get; set; }

	private void Awake()
	{
		// pegar as particulas
		for (int i = 0; i < startVfx.transform.childCount; i++)
		{
			var particle = startVfx.transform.GetChild(i).GetComponent<ParticleSystem>();
			if (particle != null)
				particles.Add(particle);
		}

		for (int i = 0; i < endVfx.transform.childCount; i++)
		{
			var particle = endVfx.transform.GetChild(i).GetComponent<ParticleSystem>();
			if (particle != null)
				particles.Add(particle);
		}

		initialChargeScale = laserChargeVfx.transform.localScale;
	}

	private void OnEnable()
	{
		laserChargeTimer = 0;

		laserChargeVfx.Stop();
		laserChargeVfx.gameObject.SetActive(false);
		StopLaser();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (Physics.SphereCast(transform.position, RaycastRadius, transform.forward, out RaycastHit hit, raycastMaxDistance, layerMask))
			Gizmos.DrawSphere(hit.point, RaycastRadius);
		else
			Gizmos.DrawSphere(hit.point, RaycastRadius);
	}

	private void Update()
	{
		lineRenderer.startWidth = LineWidth;
		lineRenderer.endWidth = LineWidth;

		if (isLaserCharging)
		{
			laserChargeTimer += Time.deltaTime;
			laserChargeVfx.transform.localScale *= laserChargeScaling;
			if (laserChargeVfx.transform.localScale.x > 1)
				laserChargeVfx.transform.localScale = new Vector3(1, 1, 1);

			if (laserChargeTimer > laserChargeDuration)
			{
				ShootLaser();
				laserChargeTimer = 0;
			}
			return;
		}

		if (isLaserEnabled == false) return;

		if (Physics.SphereCast(transform.position, RaycastRadius, transform.forward, out RaycastHit hit, raycastMaxDistance, layerMask))
		{
			float distance = Vector3.Distance(hit.collider.transform.position, transform.position);
			Vector3 hitPoint = Vector3.forward * (distance + offsetWhenHit);

			lineRenderer.SetPosition(1, hitPoint);
			endVfx.transform.localPosition = hitPoint;

			if (hit.collider.CompareTag("Player"))
			{
				// deal damage
				PlayerManager.PlayerCollision.TakeDamageExternal(Damage);
				AudioManager.Instance.PlaySFX("snd_CriticalDamage");
			}
		}
		else
		{
			Vector3 hitPoint = Vector3.forward * raycastMaxDistance;

			lineRenderer.SetPosition(1, hitPoint);
			endVfx.transform.localPosition = hitPoint;
		}
	}

	[Button]
	public void ChargeLaserToShoot()
	{
		laserChargeVfx.gameObject.SetActive(true);
		laserChargeVfx.Play();

		AudioManager.Instance.PlaySFXBoss("snd_LaserCharge");

		isLaserCharging = true;
	}

	[Button]
	public void StopLaser()
	{
		isLaserCharging = false;
		laserChargeVfx.Stop();
		laserChargeVfx.gameObject.SetActive(false);
		laserChargeVfx.transform.localScale = initialChargeScale;

		AudioManager.Instance.StopSoundBoss();
		lineRenderer.enabled = false;
		isLaserEnabled = false;

		foreach (var particle in particles)
			particle.Stop();
	}

	private IEnumerator DisableLaserChargeVfx()
	{
		yield return new WaitForSeconds(waitDisableChargeVfx);
		laserChargeVfx.Stop();
		laserChargeVfx.gameObject.SetActive(false);
	}

	private void ShootLaser()
	{
		isLaserCharging = false;
		StartCoroutine(DisableLaserChargeVfx());
		laserChargeVfx.transform.localScale = initialChargeScale;

		AudioManager.Instance.PlaySFXBoss("snd_Laser");
		lineRenderer.enabled = true;
		isLaserEnabled = true;

		foreach (var particle in particles)
			particle.Play();
	}
}
