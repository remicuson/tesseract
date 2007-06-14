using System;
using System.ComponentModel;
using Tesseract.Controls;
using Tesseract.TIM;

namespace Tesseract.Geometry
{
	public enum DistanceOrientation { Horizontal, Vertical }
	public enum DistanceUnits { Pixels, Percent, Mm }
	
    /// <summary>
    /// Represents a distance
    /// </summary>
	[TypeConverter(typeof(MeasurementConverter))]
	public class Distance
	{
		public Distance(Control c, DistanceOrientation o, DistanceUnits u, double v)
		{
			this.control = c;
			this.orientation = o;
			this.units = u;
			this.val = v;
		}
		
		public Distance(double p): this(null, DistanceOrientation.Horizontal, DistanceUnits.Pixels, p) { }
		
		Control control;
        /// <summary>
        /// The control associated with this distance
        /// </summary>
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		DistanceOrientation orientation;
        /// <summary>
        /// The orientation of this distance (is it measuring horizontally or vertically)
        /// </summary>
		public DistanceOrientation Orientation
		{
			get { return orientation; }
			set { orientation = value; }
		}
		
		DistanceUnits units;
        /// <summary>
        /// The units of this distance
        /// </summary>
		public DistanceUnits Units
		{
			get { return units; }
			set { units = value; }
		}
		
		double val;
        /// <summary>
        /// The value of this distance
        /// </summary>
		public double Value
		{
			get { return val; }
			set { val = value; }
		}
		
        /// <summary>
        /// Returns the value of this distance with units converted to pixels
        /// </summary>
		public double Pixels
		{
			get
			{
				switch (units)
				{
					case DistanceUnits.Pixels:
						return val;
					case DistanceUnits.Mm:
						return (orientation == DistanceOrientation.Horizontal ? control.Window.DpiX : control.Window.DpiY) * val / 2.45;
					case DistanceUnits.Percent:
						if (control == null || control.Parent == null)
							return 0;
						
						return (val / 100) * (orientation == DistanceOrientation.Horizontal ? control.Parent.Path.W.Pixels : control.Parent.Path.H.Pixels);
						
					default:
						return 0;
				}
			}
		}

        /// <summary>
        /// Creates an exact copy of this distance
        /// </summary>
        /// <returns></returns>
        public Distance Clone()
        {
            return new Distance(control, orientation, units, val);
        }
		
		public static Distance operator +(Distance m1, Distance m2) 
   		{
   			if (m1 == null && m2 == null)
   				throw new Exception("Attempted to add null to null");
   			
   			if ((m1 == null) || (m1.Value == 0))
   				return m2;
   			if ((m2 == null) || (m2.Value == 0))
   				return m1;
   				
   			if (m1.Units == m2.Units)
   				return new Distance(m1.Control, m1.Orientation, m1.Units, m1.Value + m2.Value);
   			
   			return new Distance(m1.Control, m1.Orientation, DistanceUnits.Pixels, m1.Pixels + m2.Pixels);
   		}
   		
   		public static Distance operator -(Distance m1, Distance m2) 
   		{
   			if (m1 == null && m2 == null)
   				throw new Exception("Attempted to subtract null from null");
   			
   			if ((m1 == null) || (m1.Value == 0))
   				return new Distance(m2.Control, m2.Orientation, m2.Units, -m2.Value);
   			if ((m2 == null) || (m2.Value == 0))
   				return m1;
   				
   			if (m1.Units == m2.Units)
   				return new Distance(m1.Control, m1.Orientation, m1.Units, m1.Value - m2.Value);
   			
   			return new Distance(m1.Control, m1.Orientation, DistanceUnits.Pixels, m1.Pixels - m2.Pixels);
   		}
   		
   		public static Distance operator -(Distance m1) 
   		{
   			return new Distance(m1.Control, m1.Orientation, m1.Units, -m1.Value);
   		}
   		
   		public static Distance operator *(Distance m1, Distance m2) 
   		{
   			if (m1 == null || m2 == null)
   				throw new Exception("Attempted to multiply by null");
   				
   			if (m1.Units == m2.Units)
   				return new Distance(m1.Control, m1.Orientation, m1.Units, m1.Value * m2.Value);
   			
   			return new Distance(m1.Control, m1.Orientation, DistanceUnits.Pixels, m1.Pixels * m2.Pixels);
   		}
   		
   		public static Distance operator /(Distance m1, Distance m2) 
   		{
   			if (m1 == null || m2 == null)
   				throw new Exception("Attempted to divide by null");
   				
   			if (m1.Units == m2.Units)
   				return new Distance(m1.Control, m1.Orientation, m1.Units, m1.Value / m2.Value);
   			
   			return new Distance(m1.Control, m1.Orientation, DistanceUnits.Pixels, m1.Pixels / m2.Pixels);
   		}
   		
   		public static implicit operator Distance(double d)
   		{
   			return new Distance(d);
   		}
   		
   		public static implicit operator double(Distance m)
   		{
            if (m == null)
                return 0;

   			return m.Pixels;
   		}
   		
   		public static Distance FromString(string s)
   		{
   			Control c = Tesseract.TIM.TIM.currentControl;
   			
   			s = s.Trim();
   			
            if (string.IsNullOrEmpty(s) || (s.ToLower() == "null"))
   				return null;
   			if (s.EndsWith("px"))
   				return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Pixels, GetNum(s));
   			if (s.EndsWith("mm"))
   				return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Mm, GetNum(s));
   			if (s.EndsWith("cm"))
   				return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Mm, GetNum(s) * 10);
   			if (s.EndsWith("inches"))
   				return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Mm, GetNum(s) * 24.5);
   			if (s.EndsWith("%"))
   				return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Percent, GetNum(s));
   			
   			return new Distance(c, DistanceOrientation.Horizontal, DistanceUnits.Pixels, GetNum(s));
   		}
   		
   		static double GetNum(string s)
   		{
   			string tmp = string.Empty;
   			
   			foreach (char c in s)
   				if (Char.IsNumber(c) || (c == '.'))
   					tmp += c;
   					
   			return (tmp != string.Empty) ? double.Parse(tmp) : 0;
   		}
	}
	
	class MeasurementConverter: TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
		{
			return (t == typeof(double)) || (t == typeof(string));
		}
		
		public override object ConvertFrom(ITypeDescriptorContext c, System.Globalization.CultureInfo i, object o)
		{
			if (o is string)
				return Distance.FromString((string)o);
			if (o is double)
				return new Distance((double)o);
			
			return new Distance(0);
		}
	}
}
