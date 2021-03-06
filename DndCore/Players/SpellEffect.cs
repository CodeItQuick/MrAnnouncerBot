﻿using System;
using System.Linq;

namespace DndCore
{
	public class SpellEffect
	{
		public int TimeOffset { get; set; } = int.MinValue;
		public string EffectName { get; set; }
		public int Hue { get; set; }
		public int Saturation { get; set; } = 100;
		public int Brightness { get; set; } = 100;
		public int SecondaryHue { get; set; }
		public int SecondarySaturation { get; set; } = 100;
		public int SecondaryBrightness { get; set; } = 100;
		public int XOffset { get; set; } = 0;
		public int YOffset { get; set; } = 0;
		public double VelocityX { get; set; } = 0;
		public double VelocityY { get; set; } = 0;
		public double Scale { get; set; } = 1;
		public double Rotation { get; set; } = 0;
		public double AutoRotation { get; set; } = 0;
		public SpellEffect(string effectName, int hue, int saturation = 100, int brightness = 100, 
			double scale = 1, double rotation = 0, double autoRotation = 0, int timeOffset = int.MinValue,
			int secondaryHue = 0, int secondarySaturation = 100, int secondaryBrightness = 100, 
			int xOffset = 0, int yOffset = 0, double velocityX = 0, double velocityY = 0)
		{
			EffectName = effectName;
			Hue = hue;
			Brightness = brightness;
			Saturation = saturation;
			SecondaryHue = secondaryHue;
			SecondaryBrightness = secondaryBrightness;
			XOffset = xOffset;
			YOffset = yOffset;
			VelocityX = velocityX;
			VelocityY = velocityY;
			SecondarySaturation = secondarySaturation;
			Scale = scale;
			Rotation = rotation;
			AutoRotation = autoRotation;
			TimeOffset = timeOffset;
		}
		public SpellEffect()
		{

		}
	}
}