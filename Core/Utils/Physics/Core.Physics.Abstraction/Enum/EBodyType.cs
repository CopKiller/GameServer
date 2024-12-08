namespace Core.Physics.Abstraction.Enum;

public enum EBodyType
{
    /// <summary>Zero velocity, may be manually moved. Note: even static bodies have mass.</summary>
    Static,

    /// <summary>Zero mass, non-zero velocity set by user, moved by solver</summary>
    Kinematic,

    /// <summary>Positive mass, non-zero velocity determined by forces, moved by solver</summary>
    Dynamic
}