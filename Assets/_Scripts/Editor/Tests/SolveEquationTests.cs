using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class SolveEquationTests {
    const float Tolerance = 0.00001f;

    [TestCase(1f, 2f)]
    public void SolveQuadraticEquation_Linear(float k, float b) {
        var roots = MathHelper.SolveQuadratic(0, k, b);
        Assert.AreEqual(1, roots.Length);
        Assert.AreEqual(-b / k, roots[0], Tolerance);
    }


    [TestCase(1f, 2f)]
    [TestCase(-1.1f, 2.2f)]
    [TestCase(10f, 10f)]
    public void SolveQuadraticEquation_RealRoots(float r0, float r1) {
        var roots = MathHelper.SolveQuadratic(2, -2 * (r0 + r1), 2 * r0 * r1);
        Assert.AreEqual(2, roots.Length);
        Assert.AreEqual(r0, roots[0], Tolerance);
        Assert.AreEqual(r1, roots[1], Tolerance);

        var roots2 = MathHelper.SolvePolynomialEquation(2, -2 * (r0 + r1), 2 * r0 * r1);
        Assert.AreEqual(2, roots2.Length);
        Assert.AreEqual(r0, roots2[0], Tolerance);
        Assert.AreEqual(r1, roots2[1], Tolerance);
    }

    [TestCase(1f, 1f, 1f)]
    [TestCase(3f, -6f, 3.0001f)]
    public void SolveQuadraticEquation_ImaginaryRoots(float a, float b, float c) {
        var roots = MathHelper.SolveQuadratic(a, b, c);
        Assert.AreEqual(0, roots.Length);
        roots = MathHelper.SolvePolynomialEquation(a, b, c);
        Assert.AreEqual(0, roots.Length);
    }

    [TestCase(1f, 2f, 3f, 4f)]
    [TestCase(-25.3f, -20.45f, 30.45f, 60.34f)]
    public void SolveQuatricEquation_AllRealRoots(float r0, float r1, float r2, float r3) {
        float a = r0 + r1 + r2 + r3;
        float b = r0 * r1 + r0 * r2 + r0 * r3 + r1 * r2 + r1 * r3 + r2 * r3;
        float c = r0 * r1 * r2 + r0 * r1 * r3 + r0 * r2 * r3 + r1 * r2 * r3;
        float d = r0 * r1 * r2 * r3;
        var roots = MathHelper.SolvePolynomialEquation(2, -2 * a, 2 * b, -2 * c, 2 * d);
        Assert.AreEqual(4, roots.Length);
        Assert.AreEqual(r0, roots[0], Tolerance);
        Assert.AreEqual(r1, roots[1], Tolerance);
        Assert.AreEqual(r2, roots[2], Tolerance);
        Assert.AreEqual(r3, roots[3], Tolerance);
    }

    [TestCase(1f, 2f, -2f, 2f)]
    [TestCase(-1f, 2f, 1f, 1f)]
    public void SolveQuatricEquation_TwoRealRoots(float r0, float r1, float a, float b) {
        float k1 = (a - r0 - r1);
        float k2 = (-r1 * a + r0 * r1 + b - r0 * a);
        float k3 = (-r0 * b - r1 * b + r0 * r1 * a);
        float k4 = r0 * r1 * b;
        var roots = MathHelper.SolvePolynomialEquation(2, 2 * k1, 2 * k2, 2 * k3, 2 * k4);
        Assert.AreEqual(2, roots.Length);
        Assert.AreEqual(r0, roots[0], Tolerance);
        Assert.AreEqual(r1, roots[1], Tolerance);
    }
    [TestCase(-6f, 9.1f, -2f, 1.001f)]
    public void SolveQuatricEquation_NoRealRoots(float a1, float b1, float a2, float b2) {
        float k1 = (a2 + a1);
        float k2 = (b2 + a1 * a2 + b1);
        float k3 = (a1 * b2 + b1 * a2);
        float k4 = b1 * b2;
        var roots = MathHelper.SolvePolynomialEquation(2, 2 * k1, 2 * k2, 2 * k3, 2 * k4);
        Assert.AreEqual(0, roots.Length);
    }
}
