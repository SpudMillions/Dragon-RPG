using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        //registering cursor change to delegate
        cameraRayCaster.onLayerChange += OnLayerChanged;
	}
	
	// used to set cursor style
	void OnLayerChanged (Layer newLayer) {
        //choose what cursor to show based on layer
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.Log("Cursor Affordance : Don't know what cursor to show");
                return;
        }
    }
}
