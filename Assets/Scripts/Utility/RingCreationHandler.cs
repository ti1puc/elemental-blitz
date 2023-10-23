using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RingCreationHandler : MonoBehaviour
{
	private enum Axis { X, Y, Z };

	[Header("Animation")]
	[SerializeField] private float rotateSpeed;
	[SerializeField] private Axis rotateAxis;
	[Header("Creation")]
	[SerializeField] private int numberOfPieces = 16;
	[SerializeField] private float ringRadius = 5;
	[SerializeField] private Axis createAxis = Axis.Z;
	[SerializeField] private bool setRotationOutwards;
	[Header("References")]
	[SerializeField] private bool useCustomPrefab = true;
	// esse ShowIf faz a variavel mostrar na Unity só se o bool for true, já o HideIf é o contrario
	[SerializeField, ShowIf("useCustomPrefab")] private GameObject ringPiecePrefab;
	[SerializeField, HideIf("useCustomPrefab")] private string emptyObjectName;

	private void Update()
	{
		Vector3 rotateDir = Vector3.zero;
		switch (rotateAxis)
		{
			case Axis.X:
				rotateDir = Vector3.right;
				break;
			case Axis.Y:
				rotateDir = Vector3.up;
				break;
			case Axis.Z:
				rotateDir = Vector3.forward;
				break;
			default:
				throw new ArgumentOutOfRangeException(name);
		}

		transform.Rotate(rotateDir * (rotateSpeed * Time.deltaTime), Space.Self);
	}

	[Button(null, EButtonEnableMode.Editor)]
	private void ClearChildren()
	{
		if (transform.childCount > 0)
		{
			foreach (Transform child in transform)
			{
				DestroyImmediate(child.gameObject);
			}
		}
	}

	[Button(null, EButtonEnableMode.Editor)]
	private void CreateRing()
	{
		ClearChildren();

		for (int i = 0; i < numberOfPieces; i++)
		{
			// calcula distancia em volta do circulo
			float radians = 2 * MathF.PI / numberOfPieces * i;

			// calcula vetor direçao
			float vertical = MathF.Sin(radians);
			float horizontal = MathF.Cos(radians);

			Vector3 spawnDir = Vector3.zero;
			switch (createAxis)
			{
				case Axis.X:
					spawnDir = new Vector3(0, horizontal, vertical);
					break;
				case Axis.Y:
					spawnDir = new Vector3(horizontal, 0, vertical);
					break;
				case Axis.Z:
					spawnDir = new Vector3(horizontal, vertical, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException(name);
			}

			// calcula posicao de spawn
			Vector3 spawnPos = transform.localPosition + (spawnDir * ringRadius);

			// spawn
			if (useCustomPrefab)
			{
				GameObject obj = Instantiate(ringPiecePrefab, spawnPos, Quaternion.identity, transform);

				if (setRotationOutwards)
				{
					obj.transform.LookAt(spawnDir);
					obj.transform.Rotate(Vector3.up * 180);
				}
			}
			else
			{
				GameObject emptyObj = new GameObject(emptyObjectName + "(Clone)");
				emptyObj.transform.position = spawnPos;
				emptyObj.transform.SetParent(transform);

				if (setRotationOutwards)
				{
					emptyObj.transform.LookAt(spawnDir);
					emptyObj.transform.Rotate(Vector3.up * 180);
				}
			}
		}
	}
}
