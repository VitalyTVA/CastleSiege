using UnityEngine;
using System.Collections;

public static class MathHelper {
	public static Vector2? SolveQuadratic(float a, float b, float c) {
		float D = b * b - 4 * a * c;
		if (D < 0)
			return null;
		return new Vector2 ((-b - Mathf.Sqrt (D)) / (2 * a), (-b + Mathf.Sqrt (D)) / (2 * a));
	}
	public static float? CalcShootAngleInRad(float distance, float height, float speed, float gravity) {
		float a = (gravity * distance * distance) / (2 * speed * speed);
		var roots = SolveQuadratic (a, distance, (a + height));
		return roots != null ? (float?)Mathf.Atan(roots.Value.Min()) : null;
	}

	public static float Min(this Vector2 vector) {
		return Mathf.Min(vector.x, vector.y);
	}
	public static float Max(this Vector2 vector) {
		return Mathf.Max(vector.x, vector.y);
	}
}