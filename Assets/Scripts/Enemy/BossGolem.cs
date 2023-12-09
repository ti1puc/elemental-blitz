using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class BossGolem : Boss
{
	public enum Phases { Phase1, Phase2, Phase3 };

	[Header("Settings")]
	[SerializeField] private float phaseColorDuration = 1f;
	[SerializeField] private Color phase1Color;
	[SerializeField] private Color phase2Color;
	[SerializeField] private Color phase3Color;
	[SerializeField] private float emissionDuration = 1f;
	[SerializeField, ColorUsage(true, true)] private Color emissionFire;
	[SerializeField, ColorUsage(true, true)] private Color emissionWater;
	[SerializeField, ColorUsage(true, true)] private Color emissionLightning;
	[Header("Phase 1")]
	[SerializeField] private float changeElementEvery;
	[SerializeField] private GameObject phase1Shooters;
	[Header("Phase 2")]
	[SerializeField, Range(0, 100)] private int enterPhase2LifePercent = 50;
	[SerializeField] private float changeElement2Every;
	[SerializeField] private float punchEvery;
	[SerializeField] private float punchDuration;
	[SerializeField] private GameObject phase2Shooters;
	[Header("Phase 3")]
	[SerializeField, Range(0, 100)] private int enterPhase3LifePercent = 20;
	[SerializeField] private float changeElement3Every = 0.1f;
	[Header("References")]
	[SerializeField] private Animator golemAnimator;
	[SerializeField] private ParticleSystem punchExplosion;
	[SerializeField] private GameObject punchExplosionCollision;
	[SerializeField] private Transform punchExplosionPoint;
	[SerializeField] private SkinnedMeshRenderer meshRenderer;
	[SerializeField] private ElementManager elementManager;
	[Header("Debug")]
	[SerializeField, ReadOnly] private Phases currentPhase;
	[SerializeField, ReadOnly] private float healthPercentage;
	[SerializeField, ReadOnly] private float changeElementTimer;
	[SerializeField, ReadOnly] private bool isChangingElement;
	[SerializeField, ReadOnly] private float punchTimer;
	[SerializeField, ReadOnly] private bool isPunching;
	[SerializeField, ReadOnly] private float emissionTimer;
	[SerializeField, ReadOnly] private float phaseColorTimer;
	[SerializeField, ReadOnly] private bool hasChangedPhaseColor;
	[SerializeField, ReadOnly] private Color currentColor;
	[SerializeField, ReadOnly] private Color nextColor;
	[SerializeField, ReadOnly] private bool hasEnteredPhase2;
	[SerializeField, ReadOnly] private bool hasEnteredPhase3;

	private MaterialPropertyBlock materialPropertyBlockEmission;
	private MaterialPropertyBlock MaterialPropertyBlockEmission
	{
		get
		{
			if (materialPropertyBlockEmission == null)
				materialPropertyBlockEmission = new MaterialPropertyBlock();
			return materialPropertyBlockEmission;
		}
	}
	private MaterialPropertyBlock materialPropertyBlock;
	private MaterialPropertyBlock MaterialPropertyBlock
	{
		get
		{
			if (materialPropertyBlock == null)
				materialPropertyBlock = new MaterialPropertyBlock();
			return materialPropertyBlock;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		InitializeEnemy(Vector3.zero, Vector3.zero, null);

		currentPhase = Phases.Phase1;
		changeElementTimer = 0;
		punchTimer = 0;
		isPunching = false;
		emissionTimer = 0;
		phaseColorTimer = 0;
		currentColor = emissionFire;
		isChangingElement = false;
		elementManager.CurrentElement = Element.Fire;
		hasEnteredPhase2 = false;
		hasEnteredPhase3 = false;

		phase1Shooters.SetActive(true);
		phase2Shooters.SetActive(false);

		MakeEnemyHitable();
	}

	private void Update()
	{
		// espera a initializacao da classe mãe Enemy
		if (hasInitialized == false) return;

		if (isChangingElement)
		{
			if (emissionTimer < emissionDuration)
			{
				MaterialPropertyBlockEmission.SetColor("_Emission", Color.Lerp(currentColor, nextColor, emissionTimer / emissionDuration));
				meshRenderer.SetPropertyBlock(MaterialPropertyBlockEmission, 1);
				emissionTimer += Time.deltaTime;
			}
			else
				isChangingElement = false;
		}

		healthPercentage = (healthController.CurrentHealth / (float)healthController.MaxHealth) * 100;
		if (healthPercentage <= enterPhase2LifePercent)
			currentPhase = Phases.Phase2;
		if (healthPercentage <= enterPhase3LifePercent)
			currentPhase = Phases.Phase3;

		switch (currentPhase)
		{
			case Phases.Phase1:
				HandleBossPhase1();
				break;
			case Phases.Phase2:
				HandleBossPhase2();
				break;
			case Phases.Phase3:
				HandleBossPhase3();
				break;
			default:
				break;
		}
	}

	private void HandleBossPhase1()
	{
		phase1Shooters.SetActive(true);
		phase2Shooters.SetActive(false);

		changeElementTimer += Time.deltaTime;
		if (changeElementTimer > changeElement2Every)
		{
			ChangeElement();
			changeElementTimer = 0f;
		}
	}

	private void HandleBossPhase2()
	{
		if (!hasEnteredPhase2)
		{
			StartCoroutine(ChangePhaseColor(Phases.Phase2));
			hasEnteredPhase2 = true;
		}

		phase1Shooters.SetActive(false);
		phase2Shooters.SetActive(true);

		punchTimer += Time.deltaTime;
		if (punchTimer > punchEvery)
		{
			if (!isPunching)
				StartCoroutine(Punch());

			isPunching = true;
			punchTimer = 0f;
		}

		if (!isPunching)
		{
			changeElementTimer += Time.deltaTime;
			if (changeElementTimer > changeElement2Every)
			{
				ChangeElement();
				changeElementTimer = 0f;
			}
		}
	}

	private void HandleBossPhase3()
	{
		if (!hasEnteredPhase3)
		{
			golemAnimator.SetTrigger("GoCrazy");
			StartCoroutine(ChangePhaseColor(Phases.Phase3));
			hasEnteredPhase3 = true;
		}

		phase1Shooters.SetActive(false);
		phase2Shooters.SetActive(true);

		changeElementTimer += Time.deltaTime;
		if (changeElementTimer > changeElement3Every)
		{
			ChangeElement();
			changeElementTimer = 0f;
		}
	}

	private IEnumerator ChangePhaseColor(Phases phase)
	{
		switch (phase)
		{
			case Phases.Phase1:
				break;
			case Phases.Phase2:
				phaseColorTimer = 0;
				while (phaseColorTimer < phaseColorDuration)
				{
					MaterialPropertyBlock.SetColor("_Tint", Color.Lerp(phase1Color, phase2Color, emissionTimer / phaseColorDuration));
					meshRenderer.SetPropertyBlock(MaterialPropertyBlock, 0);
					phaseColorTimer += Time.deltaTime;
					yield return null;
				}
				break;
			case Phases.Phase3:
				phaseColorTimer = 0;
				while (phaseColorTimer < phaseColorDuration)
				{
					MaterialPropertyBlock.SetColor("_Tint", Color.Lerp(phase2Color, phase3Color, emissionTimer / phaseColorDuration));
					meshRenderer.SetPropertyBlock(MaterialPropertyBlock, 0);
					phaseColorTimer += Time.deltaTime;
					yield return null;
				}
				break;
			default:
				break;
		}
	}

	private void ChangeElement()
	{
		currentColor = SelectColor(elementManager.CurrentElement);
		nextColor = SelectColor(elementManager.ChangeElementExternal());

		isChangingElement = true;
		emissionTimer = 0;
	}

	private Color SelectColor(Element element)
	{
		switch (element)
		{
			case Element.Lightning:
				return emissionLightning;
			case Element.Water:
				return emissionWater;
			case Element.Fire:
				return emissionFire;
			default:
				return emissionFire;
		}
	}

	private IEnumerator Punch()
	{
		golemAnimator.SetTrigger("Punch");
		yield return new WaitForSeconds(2.3f);

		Instantiate(punchExplosion, punchExplosionPoint.transform.position, punchExplosion.transform.rotation);
		GameObject collision = Instantiate(punchExplosionCollision, punchExplosionPoint.transform.position, punchExplosionCollision.transform.rotation);
		yield return new WaitForSeconds(punchDuration - 2.3f);

		isPunching = false;
		punchTimer = 0f;
		Destroy(collision);
	}
}
