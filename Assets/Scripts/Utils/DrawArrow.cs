 using UnityEngine;
 
public static class DrawArrow
{
    /// <summary>
    /// Draws an arrow at specified position and direction.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="arrowHeadLength"></param>
    /// <param name="arrowHeadAngle"></param>
    public static void ForGizmo(Vector3 pos, Vector3 dir, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay(pos, dir);
       
        var right = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 + arrowHeadAngle,0) * new Vector3(0,0,1);
        var left = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 - arrowHeadAngle,0) * new Vector3(0,0,1);
        Gizmos.DrawRay(pos + dir, right * arrowHeadLength);
        Gizmos.DrawRay(pos + dir, left * arrowHeadLength);
    }
 
    /// <summary>
    /// Draws an arrow at specified position, direction and color.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="color"></param>
    /// <param name="arrowHeadLength"></param>
    /// <param name="arrowHeadAngle"></param>
    public static void ForGizmo(Vector3 pos, Vector3 dir, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, dir);
       
        var right = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 + arrowHeadAngle,0) * new Vector3(0,0,1);
        var left = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 - arrowHeadAngle,0) * new Vector3(0,0,1);
        Gizmos.DrawRay(pos + dir, right * arrowHeadLength);
        Gizmos.DrawRay(pos + dir, left * arrowHeadLength);
    }
 
    /// <summary>
    /// Draws an arrow at specified position and direction for debugging purposes.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="arrowHeadLength"></param>
    /// <param name="arrowHeadAngle"></param>
    public static void ForDebug(Vector3 pos, Vector3 dir, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, dir);
       
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 + arrowHeadAngle,0) * new Vector3(0,0,1);
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 - arrowHeadAngle,0) * new Vector3(0,0,1);
        Debug.DrawRay(pos + dir, right * arrowHeadLength);
        Debug.DrawRay(pos + dir, left * arrowHeadLength);
    }
    
    /// <summary>
    /// Draws an arrow at specified position, direction and color for debugging purposes.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <param name="color"></param>
    /// <param name="arrowHeadLength"></param>
    /// <param name="arrowHeadAngle"></param>
    public static void ForDebug(Vector3 pos, Vector3 dir, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, dir, color);
       
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 + arrowHeadAngle,0) * new Vector3(0,0,1);
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0,180 - arrowHeadAngle,0) * new Vector3(0,0,1);
        Debug.DrawRay(pos + dir, right * arrowHeadLength, color);
        Debug.DrawRay(pos + dir, left * arrowHeadLength, color);
    }
}
 