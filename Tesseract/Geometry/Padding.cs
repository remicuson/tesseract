using System;
using System.ComponentModel;
using Tesseract.Controls;

namespace Tesseract.Geometry
{
    [TypeConverter(typeof(PaddingConverter))]
	public class Padding
	{
		public Padding(Control C, Measurement L, Measurement T, Measurement R, Measurement B)
		{
			this.control = C;
			this.l = L;
			this.t = T;
			this.r = R;
			this.b = B;
		}
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		Measurement l;
		public Measurement L
		{
			get { return l; }
			set
			{
				l = value;
				
				if (l != null)
					l.Orientation = MeasurementOrientation.Horizontal;
			}
		}
		
		Measurement t;
		public Measurement T
		{
			get { return t; }
			set
			{
				t = value;
				
				if (t != null)
					t.Orientation = MeasurementOrientation.Vertical;
			}
		}
		
		Measurement r;
		public Measurement R
		{
			get { return r; }
			set
			{
				r = value;
				
				if (r != null)
					r.Orientation = MeasurementOrientation.Horizontal;
			}
		}
		
		Measurement b;
		public Measurement B
		{
			get { return b; }
			set
			{
				b = value;
				
				if (b != null)
					b.Orientation = MeasurementOrientation.Vertical;
			}
		}

        public static Padding FromString(string s)
        {
            Control c = Tesseract.TIM.TIM.currentControl;

            s = s.Trim();
            string[] nums = s.Split(new char[] { ',' });

            if (nums.Length != 4)
                return new Padding(c, 0, 0, 0, 0);

            try
            {
                return new Padding(c, double.Parse(nums[0]), double.Parse(nums[1]), double.Parse(nums[2]), double.Parse(nums[3]));
            }
            catch
            {
                return new Padding(c, 0, 0, 0, 0);
            }
        }
	}

    class PaddingConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
        {
            return t == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext c, System.Globalization.CultureInfo i, object o)
        {
            if (o is string)
                return Padding.FromString((string)o);

            return new Padding(Tesseract.TIM.TIM.currentControl, 0, 0, 0, 0);
        }
    }
}
