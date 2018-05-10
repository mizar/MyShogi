using System;

namespace MyShogi.View.Win2Dx.Matrix
{
    public struct Quaternion
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }
        public double w { get; private set; }
        public Quaternion(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public override string ToString() => "[ " + String.Join(",", new double[] { x, y, z, w }) + " ]";
        public static Quaternion identity() => new Quaternion(0, 0, 0, 1);
        /// <summary>
        /// 任意軸周りの回転
        /// </summary>
        /// <param name="normalizedAxis"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static Quaternion rotationAround(Vector3 normalizedAxis, double radian)
        {
            double sin = Math.Sin(radian * 0.5);
            double cos = Math.Cos(radian * 0.5);
            return new Quaternion(normalizedAxis.x * sin, normalizedAxis.y * sin, normalizedAxis.z * sin, cos);
        }
        /// <summary>
        /// X軸周りの回転
        /// </summary>
        /// <param name="normalizedAxis"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static Quaternion rotationX(double radian)
        {
            double sin = Math.Sin(radian * 0.5);
            double cos = Math.Cos(radian * 0.5);
            return new Quaternion(sin, 0, 0, cos);
        }
        /// <summary>
        /// Y軸周りの回転
        /// </summary>
        /// <param name="normalizedAxis"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static Quaternion rotationY(double radian)
        {
            double sin = Math.Sin(radian * 0.5);
            double cos = Math.Cos(radian * 0.5);
            return new Quaternion(0, sin, 0, cos);
        }
        /// <summary>
        /// Z軸周りの回転
        /// </summary>
        /// <param name="normalizedAxis"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static Quaternion rotationZ(double radian)
        {
            double sin = Math.Sin(radian * 0.5);
            double cos = Math.Cos(radian * 0.5);
            return new Quaternion(0, 0, sin, cos);
        }
        /// <summary>
        /// 正規化
        /// </summary>
        /// <returns></returns>
        public Quaternion normalize()
        {
            double mag = this.magnitude();
            if (mag == 0) { return this; }
            double r = 1 / mag;
            return new Quaternion(this.x * r, this.y * r, this.z * r, this.w * r);
        }
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Quaternion add(Quaternion b) => new Quaternion(x + b.x, y + b.y, z + b.z, w + b.w);
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Quaternion sub(Quaternion b) => new Quaternion(x - b.x, y - b.y, z - b.z, w - b.w);
        /// <summary>
        /// 積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Quaternion mul(Quaternion b) => new Quaternion(
            w * b.x + x * b.w + y * b.z - z * b.y,
            w * b.y - x * b.z + y * b.w + z * b.x,
            w * b.z + x * b.y - y * b.x + z * b.w,
            w * b.w - x * b.x - y * b.y - z * b.z
        );
        /// <summary>
        /// スカラーとの積
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Quaternion mul(double f) => new Quaternion(x * f, y * f, z * f, w * f);
        /// <summary>
        /// 内積
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        double dot(Quaternion b) => x * b.x + y * b.y + z * b.z + w * b.w;
        /// <summary>
        /// 球面線形補間
        /// </summary>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Quaternion slerp(Quaternion b, double t)
        {
            double dotProd = this.dot(b);
            Quaternion other = b;
            if (dotProd < 0) {
                dotProd = -dotProd;
                other = other.mul(-1);
            }
            double omega = Math.Acos(dotProd);
            double sinOmega = Math.Sin(omega);
            Quaternion q1 = this.mul(Math.Sin((1 - t) * omega) / sinOmega);
            Quaternion q2 = other.mul(Math.Sin(t * omega) / sinOmega);
            return q1.add(q2);
        }
        /// <summary>
        /// 絶対値
        /// </summary>
        /// <returns></returns>
        public double magnitude() => Math.Sqrt(
            this.x * this.x +
            this.y * this.y +
            this.z * this.z +
            this.w * this.w
        );
        /// <summary>
        /// 3次元ベクトルの回転
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector3 rotate(Vector3 b) => new Vector3(
            (1 - 2 * y * y - 2 * z * z) * b.x + (2 * x * y - 2 * w * z) * b.y + (2 * x * z + 2 * w * y) * b.z,
            (2 * x * y + 2 * w * z) * b.x + (1 - 2 * x * x - 2 * z * z) * b.y + (2 * y * z - 2 * w * x) * b.z,
            (2 * x * z - 2 * w * y) * b.x + (2 * y * z + 2 * w * x) * b.y + (1 - 2 * x * x - 2 * y * y) * b.z
        );
        /// <summary>
        /// 3次元ベクトルの回転
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Vector4 rotate(Vector4 b) => new Vector4(
            (1 - 2 * y * y - 2 * z * z) * b.x + (2 * x * y - 2 * w * z) * b.y + (2 * x * z + 2 * w * y) * b.z,
            (2 * x * y + 2 * w * z) * b.x + (1 - 2 * x * x - 2 * z * z) * b.y + (2 * y * z - 2 * w * x) * b.z,
            (2 * x * z - 2 * w * y) * b.x + (2 * y * z + 2 * w * x) * b.y + (1 - 2 * x * x - 2 * y * y) * b.z,
            b.w
        );
        /// <summary>
        /// 回転行列
        /// </summary>
        /// <returns></returns>
        public Matrix4 toRotationMatrix4() => new Matrix4(
            1 - 2 * y * y - 2 * z * z, 2 * x * y - 2 * w * z, 2 * x * z + 2 * w * y, 0,
            2 * x * y + 2 * w * z, 1 - 2 * x * x - 2 * z * z, 2 * y * z - 2 * w * x, 0,
            2 * x * z - 2 * w * y, 2 * y * z + 2 * w * x, 1 - 2 * x * x - 2 * y * y, 0,
            0, 0, 0, 1
        );
        /// <summary>
        /// 回転行列
        /// </summary>
        /// <returns></returns>
        public Matrix43 toRotationMatrix43() => new Matrix43(
            1 - 2 * y * y - 2 * z * z, 2 * x * y - 2 * w * z, 2 * x * z + 2 * w * y, 0,
            2 * x * y + 2 * w * z, 1 - 2 * x * x - 2 * z * z, 2 * y * z - 2 * w * x, 0,
            2 * x * z - 2 * w * y, 2 * y * z + 2 * w * x, 1 - 2 * x * x - 2 * y * y, 0
        );
    }
}
