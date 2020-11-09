using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SpaceshipMessage {
    public enum MessageType {
        Created,
        Hit,
        StateReturnToDefault,
        HealthChanged,
        Died
    }

    public MessageType Type;

    public SpaceshipMessage(MessageType type) {
        this.Type = type;
    }
}

public interface ISpaceshipControllerState
{
    bool CanMove { get; }

    bool CanShoot { get; }

    bool CanBeHit { get; }

    ISpaceshipControllerState UpdateState(SpaceshipMessage.MessageType message);
}

public class DefaultSpaceshipControllerState : ISpaceshipControllerState {

    public bool CanMove { get { return true; } }

    public bool CanShoot { get { return true; } }

    public bool CanBeHit { get { return true; } }

    public ISpaceshipControllerState UpdateState(SpaceshipMessage.MessageType message) {
        if (message == SpaceshipMessage.MessageType.Hit)
            return new OutOfControlControllerState();
        if (message == SpaceshipMessage.MessageType.Died)
            return new DiedSpaceshipControllerState();

        return this;
    }
}

public class OutOfControlControllerState : ISpaceshipControllerState {

    private static TimeSpan OUT_OF_CONTROL_TIME = System.TimeSpan.FromSeconds(2);

    private IDisposable disposeTimer;

    public OutOfControlControllerState() {
        disposeTimer = Observable.Timer(OUT_OF_CONTROL_TIME)
            .Subscribe(OUT_OF_CONTROL_TIME => {
                MessageBroker.Default.Publish<SpaceshipMessage>(
                    new SpaceshipMessage(SpaceshipMessage.MessageType.StateReturnToDefault)
                );
            });
    }

    public bool CanMove { get { return false; } }

    public bool CanShoot { get { return false; } }

    public bool CanBeHit { get { return false; } }

    public ISpaceshipControllerState UpdateState(SpaceshipMessage.MessageType message) {
        
        switch (message) {
            case SpaceshipMessage.MessageType.Hit: {
                disposeTimer.Dispose();
                return new OutOfControlControllerState();
            }
            case SpaceshipMessage.MessageType.StateReturnToDefault: {
                return new DefaultSpaceshipControllerState();
            }
            case SpaceshipMessage.MessageType.Died: {
                return new DiedSpaceshipControllerState();
            }
        }
        return this;
    }
}

public class DiedSpaceshipControllerState : ISpaceshipControllerState {

    public bool CanMove { get { return false; } }

    public bool CanShoot { get { return false; } }

    public bool CanBeHit { get { return false; } }

    public ISpaceshipControllerState UpdateState(SpaceshipMessage.MessageType message) {
        return this;
    }
}