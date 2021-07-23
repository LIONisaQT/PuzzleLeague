using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;

public static class Utility
{
    private static readonly Random rng = new Random();

    public static Collider2D GetRaycastResultCollider(Vector3 from, Vector3 direction, float distance = Mathf.Infinity) => Physics2D.Raycast(from, direction, distance).collider;

    public static T GetRandomElementFromList<T>(List<T> list) => list[rng.Next(list.Count)];

    // TODO: Have some kind of ScriptableObject instead of comparing hex values.
    public static string GetColorName(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color) switch
        {
            "24AAC3" => "BLUE",
            "18CF35" => "GREEN",
            "5F0E9A" => "PURPLE",
            "D44131" => "RED",
            "DDD52D" => "YELLOW",
            _ => $"Color {color} is not defined",
        };
    }
}
