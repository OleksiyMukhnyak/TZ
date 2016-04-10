using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawPointHelper : MonoBehaviour
{

    private List<Vector2> points = new List<Vector2>();

    public GameObject gestureOnScreen;
    private LineRenderer gestureLineRenderer;
    private int vertexCount = 0;

    private string message;
    private RuntimePlatform platform;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private GestureLibrary gl;

    public string libraryToLoad;


    void Start()
    {
        gl = new GestureLibrary(libraryToLoad);

        platform = Application.platform;

        gestureLineRenderer = gestureOnScreen.GetComponent<LineRenderer>();

        drawArea = new Rect(0, 0, Screen.width, Screen.height);
    }


    void Update()
    {

        if (platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else {
            if (Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        if (drawArea.Contains(virtualKeyPosition))
        {

            if (Input.GetMouseButtonDown(0))
            {
                ClearPoints();
                
            }

            if (Input.GetMouseButton(0))
            {
                points.Add(new Vector2(virtualKeyPosition.x, -virtualKeyPosition.y));

                gestureLineRenderer.SetVertexCount(++vertexCount);
                gestureLineRenderer.SetPosition(vertexCount - 1, WorldCoordinateForGesturePoint(virtualKeyPosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                Gesture g = new Gesture(points);
                Result result = g.Recognize(gl, true);
                gameObject.GetComponent<GameHelper>().ChengeFigyre(result);
                message = result.Name + "; " + result.Score;
                Debug.Log(message);
            }

        }

    }

    public void ClearPoints()
    {
        points.Clear();
        gestureLineRenderer.SetVertexCount(0);
        vertexCount = 0;
    }

    private Vector3 WorldCoordinateForGesturePoint(Vector3 gesturePoint)
    {
        Vector3 worldCoordinate = new Vector3(gesturePoint.x, gesturePoint.y, 10);
        return Camera.main.ScreenToWorldPoint(worldCoordinate);
    }
}
