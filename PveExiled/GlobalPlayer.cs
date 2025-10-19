using Exiled;
using System.Linq.Expressions;
using UnityEngine;

public static class GlobalPlayer
{
    public static void TryPlay(string name, float volume, bool loop) { }
    public static void TryStopAudio(string name) { }

    public static void TryPlayOnPosition(string name, float volume, bool loop, Vector3 position, float range) { }
}