using System;
using Tesseract.Backends;
using Tesseract.Events;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class MouseStateControl: Control
	{
		public MouseStateControl(): base()
		{
		
		}
		
		PatternList overbackground;
		public PatternList OverBackground
		{
			get { return overbackground; }
			set { overbackground = value; }
		}
		
		PatternList downbackground;
		public PatternList DownBackground
		{
			get { return downbackground; }
			set { downbackground = value; }
		}
		
		PatternList overdownbackground;
		public PatternList OverDownBackground
		{
			get { return overdownbackground; }
			set { overdownbackground = value; }
		}
		
		double overopacity = 0;
		public double OverOpacity
		{
			get { return overopacity; }
			set { overopacity = value; }
		}
		
		bool autooveropacity = true;
		public bool AutoOverOpacity
		{
			get { return autooveropacity; }
			set { autooveropacity = value; }
		}
		
		double downopacity = 0;
		public double DownOpacity
		{
			get { return downopacity; }
			set { downopacity = value; }
		}
		
		bool autodownopacity = true;
		public bool AutoDownOpacity
		{
			get { return autodownopacity; }
			set { autodownopacity = value; }
		}
		
		double overdownopacity = 0;
		public double OverDownOpacity
		{
			get { return overdownopacity; }
			set { overdownopacity = value; }
		}
		
		bool autooverdownopacity = true;
		public bool AutoOverDownOpacity
		{
			get { return autooverdownopacity; }
			set { autooverdownopacity = value; }
		}
		
		public override bool MouseOver
		{
			get { return base.MouseOver; }
			set
			{
				base.MouseOver = value;
				
				if (autooveropacity)
				{
					overdownopacity = (value & MouseDown) ? 1 : 0;
					overopacity = value ? 1 : 0;
				}
			}
		}
		
		public override bool MouseDown
		{
			get { return base.MouseDown; }
			set
			{
				base.MouseDown = value;
				
				if (autodownopacity)
				{
					overdownopacity = (value & MouseOver) ? 1 : 0;
					downopacity = value ? 1 : 0;
				}
			}
		}
	}
}
