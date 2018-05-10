using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Matrix3
    {
        public double m11 { get; private set; }
        public double m12 { get; private set; }
        public double m13 { get; private set; }
        public double m21 { get; private set; }
        public double m22 { get; private set; }
        public double m23 { get; private set; }
        public double m31 { get; private set; }
        public double m32 { get; private set; }
        public double m33 { get; private set; }
        public Matrix3(
            double _m11, double _m12, double _m13,
            double _m21, double _m22, double _m23,
            double _m31, double _m32, double _m33
        )
        {
            m11 = _m11;
            m12 = _m12;
            m13 = _m13;
            m21 = _m21;
            m22 = _m22;
            m23 = _m23;
            m31 = _m31;
            m32 = _m32;
            m33 = _m33;
        }
        public override string ToString() => "[ " + String.Join(
            ",", new double[] {
                m11, m12, m13,
                m21, m22, m23,
                m31, m32, m33,
            }
        ) + " ]";
        /// <summary>
        /// 単位行列
        /// </summary>
        /// <returns></returns>
        public static Matrix3 identity() => new Matrix3(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1
        );
        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static Matrix3 translation(double tx, double ty) => new Matrix3(
            1, 0, tx,
            0, 1, ty,
            0, 0, 1
        );
        /// <summary>
        /// 拡大
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static Matrix3 scaling(double sx, double sy) => new Matrix3(
            sx, 0, 0,
            0, sy, 0,
            0, 0, 1
        );
        /// <summary>
        /// 回転
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix3 rotation(double a)
        {
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);
            return new Matrix3(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1
            );
        }
        /// <summary>
        /// 平面上の射影変換
        /// 正方形[(0,0),(1,0),(1,1),(0,1)] → 凸四角形[(x1,y1),(x2,y2),(x3,y3),(x4,y4)]
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <returns></returns>
        public static Matrix3 projectiveTransform(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4
        )
        {
            double x2d = x2 - x1, y2d = y2 - y1;
            double x3d = x3 - x1, y3d = y3 - y1;
            double x4d = x4 - x1, y4d = y4 - y1;
            double d123 = x2d * y3d - x3d * y2d;
            double d124 = x2d * y4d - x4d * y2d;
            double d134 = x3d * y4d - x4d * y3d;
            double d1234 = d123 + d134;
            double d234 = d1234 - d124;
            double a1 = d134 * x2d;
            double b1 = d123 * x4d;
            double a2 = d134 * y2d;
            double b2 = d123 * y4d;
            double a0 = d134 - d234;
            double b0 = d123 - d234;
            double c0 = d234;
            return new Matrix3(
                x1 * a0 + a1, x1 * b0 + b1, x1 * c0,
                y1 * a0 + a2, y1 * b0 + b2, y1 * c0,
                a0, b0, c0
            );
        }
        public static Matrix3 projectiveTransform(
            Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4
        ) => projectiveTransform(
            p1.x, p1.y,
            p2.x, p2.y,
            p3.x, p3.y,
            p4.x, p4.y
        );
        /// <summary>
        /// 平面上の射影変換
        /// 凸四角形[(x1,y1),(x2,y2),(x3,y3),(x4,y4)] → 正方形[(0,0),(1,0),(1,1),(0,1)]
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <returns></returns>
        public static Matrix3 projectiveInvTransform(
            double x1, double y1,
            double x2, double y2,
            double x3, double y3,
            double x4, double y4
        )
        {
            double x2d = x2 - x1, y2d = y2 - y1;
            double x3d = x3 - x1, y3d = y3 - y1;
            double x4d = x4 - x1, y4d = y4 - y1;
            double d123 = x2d * y3d - x3d * y2d;
            double d124 = x2d * y4d - x4d * y2d;
            double d134 = x3d * y4d - x4d * y3d;
            double d1234 = d123 + d134;
            double d234 = d1234 - d124;
            double d11 = d123 - d124;
            double d22 = d134 - d124;
            double a1 = -d123 * d234 * y4d;
            double b1 = d123 * d234 * x4d;
            double a2 = d134 * d234 * y2d;
            double b2 = -d134 * d234 * x2d;
            double a0 = d11 * d123 * y4d + d22 * d134 * y2d;
            double b0 = d11 * d123 * x4d + d22 * d134 * x2d;
            double c0 = -d123 * d124 * d134;
            return new Matrix3(
                a1, b1, -a1 * x1 - b1 * y1,
                a2, b2, -a2 * x1 - b2 * y1,
                a0, b0, -a0 * x1 - b0 * y1 + c0
            );
        }
        public static Matrix3 projectiveInvTransform(
            Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4
        ) => projectiveInvTransform(
            p1.x, p1.y,
            p2.x, p2.y,
            p3.x, p3.y,
            p4.x, p4.y
        );
        /// <summary>
        /// 行列の積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix3 mul(Matrix3 b) => new Matrix3(
            m11 * b.m11 + m12 * b.m21 + m13 * b.m31,
            m11 * b.m12 + m12 * b.m22 + m13 * b.m32,
            m11 * b.m13 + m12 * b.m23 + m13 * b.m33,
            m21 * b.m11 + m22 * b.m21 + m23 * b.m31,
            m21 * b.m12 + m22 * b.m22 + m23 * b.m32,
            m21 * b.m13 + m22 * b.m23 + m23 * b.m33,
            m31 * b.m11 + m32 * b.m21 + m33 * b.m31,
            m31 * b.m12 + m32 * b.m22 + m33 * b.m32,
            m31 * b.m13 + m32 * b.m23 + m33 * b.m33
        );
        /// <summary>
        /// 行列の積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix3 mul(Matrix32 b) => new Matrix3(
            m11 * b.m11 + m12 * b.m21,
            m11 * b.m12 + m12 * b.m22,
            m11 * b.m13 + m12 * b.m23 + m13,
            m21 * b.m11 + m22 * b.m21,
            m21 * b.m12 + m22 * b.m22,
            m21 * b.m13 + m22 * b.m23 + m23,
            m31 * b.m11 + m32 * b.m21,
            m31 * b.m12 + m32 * b.m22,
            m31 * b.m13 + m32 * b.m23 + m33
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 mul(Vector3 v) => new Vector3(
            m11 * v.x + m12 * v.y + m13 * v.z,
            m21 * v.x + m22 * v.y + m23 * v.z,
            m31 * v.x + m32 * v.y + m33 * v.z
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 mul(Vector2 v) => new Vector3(
            m11 * v.x + m12 * v.y + m13,
            m21 * v.x + m22 * v.y + m23,
            m31 * v.x + m32 * v.y + m33
        );
    }
}
