// Type: System.Numerics.Complex
// Assembly: System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 2BCD559E-1E00-4581-80D1-080BCD16D4B6
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Numerics.dll

using System;
using System.Globalization;
using System.Runtime;
using UnityEngine;

	[Serializable]
	public struct Complex : IEquatable<Complex>, IFormattable {
		private float m_real;
		private float m_imaginary;
		
		public static readonly Complex Zero;
		
		public static readonly Complex One;
		
		public static readonly Complex ImaginaryOne;
		private const float LOG_10_INV = 0.43429448190325f;
		
		
		public float Real {
			get {
				return this.m_real;
			}
		}
		
		
		public float Imaginary {
			get {
				return this.m_imaginary;
			}
		}
		
		
		public float Magnitude {
			
			get {
				return Complex.Abs(this);
			}
		}
		
		
		public float Phase {
			
			get {
				return Mathf.Atan2(this.m_imaginary, this.m_real);
			}
		}
		
		static Complex() {
			Complex.Zero = new Complex(0.0f, 0.0f);
			Complex.One = new Complex(1.0f, 0.0f);
			Complex.ImaginaryOne = new Complex(0.0f, 1.0f);
		}
		
		
		
		public Complex(float real, float imaginary) {
			this.m_real = real;
			this.m_imaginary = imaginary;
		}
		
		
		public static implicit operator Complex(short value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static implicit operator Complex(int value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static implicit operator Complex(long value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		
		public static implicit operator Complex(ushort value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		
		public static implicit operator Complex(uint value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		
		public static implicit operator Complex(ulong value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		
		public static implicit operator Complex(sbyte value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static implicit operator Complex(byte value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static implicit operator Complex(float value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static explicit operator Complex(Decimal value) {
			return new Complex((float)value, 0.0f);
		}
		
		
		public static Complex operator -(Complex value) {
			return new Complex(-value.m_real, -value.m_imaginary);
		}
		
		
		public static Complex operator +(Complex left, Complex right) {
			return new Complex(left.m_real + right.m_real, left.m_imaginary + right.m_imaginary);
		}
		
		
		public static Complex operator -(Complex left, Complex right) {
			return new Complex(left.m_real - right.m_real, left.m_imaginary - right.m_imaginary);
		}
		
		
		public static Complex operator *(Complex left, Complex right) {
			return new Complex(left.m_real * right.m_real - left.m_imaginary * right.m_imaginary, left.m_imaginary * right.m_real + left.m_real * right.m_imaginary);
		}
		
		
		public static Complex operator /(Complex left, Complex right) {
			float num1 = left.m_real;
			float num2 = left.m_imaginary;
			float num3 = right.m_real;
			float num4 = right.m_imaginary;
			if(Mathf.Abs(num4) < Mathf.Abs(num3)) {
				float num5 = num4 / num3;
				return new Complex((num1 + num2 * num5) / (num3 + num4 * num5), (num2 - num1 * num5) / (num3 + num4 * num5));
			} else {
				float num5 = num3 / num4;
				return new Complex((num2 + num1 * num5) / (num4 + num3 * num5), (-num1 + num2 * num5) / (num4 + num3 * num5));
			}
		}
		
		
		public static bool operator ==(Complex left, Complex right) {
			if(left.m_real == right.m_real)
				return left.m_imaginary == right.m_imaginary;
			else
				return false;
		}
		
		
		public static bool operator !=(Complex left, Complex right) {
			if(left.m_real == right.m_real)
				return left.m_imaginary != right.m_imaginary;
			else
				return true;
		}
		
		
		public static Complex FromPolarCoordinates(float magnitude, float phase) {
			return new Complex(magnitude * Mathf.Cos(phase), magnitude * Mathf.Sin(phase));
		}
		
		
		
		public static Complex Negate(Complex value) {
			return -value;
		}
		
		
		
		public static Complex Add(Complex left, Complex right) {
			return left + right;
		}
		
		
		
		public static Complex Subtract(Complex left, Complex right) {
			return left - right;
		}
		
		
		
		public static Complex Multiply(Complex left, Complex right) {
			return left * right;
		}
		
		
		
		public static Complex Divide(Complex dividend, Complex divisor) {
			return dividend / divisor;
		}
		
		
		public static float Abs(Complex value) {
			if(float.IsInfinity(value.m_real) || float.IsInfinity(value.m_imaginary))
				return float.PositiveInfinity;
			float num1 = Mathf.Abs(value.m_real);
			float num2 = Mathf.Abs(value.m_imaginary);
			if(num1 > num2) {
				float num3 = num2 / num1;
				return num1 * Mathf.Sqrt(1.0f + num3 * num3);
			} else {
				if(num2 == 0.0)
					return num1;
				float num3 = num1 / num2;
				return num2 * Mathf.Sqrt(1.0f + num3 * num3);
			}
		}
		
		
		public static Complex Conjugate(Complex value) {
			return new Complex(value.m_real, -value.m_imaginary);
		}
		
		
		public static Complex Reciprocal(Complex value) {
			if(value.m_real == 0.0 && value.m_imaginary == 0.0)
				return Complex.Zero;
			else
				return Complex.One / value;
		}
		
		
		public override bool Equals(object obj) {
			if(!(obj is Complex))
				return false;
			else
				return this == (Complex)obj;
		}
		
		
		public bool Equals(Complex value) {
			if(this.m_real.Equals(value.m_real))
				return this.m_imaginary.Equals(value.m_imaginary);
			else
				return false;
		}
		
		
		public override string ToString() {
			return string.Format((IFormatProvider)CultureInfo.CurrentCulture, "({0}, {1})", new object[2]
			                     {
				(object) this.m_real,
				(object) this.m_imaginary
			});
		}
		
		
		public string ToString(string format) {
			return string.Format((IFormatProvider)CultureInfo.CurrentCulture, "({0}, {1})", new object[2]
			                     {
				(object) this.m_real.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture),
				(object) this.m_imaginary.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
			});
		}
		
		
		public string ToString(IFormatProvider provider) {
			return string.Format(provider, "({0}, {1})", new object[2]
			                     {
				(object) this.m_real,
				(object) this.m_imaginary
			});
		}
		
		
		public string ToString(string format, IFormatProvider provider) {
			return string.Format(provider, "({0}, {1})", new object[2]
			                     {
				(object) this.m_real.ToString(format, provider),
				(object) this.m_imaginary.ToString(format, provider)
			});
		}
		
		
		public override int GetHashCode() {
			return this.m_real.GetHashCode() % 99999997 ^ this.m_imaginary.GetHashCode();
		}
		
		
		public static Complex Asin(Complex value) {
			return -Complex.ImaginaryOne * Complex.Log(Complex.ImaginaryOne * value + Complex.Sqrt(Complex.One - value * value));
		}
		
		
		
		
		public static Complex Acos(Complex value) {
			return -Complex.ImaginaryOne * Complex.Log(value + Complex.ImaginaryOne * Complex.Sqrt(Complex.One - value * value));
		}
		
		
		public static Complex Atan(Complex value) {
			Complex complex = new Complex(2.0f, 0.0f);
			return Complex.ImaginaryOne / complex * (Complex.Log(Complex.One - Complex.ImaginaryOne * value) - Complex.Log(Complex.One + Complex.ImaginaryOne * value));
		}
		
		
		public static Complex Log(Complex value) {
			return new Complex(Mathf.Log(Complex.Abs(value)), Mathf.Atan2(value.m_imaginary, value.m_real));
		}
		
		
		public static Complex Log(Complex value, float baseValue) {
			return Complex.Log(value) / Complex.Log((Complex)baseValue);
		}
		
		
		public static Complex Log10(Complex value) {
			return Complex.Scale(Complex.Log(value), 0.43429448190325f);
		}
		
		
		public static Complex Exp(Complex value) {
			float num = Mathf.Exp(value.m_real);
			return new Complex(num * Mathf.Cos(value.m_imaginary), num * Mathf.Sin(value.m_imaginary));
		}
		
		
		public static Complex Sqrt(Complex value) {
			return Complex.FromPolarCoordinates(Mathf.Sqrt(value.Magnitude), value.Phase / 2.0f);
		}
		
		
		public static Complex Pow(Complex value, Complex power) {
			if(power == Complex.Zero)
				return Complex.One;
			if(value == Complex.Zero)
				return Complex.Zero;
			float x = value.m_real;
			float y1 = value.m_imaginary;
			float y2 = power.m_real;
			float num1 = power.m_imaginary;
			float num2 = Complex.Abs(value);
			float num3 = Mathf.Atan2(y1, x);
			float num4 = y2 * num3 + num1 * Mathf.Log(num2);
			float num5 = Mathf.Pow(num2, y2) * Mathf.Pow(Mathf.Exp(1), -num1 * num3);
			return new Complex(num5 * Mathf.Cos(num4), num5 * Mathf.Sin(num4));
		}
		
		
		public static Complex Pow(Complex value, float power) {
			return Complex.Pow(value, new Complex(power, 0.0f));
		}
		
		private static Complex Scale(Complex value, float factor) {
			return new Complex(factor * value.m_real, factor * value.m_imaginary);
		}
	}
