using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Linq;

public static class MathHelper {
    public static float[] SolveQuadratic(float a, float b, float c) {
		float D = b * b - 4 * a * c;
		if (D < 0)
            return Enumerable.Empty<float>().ToArray();
		return new[] { (-b - Mathf.Sqrt(D)) / (2 * a), (-b + Mathf.Sqrt(D)) / (2 * a) };
	}
    public static float[] SolvePolynomialEquation(params float[] koeffs) {
        var roots = RealPolynomialRootFinder.FindRoots(koeffs.Select(x => (double)x).ToArray());
        return roots.Where(x => x.Imaginary == 0).Select(x => (float)x.Real).OrderBy(x => x).ToArray();
    }
    public static float? CalcShootAngleInRad(float distance, float height, float speed, float gravity) {
//		var sw = new Stopwatch ();
//		sw.Start ();
//		for (int i = 0; i < 100; i++) {
//			var roots_ = RealPolynomialRootFinder.FindRoots (1, -3.08, -33.9933, 13.223740, 180.9114790);
//		}
//		UnityEngine.Debug.Log(sw.ElapsedMilliseconds);


		float a = (gravity * distance * distance) / (2 * speed * speed);
		var roots = SolveQuadratic (a, distance, (a + height));
        return roots.Any() ? (float?)Mathf.Atan(roots.Min()) : null;
	}

	public static float Min(this Vector2 vector) {
		return Mathf.Min(vector.x, vector.y);
	}
	public static float Max(this Vector2 vector) {
		return Mathf.Max(vector.x, vector.y);
	}
}