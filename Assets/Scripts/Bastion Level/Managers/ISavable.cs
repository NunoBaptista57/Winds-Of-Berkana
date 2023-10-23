using System;
using UnityEngine;

public interface ISavable
{
    public SaveFile Save(SaveFile saveFile);
    public void Load(SaveFile saveFile);
}