using System.Numerics;
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Enum;
using Genbox.VelcroPhysics.Dynamics;

namespace Core.Physics.Dynamics;

public class BodyPhysics(Body body) : IBodyPhysics
{
    public Vector2 GetPosition() => body.Position;
    public void ApplyForce(Vector2 force) => body.ApplyForce(force);

    public void ApplyLinearImpulse(Vector2 impulse) => body.ApplyLinearImpulse(impulse);

    public void ApplyTorque(float torque) => body.ApplyTorque(torque);

    public void ApplyAngularImpulse(float impulse) => body.ApplyAngularImpulse(impulse);

    public void SetLinearVelocity(Vector2 velocity) => body.LinearVelocity = velocity;

    public void SetAngularVelocity(float velocity) => body.AngularVelocity = velocity;

    public void SetTransform(Vector2 position, float rotation)
    {
        body.SetTransform(position, rotation);
    }

    public void SetEnabled(bool enabled) => body.Enabled = enabled;

    public void SetFixedRotation(bool fixedRotation) => body.FixedRotation = fixedRotation;

    public void SetBullet(bool bullet) => body.IsBullet = bullet;

    public void SetType(EBodyType type)
    {
        body.BodyType = type switch
        {
            EBodyType.Static => BodyType.Static,
            EBodyType.Kinematic => BodyType.Kinematic,
            EBodyType.Dynamic => BodyType.Dynamic,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public void SetLinearDamping(float linearDamping) => body.LinearDamping = linearDamping;

    public void SetAngularDamping(float angularDamping) => body.AngularDamping = angularDamping;

    public void SetGravityScale(float gravityScale) => body.GravityScale = gravityScale;

    public void SetMass(float mass) => body.Mass = mass;
    public void Dispose() => body.RemoveFromWorld();
}