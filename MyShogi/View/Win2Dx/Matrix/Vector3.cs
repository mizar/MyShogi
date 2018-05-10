using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Vector3
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }
        public Vector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public override string ToString() => "[ " + String.Join(",", new double[] { x, y, z }) + " ]";
        public static Vector3 zero() => new Vector3(0, 0, 0);
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3 add(Vector3 b) => new Vector3(x + b.x, y + b.y, z + b.z);
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3 sub(Vector3 b) => new Vector3(x - b.x, y - b.y, z - b.z);
        /*
        /// <summary>
        /// 行ベクトルと行列の積
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Vector3 mul(Matrix3 m) => new Vector3(
            x * m.m11 + y * m.m21 + z * m.m31,
            x * m.m12 + y * m.m22 + z * m.m32,
            x * m.m13 + y * m.m23 + z * m.m33
        );
        /// <summary>
        /// 行ベクトルと行列の積
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Vector4 mul(Matrix4 m) => new Vector4(
            x * m.m11 + y * m.m21 + z * m.m31 + m.m41,
            x * m.m12 + y * m.m22 + z * m.m32 + m.m42,
            x * m.m13 + y * m.m23 + z * m.m33 + m.m43,
            x * m.m14 + y * m.m24 + z * m.m34 + m.m44
        );
        */
        /// <summary>
        /// スカラーとの積
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vector3 mul(double s) => new Vector3(x * s, y * s, z * s);
        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public double dot(Vector3 b) => x * b.x + y * b.y + z * b.z;
        /// <summary>
        /// クロス積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3 cross(Vector3 b) => new Vector3(
            y * b.z - z * b.y,
            z * b.x - x * b.z,
            x * b.y - y * b.x
        );
        /// <summary>
        /// 絶対値
        /// </summary>
        /// <returns></returns>
        public double magnitude() => Math.Sqrt(x * x + y * y + z * z);
        /// <summary>
        /// 正規化
        /// </summary>
        /// <returns></returns>
        public Vector3 normalize()
        {
            double mag = this.magnitude();
            if (mag == 0) return this;
            double maginv = 1 / mag;
            return new Vector3(x * maginv, y * maginv, z * maginv);
        }
        /// <summary>
        /// xyベクトル
        /// </summary>
        /// <returns></returns>
        public Vector2 xy() => new Vector2(x, y);
        /// <summary>
        /// 同次座標からユークリッド座標(cartesian coordinate)に変換
        /// </summary>
        /// <returns></returns>
        public Vector2 cart() => z == 1 ? xy() : new Vector2(x / z, y / z);
        /// <summary>
        /// ユークリッド座標から同次座標(homogeneous coordinates)に変換
        /// </summary>
        /// <returns></returns>
        public Vector4 homo() => new Vector4(x, y, z, 1);
    }
}
