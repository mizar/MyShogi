using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Matrix32
    {
        public double m11 { get; private set; }
        public double m12 { get; private set; }
        public double m13 { get; private set; }
        public double m21 { get; private set; }
        public double m22 { get; private set; }
        public double m23 { get; private set; }
        public Matrix32(
            double _m11, double _m12, double _m13,
            double _m21, double _m22, double _m23
        )
        {
            m11 = _m11;
            m12 = _m12;
            m13 = _m13;
            m21 = _m21;
            m22 = _m22;
            m23 = _m23;
        }
        public override string ToString() => "[ " + String.Join(
            ",", new double[] {
                m11, m12, m13,
                m21, m22, m23,
            }
        ) + " ]";
        /// <summary>
        /// 単位行列
        /// </summary>
        /// <returns></returns>
        public static Matrix32 identity() => new Matrix32(
            1, 0, 0,
            0, 1, 0
        );
        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static Matrix32 translation(double tx, double ty) => new Matrix32(
            1, 0, tx,
            0, 1, ty
        );
        /// <summary>
        /// 拡大
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static Matrix32 scaling(double sx, double sy) => new Matrix32(
            sx, 0, 0,
            0, sy, 0
        );
        /// <summary>
        /// 回転
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix32 rotation(double a)
        {
            double sin = Math.Sin(a);
            double cos = Math.Cos(a);
            return new Matrix32(
                cos, -sin, 0,
                sin, cos, 0
            );
        }
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
            b.m31,
            b.m32,
            b.m33
        );
        /// <summary>
        /// 行列の積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Matrix32 mul(Matrix32 b) => new Matrix32(
            m11 * b.m11 + m12 * b.m21,
            m11 * b.m12 + m12 * b.m22,
            m11 * b.m13 + m12 * b.m23 + m13,
            m21 * b.m11 + m22 * b.m21,
            m21 * b.m12 + m22 * b.m22,
            m21 * b.m13 + m22 * b.m23 + m23
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 mul(Vector3 v) => new Vector3(
            m11 * v.x + m12 * v.y + m13 * v.z,
            m21 * v.x + m22 * v.y + m23 * v.z,
            v.z
        );
        /// <summary>
        /// 行列と列ベクトルの積
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector2 mul(Vector2 v) => new Vector2(
            m11 * v.x + m12 * v.y + m13,
            m21 * v.x + m22 * v.y + m23
        );
    }
}
