using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Matrix43
    {
        public double m11 { get; private set; }
        public double m12 { get; private set; }
        public double m13 { get; private set; }
        public double m14 { get; private set; }
        public double m21 { get; private set; }
        public double m22 { get; private set; }
        public double m23 { get; private set; }
        public double m24 { get; private set; }
        public double m31 { get; private set; }
        public double m32 { get; private set; }
        public double m33 { get; private set; }
        public double m34 { get; private set; }
        public Matrix43(
            double _m11, double _m12, double _m13, double _m14,
            double _m21, double _m22, double _m23, double _m24,
            double _m31, double _m32, double _m33, double _m34
        )
        {
            m11 = _m11;
            m12 = _m12;
            m13 = _m13;
            m14 = _m14;
            m21 = _m21;
            m22 = _m22;
            m23 = _m23;
            m24 = _m24;
            m31 = _m31;
            m32 = _m32;
            m33 = _m33;
            m34 = _m34;
        }
        public override string ToString() => "[ " + String.Join(
            ",", new double[] {
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34
            }
        ) + " ]";
        /// <summary>
        /// 単位行列
        /// </summary>
        /// <returns></returns>
        public static Matrix43 identity() => new Matrix43(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0
        );
        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <returns></returns>
        public static Matrix43 translation(double tx, double ty, double tz) => new Matrix43(
            1, 0, 0, tx,
            0, 1, 0, ty,
            0, 0, 1, tz
        );
        /// <summary>
        /// 拡大
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static Matrix43 scaling(double sx, double sy, double sz) => new Matrix43(
            sx, 0, 0, 0,
            0, sy, 0, 0,
            0, 0, sz, 0
        );
        /// <summary>
        /// X軸周りの回転
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix43 rotationX(double a)
        {
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);
            return new Matrix43(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0
            );
        }
        /// <summary>
        /// Y軸周りの回転
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix43 rotationY(double a)
        {
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);
            return new Matrix43(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0
            );
        }
        /// <summary>
        /// Z軸周りの回転
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix43 rotationZ(double a)
        {
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);
            return new Matrix43(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0
            );
        }
        /// <summary>
        /// 任意軸周りの回転
        /// </summary>
        /// <param name="normalizedAxis"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static Matrix43 rotationAround(Vector3 normalizedAxis, double radian) =>
            Quaternion.rotationAround(normalizedAxis, radian).toRotationMatrix43();
        /// <summary>
        /// 視野変換行列
        /// </summary>
        /// <param name="cameraPosition"></param>
        /// <param name="lookAtPosition"></param>
        /// <param name="cameraUp"></param>
        /// <returns></returns>
        public static Matrix43 lookAt(Vector3 cameraPosition, Vector3 lookAtPosition, Vector3 cameraUp)
        {
            Vector3 zAxis = cameraPosition.sub(lookAtPosition).normalize();
            Vector3 xAxis = cameraUp.cross(zAxis).normalize();
            Vector3 yAxis = zAxis.cross(xAxis).normalize();
            return new Matrix43(
                xAxis.x, xAxis.y, xAxis.z, -cameraPosition.dot(xAxis),
                yAxis.x, yAxis.y, yAxis.z, -cameraPosition.dot(yAxis),
                zAxis.x, zAxis.y, zAxis.z, -cameraPosition.dot(zAxis)
            );
        }
        /// <summary>
        /// 視野変換逆行列
        /// </summary>
        /// <param name="cameraPosition"></param>
        /// <param name="lookAtPosition"></param>
        /// <param name="cameraUp"></param>
        /// <returns></returns>
        public static Matrix43 lookAtInv(Vector3 cameraPosition, Vector3 lookAtPosition, Vector3 cameraUp)
        {
            Vector3 zAxis = cameraPosition.sub(lookAtPosition).normalize();
            Vector3 xAxis = cameraUp.cross(zAxis).normalize();
            Vector3 yAxis = zAxis.cross(xAxis).normalize();
            return new Matrix43(
                xAxis.x, yAxis.x, zAxis.x, cameraPosition.x,
                xAxis.y, yAxis.y, zAxis.y, cameraPosition.y,
                xAxis.z, yAxis.z, zAxis.z, cameraPosition.z
            );
        }
        /// <summary>
        /// 平行投影行列
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        /// <returns></returns>
        public static Matrix43 orthographic(double top, double bottom, double left, double right, double near, double far) => new Matrix43(
            2 / (right - left), 0, 0, (left + right) / (left - right),
            0, 2 / (top - bottom), 0, (bottom + top) / (bottom - top),
            0, 0, 2 / (near - far), (near + far) / (near - far)
        );
        /// <summary>
        /// 行列の積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix4 mul(Matrix4 b) => new Matrix4(
            m11 * b.m11 + m12 * b.m21 + m13 * b.m31 + m14 * b.m41,
            m11 * b.m12 + m12 * b.m22 + m13 * b.m32 + m14 * b.m42,
            m11 * b.m13 + m12 * b.m23 + m13 * b.m33 + m14 * b.m43,
            m11 * b.m14 + m12 * b.m24 + m13 * b.m34 + m14 * b.m44,
            m21 * b.m11 + m22 * b.m21 + m23 * b.m31 + m24 * b.m41,
            m21 * b.m12 + m22 * b.m22 + m23 * b.m32 + m24 * b.m42,
            m21 * b.m13 + m22 * b.m23 + m23 * b.m33 + m24 * b.m43,
            m21 * b.m14 + m22 * b.m24 + m23 * b.m34 + m24 * b.m44,
            m31 * b.m11 + m32 * b.m21 + m33 * b.m31 + m34 * b.m41,
            m31 * b.m12 + m32 * b.m22 + m33 * b.m32 + m34 * b.m42,
            m31 * b.m13 + m32 * b.m23 + m33 * b.m33 + m34 * b.m43,
            m31 * b.m14 + m32 * b.m24 + m33 * b.m34 + m34 * b.m44,
            b.m41,
            b.m42,
            b.m43,
            b.m44
        );
        /// <summary>
        /// 行列の積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix43 mul(Matrix43 b) => new Matrix43(
            m11 * b.m11 + m12 * b.m21 + m13 * b.m31,
            m11 * b.m12 + m12 * b.m22 + m13 * b.m32,
            m11 * b.m13 + m12 * b.m23 + m13 * b.m33,
            m11 * b.m14 + m12 * b.m24 + m13 * b.m34 + m14,
            m21 * b.m11 + m22 * b.m21 + m23 * b.m31,
            m21 * b.m12 + m22 * b.m22 + m23 * b.m32,
            m21 * b.m13 + m22 * b.m23 + m23 * b.m33,
            m21 * b.m14 + m22 * b.m24 + m23 * b.m34 + m24,
            m31 * b.m11 + m32 * b.m21 + m33 * b.m31,
            m31 * b.m12 + m32 * b.m22 + m33 * b.m32,
            m31 * b.m13 + m32 * b.m23 + m33 * b.m33,
            m31 * b.m14 + m32 * b.m24 + m33 * b.m34 + m34
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector4 mul(Vector4 v) => new Vector4(
            m11 * v.x + m12 * v.y + m13 * v.z + m14 * v.w,
            m21 * v.x + m22 * v.y + m23 * v.z + m24 * v.w,
            m31 * v.x + m32 * v.y + m33 * v.z + m34 * v.w,
            v.w
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 mul(Vector3 v) => new Vector3(
            m11 * v.x + m12 * v.y + m13 * v.z + m14,
            m21 * v.x + m22 * v.y + m23 * v.z + m24,
            m31 * v.x + m32 * v.y + m33 * v.z + m34
        );
    }
}
