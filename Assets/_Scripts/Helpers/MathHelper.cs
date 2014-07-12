using UnityEngine;
using System.Collections;
using System.Linq;

public static class MathHelper {
    public static float[] SolveQuadratic(float a, float b, float c) {
        if(a == 0)
            return new[] { -c / b };
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
        Vector3 distanceVector = target - origination;
        distanceVector.y = 0;
        float? angle = MathHelper.CalcShootAngleInRad(distanceVector.magnitude, origination.y - target.y, velocity, gravity);
        if(angle != null) {
            var direction = Mathf.Atan(distanceVector.z / distanceVector.x);
            if(distanceVector.x < 0)
                direction += Mathf.PI;
            Vector3 velocityDirection = new Vector3(Mathf.Cos(direction) * Mathf.Cos(angle.Value), Mathf.Sin(angle.Value), Mathf.Sin(direction) * Mathf.Cos(angle.Value));
            return velocityDirection * velocity;
        }
        return null;
    }
}