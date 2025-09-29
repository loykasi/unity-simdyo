using System;
using System.Collections.Generic;
using System.Linq;
using Loykas.Scripting;

public static class KeyHandler
{
    public static KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                .Cast<KeyCode>()
                                                .ToArray();
    public static List<string> KeyOptions = Enum.GetValues(typeof(KeyCode))
                                                .Cast<KeyCode>()
                                                .Select(k => k.ToString())
                                                .ToList();

    public static Key ToKey(int index)
    {
        KeyCode keyCode = keyCodes[index];
        return new Key(keyCode);
    }

    public static int ToIndex(Key key)
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (keyCodes[i] == key.Value)
            {
                return i;
            }
        }
        
        return -1;
    }
}