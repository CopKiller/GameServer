namespace Game.Scripts.Extensions.Attributes;

using System;

/// <summary>
/// Atributo para armazenar o caminho da cena, para uso em cenas de estados do jogo.
/// </summary>
/// <param name="path">Caminho da cena no modelo de diretorios do godot.</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ScenePathAttribute(string path) : Attribute
{
    public string Path { get; } = path;
}