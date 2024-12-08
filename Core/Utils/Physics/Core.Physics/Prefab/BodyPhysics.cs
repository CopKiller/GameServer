using System.Numerics;
using Core.Physics.Abstraction;
using Core.Physics.Abstraction.Enum;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Core.Physics.Prefab;

public class BodyPhysics : IBodyPhysics
{
    private readonly Body _body;

    public BodyPhysics(Body body) => _body = body;

    public void ApplyForce(Vector2 force) => _body.ApplyForce(new Vector2(force.X, force.Y));

    public void ApplyLinearImpulse(Vector2 impulse) => _body.ApplyLinearImpulse(new Vector2(impulse.X, impulse.Y));

    public void ApplyTorque(float torque) => _body.ApplyTorque(torque);

    public void ApplyAngularImpulse(float impulse) => _body.ApplyAngularImpulse(impulse);

    public void SetLinearVelocity(Vector2 velocity) => _body.LinearVelocity = new Vector2(velocity.X, velocity.Y);

    public void SetAngularVelocity(float velocity) => _body.AngularVelocity = velocity;

    public void SetTransform(Vector2 position, float rotation)
    {
        _body.SetTransform(new Vector2(position.X, position.Y), rotation);
    }

    public void SetEnabled(bool enabled) => _body.Enabled = enabled;

    public void SetFixedRotation(bool fixedRotation) => _body.FixedRotation = fixedRotation;

    public void SetBullet(bool bullet) => _body.IsBullet = bullet;

    public void SetType(EBodyType type)
    {
        _body.BodyType = type switch
        {
            EBodyType.Static => BodyType.Static,
            EBodyType.Kinematic => BodyType.Kinematic,
            EBodyType.Dynamic => BodyType.Dynamic,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public void SetLinearDamping(float linearDamping) => _body.LinearDamping = linearDamping;

    public void SetAngularDamping(float angularDamping) => _body.AngularDamping = angularDamping;

    public void SetGravityScale(float gravityScale) => _body.GravityScale = gravityScale;

    public void SetMass(float mass) => _body.Mass = mass;
}