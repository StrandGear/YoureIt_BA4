using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerInterfaces : MonoBehaviour
{
    //
}

public interface ITransparency
{
    float Transparency { get; set; }
}

public interface ILockable
{
    bool isLocked { get; set; }
}

public interface ILayerOrder
{
    int LayerOrder { get; set; }
}

public interface ILinkable
{
    bool isLinkable { get; set; }
}