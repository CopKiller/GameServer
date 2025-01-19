using Core.Physics.Interface.Dynamics.Handlers;

namespace Core.Physics.Interface.Dynamics.Events;

public interface IWorldEvents
{
    event BodyHandler? OnBodyAdded;

    event BodyHandler? OnBodyRemoved;

    /*event ControllerHandler? OnControllerAdded;

    event ControllerHandler? OnControllerRemoved;

    event FixtureHandler? OnFixtureAdded;

    event FixtureHandler? OnFixtureRemoved;

    event JointHandler? OnJointAdded;

    event JointHandler? OnJointRemoved;*/
}