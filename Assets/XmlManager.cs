using System;
using UnityEngine;
using static Utils;
public class XmlManager : MonoBehaviour
{
    private void Awake()
    {
        InitializeXml();
    }

    private void InitializeXml()
    {
        string path = Application.dataPath + "/StreamingAssets/Game.xml";
        XmlLoader(path);
    }

}
