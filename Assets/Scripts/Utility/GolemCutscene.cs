using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCutscene : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float wakeDelay = 1.5f;
	[SerializeField] private float emissionDuration = 1f;
	[SerializeField, ColorUsage(true, true)] private Color emissionNull;
	[SerializeField, ColorUsage(true, true)] private Color emissionColor;
	[Header("References")]
	[SerializeField] private Animator animator;
	[SerializeField] private SkinnedMeshRenderer meshRenderer;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool startEmission;
	[SerializeField, ReadOnly] private float emissionTimer;

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

	private void Start()
	{
		emissionTimer = 0;
	}

	private void Update()
	{
		if (startEmission)
		{
			if (emissionTimer < emissionDuration)
			{
				MaterialPropertyBlock.SetColor("_Emission", Color.Lerp(emissionNull, emissionColor, emissionTimer / emissionDuration));
				meshRenderer.SetPropertyBlock(MaterialPropertyBlock, 1);
				emissionTimer += Time.deltaTime;
			}
		}
	}

	public void WakeUpAnim()
	{
		animator.SetTrigger("WakeUp");
		AudioManager.Instance.PlaySFXBoss("snd_GolemWake");
		StartCoroutine(WaitSecondsToWake());
	}

	private IEnumerator WaitSecondsToWake()
	{
		yield return new WaitForSeconds(wakeDelay);
		startEmission = true;
	}
}
