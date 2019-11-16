using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour
{
    public Material Material1;
    public Material Material2;
    
    private Renderer _renderer;
    public Renderer Renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            return _renderer;
        }
    }

    // Use this for initialization
	void Start ()
	{
	    Destroy(this);
	}

    public void ToggleMaterial()
    {
        if (Renderer.sharedMaterial == Material1)
            SetMaterial(Type.First);
        else if (Renderer.sharedMaterial == Material2)
            SetMaterial(Type.Second);
    }

    public void SetMaterial(Type type)
    {
        Renderer.sharedMaterial = type == Type.First ? Material1 : Material2;
    }

    public void SetAll(Type type)
    {
        SwitchMaterial[] sms = GameObject.FindObjectsOfType<SwitchMaterial>();
        foreach (SwitchMaterial switchMaterial in sms)
        {
            switchMaterial.SetMaterial(type);
        }
    }

    public enum Type
    {
        First,
        Second
    }
}
