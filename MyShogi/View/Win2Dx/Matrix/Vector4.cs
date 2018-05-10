using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Vector4
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }
        public double w { get; private set; }
        public Vector4(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public override string ToString() => "[ " + String.Join(",", new double[] { x, y, z, w }) + " ]";
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector4 add(Vector4 b) => new Vector4(
            x + b.x,
            y + b.y,
            z + b.z,
            w + b.w
        );
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector4 sub(Vector4 b) => new Vector4(
            x - b.x,
            y - b.y,
            z - b.z,
            w - b.w
        );
        /*
        /// <summary>
        /// 行ベクトルと行列の積
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Vector4 mul(Matrix4 m) => new Vector4(
            x * m.m11 + y * m.m21 + z * m.m31 + w * m.m41,
            x * m.m12 + y * m.m22 + z * m.m32 + w * m.m42,
            x * m.m13 + y * m.m23 + z * m.m33 + w * m.m43,
            x * m.m14 + y * m.m24 + z * m.m34 + w * m.m44
        );
        */
        /// <summary>
        /// スカラーとの積
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vector4 mul(double s) => new Vector4(x * s, y * s, z * s, w * s);
        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public double dot(Vector4 b) => x * b.x + y * b.y + z * b.z + w * b.w;
        /// <summary>
        /// 絶対値
        /// </summary>
        /// <returns></returns>
        public double magnitude() => Math.Sqrt(x * x + y * y + z * z + w * w);
        /// <summary>
        /// 正規化
        /// </summary>
        /// <returns></returns>
        public Vector4 normalize()
        {
            double mag = this.magnitude();
            if (mag == 0) return this;
            double maginv = 1 / mag;
            return new Vector4(x * maginv, y * maginv, z * maginv, w * maginv);
        }
        /// <summary>
        /// xyベクトル
        /// </summary>
        /// <returns></returns>
        public Vector2 xy() => new Vector2(x, y);
        /// <summary>
        /// xyzベクトル
        /// </summary>
        /// <returns></returns>
        public Vector3 xyz() => new Vector3(x, y, z);
        /// <summary>
        /// 同次座標からユークリッド座標(cartesian coordinate)に変換
        /// </summary>
        /// <returns></returns>
        public Vector3 cart() => w == 1 ? xyz() : new Vector3(x / w, y / w, z / w);
    }
}
