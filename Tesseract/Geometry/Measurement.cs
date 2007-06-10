using System;
using System.ComponentModel;
using Tesseract.Controls;
using Tesseract.TIM;

namespace Tesseract.Geometry
{
	public enum MeasurementOrientation { Horizontal, Vertical }
	public enum MeasurementUnits { Pixels, Percent, Mm }
	
	[TypeConverter(typeof(MeasurementConverter))]
	public class Measurement
	{
		public Measurement(Control c, MeasurementOrientation o, MeasurementUnits u, double v)
		{
			this.control = c;
			this.orientation = o;
			this.units = u;
			this.val = v;
		}
		
		public Measurement(double p): this(null, MeasurementOrientation.Horizontal, MeasurementUnits.Pixels, p) { }
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		MeasurementOrientation orientation;
		public MeasurementOrientation Orientation
		{
			get { return orientation; }
			set { orientation = value; }
		}
		
		MeasurementUnits units;
		public MeasurementUnits Units
		{
			get { return units; }
			set { units = value; }
		}
		
		double val;
		public double Value
		{
			get { return val; }
			set { val = value; }
		}
		
		public double Pixels
		{
			get
			{
				switch (units)
				{
					case MeasurementUnits.Pixels:
						return val;
					case MeasurementUnits.Mm:
						return (orientation == MeasurementOrientation.Horizontal ? control.Window.DpiX : control.Window.DpiY) * val / 2.45;
					case MeasurementUnits.Percent:
						if (control.Parent == null)
							return 0;
						
						return (val / 100) * (orientation == MeasurementOrientation.Horizontal ? control.Parent.Path.W.Pixels : control.Parent.Path.H.Pixels);
						
					default:
						return 0;
				}
			}
		}
		
		public static Measurement operator +(Measurement m1, Measurement m2) 
   		{
   			if (m1 == null && m2 == null)
   				throw new Exception("Attempted to add null to null");
   			
   			if ((m1 == null) || (m1.Value == 0))
   				return m2;
   			if ((m2 == null) || (m2.Value == 0))
   				return m1;
   				
   			if (m1.Units == m2.Units)
   				return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value + m2.Value);
   			
   			return new Measurement(m1.Control, m1.Orientation, MeasurementUnits.Pixels, m1.Pixels + m2.Pixels);
   		}
   		
   		public static Measurement operator -(Measurement m1, Measurement m2) 
   		{
   			if (m1 == null && m2 == null)
   				throw new Exception("Attempted to subtract null from null");
   			
   			if ((m1 == null) || (m1.Value == 0))
   				return new Measurement(m2.Control, m2.Orientation, m2.Units, -m2.Value);
   			if ((m2 == null) || (m2.Value == 0))
   				return m1;
   				
   			if (m1.Units == m2.Units)
   				return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value - m2.Value);
   			
   			return new Measurement(m1.Control, m1.Orientation, MeasurementUnits.Pixels, m1.Pixels - m2.Pixels);
   		}
   		
   		public static Measurement operator -(Measurement m1) 
   		{
   			return new Measurement(m1.Control, m1.Orientation, m1.Units, -m1.Value);
   		}
   		
   		public static Measurement operator *(Measurement m1, Measurement m2) 
   		{
   			if (m1 == null || m2 == null)
   				throw new Exception("Attempted to multiply by null");
   				
   			if (m1.Units == m2.Units)
   				return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value * m2.Value);
   			
   			return new Measurement(m1.Control, m1.Orientation, MeasurementUnits.Pixels, m1.Pixels * m2.Pixels);
   		}
   		
   		/*public static Measurement operator *(Measurement m1, double d) 
   		{
   			if (m1 == null)
   				throw new Exception("Attempted to multiply null");
   				
   			return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value * d);
   		}
   		
   		public static Measurement operator *(double d, Measurement m1) 
   		{
   			if (m1 == null)
   				throw new Exception("Attempted to multiply null");
   				
   			return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value * d);
   		}*/
   		
   		public static Measurement operator /(Measurement m1, Measurement m2) 
   		{
   			if (m1 == null || m2 == null)
   				throw new Exception("Attempted to divide by null");
   				
   			if (m1.Units == m2.Units)
   				return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value / m2.Value);
   			
   			return new Measurement(m1.Control, m1.Orientation, MeasurementUnits.Pixels, m1.Pixels / m2.Pixels);
   		}
   		
   		/*public static Measurement operator /(Measurement m1, double d) 
   		{
   			if (m1 == null)
   				throw new Exception("Attempted to divide null");
   				
   			return new Measurement(m1.Control, m1.Orientation, m1.Units, m1.Value / d);
   		}*/
   		
   		public static implicit operator Measurement(double d)
   		{
   			return new Measurement(d);
   		}
   		
   		public static implicit operator double(Measurement m)
   		{
            if (m == null)
                return 0;

   			return m.Pixels;
   		}
   		
   		public static Measurement FromString(string s)
   		{
   			Control c = Tesseract.TIM.TIM.currentControl;
   			
   			s = s.Trim();
   			
   			if (s.ToLower() == "null")
   				return null;
   			if (s.EndsWith("px"))
   				return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Pixels, GetNum(s));
   			if (s.EndsWith("mm"))
   				return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Mm, GetNum(s));
   			if (s.EndsWith("cm"))
   				return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Mm, GetNum(s) * 10);
   			if (s.EndsWith("inches"))
   				return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Mm, GetNum(s) * 24.5);
   			if (s.EndsWith("%"))
   				return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Percent, GetNum(s));
   			
   			return new Measurement(c, MeasurementOrientation.Horizontal, MeasurementUnits.Pixels, GetNum(s));
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
				return Measurement.FromString((string)o);
			if (o is double)
				return new Measurement((double)o);
			
			return new Measurement(0);
		}
	}
}
