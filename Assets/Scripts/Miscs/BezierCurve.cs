using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    /// <summary>
    /// ���ض��α��������ߵĵ�
    /// </summary>
    /// <param name="startPoint">���</param>
    /// <param name="endPoint">�յ�</param>
    /// <param name="controlPoint">���Ƶ�</param>
    /// <param name="by">0~1�����ֵ</param>
    /// <returns>��Ӧ��ֵ</returns>
    public static Vector3 QuadraticPoint(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float by)
    {
        return Vector3.Lerp
            (
                Vector3.Lerp(startPoint, controlPoint, by), 
                Vector3.Lerp(controlPoint, endPoint, by), 
                by
            );
    }

    /// <summary>
    /// Return a point of cubic Bezier curve.
    /// �������α����������ϵĵ㡣
    /// </summary>
    public static Vector3 CubicPoint(Vector3 startPoint, Vector3 endPoint, Vector3 controlPointStart, Vector3 controlPointEnd, float t)
    {
        // * Method 01
        // float oneMinusT = 1f - t;

        // return oneMinusT * oneMinusT * oneMinusT * startPoint + 
        //     t * t * t * endPoint +
        //     3 * t * oneMinusT * oneMinusT * controlPointStart + 
        //     3 * t * t * oneMinusT * controlPointEnd;

        // * Method 02
        // var AB = Vector3.Lerp(startPoint, controlPointStart, t);
        // var BC = Vector3.Lerp(controlPointStart, controlPointEnd, t);
        // var CD = Vector3.Lerp(controlPointEnd, endPoint, t);
        // var AB2BC = Vector3.Lerp(AB, BC, t);
        // var BC2CD = Vector3.Lerp(BC, CD, t);

        // return Vector3.Lerp(AB2BC, BC2CD, t);

        // * Method 03
        // return Vector3.Lerp(
        //         Vector3.Lerp(
        //             Vector3.Lerp(startPoint, controlPointStart, t), 
        //             Vector3.Lerp(controlPointStart, controlPointEnd, t), 
        //             t), 
        //         Vector3.Lerp(
        //             Vector3.Lerp(controlPointStart, controlPointEnd, t), 
        //             Vector3.Lerp(controlPointEnd, endPoint, t), t), 
        //             t);

        // * Method 04
        // var AB = Vector3.Lerp(startPoint, controlPointStart, t);
        // var BC = Vector3.Lerp(controlPointStart, controlPointEnd, t);
        // var CD = Vector3.Lerp(controlPointEnd, endPoint, t);

        // return QuadraticBezierCurvePoint(AB, CD, BC, t);

        // * Method 05
        return QuadraticPoint(
            Vector3.Lerp(startPoint, controlPointStart, t),
            Vector3.Lerp(controlPointEnd, endPoint, t),
            Vector3.Lerp(controlPointStart, controlPointEnd, t), t);
    }
}