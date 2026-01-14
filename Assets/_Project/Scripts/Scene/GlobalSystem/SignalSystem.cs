using UnityEngine;

public class SignalSystem : Singleton<SignalSystem>
{
    public void SendSignal(string signalName, SceneEntity entity)
    {
        if (entity != null)
        {
            entity.Script.SendSignal(signalName);
            return;
        }

        SceneManager.Instance.GlobalScript.SendSignal(signalName);
        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Script.SendSignal(signalName);
        }
    }
}