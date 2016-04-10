using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class GestureLibrary
{
    private string libraryName;
    private string libraryFilename;
    private string persistentLibraryPath;
    private string xmlContents;
    private XmlDocument gestureLibrary = new XmlDocument();
    private List<Gesture> library = new List<Gesture>();

    public List<Gesture> Library { get { return library; } }

    public GestureLibrary(string libraryName)
    {
        this.libraryName = libraryName;
        this.libraryFilename = libraryName + ".xml";
        this.persistentLibraryPath = Path.Combine(Application.persistentDataPath, libraryFilename);

        if (!Application.isWebPlayer)
        {
            CopyToPersistentPath();
        }

        LoadLibrary();
    }


    public void LoadLibrary()
    {
        string xmlContents = "";

        xmlContents = FileTools.Read(persistentLibraryPath);
        gestureLibrary.LoadXml(xmlContents);

        XmlNodeList xmlGestureList = gestureLibrary.GetElementsByTagName("gesture");

        foreach (XmlNode xmlGestureNode in xmlGestureList)
        {
            string gestureName = xmlGestureNode.Attributes.GetNamedItem("name").Value;
            XmlNodeList xmlPoints = xmlGestureNode.ChildNodes;
            List<Vector2> gesturePoints = new List<Vector2>();

            foreach (XmlNode point in xmlPoints)
            {
                Vector2 gesturePoint = new Vector2();
                gesturePoint.x = (float)System.Convert.ToDouble(point.Attributes.GetNamedItem("x").Value);
                gesturePoint.y = (float)System.Convert.ToDouble(point.Attributes.GetNamedItem("y").Value);
                gesturePoints.Add(gesturePoint);
            }
            Gesture gesture = new Gesture(gesturePoints, gestureName);
            library.Add(gesture);
        }
    }

    public void AddGesture(Gesture gesture)
    {
        XmlElement rootElement = gestureLibrary.DocumentElement;
        XmlElement gestureNode = gestureLibrary.CreateElement("gesture");
        gestureNode.SetAttribute("name", gesture.Name);

        foreach (Vector2 v in gesture.Points)
        {
            XmlElement gesturePoint = gestureLibrary.CreateElement("point");
            gesturePoint.SetAttribute("x", v.x.ToString());
            gesturePoint.SetAttribute("y", v.y.ToString());

            gestureNode.AppendChild(gesturePoint);
        }
        rootElement.AppendChild(gestureNode);

        this.Library.Add(gesture);
        FileTools.Write(persistentLibraryPath, gestureLibrary.OuterXml);

    }

    private void CopyToPersistentPath()
    {
        if (!FileTools.Exists(persistentLibraryPath))
        {
            string fileContents = Resources.Load<TextAsset>(libraryName).text;
            FileTools.Write(persistentLibraryPath, fileContents);
        }

    }


}