using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
        List<double> doubleKoeffs = new List<double>();
        bool nonZeroKeffFound = false;
        foreach(float koeff in koeffs) {
            if(koeff != 0)
                nonZeroKeffFound = true;
            if(nonZeroKeffFound)
                doubleKoeffs.Add((double)koeff);
        }
        var roots = RealPolynomialRootFinder.FindRoots(doubleKoeffs.ToArray());
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
            var direction = CalcRotationAngle(distanceVector.x, distanceVector.z);
            Vector3 velocityDirection = new Vector3(Mathf.Cos(direction) * Mathf.Cos(angle.Value), Mathf.Sin(angle.Value), Mathf.Sin(direction) * Mathf.Cos(angle.Value));
            return velocityDirection * velocity;
        }
        return null;
    }
    static float CalcRotationAngle(float x, float y) {
        var angle = Mathf.Atan(y / x);
        if(x < 0)
            angle += Mathf.PI;
        return angle;
    }
    public static Vector3? CalcShootVelocity(Vector3 origination, Vector3 target, Vector3 targetVelocity, float velocity, float gravity) {
        Vector3 distanceVector = target - origination;
        return CalcShootVelocityCore(distanceVector.x, distanceVector.y, distanceVector.z, targetVelocity.x, targetVelocity.y, targetVelocity.z, gravity, velocity);
    }
    //x + v_x * t = v * cos(a) * cos(b) * t
    //y + v_y * t = v * sin(a) * t * g ^2 / t
    //z + v_z * t = v * cos(a) * sin(b) * t
    //1/4*g^2*t^4-v_y*t^3*g+(v_x^2+v_z^2-v^2-y*g+v_y^2)*t^2+(2*z*v_z+2*y*v_y+2*x*v_x)*t+x^2+z^2+y^2
    static Vector3? CalcShootVelocityCore(float x, float y, float z, float v_x, float v_y, float v_z, float g, float v) {
        float k1 = g * g / 4;
        float k2 = -v_y * g;
        float k3 = v_x * v_x + v_z * v_z - v  * v- y * g + v_y * v_y;
        float k4 = 2 * z * v_z + 2 * y * v_y + 2 * x * v_x;
        float k5 = x * x + y * y + z * z;
        float[] roots = SolvePolynomialEquation(k1, k2, k3, k4, k5);
        if(!roots.Any())
            return null;
        float t = roots.First(r => r > 0);
        float b = CalcRotationAngle(x + v_x * t, z + v_z * t);
        float a = Mathf.Asin((y + v_y * t - g * t * t / 2) / (v * t));
        Vector3 velocityDirection = new Vector3(Mathf.Cos(b) * Mathf.Cos(a), Mathf.Sin(a), Mathf.Sin(b) * Mathf.Cos(a));
        return velocityDirection * v;
    }
}