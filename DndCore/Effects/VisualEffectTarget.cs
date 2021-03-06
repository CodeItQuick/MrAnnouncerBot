﻿using System;
using System.Linq;
using DndCore;

namespace DndCore
{
	public enum TargetPage
	{
		None,
		Main,
		Skills,
		Equipment
	}

	public class VisualEffectTarget
	{
		public string entryName = string.Empty;
		public Vector screenPosition;
		public Vector targetOffset;
		public TargetPage targetPage = TargetPage.None;
		public TargetType targetType;

		public VisualEffectTarget()
		{
			screenPosition = Vector.zero;
			targetOffset = Vector.zero;
		}

		public VisualEffectTarget(Vector vector) : this(vector.x, vector.y)
		{
		}

		public VisualEffectTarget(double x, double y) : this()
		{
			targetType = TargetType.ScreenPosition;
			screenPosition = new Vector(x, y);
		}

		public VisualEffectTarget(TargetType targetType, Vector screenPosition, Vector targetOffset) : this()
		{
			this.targetType = targetType;
			this.screenPosition = screenPosition;
			this.targetOffset = targetOffset;
		}
	}
}
