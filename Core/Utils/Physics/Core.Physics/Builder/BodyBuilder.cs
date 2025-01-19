using System.Numerics;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Enum;
using Genbox.VelcroPhysics.Definitions;

namespace Core.Physics.Builder;

// Definição de corpo físico (Builder)
public class BodyBuilder : IBodyBuilder
{
    private readonly BodyDef _bodyDef = new();

    public int Id { get; set; }

    public EEntityType EntityType { get; set; }

    /// <summary>Is this body initially awake or sleeping?</summary>
    public bool Awake
    {
        get => _bodyDef.Awake;
        set => _bodyDef.Awake = value;
    }

    /// <summary>Does this body start out active?</summary>
    public bool Enabled
    {
        get => _bodyDef.Enabled;
        set => _bodyDef.Enabled = value;
    }

    /// <summary>Is this a fast moving body that should be prevented from tunneling through other moving bodies? Note that all
    /// bodies are prevented from tunneling through kinematic and static bodies. This setting is only considered on dynamic
    /// bodies.
    /// <remarks>Warning: You should use this flag sparingly since it increases processing time.</remarks>
    /// </summary>
    public bool IsBullet
    {
        get => _bodyDef.IsBullet;
        set => _bodyDef.IsBullet = value;
    }

    /// <summary>Set this flag to false if this body should never fall asleep.
    /// <remarks>Note: Setting this to false increases CPU usage.</remarks>
    /// </summary>
    public bool AllowSleep
    {
        get => _bodyDef.AllowSleep;
        set => _bodyDef.AllowSleep = value;
    }

    /// <summary>The world angle of the body in radians.</summary>
    public float Angle
    {
        get => _bodyDef.Angle;
        set => _bodyDef.Angle = value;
    }

    /// <summary>Angular damping is use to reduce the angular velocity. The damping parameter can be larger than 1.0f but the
    /// damping effect becomes sensitive to the time step when the damping parameter is large.</summary>
    public float AngularDamping
    {
        get => _bodyDef.AngularDamping;
        set => _bodyDef.AngularDamping = value;
    }

    /// <summary>The angular velocity of the body.</summary>
    public float AngularVelocity
    {
        get => _bodyDef.AngularVelocity;
        set => _bodyDef.AngularVelocity = value;
    }

    /// <summary>Scale the gravity applied to this body.</summary>
    public float GravityScale
    {
        get => _bodyDef.GravityScale;
        set => _bodyDef.GravityScale = value;
    }

    /// <summary>Linear damping is use to reduce the linear velocity. The damping parameter can be larger than 1.0f but the
    /// damping effect becomes sensitive to the time step when the damping parameter is large.</summary>
    public float LinearDamping
    {
        get => _bodyDef.LinearDamping;
        set => _bodyDef.LinearDamping = value;
    }

    /// <summary>The linear velocity of the body's origin in world co-ordinates.</summary>
    public Vector2 LinearVelocity
    {
        get => _bodyDef.LinearVelocity;
        set => _bodyDef.LinearVelocity = value;
    }

    /// <summary>The world position of the body.</summary>
    public Vector2 Position
    {
        get => _bodyDef.Position;
        set => _bodyDef.Position = value;
    }

    /// <summary>Set the type of body
    /// <remarks>Note: if a dynamic body would have zero mass, the mass is set to one.</remarks>
    /// </summary>
    public EBodyType Type
    {
        get => _bodyDef.Type.ToCorePhysics();
        set => _bodyDef.Type = value.ToVelcroPhysics();
    }

    /// <summary>Use this to store application specific body data.</summary>
    public object UserData
    {
        get => _bodyDef.UserData;
        set => _bodyDef.UserData = value;
    }

    /// <summary>Should this body be prevented from rotating? Useful for characters.</summary>
    public bool FixedRotation
    {
        get => _bodyDef.FixedRotation;
        set => _bodyDef.FixedRotation = value;
    }

    /// <summary>
    /// Largura do corpo físico
    /// </summary>
    public float Width { get; set; }
    
    /// <summary>
    /// Altura do corpo físico
    /// </summary>
    public float Height { get; set; }

    public float Density{ get; set; }

    public void SetDefaults()
    {
        Position = Vector2.Zero;
        Angle = 0.0f;
        LinearVelocity = Vector2.Zero;
        AngularVelocity = 0.0f;
        LinearDamping = 0.0f;
        AngularDamping = 0.0f;
        AllowSleep = true;
        Awake = true;
        FixedRotation = false;
        IsBullet = false;
        Type = EBodyType.Static;
        Enabled = true;
        GravityScale = 1.0f;
        Width = 1.0f;
        Height = 1.0f;
        Density = 1.0f;
    }

    public object ToVelcroPhysics() => _bodyDef;
}