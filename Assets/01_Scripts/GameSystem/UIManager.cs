using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton
{
    [field: SerializeField] public GameObject LayerUI {get; private set;}
    [field: SerializeField] public GameObject GameUI {get; private set;}
}
