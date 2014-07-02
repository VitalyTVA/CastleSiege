
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

static class RealPolynomialRootFinder
{
	//Global variables that assist the computation, taken from the Visual Studio C++ compiler class float
	// smallest such that 1.0+DBL_EPSILON != 1.0 
	static float DBL_EPSILON = 2.22044604925031E-16f;
	// max value 
	static float DBL_MAX = float.MaxValue;
	// min positive value 
	static float DBL_MIN = float.MinValue;
	
	//If needed, set the maximum allowed degree for the polynomial here:
	
	static int Max_Degree_Polynomial = 100;
	//It is done to allocate memory for the computation arrays, so be careful to not set i too high, though in practice it should not be a problem as it is now.
	
	/// <summary>
	/// The Jenkins–Traub algorithm for polynomial zeros translated into pure VB.NET. It is a translation of the C++ algorithm, which in turn is a translation of the FORTRAN code by Jenkins. See Wikipedia for referances: http://en.wikipedia.org/wiki/Jenkins%E2%80%93Traub_algorithm 
	/// </summary>
	/// <param name="Input">The coefficients for the polynomial starting with the highest degree and ends on the constant, missing degree must be implemented as a 0.</param>
	/// <returns>All the real and complex roots that are found is returned in a list of complex numbers.</returns>
	/// <remarks>The maximum alloed degree polynomial for this implementation is set to 100. It can only take real coefficients.</remarks>
	public static List<Complex> FindRoots(params float[] Input)
	{
		List<Complex> result = new List<Complex>();
		
		int j = 0;
		int l = 0;
		int N = 0;
		int NM1 = 0;
		int NN = 0;
		int NZ = 0;
		int zerok = 0;
		
		//Helper variable that indicates the maximum length of the polynomial array
		int Max_Degree_Helper = Max_Degree_Polynomial + 1;
		
		//Actual degree calculated from the imtems in the Input ParamArray
		int Degree = Input.Length - 1;
		
		float[] op = new float[Max_Degree_Helper + 1];
		float[] K = new float[Max_Degree_Helper + 1];
		float[] p = new float[Max_Degree_Helper + 1];
		float[] pt = new float[Max_Degree_Helper + 1];
		float[] qp = new float[Max_Degree_Helper + 1];
		float[] temp = new float[Max_Degree_Helper + 1];
		float[] zeror = new float[Max_Degree_Helper + 1];
		float[] zeroi = new float[Max_Degree_Polynomial + 1];
		float bnd = 0;
		float df = 0;
		float dx = 0;
		float ff = 0;
		float moduli_max = 0;
		float moduli_min = 0;
		float x = 0;
		float xm = 0;
		float aa = 0;
		float bb = 0;
		float cc = 0;
		float lzi = 0;
		float lzr = 0;
		float sr = 0;
		float szi = 0;
		float szr = 0;
		float t = 0;
		float u = 0;
		float xx = 0;
		float xxx = 0;
		float yy = 0;
		
		// These are used to scale the polynomial for more accurecy
		float factor = 0;
		float sc = 0;
		
		float RADFAC = 3.14159265358979f / 180;
		// Degrees-to-radians conversion factor = pi/180
		float lb2 = Mathf.Log(2.0f);
		// Dummy variable to avoid re-calculating this value in loop below
		float lo = DBL_MIN / DBL_EPSILON;
		//float.MinValue / float.Epsilon
		float cosr = Mathf.Cos(94.0f * RADFAC);
		// = -0.069756474
		float sinr = Mathf.Sin(94.0f * RADFAC);
		// = 0.99756405
		
		//Are the polynomial larger that the maximum allowed?
		if (Degree > Max_Degree_Polynomial) {
			throw new Exception("The entered Degree is greater than MAXDEGREE. Exiting root finding algorithm. No further action taken.");
		}
		
		//Check if the leading coefficient is zero
		
		if (Input[0] != 0) {
			for (int i = 0; i <= Input.Length - 1; i++) {
				op[i] = Input[i];
			}
			
			N = Degree;
			xx = Mathf.Sqrt(0.5f);
			//= 0.70710678
			yy = -xx;
			
			// Remove zeros at the origin, if any
			j = 0;
			while ((op[N] == 0)) {
				zeror[j] = 0;
				zeroi[j] = 0.0f;
				N -= 1;
				j += 1;
			}
			
			NN = N + 1;
			
			for (int i = 0; i <= NN - 1; i++) {
				p[i] = op[i];
			}
			
			while (N >= 1) {
				//Start the algorithm for one zero
				if (N <= 2) {
					if (N < 2) {
						//1st degree polynomial
						zeror[(Degree) - 1] = -(p[1] / p[0]);
						zeroi[(Degree) - 1] = 0.0f;
					} else {
						//2nd degree polynomial
						Quad_ak1(p[0], p[1], p[2], ref zeror[((Degree) - 2)], ref zeroi[((Degree) - 2)], ref zeror[((Degree) - 1)], ref zeroi[(Degree) - 1]);
					}
					//Solutions have been calculated, so exit the loop
					break; // TODO: might not be correct. Was : Exit While
				}
				
				moduli_max = 0.0f;
				moduli_min = DBL_MAX;
				
				for (int i = 0; i <= NN - 1; i++) {
					x = Mathf.Abs(p[i]);
					if ((x > moduli_max))
						moduli_max = x;
					if (((x != 0) & (x < moduli_min)))
						moduli_min = x;
				}
				
				// Scale if there are large or very small coefficients
				// Computes a scale factor to multiply the coefficients of the polynomial. The scaling
				// is done to avoid overflow and to avoid undetected underflow interfering with the
				// convergence criterion.
				// The factor is a power of the base.
				
				//  Scaling the polynomial
				sc = lo / moduli_min;
				
				if ((((sc <= 1.0) & (moduli_max >= 10)) | ((sc > 1.0) & (DBL_MAX / sc >= moduli_max)))) {
					if (sc == 0) {
						sc = DBL_MIN;
					}
					
					l = Convert.ToInt32(Mathf.Log(sc) / lb2 + 0.5);
					factor = Mathf.Pow(2.0f, l);
					if ((factor != 1.0f)) {
						for (int i = 0; i <= NN; i++) {
							p[i] *= factor;
						}
					}
				}
				
				//Compute lower bound on moduli of zeros
				for (int i = 0; i <= NN - 1; i++) {
					pt[i] = Mathf.Abs(p[i]);
				}
				pt[N] = -(pt[N]);
				
				NM1 = N - 1;
				
				// Compute upper estimate of bound
				x = Mathf.Exp((Mathf.Log(-pt[N]) - Mathf.Log(pt[0])) / Convert.ToSingle(N));
				
				if ((pt[NM1] != 0)) {
					// If Newton step at the origin is better, use it
					xm = -pt[N] / pt[NM1];
					if (xm < x) {
						x = xm;
					}
				}
				
				// Chop the interval (0, x) until ff <= 0
				xm = x;
				
				do {
					x = xm;
					xm = 0.1f * x;
					ff = pt[0];
					for (int i = 1; i <= NN - 1; i++) {
						ff = ff * xm + pt[i];
					}
				} while ((ff > 0));
				
				dx = x;
				
				do {
					df = pt[0];
					ff = pt[0];
					for (int i = 1; i <= N - 1; i++) {
						ff = x * ff + pt[i];
						df = x * df + ff;
					}
					ff = x * ff + pt[N];
					dx = ff / df;
					x -= dx;
				} while ((Mathf.Abs(dx / x) > 0.005));
				
				bnd = x;
				
				// Compute the derivative as the initial K polynomial and do 5 steps with no shift
				for (int i = 1; i <= N - 1; i++) {
					K[i] = Convert.ToSingle(N - i) * p[i] / (Convert.ToSingle(N));
				}
				K[0] = p[0];
				
				aa = p[N];
				bb = p[NM1];
				if ((K[NM1] == 0)) {
					zerok = 1;
				} else {
					zerok = 0;
				}
				
				for (int jj = 0; jj <= 4; jj++) {
					cc = K[NM1];
					if ((zerok==1)) {
						// Use unscaled form of recurrence
						for (int i = 0; i <= NM1 - 1; i++) {
							j = NM1 - i;
							K[j] = K[j - 1];
						}
						K[0] = 0;
						if ((K[NM1] == 0)) {
							zerok = 1;
						} else {
							zerok = 0;
						}
					} else {
						// Used scaled form of recurrence if value of K at 0 is nonzero
						t = -aa / cc;
						for (int i = 0; i <= NM1 - 1; i++) {
							j = NM1 - i;
							K[j] = t * K[j - 1] + p[j];
						}
						K[0] = p[0];
						if ((Mathf.Abs(K[NM1]) <= Mathf.Abs(bb) * DBL_EPSILON * 10.0)) {
							zerok = 1;
						} else {
							zerok = 0;
						}
					}
				}
				
				// Save K for restarts with new shifts
				for (int i = 0; i <= N - 1; i++) {
					temp[i] = K[i];
				}
				
				
				for (int jj = 1; jj <= 20; jj++) {
					// Quadratic corresponds to a float shift to a non-real point and its
					// complex conjugate. The point has modulus BND and amplitude rotated
					// by 94 degrees from the previous shift.
					
					xxx = -(sinr * yy) + cosr * xx;
					yy = sinr * xx + cosr * yy;
					xx = xxx;
					sr = bnd * xx;
					u = -(2.0f * sr);
					
					// Second stage calculation, fixed quadratic
					Fxshfr_ak1(20 * jj, ref NZ, sr, bnd, K, N, p, NN, qp, u,
					           ref lzi, ref lzr, ref szi, ref szr);
					
					
					if ((NZ != 0)) {
						// The second stage jumps directly to one of the third stage iterations and
						// returns here if successful. Deflate the polynomial, store the zero or
						// zeros, and return to the main algorithm.
						
						j = (Degree) - N;
						zeror[j] = szr;
						zeroi[j] = szi;
						NN = NN - NZ;
						N = NN - 1;
						for (int i = 0; i <= NN - 1; i++) {
							p[i] = qp[i];
						}
						if ((NZ != 1)) {
							zeror[j + 1] = lzr;
							zeroi[j + 1] = lzi;
						}
						
						//Found roots start all calulations again, with a lower order polynomial
						break; // TODO: might not be correct. Was : Exit For
					} else {
						// If the iteration is unsuccessful, another quadratic is chosen after restoring K
						for (int i = 0; i <= N - 1; i++) {
							K[i] = temp[i];
						}
					}
					if ((jj >= 20)) {
						throw new Exception("Failure. No convergence after 20 shifts. Program terminated.");
					}
				}
			}
			
		} else {
			throw new Exception("The leading coefficient is zero. No further action taken. Program terminated.");
		}
		
		for (int i = 0; i <= Degree - 1; i++) {
			result.Add(new Complex(zeror[Degree - 1 - i], zeroi[Degree - 1 - i]));
		}
		
		return result;
	}
	
	private static void Fxshfr_ak1(int L2, ref int NZ, float sr, float v, float[] K, int N, float[] p, int NN, float[] qp, float u,
	                               
	                               ref float lzi, ref float lzr, ref float szi, ref float szr)
	{
		// Computes up to L2 fixed shift K-polynomials, testing for convergence in the linear or
		// quadratic case. Initiates one of the variable shift iterations and returns with the
		// number of zeros found.
		
		// L2 limit of fixed shift steps
		// NZ number of zeros found
		
		int fflag = 0;
		int i = 0;
		int iFlag = 0;
		int j = 0;
		int spass = 0;
		int stry = 0;
		int tFlag = 0;
		int vpass = 0;
		int vtry = 0;
		iFlag = 1;
		float a = 0;
		float a1 = 0;
		float a3 = 0;
		float a7 = 0;
		float b = 0;
		float betas = 0;
		float betav = 0;
		float c = 0;
		float d = 0;
		float e = 0;
		float f = 0;
		float g = 0;
		float h = 0;
		float oss = 0;
		float ots = 0;
		float otv = 0;
		float ovv = 0;
		float s = 0;
		float ss = 0;
		float ts = 0;
		float tss = 0;
		float tv = 0;
		float tvv = 0;
		float ui = 0;
		float vi = 0;
		float vv = 0;
		float[] qk = new float[100 + 2];
		float[] svk = new float[100 + 2];
		
		NZ = 0;
		betav = 0.25f;
		betas = 0.25f;
		oss = sr;
		ovv = v;
		
		// Evaluate polynomial by synthetic division
		QuadSD_ak1(NN, u, v, p, qp, ref a, ref b);
		
		tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
		                   ref g, ref h, K, u, v, qk);
		
		
		for (j = 0; j <= L2 - 1; j++) {
			fflag = 1;
			// Calculate next K polynomial and estimate v
			nextK_ak1(N, tFlag, a, b, a1, ref a3, ref a7, K, qk, qp);
			tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
			                   ref g, ref h, K, u, v, qk);
			newest_ak1(tFlag, ref ui, ref vi, a, a1, a3, a7, b, c, d,
			           f, g, h, u, v, K, N, p);
			
			vv = vi;
			
			// Estimate s
			if (K[N - 1] != 0) {
				ss = -(p[N] / K[N - 1]);
			} else {
				ss = 0;
			}
			
			ts = 1;
			tv = 1.0f;
			
			
			if (((j != 0) & (tFlag != 3))) {
				// Compute relative measures of convergence of s and v sequences
				if (vv != 0) {
					tv = Mathf.Abs((vv - ovv) / vv);
				}
				
				if (ss != 0) {
					ts = Mathf.Abs((ss - oss) / ss);
				}
				
				
				// If decreasing, multiply the two most recent convergence measures
				
				if (tv < otv) {
					tvv = tv * otv;
				} else {
					tvv = 1;
				}
				
				
				if (ts < ots) {
					tss = ts * ots;
				} else {
					tss = 1;
				}
				
				// Compare with convergence criteria
				
				if (tvv < betav) {
					vpass = 1;
				} else {
					vpass = 0;
				}
				
				if (tss < betas) {
					spass = 1;
				} else {
					spass = 0;
				}
				
				
				
				if (((spass==1) | (vpass==1))) {
					// At least one sequence has passed the convergence test.
					// Store variables before iterating
					
					for (i = 0; i <= N - 1; i++) {
						svk[i] = K[i];
					}
					
					s = ss;
					
					// Choose iteration according to the fastest converging sequence
					stry = 0;
					vtry = 0;
					
					
					do {
						if ((fflag==1 & ((fflag == 0))) & ((spass==1) & (!(vpass ==1)| (tss < tvv))))
						{
							// Do nothing. Provides a quick "short circuit".
						} else {
							QuadIT_ak1(N, ref NZ, ui, vi, ref szr, ref szi, ref lzr, ref lzi, qp, NN,
							           ref a, ref b, p, qk, ref a1, ref a3, ref a7, ref c, ref d, ref e,
							           ref f, ref g, ref h, K);
							
							if (((NZ) > 0))
								return;
							
							// Quadratic iteration has failed. Flag that it has been tried and decrease the
							// convergence criterion
							
							iFlag = 1;
							vtry = 1;
							betav *= 0.25f;
							
							// Try linear iteration if it has not been tried and the s sequence is converging
							if ((stry==1 | (!(spass==1)))) {
								iFlag = 0;
							} else {
								for (i = 0; i <= N - 1; i++) {
									K[i] = svk[i];
								}
							}
							
						}
						
						if ((iFlag != 0)) {
							RealIT_ak1(ref iFlag, ref NZ, ref s, N, p, NN, qp, ref szr, ref szi, K,
							           qk);
							
							if (((NZ) > 0))
								return;
							
							// Linear iteration has failed. Flag that it has been tried and decrease the
							// convergence criterion
							stry = 1;
							betas *= 0.25f;
							
							
							if ((iFlag != 0)) {
								// If linear iteration signals an almost float real zero, attempt quadratic iteration
								ui = -(s + s);
								vi = s * s;
							}
						}
						
						// Restore variables
						for (i = 0; i <= N - 1; i++) {
							K[i] = svk[i];
						}
						
						
						// Try quadratic iteration if it has not been tried and the v sequence is converging
						if ((!(vpass==1) | vtry==1)) {
							// Break out of infinite for loop
							break; // TODO: might not be correct. Was : Exit Do
						}
						
						
					} while (true);
					
					// Re-compute qp and scalar values to continue the second stage
					QuadSD_ak1(NN, u, v, p, qp, ref a, ref b);
					tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
					                   ref g, ref h, K, u, v, qk);
					
				}
			}
			
			ovv = vv;
			oss = ss;
			otv = tv;
			ots = ts;
		}
		
	}
	
	
	private static void QuadSD_ak1(int NN, float u, float v, float[] p, float[] q, ref float a, ref float b)
	{
		// Divides p by the quadratic 1, u, v placing the quotient in q and the remainder in a, b
		
		int i = 0;
		
		b = p[0];
		q[0] = p[0];
		
		a = -((b) * u) + p[1];
		q[1] = -((b) * u) + p[1];
		
		for (i = 2; i <= NN - 1; i++) {
			q[i] = -((a) * u + (b) * v) + p[i];
			b = (a);
			a = q[i];
		}
		
	}
	
	private static int calcSC_ak1(int N, float a, float b, ref float a1, ref float a3, ref float a7, ref float c, ref float d, ref float e, ref float f,
	                              ref float g, ref float h, float[] K, float u, float v, float[] qk)
	{
		
		// This routine calculates scalar quantities used to compute the next K polynomial and
		// new estimates of the quadratic coefficients.
		
		// calcSC - integer variable set here indicating how the calculations are normalized
		// to avoid overflow.
		
		int dumFlag = 3;
		// TYPE = 3 indicates the quadratic is almost a factor of K
		
		// Synthetic division of K by the quadratic 1, u, v
		QuadSD_ak1(N, u, v, K, qk, ref c, ref d);
		
		if ((Mathf.Abs((c)) <= (100.0 * DBL_EPSILON * Mathf.Abs(K[N - 1])))) {
			if ((Mathf.Abs((d)) <= (100.0 * DBL_EPSILON * Mathf.Abs(K[N - 2])))) {
				return dumFlag;
			}
		}
		
		h = v * b;
		if ((Mathf.Abs((d)) >= Mathf.Abs((c)))) {
			dumFlag = 2;
			// TYPE = 2 indicates that all formulas are divided by d
			e = a / (d);
			f = (c) / (d);
			g = u * b;
			a3 = (e) * ((g) + a) + (h) * (b / (d));
			a1 = -a + (f) * b;
			a7 = (h) + ((f) + u) * a;
		} else {
			dumFlag = 1;
			// TYPE = 1 indicates that all formulas are divided by c
			e = a / (c);
			f = (d) / (c);
			g = (e) * u;
			a3 = (e) * a + ((g) + (h) / (c)) * b;
			a1 = -(a * ((d) / (c))) + b;
			a7 = (g) * (d) + (h) * (f) + a;
		}
		
		return dumFlag;
	}
	
	
	private static void nextK_ak1(int N, int tFlag, float a, float b, float a1, ref float a3, ref float a7, float[] K, float[] qk, float[] qp)
	{
		// Computes the next K polynomials using the scalars computed in calcSC_ak1
		
		int i = 0;
		float temp = 0;
		
		// Use unscaled form of the recurrence
		if ((tFlag == 3)) {
			K[1] = 0;
			K[0] = 0.0f;
			
			for (i = 2; i <= N - 1; i++) {
				K[i] = qk[i - 2];
			}
			
			return;
		}
		
		if (tFlag == 1) {
			temp = b;
		} else {
			temp = a;
		}
		
		
		if ((Mathf.Abs(a1) > (10.0 * DBL_EPSILON * Mathf.Abs(temp)))) {
			// Use scaled form of the recurrence
			
			a7 = a7 / a1;
			a3 = a3 / a1;
			K[0] = qp[0];
			K[1] = -((a7) * qp[0]) + qp[1];
			
			for (i = 2; i <= N - 1; i++) {
				K[i] = -((a7) * qp[i - 1]) + (a3) * qk[i - 2] + qp[i];
			}
		} else {
			// If a1 is nearly zero, then use a special form of the recurrence
			
			K[0] = 0.0f;
			K[1] = -(a7) * qp[0];
			
			for (i = 2; i <= N - 1; i++) {
				K[i] = -((a7) * qp[i - 1]) + (a3) * qk[i - 2];
			}
		}
	}
	
	private static void newest_ak1(int tFlag, ref float uu, ref float vv, float a, float a1, float a3, float a7, float b, float c, float d,
	                               float f, float g, float h, float u, float v, float[] K, int N, float[] p)
	{
		// Compute new estimates of the quadratic coefficients using the scalars computed in calcSC_ak1
		
		float a4 = 0;
		float a5 = 0;
		float b1 = 0;
		float b2 = 0;
		float c1 = 0;
		float c2 = 0;
		float c3 = 0;
		float c4 = 0;
		float temp = 0;
		
		vv = 0;
		//The quadratic is zeroed
		uu = 0.0f;
		//The quadratic is zeroed
		
		
		if ((tFlag != 3)) {
			if ((tFlag != 2)) {
				a4 = a + u * b + h * f;
				a5 = c + (u + v * f) * d;
				
			} else {
				a4 = (a + g) * f + h;
				a5 = (f + u) * c + v * d;
			}
			
			// Evaluate new quadratic coefficients
			b1 = -K[N - 1] / p[N];
			b2 = -(K[N - 2] + b1 * p[N - 1]) / p[N];
			c1 = v * b2 * a1;
			c2 = b1 * a7;
			c3 = b1 * b1 * a3;
			c4 = -(c2 + c3) + c1;
			temp = -c4 + a5 + b1 * a4;
			if ((temp != 0.0)) {
				uu = -((u * (c3 + c2) + v * (b1 * a1 + b2 * a7)) / temp) + u;
				vv = v * (1.0f + c4 / temp);
			}
			
		}
	}
	
	private static void QuadIT_ak1(int N, ref int NZ, float uu, float vv, ref float szr, ref float szi, ref float lzr, ref float lzi, float[] qp, int NN,
	                               ref float a, ref float b, float[] p, float[] qk, ref float a1, ref float a3, ref float a7, ref float c, ref float d, ref float e,
	                               
	                               ref float f, ref float g, ref float h, float[] K)
	{
		// Variable-shift K-polynomial iteration for a quadratic factor converges only if the
		// zeros are equimodular or nearly so.
		
		int i = 0;
		int j = 0;
		int tFlag = 0;
		int triedFlag = 0;
		j = 0;
		triedFlag = 0;
		
		float ee = 0;
		float mp = 0;
		float omp = 0;
		float relstp = 0;
		float t = 0;
		float u = 0;
		float ui = 0;
		float v = 0;
		float vi = 0;
		float zm = 0;
		
		NZ = 0;
		//Number of zeros found
		u = uu;
		//uu and vv are coefficients of the starting quadratic
		v = vv;
		
		do {
			Quad_ak1(1.0f, u, v, ref szr, ref szi, ref lzr, ref lzi);
			
			// Return if roots of the quadratic are real and not close to multiple or nearly
			// equal and of opposite sign.
			if ((Mathf.Abs(Mathf.Abs(szr) - Mathf.Abs(lzr)) > 0.01 * Mathf.Abs(lzr))) {
				break; // TODO: might not be correct. Was : Exit Do
			}
			
			// Evaluate polynomial by quadratic synthetic division
			QuadSD_ak1(NN, u, v, p, qp, ref a, ref b);
			
			mp = Mathf.Abs(-((szr) * (b)) + (a)) + Mathf.Abs((szi) * (b));
			
			// Compute a rigorous bound on the rounding error in evaluating p
			zm = Mathf.Sqrt(Mathf.Abs(v));
			ee = 2.0f * Mathf.Abs(qp[0]);
			t = -((szr) * (b));
			
			for (i = 1; i <= N - 1; i++) {
				ee = ee * zm + Mathf.Abs(qp[i]);
			}
			
			ee = ee * zm + Mathf.Abs((a) + t);
			ee = (9.0f * ee + 2.0f * Mathf.Abs(t) - 7.0f * (Mathf.Abs((a) + t) + zm * Mathf.Abs((b)))) * DBL_EPSILON;
			
			// Iteration has converged sufficiently if the polynomial value is less than 20 times this bound
			if ((mp <= 20.0 * ee)) {
				NZ = 2;
				break; // TODO: might not be correct. Was : Exit Do
			}
			
			j += 1;
			
			// Stop iteration after 20 steps
			if ((j > 20))
				break; // TODO: might not be correct. Was : Exit Do
			
			if ((j >= 2)) {
				if (((relstp <= 0.01) & (mp >= omp) & (!(triedFlag==1)))) {
					// A cluster appears to be stalling the convergence. Five fixed shift
					// steps are taken with a u, v close to the cluster.
					if (relstp < DBL_EPSILON) {
						relstp = Mathf.Sqrt(DBL_EPSILON);
					} else {
						relstp = Mathf.Sqrt(relstp);
					}
					
					u -= u * relstp;
					v += v * relstp;
					
					QuadSD_ak1(NN, u, v, p, qp, ref a, ref b);
					
					for (i = 0; i <= 4; i++) {
						tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
						                   ref g, ref h, K, u, v, qk);
						nextK_ak1(N, tFlag, a, b, a1, ref a3, ref a7, K, qk, qp);
					}
					
					triedFlag = 1;
					j = 0;
					
				}
				
			}
			
			omp = mp;
			
			// Calculate next K polynomial and new u and v
			tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
			                   ref g, ref h, K, u, v, qk);
			nextK_ak1(N, tFlag, a, b, a1, ref a3, ref a7, K, qk, qp);
			tFlag = calcSC_ak1(N, a, b, ref a1, ref a3, ref a7, ref c, ref d, ref e, ref f,
			                   ref g, ref h, K, u, v, qk);
			newest_ak1(tFlag, ref ui, ref vi, a, a1, a3, a7, b, c, d,
			           f, g, h, u, v, K, N, p);
			
			// If vi is zero, the iteration is not converging
			if ((vi != 0)) {
				relstp = Mathf.Abs((-v + vi) / vi);
				u = ui;
				v = vi;
			}
		} while ((vi != 0));
	}
	
	private static void RealIT_ak1(ref int iFlag, ref int NZ, ref float sss, int N, float[] p, int NN, float[] qp, ref float szr, ref float szi, float[] K,
	                               
	                               float[] qk)
	{
		// Variable-shift H-polynomial iteration for a real zero
		
		// sss - starting iterate
		// NZ - number of zeros found
		// iFlag - flag to indicate a pair of zeros near real axis
		int i = 0;
		int j = 0;
		int nm1 = 0;
		j = 0;
		nm1 = N - 1;
		float ee = 0;
		float kv = 0;
		float mp = 0;
		float ms = 0;
		float omp = 0;
		float pv = 0;
		float s = 0;
		float t = 0;
		
		iFlag = 0;
		NZ = 0;
		s = sss;
		
		do {
			pv = p[0];
			
			// Evaluate p at s
			qp[0] = pv;
			for (i = 1; i <= NN - 1; i++) {
				qp[i] = pv * s + p[i];
				pv = pv * s + p[i];
			}
			mp = Mathf.Abs(pv);
			
			// Compute a rigorous bound on the error in evaluating p
			ms = Mathf.Abs(s);
			ee = 0.5f * Mathf.Abs(qp[0]);
			for (i = 1; i <= NN - 1; i++) {
				ee = ee * ms + Mathf.Abs(qp[i]);
			}
			
			// Iteration has converged sufficiently if the polynomial value is less than
			// 20 times this bound
			if ((mp <= 20.0 * DBL_EPSILON * (2.0 * ee - mp))) {
				NZ = 1;
				szr = s;
				szi = 0.0f;
				break; // TODO: might not be correct. Was : Exit Do
			}
			
			j += 1;
			
			// Stop iteration after 10 steps
			if ((j > 10))
				break; // TODO: might not be correct. Was : Exit Do
			
			if ((j >= 2)) {
				if (((Mathf.Abs(t) <= 0.001 * Mathf.Abs(-t + s)) & (mp > omp))) {
					// A cluster of zeros near the real axis has been encountered                    ' Return with iFlag set to initiate a quadratic iteration
					
					iFlag = 1;
					sss = s;
					break; // TODO: might not be correct. Was : Exit Do
				}
				
			}
			
			// Return if the polynomial value has increased significantly
			omp = mp;
			
			// Compute t, the next polynomial and the new iterate
			qk[0] = K[0];
			kv = K[0];
			for (i = 1; i <= N - 1; i++) {
				kv = kv * s + K[i];
				qk[i] = kv;
			}
			if ((Mathf.Abs(kv) > Mathf.Abs(K[nm1]) * 10.0 * DBL_EPSILON)) {
				// Use the scaled form of the recurrence if the value of K at s is non-zero
				t = -(pv / kv);
				K[0] = qp[0];
				for (i = 1; i <= N - 1; i++) {
					K[i] = t * qk[i - 1] + qp[i];
				}
			} else {
				// Use unscaled form
				K[0] = 0.0f;
				for (i = 1; i <= N - 1; i++) {
					K[i] = qk[i - 1];
				}
			}
			
			kv = K[0];
			for (i = 1; i <= N - 1; i++) {
				kv = kv * s + K[i];
			}
			
			if ((Mathf.Abs(kv) > (Mathf.Abs(K[nm1]) * 10.0 * DBL_EPSILON))) {
				t = -(pv / kv);
			} else {
				t = 0.0f;
			}
			
			s += t;
			
		} while (true);
	}
	
	private static void Quad_ak1(float a, float b1, float c, ref float sr, ref float si, ref float lr, ref float li)
	{
		// Calculates the zeros of the quadratic a*Z^2 + b1*Z + c
		// The quadratic formula, modified to avoid overflow, is used to find the larger zero if the
		// zeros are real and both zeros are complex. The smaller real zero is found directly from
		// the product of the zeros c/a.
		
		float b = 0;
		float d = 0;
		float e = 0;
		
		sr = 0;
		si = 0;
		lr = 0;
		li = 0.0f;
		
		if (a == 0) {
			if (b1 == 0) {
				sr = -c / b1;
			}
		}
		
		if (c == 0) {
			lr = -b1 / a;
		}
		
		//Compute discriminant avoiding overflow
		b = b1 / 2.0f;
		
		if (Mathf.Abs(b) < Mathf.Abs(c)) {
			if (c >= 0) {
				e = a;
			} else {
				e = -a;
			}
			
			e = -e + b * (b / Mathf.Abs(c));
			d = Mathf.Sqrt(Mathf.Abs(e)) * Mathf.Sqrt(Mathf.Abs(c));
		} else {
			e = -((a / b) * (c / b)) + 1.0f;
			d = Mathf.Sqrt(Mathf.Abs(e)) * (Mathf.Abs(b));
		}
		
		
		
		if ((e >= 0)) {
			// Real zero
			if (b >= 0) {
				d *= -1;
			}
			lr = (-b + d) / a;
			
			if (lr != 0) {
				sr = (c / (lr)) / a;
			}
		} else {
			// Complex conjugate zeros
			lr = -(b / a);
			sr = -(b / a);
			si = Mathf.Abs(d / a);
			li = -(si);
		}
		
	}
}