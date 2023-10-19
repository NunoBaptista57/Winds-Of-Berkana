using System;
using UnityEngine;

public interface IManager
{
    public SaveFile Save(SaveFile saveFile);
    public void Load(SaveFile saveFile);
}