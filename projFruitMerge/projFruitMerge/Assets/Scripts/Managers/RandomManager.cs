using System;
using System.Collections.Generic;

public static class RandomManager
{
    public static T GetRandomObject<T>(List<T> objects)
    {
        Random random = new Random();
        return objects[random.Next(0, objects.Count)];
    }

    public static int GetRandomIndex(int max)
    {
        Random random = new Random();
        return random.Next(0, max);
    }
}
