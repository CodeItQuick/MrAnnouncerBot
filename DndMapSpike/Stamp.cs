﻿using System;
using Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DndMapSpike
{
	public class Stamp
	{
		// TODO: Any new writeable properties added need to be copied in the Clone method.

		public bool FlipHorizontally
		{
			get { return flipHorizontally; }
			set
			{
				if (flipHorizontally == value)
					return;

				image = null;
				flipHorizontally = value;
			}
		}
		public bool FlipVertically
		{
			get { return flipVertically; }
			set
			{
				if (flipVertically == value)
					return;

				image = null;
				flipVertically = value;
			}
		}

		public int RelativeX { get; set; }
		public int RelativeY { get; set; }
		bool flipHorizontally;
		bool flipVertically;
		StampRotation rotation;
		public StampRotation Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				if (rotation == value)
					return;

				rotation = value;
				image = null;
			}
		}

		Image image;
		int zOrder = -1;

		public Stamp()
		{
		}

		public Stamp(string fileName, int x = 0, int y = 0)
		{
			X = x;
			Y = y;
			FileName = fileName;
		}

		public string FileName { get; set; }
		static Stamp Clone(Stamp stamp)
		{
			Stamp result = new Stamp(stamp.FileName, stamp.X, stamp.Y);
			result.Contrast = stamp.Contrast;
			result.FlipHorizontally = stamp.FlipHorizontally;
			result.FlipVertically = stamp.FlipVertically;
			result.HueShift = stamp.HueShift;
			result.Lightness = stamp.Lightness;
			result.RelativeX = stamp.RelativeX;
			result.RelativeY = stamp.RelativeY;
			result.Rotation = stamp.Rotation;
			result.Saturation = stamp.Saturation;
			result.Scale = stamp.Scale;
			return result;
		}

		int GetAngle(StampRotation rotation)
		{
			switch (rotation)
			{
				case StampRotation.Ninety:
					return 90;
				case StampRotation.OneEighty:
					return 180;
				case StampRotation.TwoSeventy:
					return 270;
			}
			return 0;
		}
		
		public Image Image
		{
			get
			{
				if (image == null)
					image = ImageUtils.CreateImage(GetAngle(Rotation), HueShift, Saturation, Lightness, Contrast, ScaleX, ScaleY, FileName);

				return image;
			}
		}

		public double ScaleX
		{
			get
			{
				double horizontalFlipFactor = 1;
				if (FlipHorizontally)
					horizontalFlipFactor = -1;
				return Scale * horizontalFlipFactor;
			}
		}

		public double ScaleY
		{
			get
			{
				double verticalFlipFactor = 1;
				if (FlipVertically)
					verticalFlipFactor = -1;
				return Scale * verticalFlipFactor;
			}
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int ZOrder
		{
			get { return zOrder; }
			set
			{
				if (zOrder == value)
				{
					return;
				}

				zOrder = value;
				OnZOrderChanged();
			}
		}
		double scale = 1;
		public double Scale
		{
			get
			{
				return scale;
			}
			set
			{
				if (scale == value)
					return;

				image = null;
				scale = value;
			}
		}

		protected virtual void OnZOrderChanged()
		{
			ZOrderChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool ContainsPoint(Point point)
		{
			int left = GetLeft();
			int top = GetTop();
			if (point.X < left)
				return false;
			if (point.Y < top)
				return false;

			if (point.X > left + Width)
				return false;
			if (point.Y > top + Height)
				return false;

			return ImageUtils.HasPixelAt(Image, (int)(point.X - left), (int)(point.Y - top));
		}
		public int GetLeft()
		{
			return (int)Math.Round(X - Width / 2.0);
		}

		public int Width
		{
			get
			{
				return (int)Math.Round(Image.Source.Width);
			}
		}

		public int Height
		{
			get
			{
				return (int)Math.Round(Image.Source.Height);
			}
		}

		public double Diagonal
		{
			get
			{
				return Math.Sqrt(Width * Width + Height * Height);
			}
		}
		double hueShift;
		public double HueShift
		{
			get
			{
				return hueShift;
			}
			set
			{
				if (hueShift == value)
					return;

				image = null;
				hueShift = value;
			}
		}

		double saturation;
		public double Saturation
		{
			get
			{
				return saturation;
			}
			set
			{
				if (saturation == value)
					return;

				image = null;
				saturation = value;
			}
		}
		double lightness;
		public double Lightness
		{
			get
			{
				return lightness;
			}
			set
			{
				if (lightness == value)
					return;

				image = null;
				lightness = value;
			}
		}

		double contrast;
		public double Contrast
		{
			get
			{
				return contrast;
			}
			set
			{
				if (contrast == value)
					return;

				image = null;
				contrast = value;
			}
		}

		public int GetTop()
		{
			return (int)Math.Round(Y - Height / 2.0);
		}
		public void RotateRight()
		{
			switch (Rotation)
			{
				case StampRotation.Zero:
					Rotation = StampRotation.Ninety;
					break;
				case StampRotation.Ninety:
					Rotation = StampRotation.OneEighty;
					break;
				case StampRotation.OneEighty:
					Rotation = StampRotation.TwoSeventy;
					break;
				case StampRotation.TwoSeventy:
					Rotation = StampRotation.Zero;
					break;
			}
		}
		public void RotateLeft()
		{
			switch (Rotation)
			{
				case StampRotation.Zero:
					Rotation = StampRotation.TwoSeventy;
					break;
				case StampRotation.Ninety:
					Rotation = StampRotation.Zero;
					break;
				case StampRotation.OneEighty:
					Rotation = StampRotation.Ninety;
					break;
				case StampRotation.TwoSeventy:
					Rotation = StampRotation.OneEighty;
					break;
			}
		}
		public void Move(int deltaX, int deltaY)
		{
			X += deltaX;
			Y += deltaY;
		}

		public Stamp Copy(int deltaX, int deltaY)
		{
			Stamp result = Stamp.Clone(this);
			result.Move(deltaX, deltaY);
			return result;
		}

		public void ResetZOrder()
		{
			ZOrder = -1;
		}

		public bool HasNoZOrder()
		{
			return ZOrder == -1;
		}

		public event EventHandler ZOrderChanged;
	}
}
