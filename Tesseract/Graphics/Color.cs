using System;
using System.ComponentModel;

namespace Tesseract.Graphics
{
	[TypeConverter(typeof(ColorConverter))]
	public class Color
	{
		public Color(double A, double R, double G, double B)
		{
			this.a = A;
			this.r = R;
			this.g = G;
			this.b = B;
		}
		
		double a;
		public double A
		{
			get { return a; }
			set { a = value; }
		}
		
		double r;
		public double R
		{
			get { return r; }
			set { r = value; }
		}
		
		double g;
		public double G
		{
			get { return g; }
			set { g = value; }
		}
		
		double b;
		public double B
		{
			get { return b; }
			set { b = value; }
		}
		
		public byte AByte
		{
			get { return (byte)(a * 255); }
		}
		
		public byte RByte
		{
			get { return (byte)(r * 255); }
		}
		
		public byte GByte
		{
			get { return (byte)(g * 255); }
		}
		
		public byte BByte
		{
			get { return (byte)(b * 255); }
		}
		
		public override string ToString()
   		{
   			return string.Format("Color[{0}:{1},{2},{3}]", a, r, g, b);
   		}
		
		public static Color FromString(string s)
   		{
   			if (typeof(Colors).GetProperty(s) != null)
   				return (Color)typeof(Colors).GetProperty(s).GetValue(null, null);

   			return Colors.White;
   		}
	}
	
	class ColorConverter: TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
		{
			return (t == typeof(string));
		}
		
		public override object ConvertFrom(ITypeDescriptorContext c, System.Globalization.CultureInfo i, object o)
		{
			if (o is string)
				return Color.FromString((string)o);
			
			return Colors.Black;
		}
	}
}
