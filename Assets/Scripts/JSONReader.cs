using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    [System.Serializable]
    public class LevelInfo
    {
       public string [] objectList_M;
       public string[] objectList_F ;
       public string[] objectList_N ;
       public StorePoint[] storePoints ;
    }
    [System.Serializable]
    public class StorePoint
    {
        public string name;
        public Object[] objects;
    }
    [System.Serializable]
    public class Object
    {
        public string name;
        public int position;
        public string gender;

    }
    public LevelInfo LoadFile(string file)
    {
        LevelInfo levelInfo;
        levelInfo = JsonUtility.FromJson<LevelInfo>(file);
        return levelInfo;
    }

}
