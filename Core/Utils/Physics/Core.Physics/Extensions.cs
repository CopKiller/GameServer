using System.Numerics;
using Core.Physics.Abstraction;
using Core.Physics.Abstraction.Enum;
using Core.Physics.Prefab;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;

namespace Core.Physics;

public static class Extensions
{
    public static BodyType ToVelcroPhysics(this EBodyType bodyType)
    {
        return bodyType switch
        {
            EBodyType.Static => BodyType.Static,
            EBodyType.Kinematic => BodyType.Kinematic,
            EBodyType.Dynamic => BodyType.Dynamic,
            _ => BodyType.Static
        };
    }
    
    public static EBodyType ToCorePhysics(this BodyType bodyType)
    {
        return bodyType switch
        {
            BodyType.Static => EBodyType.Static,
            BodyType.Kinematic => EBodyType.Kinematic,
            BodyType.Dynamic => EBodyType.Dynamic,
            _ => EBodyType.Static
        };
    }
    
    public static BodyDef ToVelcroPhysics(this IBodyDefBuilder iBodyDef)
    {
        var def = new BodyDef
        {
            Position = new Vector2(iBodyDef.Position.X, iBodyDef.Position.Y),
            Type = iBodyDef.Type.ToVelcroPhysics(),
            FixedRotation = iBodyDef.FixedRotation,
            IsBullet = iBodyDef.IsBullet,
            Angle = iBodyDef.Angle,
            Enabled = iBodyDef.Enabled
        };
        
        return def;
    }
}