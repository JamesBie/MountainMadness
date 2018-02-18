﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hexmetrics{
	public const float outerRadius = 10f;
	public const float innerRadius = outerRadius * 0.866025404f;
	public const float elevationChange = 2f;


	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};



}
