using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
	[SerializeField] private Vector2 minMaxRangeHorizontal;
	[SerializeField] private Vector2 minMaxRangeVertical;
	[Header("Tilt")]
	[SerializeField] private float tiltSpeed;
    [SerializeField] private float tiltAngleHorizontal;
	[SerializeField] private float tiltAngleVertical;
	[Header("References")]
	[SerializeField] private Transform visual;

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
		Quaternion targetAngle = Quaternion.Euler(xTilt, 0, zTilt);
		visual.rotation = Quaternion.Lerp(visual.rotation, targetAngle, Time.deltaTime * tiltSpeed);
	}
}
