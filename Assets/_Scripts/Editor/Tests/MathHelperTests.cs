using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class MathHelperTests {
    const float Tolerance = 0.000001f;

    [TestCase(1f, 2f)]
    [TestCase(-1.1f, 2.2f)]
    [TestCase(10f, 10f)]
    public void SolveQuadraticEquation_RealRoots(float root0, float root1) {
        var roots = MathHelper.SolveQuadratic(1, -(root0 + root1), root0 * root1);
        Assert.AreEqual(2, roots.Length);
        Assert.AreEqual(root0, roots[0], Tolerance);
        Assert.AreEqual(root1, roots[1], Tolerance);
    }
    [TestCase(1f, 1f, 1f)]
    [TestCase(1f, -2f, 1.0001f)]
    public void SolveQuadraticEquation_ImaginaryRoots(float a, float b, float c) {
        var roots = MathHelper.SolveQuadratic(a, b, c);
        Assert.AreEqual(0, roots.Length);
    }

}
