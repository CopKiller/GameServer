
using Core.Physics.Interface.Dynamics;
using Core.Physics.Interface.Dynamics.Events;
using Core.Physics.Interface.Dynamics.Handlers;
using Genbox.VelcroPhysics.Dynamics.Events;

namespace Core.Physics.Dynamics.Events;

/// <summary>
/// This class is a wrapper for the VelcroPhysics World events.
/// </summary>
public class WorldEvents(IWorldVelcroEvents events) : IWorldEvents
{
    /// <summary>Fires whenever a body has been added</summary>
    public event BodyHandler? OnBodyAdded
    {
        add => events.OnBodyAdded += body => value?.Invoke(new BodyPhysics(body));
        remove => events.OnBodyAdded -= body => value?.Invoke(new BodyPhysics(body));
    }

    /// <summary>Fires whenever a body has been removed</summary>
    public event BodyHandler? OnBodyRemoved
    {
        add => events.OnBodyRemoved += body => value?.Invoke(new BodyPhysics(body));
        remove => events.OnBodyRemoved -= body => value?.Invoke(new BodyPhysics(body));
    }

    /*/// <summary>Fires every time a controller is added to the World.</summary>
    public event ControllerHandler? OnControllerAdded;

    /// <summary>Fires every time a controller is removed form the World.</summary>
    public event ControllerHandler? OnControllerRemoved;

    /// <summary>Fires whenever a fixture has been added</summary>
    public event FixtureHandler? OnFixtureAdded;

    /// <summary>Fires whenever a fixture has been removed</summary>
    public event FixtureHandler? OnFixtureRemoved;

    /// <summary>Fires whenever a joint has been added</summary>
    public event JointHandler? OnJointAdded;

    /// <summary>Fires whenever a joint has been removed</summary>
    public event JointHandler? OnJointRemoved;
    */
}