using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Vector2
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public Vector2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        public override string ToString() => "[ " + String.Join(",", new double[] { x, y }) + " ]";
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector2 add(Vector2 b) => new Vector2(x + b.x, y + b.y);
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector2 sub(Vector2 b) => new Vector2(x - b.x, y - b.y);
        /*
        /// <summary>
        /// 行ベクトルと行列の積
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Vector3 mul(Matrix3 m) => new Vector3(
            x * m.m11 + y * m.m21 + m.m31,
            x * m.m12 + y * m.m22 + m.m32,
            x * m.m13 + y * m.m23 + m.m33
        );
        */
        /// <summary>
        /// スカラーとの積
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vector2 mul(double s) => new Vector2(x * s, y * s);
        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public double dot(Vector2 b) => x * b.x + y * b.y;
        /// <summary>
        /// 絶対値
        /// </summary>
        /// <returns></returns>
        public double magnitude() => Math.Sqrt(x * x + y * y);
        /// <summary>
        /// 正規化
        /// </summary>
        /// <returns></returns>
        public Vector2 normalize()
        {
            double mag = this.magnitude();
            if (mag == 0) return this;
            double maginv = 1 / mag;
            return new Vector2(x * maginv, y * maginv);
        }
        /// <summary>
        /// ユークリッド座標から同次座標(homogeneous coordinates)に変換
        /// </summary>
        /// <returns></returns>
        public Vector3 homo() => new Vector3(x, y, 1);
    }
}
