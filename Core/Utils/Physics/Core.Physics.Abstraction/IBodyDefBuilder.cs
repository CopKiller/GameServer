using System.Numerics;
using Core.Physics.Abstraction.Enum;

namespace Core.Physics.Abstraction;

public interface IBodyDefBuilder
{
    int Id { get; set; }
    EEntityType EntityType { get; set; }
    bool Awake { get; set; }
    bool Enabled { get; set; }
    bool IsBullet { get; set; }
    bool AllowSleep { get; set; }
    float Angle { get; set; }
    float AngularDamping { get; set; }
    float AngularVelocity { get; set; }
    float GravityScale { get; set; }
    float LinearDamping { get; set; }
    Vector2 LinearVelocity { get; set; }
    Vector2 Position { get; set; }
    EBodyType Type { get; set; }
    object UserData { get; set; }
    bool FixedRotation { get; set; }
    float Width { get; set; }
    float Height { get; set; }
    float Density { get; set; }
    void SetDefaults();
}