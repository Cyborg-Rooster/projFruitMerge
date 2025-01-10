using System;
using System.Collections.Generic;
using UnityEngine;

class RaycastManager
{
    static Vector2 Direction = Vector2.right;

    static Dictionary<string, float> collisionsTimers = new Dictionary<string, float>();

    public static bool IsColliding(float distance, float collisionTime, Vector2 StartPos, LayerMask layerMask)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(StartPos, Direction, distance, layerMask);
        Debug.DrawLine(StartPos, StartPos + Direction * distance, Color.red);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                string objectName = hit.collider.name;

                if (collisionsTimers.ContainsKey(objectName)) collisionsTimers[objectName] += Time.deltaTime;
                else collisionsTimers[objectName] = Time.deltaTime;

                if (collisionsTimers[objectName] >= collisionTime) return true;
            }
        }

        List<string> keysToRemove = new List<string>();
        foreach (var key in collisionsTimers.Keys)
        {
            bool stillColliding = false;
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.name == key) stillColliding = true; break;
            }

            if (!stillColliding) keysToRemove.Add(key);
        }

        foreach (var key in keysToRemove) collisionsTimers.Remove(key);

        return false;
    }
}
