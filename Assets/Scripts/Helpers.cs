using UnityEngine;

public static class Helpers
{
    public static Rect GetScreenRect(Bounds bounds)
    {
        Vector3 cen = bounds.center;
        Vector3 ext = bounds.extents;
        Camera cam = Camera.main;

        Vector2 min = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z));
        Vector2 max = min;

        //0
        Vector2 point = min;
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //1
        point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //2
        point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //3
        point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //4
        point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //5
        point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //6
        point = cam.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        //7
        point = cam.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z));
        min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
        max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    public static Rect GetScreenRect(Vector2 point1, Vector2 point2)
    {
        Vector2 boxCenter = (point1 + point2) * 0.5f;
        float boxWidth = Mathf.Abs(point1.x - point2.x);
        float boxHight = Mathf.Abs(point1.y - point2.y);
        float boxX = boxCenter.x - (boxWidth * 0.5f);
        float boxY = boxCenter.y - (boxHight * 0.5f);

        return new Rect(boxX, boxY, boxWidth, boxHight);
    }
}
