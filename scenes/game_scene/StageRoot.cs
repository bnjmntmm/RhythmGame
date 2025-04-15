using Godot;
using System;

public partial class StageRoot : Node3D
{

[Export]    
private SpotLight3D[] _spotLights;

public SpotLight3D[] SpotLights => _spotLights;

public override void _Ready()
{
}
}
