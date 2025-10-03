using UnityEngine;

public class SignalSystem : Singleton<SignalSystem>
{
    public void SendSignal(string signalName)
    {
        Debug.Log($"Try send signal: {signalName}");
        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Script.SendSignal(signalName);
        }
    }
}