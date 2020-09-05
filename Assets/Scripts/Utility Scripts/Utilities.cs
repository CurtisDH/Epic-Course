using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CurtisDH
{
    namespace Utilites
    {
        public class Utilites
        {
            public static void GenerateText(Vector2 position, string text, float ScaleSize)
            {
                if (GameObject.Find("WorldSpaceTextCanvas") == null)
                {
                    new GameObject("WorldSpaceTextCanvas").AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                    var textobj = new GameObject("GeneratedText");
                    textobj.AddComponent<TextMeshPro>().text = text;
                    textobj.transform.position = position;
                    textobj.transform.parent = GameObject.Find("WorldSpaceTextCanvas").transform;
                    textobj.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);

                }
                else
                {
                    var textobj = new GameObject("GeneratedText");
                    textobj.AddComponent<TextMeshPro>().text = text;
                    textobj.transform.position = position;
                    textobj.transform.parent = GameObject.Find("WorldSpaceTextCanvas").transform;
                    textobj.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
                }
                Debug.Log("GenerateText");

            }

            public static Vector3 GetMousePosition(bool WorldSpace = true)
            {
                Vector3 mousepos = Input.mousePosition;

                if (WorldSpace == true)
                {
                    Vector3 MousePosWS = Camera.main.ScreenToWorldPoint(mousepos);
                    return new Vector3(MousePosWS.x, MousePosWS.y, 0);
                }
                else
                {
                    return mousepos;
                }
            }
            public static void RandomiseList<T>(List<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var randomIndex = Random.Range(0, list.Count);
                    var val = list[randomIndex];
                    list[randomIndex] = list[i];
                    list[i] = val;

                }
            }
        }
    }

}
