using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class CalcShootAngleScript {
    const float Tolerance = 0.00001f;
    [Test]
    public void ZeroTargetVelocityAndGravity() {
        AssertVelocity(Vector3.zero, new Vector3(10, 5, 0), Vector3.zero, 10, 0);
        AssertVelocity(Vector3.zero, new Vector3(10, -5, 0), Vector3.zero, 10, 0);
        AssertVelocity(Vector3.zero, new Vector3(-10, -5, 0), Vector3.zero, 10, 0);
        AssertVelocity(Vector3.zero, new Vector3(-10, 5, 0), Vector3.zero, 10, 0);
    }
    [Test]
    public void ZeroTargetVelocity() {
        AssertVelocity(Vector3.zero, new Vector3(10, 5, 0), Vector3.zero, 10, 10);
        AssertVelocity(new Vector3(1, 2, 3), new Vector3(-10, 5, 2), Vector3.zero, 10, 10);
    }

    void AssertVelocity(Vector3 origination, Vector3 target, Vector3 targetVelocity, float velocity, float gravity) {
        Vector3 calculatedVelocity = MathHelper.CalcShootVelocity(origination, target, velocity, gravity).Value;
        target = target - origination;
        Vector3 relativeVelocity = targetVelocity - calculatedVelocity;
        float time = -target.x / relativeVelocity.x;
        Assert.IsTrue(0 <= time);
        Assert.AreEqual(0, target.z  + relativeVelocity.z * time, Tolerance);
        Assert.AreEqual(0, target.y + relativeVelocity.y * time - gravity * time * time / 2, Tolerance);
        Assert.AreEqual(velocity, calculatedVelocity.magnitude, Tolerance);
    }
}
