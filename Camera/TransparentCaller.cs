using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCaller : MonoBehaviour
{
    
    public float RayDistance = 30f;
    public LayerMask LayerMask;
    public float UpdateRate = 10f;
    public float FadeSpeed = 1f;
    public float MinAlpha = 0.6f;

    [ReadOnly]
    public int ObstaclesFound;

    [ReadOnly]
    public Transform[] Targets;

    private float _nextTick;
    private Dictionary<Transform, TransparentElement> _obstacles = new Dictionary<Transform, TransparentElement>();

    private void Start()
    {
        Targets = new Transform[0];
        if (Targets.Length == 0)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            Targets = new Transform[gos.Length];
            for (int i = 0; i < gos.Length; ++i)
            {
                Targets[i] = gos[i].transform;
            }
        }

        if (MinAlpha < 0)
        {
            Debug.Log("Min Alpha too low, setting to 0");
            MinAlpha = 0;
        }
        else if (MinAlpha >= 1)
        {
            Debug.Log("Min alpha too high, setting to 0,9");
            MinAlpha = 0.9f;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		if (Time.time > _nextTick)
        {
            // Set all elements to disappear, they will invert if they are still colliding with cam rays
            foreach(TransparentElement element in _obstacles.Values)
                element.FadingIn = false;

            // Add all elements in cam rays
            List<Transform> obstaclesCollided = new List<Transform>();
            foreach (Transform target in Targets)
            {
                Vector3 direction = target.position - transform.position;
                Ray ray = new Ray(transform.position, direction);
                //Debug.DrawRay(transform.position, direction, Color.yellow, (1f / UpdateRate));
                
                RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude - 1, LayerMask);
                foreach(RaycastHit hit in hits)
                {
                    if (!obstaclesCollided.Contains(hit.transform))
                        obstaclesCollided.Add(hit.transform);
                }
            }

            // Set fadingIn for all colliding walls
            foreach (Transform obstacleCollided in obstaclesCollided)
            {
                // Add the new colliding wall if not already in the main list
                if (_obstacles.ContainsKey(obstacleCollided))
                    _obstacles[obstacleCollided].FadingIn = true;
                else
                {
                    MeshRenderer renderer = obstacleCollided.GetComponent<MeshRenderer>();
                    if (renderer == null)
                    {
                        Debug.LogWarning("[TRANSPARENT CALLER] Can't find renderer for " + obstacleCollided.name, obstacleCollided);
                        continue;
                    }

                    _obstacles.Add(obstacleCollided, new TransparentElement(renderer.materials, MinAlpha, true));
                }
            }

            // Fade in or out every obstacles
            List<Transform> toRemove = new List<Transform>();
            foreach (KeyValuePair<Transform, TransparentElement> obstacle in _obstacles)
            {
                bool finished = obstacle.Value.Fade((1f / UpdateRate) * FadeSpeed);
                if (obstacle.Value.FadingOut && finished)
                    toRemove.Add(obstacle.Key);
            }

            // Remove all finished fade out
            foreach(Transform toRemoveItem in toRemove)
                _obstacles.Remove(toRemoveItem);

            _nextTick = Time.time + (1f / UpdateRate);
            ObstaclesFound = _obstacles.Count;
        }
	}
}


internal class TransparentElement
{
    // fading in = disappearing
    // fading out = appearing
    public bool FadingIn;
    public bool FadingOut {  get { return !FadingIn; } }

    public Material[] Materials;
    public float MinAlpha;
    
    public TransparentElement(Material[] materials, float minAlpha, bool fadingIn = true)
    {
        MinAlpha = minAlpha;
        Materials = materials;
        FadingIn = fadingIn;
    }

    public bool Fade(float speed)
    {
        if (FadingIn)
            return FadeIn(speed);
        else
            return FadeOut(speed);
    }

    public bool FadeIn(float speed)
    {
        foreach(Material mat in Materials)
        {
            Color color = mat.color;
            color.a -= speed;
            if (color.a < MinAlpha)
                color.a = MinAlpha;

            mat.color = color;
        }

        return Materials[0].color.a == MinAlpha;
    }

    public bool FadeOut(float speed)
    {
        foreach (Material mat in Materials)
        {
            Color color = mat.color;
            color.a += speed;
            if (color.a > 1)
                color.a = 1;

            mat.color = color;
        }

        return Materials[0].color.a == 1;
    }
}