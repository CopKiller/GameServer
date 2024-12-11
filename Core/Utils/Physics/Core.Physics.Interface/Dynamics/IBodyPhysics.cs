using System.Numerics;
using Core.Physics.Interface.Enum;

namespace Core.Physics.Interface.Dynamics;

public interface IBodyPhysics
{
    Vector2 GetPosition();
    void ApplyForce(Vector2 force);
    void ApplyLinearImpulse(Vector2 impulse);
    void ApplyTorque(float torque);
    void ApplyAngularImpulse(float impulse);
    void SetLinearVelocity(Vector2 velocity);
    void SetAngularVelocity(float velocity);
    void SetTransform(Vector2 position, float rotation);
    void SetEnabled(bool enabled);
    void SetFixedRotation(bool fixedRotation);
    void SetBullet(bool bullet);
    void SetType(EBodyType type);
    void SetLinearDamping(float linearDamping);
    void SetAngularDamping(float angularDamping);
    void SetGravityScale(float gravityScale);
    void SetMass(float mass);
    void Dispose();
}