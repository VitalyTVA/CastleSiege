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
    public static float? CalcShootAngleInRad(float distance, float height, float velocity, float gravity) {
		float a = (gravity * distance * distance) / (2 * velocity * velocity);
		var roots = SolveQuadratic (a, distance, (a + height));
        return roots.Any() ? (float?)Mathf.Atan(roots.Min()) : null;
	}
    public static Vector3? CalcShootVelocity(Vector3 origination, Vector3 target, float velocity, float gravity) {
        Vector3 distanceVector = origination - target;
        distanceVector.y = 0;
        float? angle = MathHelper.CalcShootAngleInRad(distanceVector.magnitude, origination.y - target.y, velocity, gravity);
        if(angle != null) {
            var direction = Mathf.Deg2Rad * (Quaternion.LookRotation(target - origination).eulerAngles.y);
            Vector3 velocityDirection = new Vector3(Mathf.Sin(direction) * Mathf.Cos(angle.Value), Mathf.Sin(angle.Value), Mathf.Cos(direction) * Mathf.Cos(angle.Value));
            return velocityDirection * velocity;
        }
        return null;
    }

    public static float Min(this Vector2 vector) {
		return Mathf.Min(vector.x, vector.y);
	}
	public static float Max(this Vector2 vector) {
		return Mathf.Max(vector.x, vector.y);
	}
}