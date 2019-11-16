using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public static Cameraman Instance;

    public Camera[] Cameras;
    private Dictionary<int, List<Camera>> _displaysWithCams;

	// Use this for initialization
	void Awake ()
	{
	    if (Instance != null && Instance != this)
	    {
            Destroy(gameObject);
	        return;
	    }

	    Instance = this;
	}

    public void Update()
    {
        SetCameras();
    }

    internal void SetCameras()
    {
        _displaysWithCams = new Dictionary<int, List<Camera>>();

        foreach (Camera camera in Cameras)
        {
            int displayIndex = camera.targetDisplay;
            // If new display
            if (!_displaysWithCams.ContainsKey(displayIndex))
                _displaysWithCams.Add(displayIndex, new List<Camera>());

            _displaysWithCams[displayIndex].Add(camera);
        }

        // Foreach display, set split screens
        foreach (List<Camera> cameras in _displaysWithCams.Values)
        {
            float division = 1f / cameras.Count;

            for (int i = 0; i < cameras.Count; ++i)
            {
                Vector2 position = new Vector2(division * i, 0);
                Vector2 size = new Vector2(division, 1);
                cameras[i].rect = new Rect(position, size);
            }
        }
    }
}
