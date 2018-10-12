using System;
using System.Collections.Generic;
using System.Linq;
using SMath = System.Math;

namespace MyShogi.Model.Common.Math
{
    public static class SpecialFunc
    {

        /// <summary>
        /// 収束条件
        /// </summary>
        public const double MEPS = 2.2204460492503130808472633361816e-16;
        public const double EPS3 = double.Epsilon * 3;
        public const double ME100 = MEPS * 100;

        public const double NaN = double.NaN;
        public const double PositiveInfinity = double.PositiveInfinity;
        public const double NegativeInfinity = double.NegativeInfinity;
        public const double PI = SMath.PI;
        public const double LN2 = 0.69314718055994530941723212145818;
        public const double LN10 = 2.3025850929940456840179914546844;
        public const double LOGE = 0.43429448190325182765112891891661;

        const double HalfLog2PI = 0.91893853320467274178032973640562; // Log(Sqrt(2 * PI))

        private static bool IsNaN(double x) => double.IsNaN(x);
        private static double Abs(double x) => SMath.Abs(x);
        private static double Floor(double x) => SMath.Floor(x);
        private static double Ceiling(double x) => SMath.Ceiling(x);
        private static double Sqrt(double x) => SMath.Sqrt(x);
        private static double Exp(double x) => SMath.Exp(x);
        private static double Pow(double x, double y) => SMath.Pow(x, y);
        private static double Log(double x) => SMath.Log(x);
        private static double Sin(double x) => SMath.Sin(x);
        private static double Min(double x, double y) => SMath.Min(x, y);
        private static double Max(double x, double y) => SMath.Max(x, y);

        /// <summary>
        /// log(1+x)
        /// </summary>
        /// <remarks>
        /// log((1 + y) / (1 - y)) = 2 * (y + ((y ** 3) / 3) + ((y ** 5) / 5) + ((y ** 7) / 7) + ...)
        /// </remarks>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Log1p(double x)
        {
            if (IsNaN(x)) return NaN;
            if (x < -0.5 || x > +1.0) return Log(1 + x);
            // -1/3 <= y <= +1/3
            var y = x / (x + 2);
            // 0 <= y2 <= 1/9
            var y2 = y * y;
            var p = 2 * y;
            var s = p;

            for (var i = 3; i < 1000; i += 2)
            {
                var ps = s;
                p *= y2;
                s += p / i;
                if (Abs(s - ps) < EPS3) break;
            }
            return s;
        }

        /// <summary>
        /// exp(x)-1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Expm1(double x)
        {
            if (IsNaN(x)) return NaN;
            if (x < -2 || x > 2) return Exp(x) - 1;
            var p = x;
            var s = x;

            for (var i = 2; i <= 1000; ++i)
            {
                var ps = s;
                p *= x / i;
                s += p;
                if (Abs(s - ps) < EPS3) break;
            }
            return s;
        }

        /// <summary>
        /// log(1-exp(x))
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Log1mExp(double x)
            => !(x < 0) ? double.NaN :
            x > -LN2 ? Log(-Expm1(x)) : Log1p(-Exp(x));

        /// <summary>
        /// log(1+exp(x))
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Log1pExp(double x)
            => x <= 0 ? Log1p(Exp(x)) : (x + Log1p(Exp(-x)));

        /// <summary>
        /// ロジット関数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double EnLogit(double x) =>
            IsNaN(x) ? NaN :
            (x <= 0) ? NegativeInfinity :
            (x >= 1) ? PositiveInfinity :
            Log(x) - Log1p(-x);

        /// <summary>
        /// 逆ロジット関数（ロジスティック関数）
        /// </summary>
        /// <param name="logit"></param>
        /// <returns></returns>
        public static double DeLogit(double logit)
        {
            if (logit > 0)
            {
                var nexp = Exp(-logit);
                return 1 / (nexp + 1);
            }
            else
            {
                var pexp = Exp(logit);
                return pexp / (pexp + 1);
            }
        }

        /// <summary>
        /// 逆ロジット関数の対数
        /// </summary>
        public static double LogDeLogit(double logit)
            => -Log1pExp(-logit);

        /// ロジット値参照
        public class Logit
        {
            public double V { get; }
            public double Px { get; }
            public double Nx { get; }
            public double LogPx { get; }
            public double LogNx { get; }
            private Logit(double logit, double px, double nx, double logpx, double lognx)
            {
                V = logit;
                Px = px;
                Nx = nx;
                LogPx = logpx;
                LogNx = lognx;
            }
            public static Logit FromLogit(double logit) =>
                new Logit(logit, DeLogit(logit), DeLogit(-logit), LogDeLogit(logit), LogDeLogit(-logit));
            public static Logit FromPx(double px) =>
                new Logit(EnLogit(px), px, 1 - px, Log(px), Log1p(-px));
            public static Logit FromNx(double nx) =>
                new Logit(-EnLogit(nx), 1 - nx, nx, Log1p(-nx), Log(nx));
            public Logit Negate() =>
                new Logit(-V, Nx, Px, LogNx, LogPx);
        }

        /// <summary>
        /// 対数ガンマ関数
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double GammaLn(double z)
        {
            // z <= 0 の場合の返り値は保証外
            // (特に0以上の整数n に対して z == n, -2n-1 <= z <= -2n, z == -Infinity の場合)
            // Lanczos近似 : https://en.wikipedia.org/wiki/Lanczos_approximation
            // パラメータ算出 : https://mrob.com/pub/ries/lanczos-gamma.html
            if (z < 0.5)
            {
                // Log(PI) = 1.1447298858494001741434273513531
                const double log_pi = 1.1447298858494001741434273513531;
                return log_pi - Log(Sin(PI * z)) - GammaLn(1 - z);
            }

            // g = 607.0 / 128.0;
            var g = 4.7421875;
            var x = -2.90818e-15;
            double[] p = {
                57.156235665862923517,
                -59.597960355475491248,
                14.136097974741747174,
                -0.49191381609762019978,
                0.33994649984811888699e-4,
                0.46523628927048575665e-4,
                -0.98374475304879564677e-4,
                0.15808870322491248884e-3,
                -0.21026444172410488319e-3,
                0.21743961811521264320e-3,
                -0.16431810653676389022e-3,
                0.84418223983852743293e-4,
                -0.26190838401581408670e-4,
                0.36899182659531622704e-5,
            };
            for (var i = p.Length - 1; i >= 0; --i)
            {
                x += p[i] / (z + i);
            }
            var t = g - 0.5 + z;
            return (Log(t) - 1) * (z - 0.5) - g + HalfLog2PI + Log1p(x);
        }

        /// <summary>
        /// ガンマ関数（対数ガンマから算出）
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Gamma(double z)
            => z >= 0.5 ? Exp(GammaLn(z)) :
            PI / (Sin(PI * z) * Exp(GammaLn(1 - z)));

        /// <summary>
        /// 規格化第1種不完全ガンマ関数
        /// GammaP(a, x) = γ(a, x) / Γ(a) = 1 - GammaQ(a, x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GammaP(double a, double x)
        {
            if (!(a > 0 && x > 0)) return double.NaN;
            var t = Exp(Log(x) * a - GammaLn(a));
            var s = t / a;
            for (var k = 1; k <= 1000; ++k)
            {
                var ps = s;
                t *= -x / k;
                s += t / (a + k);
                if (Abs(ps - s) < EPS3) break;
            }
            return s;
        }

        /// <summary>
        /// 規格化第2種不完全ガンマ関数の対数
        /// LogGammaQ(a, x) = Log(GammaQ(a, x) = Log(Γ(a, x) / Γ(a)) = Log(1 - GammaP(a, x))
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double LogGammaQ(double a, double x)
        {
            if (!(a > 0 && x > 0)) return double.NaN;
            double aFunc(int m) => m * (a - m);
            double bFunc(int m) => m + m + x - a + 1;
            var a0 = bFunc(0);
            var a1 = aFunc(1);
            var b1 = bFunc(1);

            var u = b1 + a1 / a0;
            var v = b1;
            var d = a0 + a1 / b1;

            for (var n = 2; n <= 1000; ++n)
            {
                var alpha = aFunc(n);
                var beta = bFunc(n);
                var pu = u;
                var pv = v;
                var pd = d;
                u = beta + alpha / pu;
                v = beta + alpha / pv;
                d = u / v * pd;
                if (Abs(d - pd) <= EPS3) break;
            }

            return Log(x) * a - x - GammaLn(a) - Log(d);
        }

        /// <summary>
        /// 規格化第2種不完全ガンマ関数
        /// GammaQ(a, x) = Γ(a, x) / Γ(a) = 1 - GammaP(a, x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GammaQ(double a, double x) => Exp(LogGammaQ(a, x));

        /// <summary>
        /// 誤差関数
        /// - 不完全ガンマ関数
        ///   - https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0
        ///   - https://en.wikipedia.org/wiki/Incomplete_gamma_function
        ///   - https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_gamma/igamma.html#math_toolkit.sf_gamma.igamma.implementation
        /// </summary>
        public static double Erf(double z)
        {
            if (IsNaN(z)) return NaN;
            if (z == 0) return 0;
            if (z < -0.5) return Erfc(-z) - 1;
            if (z < 0) return -Erf(-z);

            // 1/Sqrt(PI)
            const double invSqrtPi = 0.56418958354775628694807945156077;

            var zSq = z * z;

            var t = invSqrtPi * z;
            var s = t + t;

            for (var m = 1; m <= 1000; ++m)
            {
                t *= -zSq / m;
                var ps = s;
                s += t / (m + 0.5);
                if (Abs(s - ps) < EPS3) break;
            }

            return s;
        }

        /// <summary>
        /// スケーリング相補誤差関数
        /// - 不完全ガンマ関数
        ///   - https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0
        ///   - https://en.wikipedia.org/wiki/Incomplete_gamma_function
        ///   - https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_gamma/igamma.html#math_toolkit.sf_gamma.igamma.implementation
        /// - 連分数の計算法
        ///   - πとeの連分数展開とその数値計算法 - Expansions of π and e into Continued Fractions and their Numerical Evaluation
        ///     https://ci.nii.ac.jp/naid/110006459103
        /// </summary>
        public static double Erfcx(double z)
        {
            if (IsNaN(z)) return NaN;

            var zSq = z * z;

            // z < 0.5 では Erf() から算出
            if (z < 0.5) return Exp(zSq) * (1 - Erf(z));

            double aFunc(int m) => m * (0.5 - m);
            double bFunc(int m) => m + m + zSq + 0.5;
            var a0 = bFunc(0);
            var a1 = aFunc(1);
            var b1 = bFunc(1);

            var u = b1 + a1 / a0;
            var v = b1;
            var d = a0 + a1 / b1;

            for (var n = 2; n <= 1000; ++n)
            {
                var alpha = aFunc(n);
                var beta = bFunc(n);
                var pu = u;
                var pv = v;
                var pd = d;
                u = beta + alpha / pu;
                v = beta + alpha / pv;
                d = u / v * pd;
                if (Abs(d - pd) <= EPS3) break;
            }

            // 1/Sqrt(PI)
            const double invSqrtPi = 0.56418958354775628694807945156077;

            return invSqrtPi * z / d;
        }

        /// <summary>
        /// 相補誤差関数
        /// </summary>
        public static double Erfc(double z)
            => z < 0.5 ? 1 - Erf(z) : Exp(-z * z) * Erfcx(z);

        /// <summary>
        /// 逆相補誤差関数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double ErfcInv(double x)
        {
            if (IsNaN(x)) return NaN;
            if (x == 0) return PositiveInfinity;
            if (x == 1) return 0;
            if (x == 2) return NegativeInfinity;
            if (!(0 <= x && x <= 2)) return NaN;

            var minz = x < 1 ? 0.0 : -6.0;
            var maxz = x < 1 ? 28.0 : 0.0;
            var minx = x < 1 ? 1.0 : 2.0;
            var maxx = x < 1 ? 0.0 : 1.0;
            var pivz = 0.5 * (minz + maxz);

            for (var n = 1; n <= 1000; ++n)
            {
                var pivx = Erfc(pivz);
                if (Abs(x - pivx) <= EPS3) break;
                if (pivx < x)
                {
                    maxz = pivz;
                    maxx = pivx;
                }
                else
                {
                    minz = pivz;
                    minx = pivx;
                }
                pivz = 0.5 * (minz + maxz);
                if (Min(Abs(pivz - minz), Abs(maxz - pivz)) <= EPS3) break;
            }
            return pivz;
        }

        /// <summary>
        /// 正規分布の累積分布関数
        /// </summary>
        public static double NormCdf(double z)
            => 0.5 * Erfc(-0.70710678118654752440084436210485 * z);

        /// <summary>
        /// 標準分位関数(プロビット関数)
        /// </summary>
        public static double NormCdfInv(double x)
            => -1.4142135623730950488016887242097 * ErfcInv(2 * x);

        /// <summary>
        /// ベータ関数の対数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double BetaLn(double x, double y)
            => GammaLn(x) + GammaLn(y) - GammaLn(x + y);

        /// <summary>
        /// ベータ関数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Beta(double x, double y)
        {
            if (x > 0 && y > 0) return Exp(GammaLn(x) + GammaLn(y) - GammaLn(x + y));
            if (!(x + y > 0)) return Gamma(x) * Gamma(y) / Gamma(x + y);
            if (x > 0) return Exp(GammaLn(x) - GammaLn(x + y)) * Gamma(y);
            if (y > 0) return Exp(GammaLn(y) - GammaLn(x + y)) * Gamma(x);
            return Gamma(x) * Gamma(y) / Gamma(x + y);
        }

        /// <summary>
        /// IBeta(a, b, logit) = IBeta(a+n, b, logit) + Exp(logit.logpx * a + logit.lognx * b - BetaLn(a, b) + IBeta_AMod(a, b, n, logit)) / a;
        /// <br />
        /// IBeta(a+n, b, logit) = IBeta(a, b, logit) - Exp(logit.logpx * a + logit.lognx * b - BetaLn(a, b) + IBeta_AMod(a, b, n, logit)) / a;
        /// </summary>
        private static double IBeta_AMod(double a, double b, int n, Logit logit)
        {
            var s = 0.0;
            var t = 0.0;
            for (var i = 1; i < n; ++i)
            {
                t += logit.LogPx + Log((a + b + i - 1) / (a + i));
                s += Log1pExp(t - s);
            }
            return s;
        }

        /// <summary>
        /// IBeta(a, b, logit) = IBeta(a, b+n, logit) - Exp(logit.logpx * a + logit.lognx * b - BetaLn(a, b) + IBeta_BMod(a, b, n, logit)) / b;
        /// <br />
        /// IBeta(a, b+n, logit) = IBeta(a, b, logit) + Exp(logit.logpx * a + logit.lognx * b - BetaLn(a, b) + IBeta_BMod(a, b, n, logit)) / b;
        /// </summary>
        private static double IBeta_BMod(double a, double b, int n, Logit logit)
        {
            var s = 0.0;
            var t = 0.0;
            for (var i = 1; i < n; ++i)
            {
                t += logit.LogNx + Log((a + b + i - 1) / (b + i));
                s += Log1pExp(t - s);
            }
            return s;
        }

        /// <summary>
        /// 正則ベータ関数 (regularized incomplete beta function / regularized beta function)
        /// * 連分数展開 (due to Didonato and Morris)
        ///   * Incomplete Beta Functions - Implementation
        ///     https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_beta/ibeta_function.html#math_toolkit.sf_beta.ibeta_function.implementation
        ///   * Significant Digit Computation of the Incomplete Beta Function Ratios
        ///     AR DiDonato, AH Morris Jr - 1988 - dtic.mil
        ///     http://www.dtic.mil/dtic/tr/fulltext/u2/a210118.pdf
        ///   * Armido R. Didonato and Alfred H. Morris, Jr.. 1992. Algorithm 708: Significant digit computation of the incomplete beta function ratios. ACM Trans. SMath. Softw. 18, 3 (September 1992), 360-373.
        ///     DOI: https://doi.org/10.1145/131766.131776
        /// * 連分数の計算法
        ///   * πとeの連分数展開とその数値計算法 - Expansions of π and e into Continued Fractions and their Numerical Evaluation
        ///     https://ci.nii.ac.jp/naid/110006459103
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double IBeta(double a, double b, double x) => IBetaLogit(a, b, Logit.FromPx(x));

        /// <summary>
        /// 正則ベータ関数（但し、x値の代わりに logit(x) = log(x / (1 - x)) で入力）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logit"></param>
        /// <returns></returns>
        public static double IBetaLogit(double a, double b, double logit) => IBetaLogit(a, b, Logit.FromLogit(logit));

        /// <summary>
        /// 正則ベータ関数（但し、x値の代わりに logit(x) = log(x / (1 - x)) で入力）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logit"></param>
        /// <returns></returns>
        public static double IBetaLogit(double a, double b, Logit logit)
        {
            // 0 < a, 0 < b, 0 <= x, x <= 1
            if (!(a > 0 && b > 0) || double.IsNaN(logit.V)) return double.NaN;

            var px = logit.Px;
            var nx = logit.Nx;
            var logpx = logit.LogPx;
            var lognx = logit.LogNx;

            // $ I_0(a, b) = 0 $
            if (logit.Px == 0) return 0;
            // $ I_1(a, b) = 1 $
            if (logit.Nx == 0) return 1;
            // $ I_x(1, b) = (1 - x) ^ b $
            if (a == 1)
                return -Expm1(logit.LogNx * b);
            // $ I_x(a, 1) = x ^ a $
            if (b == 1)
                return Exp(logit.LogPx * a);
            // $ I_x(1, b) = (1 - x) ^ b $
            if (a <= 260 && a == Floor(a))
                return -Expm1(logit.LogNx * b + Log1pExp(Log(b) + logpx + IBeta_AMod(1, b, (int)(a - 1), logit)));
            // $ I_x(a, 1) = x ^ a $
            if (b <= 260 && b == Floor(b))
                return Exp(logit.LogPx * a + Log1pExp(Log(a) + lognx + IBeta_BMod(a, 1, (int)(b - 1), logit)));

            var p = a / (a + b);
            var q = b / (a + b);

            /*
            void noop() { }

            if (b < 40 && px > 0.7)
                return IBeta_BUP_Logit(a, b, logit, px, nx);
            if (a < 40 && px < 0.3)
                return 1 - IBeta_BUP_Logit(b, a, -logit, nx, px);

            // $ I_x(a, b) = I_x(a + 1, b) + \frac{x^a (1 - x)^b}{a B(a, b)} $
            if (a < 1) return logit.v < 0 ?
                IBetaLogit(a + 1, b, logit) + Exp(logpx * a + lognx * b - BetaLn(a, b)) / a:
                IBetaLogit(a + 1, b, logit) + Exp(logpx * a + lognx * b - BetaLn(a, b)) / a;
            // $ I_x(a, b) = I_x(a, b + 1) - \frac{x^a (1 - x)^b}{b B(a, b)} $
            if (b < 1) return logit.v < 0 ?
                IBetaLogit(a, b + 1, logit) - Exp(logpx * a + lognx * b - BetaLn(a, b)) / b:
                IBetaLogit(a, b + 1, logit) - Exp(logpx * a + lognx * b - BetaLn(a, b)) / b;

            // b < 40, bx <= .7
            if (a > 1 && b > 1 && b < 40 && b * px <= 0.7)
                noop();
                // return IBeta_BPSER(a, b, logit);

            if (a > 1 && b > 1 && b < 40 && b * px > 0.7 && px <= 0.7)
                noop();
            */

            /*
            if (100 < a && a <= b && logit.Px >= 0.97 * p)
                return IBeta_BASYM(a, b, logit);
            if (100 < b && b < a && logit.Nx <= 1.03 * q)
                return IBeta_BASYM(a, b, logit);
            */

            return px < (a + 1) / (a + b + 2) ?
                IBeta_BUP_BFRAC(a, b, logit):
                1 - IBeta_BUP_BFRAC(b, a, logit.Negate());
        }

        /// <summary>
        /// b < 40, px > 0.7
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logit"></param>
        /// <returns></returns>
        private static double IBeta_BUP_BGRAT(double a, double b, Logit logit)
        {
            var px = logit.Px;
            var nx = logit.Nx;
            var logpx = logit.LogPx;
            var lognx = logit.LogNx;
            if (a <= 15)
            {
                var n = 16 - (int)Ceiling(a);
                var an = a + n;
                return IBeta_BUP_BGRAT(an, b, logit) + Exp(logpx * a + lognx * b - BetaLn(a, b) + IBeta_AMod(a, b, n, logit)) / a;
            }
            if (b > 1)
            {
                var n = (int)Ceiling(b) - 1;
                var bn = b - n;
                return IBeta_BUP_BGRAT(a, bn, logit) + Exp(logpx * a + lognx * bn - BetaLn(a, bn) + IBeta_BMod(a, bn, n, logit)) / bn;
            }
            return IBeta_BGRAT(a, b, logit);
        }

        /// <summary>
        /// 15 < a, b <= 1, px > 0.7
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logit"></param>
        /// <returns></returns>
        private static double IBeta_BGRAT(double a, double b, Logit logit)
        {
            var px = logit.Px;
            var nx = logit.Nx;
            var logx = logit.LogPx;
            var t = a + (b - 1) * 0.5;
            var u = -t * logx;
            var logr = -u + Log(u) * b - GammaLn(b);
            var m = Exp(logr + GammaLn(a + b) - GammaLn(a)) * Pow(t, -b);
            var p = new List<double> { 1.0 };
            var logqb = LogGammaQ(b, u);
            var j = new List<double> { Exp(LogGammaQ(b, u) - logr) };
            var c = new List<double> { 1.0 };
            var s = p[0] * j[0];
            for (var n = 1; n < 1000; ++n)
            {
                c.Add(c[n - 1] / ((2.0 * n) * (2.0 * n + 1)));
                var sx = 0.0;
                for (var _m = 1; _m < n; ++_m)
                {
                    sx += (_m * b - n) * c[_m] * p[n - _m];
                }
                p.Add((b - 1) * c[n] + sx / n);
                j.Add(((b + 2 * n - 2) * (b + 2 * n - 1) * j[n - 1] + (u + b + 2 * n - 1) * Pow(logx * 0.5, 2 * n)) / (4 * t * t));
                var ps = s;
                s += p[n] * j[n];
                if (Abs(ps - s) < EPS3) break;
            }
            var ms = m * s;
            return ms;
        }

        /// <summary>
        /// * min(a, b) > 1, b < 40, bx <= .7
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double IBeta_BPSER(double a, double b, Logit logit)
        {
            var logx = logit.LogPx;
            var x = logit.Px;
            double t = 1;
            var s = 1 / a;

            for (var j = 1; j < 1000; ++j)
            {
                var ps = s;
                t *= x * (j - b) / j;
                s += t / (a + j);
                if (Abs(s - ps) < EPS3) break;
            }

            return s * Exp(logx * a - BetaLn(a, b));
        }

        /// <summary>
        /// b < 40, px > 0.7
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logit"></param>
        /// <returns></returns>
        private static double IBeta_BUP_BFRAC(double a, double b, Logit logit)
        {
            var px = logit.Px;
            var nx = logit.Nx;
            var logpx = logit.LogPx;
            var lognx = logit.LogNx;
            if (a <= 2)
            {
                var an = 3 - (int)Ceiling(a);
                return IBeta_BUP_BFRAC(a + an, b, logit) + Exp(logpx * a + lognx * b - BetaLn(a, b) + IBeta_AMod(a, b, an, logit)) / a;
            }
            if (b <= 2)
            {
                var bn = 3 - (int)Ceiling(b);
                return IBeta_BUP_BFRAC(a, b + bn, logit) - Exp(logpx * a + lognx * b - BetaLn(a, b) + IBeta_BMod(a, b, bn, logit)) / b;
            }
            return IBeta_BFRAC(a, b, logit);
        }

        private static double IBeta_BFRAC(double a, double b, Logit logit)
        {
            var px = logit.Px;
            var nx = logit.Nx;
            var logpx = logit.LogPx;
            var lognx = logit.LogNx;
            double Sq(double x) => x * x;
            double aFunc(int m) => (a + (m - 1)) * (a + b + (m - 1)) * m * (b - m) * Sq(px / (a + (m + m - 1)));
            double bFunc(int m) => m + m * (b - m) * px / (a + (m + m - 1)) + (a + m) * (a - (a + b) * px + 1 + m * (2 - px)) / (a + (m + m + 1));
            var a0 = bFunc(0);
            var a1 = aFunc(1);
            var b1 = bFunc(1);

            var u = b1 + a1 / a0;
            var v = b1;
            var d = a0 + a1 / b1;

            for (var n = 2; n <= 1000; ++n)
            {
                var alpha = aFunc(n);
                var beta = bFunc(n);
                var pu = u;
                var pv = v;
                var pd = d;
                u = beta + alpha / pu;
                v = beta + alpha / pv;
                d *= u / v;
                if (!(Abs(d - pd) > EPS3)) break;
            }

            return Exp(logpx * a + lognx * b - BetaLn(a, b)) / d;
        }

        private static double IBeta_BASYM(double a, double b, Logit logit)
        {
            // sqrt(0.125)
            const double sqrt1_8 = 0.35355339059327376220042218105242;
            // sqrt(2)
            const double sqrt2 = 1.4142135623730950488016887242097;
            // 2 / sqrt(PI)
            const double half_sqrt_pi_inv = 1.1283791670955125738961589031215;
            var p = a / (a + b);
            var q = b / (a + b);
            var logp = Log(p);
            var logq = Log(q);
            var phix = -a * (logit.LogPx - logp) - b * (logit.LogNx - logq);
            var z = Sqrt(phix);
            var betagamma = Sqrt(a <= b ? (q / a) : (p / b));
            var bg = 1.0;
            var abrate = a <= b ? a / b : b / a;
            var ab = 1.0;
            var al = new List<double>();
            var cl = new List<double>();
            var el = new List<double> { 1 };
            var ll = new List<double> { PI / 8 * Erfcx(z), sqrt1_8 };
            var s = 0.0;
            double dfn(double t) => GammaLn(t) - (t - 0.5) * Log(t) + t - HalfLog2PI;
            for (var n = 0; n <= 100; ++n)
            {
                ab *= abrate;
                var pm = (n % 2) == 0 ? 1 : -1;
                al.Add(2 / (n + 2.0) * (a <= b ? q * (1 + pm * ab) : p * (pm + ab)));
                double bfn(int _n, double r)
                {
                    if (_n < 1) return 1;
                    var bl = new List<double> { 1, r * al[1] };
                    for (var j = 2; j <= _n; ++j)
                    {
                        var bv = 0.0;
                        for (var i = 1; i < j; ++i)
                        {
                            bv += ((j - i) * r - i) * bl[i] * al[j - i];
                        }
                        bl.Add(r * al[j] + bv / j);
                    }
                    return bl[_n];
                }
                cl.Add(bfn(n, -0.5 * (n + 1)) / (n + 1));
                if (n > 0)
                {
                    var ev = 0.0;
                    for (var i = 0; i < n; ++i)
                    {
                        ev -= el[i] * cl[n - i];
                    }
                    el.Add(ev);
                }
                if (n > 1)
                    ll.Add(sqrt1_8 * Pow(sqrt2 * z, n - 1) + (n - 1) * ll[n - 2]);
                var ps = s;
                s += el[n] * ll[n] * bg;
                if (Abs(s - ps) < Abs(s * ME100))
                    break;
                bg *= betagamma;
            }
            return half_sqrt_pi_inv * Exp(dfn(a + b) - dfn(a) - dfn(b) - z * z) * Sqrt(2 * (a + b) / (a * b)) * s;
        }

        /// <summary>
        /// ベータ逆累積分布関数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double InvBeta(double a, double b, double p)
        {
            // 0 < a, 0 < b, 0 <= p <= 1
            if (!(a > 0 && b > 0)) return NaN;
            if (!(p >= 0 && p <= 1)) return NaN;
            if (p == 0) return 0;
            if (p == 1) return 1;

            var minx = 0.0;
            var maxx = 1.0;
            var minp = 0.0;
            var maxp = 1.0;
            var pivx = 0.0;

            for (var n = 1; n <= 1000; ++n)
            {
                var dpl = Max(p - minp, 0);
                var dph = Max(maxp - p, 0);
                pivx = minx + Max(Min(dpl / (dpl + dph), 0.9375), 0.0625) * (maxx - minx);
                if (Max(0, Min(Abs(pivx - minx), Abs(maxx - pivx))) <= EPS3) pivx = (minx + maxx) * 0.5;
                if (Min(Abs(pivx - minx), Abs(maxx - pivx)) <= EPS3) break;
                var pivp = IBeta(a, b, pivx);
                if (Abs(p - pivp) <= EPS3) break;
                if (pivp < p)
                {
                    minx = pivx;
                    minp = pivp;
                }
                else
                {
                    maxx = pivx;
                    maxp = pivp;
                }
            }
            return pivx;
        }

        /// <summary>
        /// ベータ逆累積分布関数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double InvBetaLogit(double a, double b, double p)
        {
            // 0 < a, 0 < b, 0 <= p <= 1
            if (!(a > 0 && b > 0)) return NaN;
            if (!(p >= 0 && p <= 1)) return NaN;
            if (p == 0) return 0;
            if (p == 1) return 1;

            var minx = -100.0;
            var maxx = +100.0;
            var minp = 0.0;
            var maxp = 1.0;
            var pivx = 0.0;

            for (var n = 1; n <= 1000; ++n)
            {
                var dpl = Max(p - minp, 0);
                var dph = Max(maxp - p, 0);
                pivx = minx + Max(Min(dpl / (dpl + dph), 0.9375), 0.0625) * (maxx - minx);
                if (Max(0, Min(Abs(pivx - minx), Abs(maxx - pivx))) <= EPS3) pivx = (minx + maxx) * 0.5;
                if (Min(Abs(pivx - minx), Abs(maxx - pivx)) <= EPS3) break;
                var pivp = IBetaLogit(a, b, pivx);
                if (Abs(p - pivp) <= EPS3) break;
                if (pivp < p)
                {
                    minx = pivx;
                    minp = pivp;
                }
                else
                {
                    maxx = pivx;
                    maxp = pivp;
                }
            }
            return pivx;
        }

    }

    public static class EloRating
    {

        /// <summary>
        /// 収束条件
        /// </summary>
        public const double EPS3 = SpecialFunc.EPS3;
        public const double Elo_logit_mul = SpecialFunc.LN10 / 400;
        public const double Elo_logit_imul = 400 / SpecialFunc.LN10;

        private const double PositiveInfinity = double.PositiveInfinity;
        private const double NegativeInfinity = double.NegativeInfinity;
        private static bool IsNaN(double x) => double.IsNaN(x);

        public static double WinRate2Elo(double winrate)
            => Elo_logit_imul * SpecialFunc.EnLogit(winrate);
        public static double Elo2WinRate(double elo)
            // => 1 / (1 + Pow(10, elo / -400));
            => SpecialFunc.DeLogit(Elo_logit_mul * elo);
        public static double Elo2LossRate(double elo)
            // => 1 / (1 + Pow(10, elo / 400));
            => SpecialFunc.DeLogit(-Elo_logit_mul * elo);

        public static double IBeta_Elo(double a, double b, double elo)
            => SpecialFunc.IBetaLogit(a, b, Elo_logit_mul * elo);
        public static double InvBetaElo(double a, double b, double p)
            => Elo_logit_imul * SpecialFunc.InvBetaLogit(a, b, p);

        // レーティング差の範囲
        public class RateInterval
        {
            // レーティング差・下側確率のパーセント点
            public double LowElo { get; }
            // 勝率・下側確率のパーセント点
            public double LowWin { get; }
            // 敗率・下側確率のパーセント点
            public double LowLoss { get; }
            // レーティング差・上側確率のパーセント点
            public double HighElo { get; }
            // 勝率・上側確率のパーセント点
            public double HighWin { get; }
            // 敗率・上側確率のパーセント点
            public double HighLoss { get; }

            /// <summary>
            /// レーティング差の範囲
            /// </summary>
            /// <param name="lElo">レーティング差・下側確率のパーセント点</param>
            /// <param name="hElo">レーティング差・上側確率のパーセント点</param>
            public RateInterval(double lElo, double hElo)
            {
                LowElo = IsNaN(lElo) ? NegativeInfinity : lElo;
                HighElo = IsNaN(hElo) ? PositiveInfinity : hElo;
                LowWin = Elo2WinRate(LowElo);
                LowLoss = Elo2LossRate(LowElo);
                HighWin = Elo2WinRate(HighElo);
                HighLoss = Elo2LossRate(HighElo);
            }
            public override string ToString() =>
                $"[{LowElo,8:+0.00;-0.00}({LowWin,7:0.00%}) ～ {HighElo,8:+0.00;-0.00}({HighWin,7:0.00%}), Range: {HighElo - LowElo:0.00}({HighWin - LowWin:0.00%}) ]";
        }

        /// <summary>
        /// 2者の対局の勝数および負数から推測されるレーティング差の範囲の算出
        /// </summary>
        /// <param name="win">勝数 0 ≦ win</param>
        /// <param name="loss">負数 0 ≦ loss</param>
        /// <param name="p">上側確率および下側確率 0 ＜ p ＜ 0.5</param>
        /// <returns></returns>
        public static RateInterval WinLossElo(double win, double loss, double p) =>
            new RateInterval(InvBetaElo(win, loss + 1, p), InvBetaElo(win + 1, loss, 1 - p));

    }

}
