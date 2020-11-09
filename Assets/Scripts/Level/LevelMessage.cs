using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMessage
{
    public enum MessageType {
        GameWin,
        GameLose,
        CallToRestart
    }

    public MessageType Type { get; protected set; }

    public object[] Data;

    public LevelMessage(MessageType type, params object[] data) {
        this.Type = type;
        this.Data = data;
    }
}
