using System.Numerics;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Enum;
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
}