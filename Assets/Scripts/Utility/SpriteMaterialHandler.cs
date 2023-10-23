using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteMaterialHandler : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private Texture2D mainTexture;
	[SerializeField] private bool useGlow;
	[SerializeField, ShowIf("useGlow")] private Texture2D glowMask;
	[SerializeField, ShowIf("useGlow"), ColorUsageAttribute(true, true)] private Color glowColor;
	[SerializeField, ShowIf("useGlow")] private float glowFactor;
	[SerializeField] private bool useOutline;
	[SerializeField, ShowIf("useOutline")] private Color outlineColor;
	[SerializeField, ShowIf("useOutline")] private float outlineThickness;
	[SerializeField] private float colorLerp;
	[Header("Debug")]
	[SerializeField, ReadOnly] private SpriteRenderer spriteRenderer;

	// usar isso aqui permite mudar as settings de um material sem mudar pra todos
	// se n usar isso teria q criar um material pra cada sprite
	private MaterialPropertyBlock materialPropertyBlock;
	#endregion

	#region Properties
	private MaterialPropertyBlock MaterialPropertyBlock
	{
		get
		{
			if (materialPropertyBlock == null)
				materialPropertyBlock = new MaterialPropertyBlock();
			return materialPropertyBlock;
		}
	}
	#endregion

	#region Unity Messages
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateMaterial();
	}

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnValidate()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateMaterial();
	}
	#endregion

	#region Private Methods
	private void UpdateMaterial()
	{
		if (spriteRenderer == null) return;
		if (mainTexture == null) return;

		MaterialPropertyBlock.SetTexture("_MainTex", mainTexture);
		MaterialPropertyBlock.SetInteger("_UseGlow", useGlow ? 1 : 0);
		MaterialPropertyBlock.SetInteger("_UseOutline", useOutline ? 1 : 0);
		MaterialPropertyBlock.SetFloat("_ColorLerp", colorLerp);

		if (useGlow)
		{
			MaterialPropertyBlock.SetTexture("_GlowMask", glowMask);
			MaterialPropertyBlock.SetColor("_GlowColor", glowColor);
			MaterialPropertyBlock.SetFloat("_GlowFactor", glowFactor);
		}

        if (useOutline)
		{
			MaterialPropertyBlock.SetColor("_OutlineColor", outlineColor);
			MaterialPropertyBlock.SetFloat("_OutlineThickness", outlineThickness);
		}

		spriteRenderer.SetPropertyBlock(MaterialPropertyBlock);
	}
	#endregion
}
