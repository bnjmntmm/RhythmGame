using Godot;
using Godot.Collections;
using System;

public partial class Lights : Node3D
{
    [Export] public Color defaultColor = Colors.White;

    private Array<Node> _spotLights;

    public override void _Ready()
    {
        // Get all SpotLight3D nodes in the scene
        _spotLights = GetChildren();
    }


    public void ChangeLightToColor(Color color)
    {
        // Iterate through all SpotLight3D nodes and change their color
        foreach (Node child in _spotLights)
        {
            if (child is SpotLight3D light)
            {
                light.LightColor = color;
            }
        }
    }
    public void ResetColor()
    {
        // Iterate through all SpotLight3D nodes and reset their color to default
        foreach (Node child in _spotLights)
        {
            if (child is SpotLight3D light)
            {
                light.LightColor = defaultColor;
            }
        }
    }
}
