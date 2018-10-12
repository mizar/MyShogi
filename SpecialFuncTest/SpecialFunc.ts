class SpecialFunc
{
  private constructor() { /* noop */ }
  static readonly MEPS = 2.2204460492503130808472633361816e-16; // pow(2,-52)
  static readonly ME100 = SpecialFunc.MEPS * 100;
  static readonly EPS = Number.MIN_VALUE; // pow(2, -1074)
  static readonly EPS3 = Number.MIN_VALUE * 3; // 3 * pow(2, -1074)
  static readonly LN2 = 0.69314718055994530941723212145818; // log(2)
  static readonly LN10 = 2.3025850929940456840179914546844; // log(10)
  static readonly LOGE = 0.43429448190325182765112891891661; // log(e)
  static readonly LnSqrt2PI = 0.91893853320467274178032973640562; // log(sqrt(2 * PI))
  /**
   * log(1+x)
   * @param x
   */
  private static log1p(x: number): number { return Math.log1p(x); }
  /**
   * exp(x)-1
   * @param x
   */
  private static expm1(x: number): number { return Math.expm1(x); }
  /**
   * log(1-exp(x))
   * @param x
   */
  static log1mexp(x: number): number
  {
    return !(x < 0) ? Number.NaN:
      x > -this.LN2 ?
        Math.log(-Math.expm1(x)):
        Math.log1p(-Math.exp(x));
  }
  /**
   * log(1+exp(x))
   * @param x
   */
  static log1pexp(x: number): number
  {
    return x <= 0 ?
      Math.log1p(Math.exp(x)):
      (x + Math.log1p(Math.exp(-x)));
  }
  /**
   * ロジット関数
   * @param px
   */
  static enlogit(px: number): number
  {
    return Number.isNaN(px) ? Number.NaN:
      px <= 0 ? Number.NEGATIVE_INFINITY:
      px >= 1 ? Number.POSITIVE_INFINITY:
      Math.log(px) - Math.log1p(-px);
  }
  /**
   * 逆ロジット関数（ロジスティック関数）
   * @param logit
   */
  static delogit(logit: number): number
  {
    if (logit > 0)
    {
      var nexp = Math.exp(-logit);
      return 1 / (nexp + 1);
    }
    else
    {
      var pexp = Math.exp(logit);
      return pexp / (pexp + 1);
    }
  }
  /**
   * 逆ロジット関数の対数
   * @param logit
   */
  static logdelogit(logit: number): number
  {
    return -this.log1pexp(-logit);
  }
  /**
   * ガンマ関数
   * @param z
   */
  private static gamma_intl(z: number): number
  {
    /*
    // z <= 0 の場合の返り値は保証外
    // (特に0以上の整数n に対して z == n, -2n-1 <= z <= -2n, z == -Infinity の場合)
    // Lanczos近似 : https://en.wikipedia.org/wiki/Lanczos_approximation
    // パラメータ算出 : https://mrob.com/pub/ries/lanczos-gamma.html
    // g = 607.0 / 128.0;
    var g = 4.7421875;
    var x = -2.90818e-15;
    var p = [
      57.156235665862923517,
      -59.597960355475491248,
      14.136097974741747174,
      -0.49191381609762019978,
      0.000033994649984811888699,
      0.000046523628927048575665,
      -0.000098374475304879564677,
      0.00015808870322491248884,
      -0.00021026444172410488319,
      0.00021743961811521264320,
      -0.00016431810653676389022,
      0.000084418223983852743293,
      -0.000026190838401581408670,
      3.6899182659531622704e-6,
    ];
    for (var i = p.length - 1; i >= 0; --i)
    {
        x += p[i] / (z + i);
    }
    var t = g - 0.5 + z;
    return this.LnSqrt2PI + Math.log1p(x) + (Math.log(t) - 1) * (z - 0.5) - g; // gammaln
    */
    /*
      http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html : RGAMMA06.TBL
      6
      '
      Gamma(x) = exp((x-0.5)*log(x+V)-x)
                  *(((...(An/(x+Bn)+...A1)/(x+B1)+A0)/x+Ar)
              ,0<x<infinity
      ''
      V=	 6.0975075753906857609437558E+0000
      Ar=	 5.6360656189756064967977564E-0003
      A0=	 1.2242597732687991784645973E-0001
      A1=	 8.5137081316503418312411656E-0001
      A2=	 2.2502304753561816836695856E+0000
      A3=	 2.0962955353894997733869983E+0000
      A4=	 5.0219722703392090725884168E-0001
      A5=	 1.1240582657165407383437999E-0002
      B1=	 1.0000000000006553243170562E+0000
      B2=	 1.9999999996201023058065171E+0000
      B3=	 3.0000000467265241458431618E+0000
      B4=	 3.9999966300007508932097016E+0000
      B5=	 5.0003589884831925541613237E+0000
      err=	 2.09144255E-0018
    */
    /*
      http://www.kurims.kyoto-u.ac.jp/~ooura/gamerf-j.html : RGAMMA13.TBL
      13
      '
      Gamma(x) = exp((x-0.5)*log(x+V)-x)
                *(((...(An/(x+Bn)+...A1)/(x+B1)+A0)/x+Ar)
              ,0<x<infinity
      ''
      V=	 1.35781220007039464739769136052735188826566614E+0001
      Ar=	 3.17823842997348984212895391439981193809771347E-0006
      A0=	 3.14820702833493003545826236239083394571995671E-0004
      A1=	 1.27937416087229845006934584904736618598070572E-0002
      A2=	 2.78748303060299808744345690552596166059251493E-0001
      A3=	 3.57487639582285701807582585579290271336089099E+0000
      A4=	 2.79272804215633250156669351783752812175650972E+0001
      A5=	 1.33213846503797389894468858322687847549726114E+0002
      A6=	 3.79504051924654223127926344491479357839857722E+0002
      A7=	 6.15621499930282594633468081962352923412741184E+0002
      A8=	 5.24004008691006507011182613589749851171431576E+0002
      A9=	 2.04187662020237118761681790759964964799068736E+0002
      A10=	 2.86456197727291086831913426471935542005422307E+0001
      A11=	 8.95072101413389847373058347512910403979947773E-0001
      A12=	 1.84108633157612656306027334817135207544862157E-0003
      B1=	 9.99999999999999999999999999829177067439186649E-0001
      B2=	 2.00000000000000000000000143725325109773686179E+0000
      B3=	 2.99999999999999999999878069870191969828857942E+0000
      B4=	 4.00000000000000000031822896056305388990947492E+0000
      B5=	 4.99999999999999996032765694796886928790459606E+0000
      B6=	 6.00000000000000300343091566980971296037249121E+0000
      B7=	 6.99999999999983752474626982882253159057375788E+0000
      B8=	 8.00000000000719155188030217651616848093810689E+0000
      B9=	 8.99999999970006818618226539512826008489484540E+0000
      B10=	 1.00000000142050052373091324295304916612836237E+0001
      B11=	 1.09999989539201196803612424783730853335056534E+0001
      B12=	 1.20002381089341943372805397259444226612900582E+0001
      err=	 8.43420741E-0037
    */
    return Math.exp((z - 0.5) * Math.log(z + 6.0975075753906857609437558e+0) - z) * ((((((
      1.1240582657165407383437999e-2 / (z + 5.0003589884831925541613237e+0) +
      5.0219722703392090725884168e-1) / (z + 3.9999966300007508932097016e+0) +
      2.0962955353894997733869983e+0) / (z + 3.0000000467265241458431618e+0) +
      2.2502304753561816836695856e+0) / (z + 1.9999999996201023058065171e+0) +
      8.5137081316503418312411656e-1) / (z + 1.0000000000006553243170562e+0) +
      1.2242597732687991784645973e-1) / z + 5.6360656189756064967977564e-3);
  }
  /**
   * ガンマ関数
   * @param z
   */
  static gamma(z: number): number
  {
    return z >= 0.5 ?
      this.gamma_intl(z):
      Math.PI / (Math.sin(Math.PI * z) * this.gamma_intl(1 - z));
  }
  /**
   * 対数ガンマ関数
   * @param z
   */
  private static gammaln_intl(z: number): number
  {
    return (z - 0.5) * Math.log(z + 6.0975075753906857609437558e+0) - z + Math.log((((((
      1.1240582657165407383437999e-2 / (z + 5.0003589884831925541613237e+0) +
      5.0219722703392090725884168e-1) / (z + 3.9999966300007508932097016e+0) +
      2.0962955353894997733869983e+0) / (z + 3.0000000467265241458431618e+0) +
      2.2502304753561816836695856e+0) / (z + 1.9999999996201023058065171e+0) +
      8.5137081316503418312411656e-1) / (z + 1.0000000000006553243170562e+0) +
      1.2242597732687991784645973e-1) / z + 5.6360656189756064967977564e-3);
  }

  /**
   * 対数ガンマ関数
   * @param z
   */
  static gammaln(z: number): number
  {
    const log_pi = 1.1447298858494001741434273513531; // log(PI)
    return z >= 0.5 ?
      this.gammaln_intl(z):
      log_pi - Math.log(Math.sin(Math.PI * z)) - this.gammaln_intl(1 - z);
  }
  /**
   * 規格化第1種不完全ガンマ関数
   * GammaP(a, x) = γ(a, x) / Γ(a) = 1 - GammaQ(a, x)
   * @param a
   * @param x
   */
  static gammap(a: number, x: number)
  {
    if (!(a > 0 && x > 0)) return Number.NaN;
    var t = Math.exp(Math.log(x) * a - this.gammaln(a));
    var s = t / a;
    for (var k = 1; k <= 1000; ++k)
    {
      var ps = s;
      t *= -x / k;
      s += t / (a + k);
      if (Math.abs(ps - s) < this.EPS3) break;
    }
    return s;
  }
  /**
   * 規格化第2種不完全ガンマ関数の対数
   * GammaQ(a, x) = Γ(a, x) / Γ(a) = 1 - GammaP(a, x)
   * @param a
   * @param x
   */
  static loggammaq(a: number, x: number): number
  {
    if (!(a > 0 && x > 0)) return Number.NaN;
    var afunc = (m: number) => m * (a - m);
    var bfunc = (m: number) => m + m + x - a + 1;
    var a0 = bfunc(0);
    var a1 = afunc(1);
    var b1 = bfunc(1);
    var u = b1 + a1 / a0;
    var v = b1;
    var d = a0 + a1 / b1;
    for (var n = 2; n <= 1000; ++n)
    {
      var alpha = afunc(n);
      var beta = bfunc(n);
      var pu = u;
      var pv = v;
      var pd = d;
      u = beta + alpha / pu;
      v = beta + alpha / pv;
      d = u / v * pd;
      if (Math.abs(d - pd) <= this.EPS3) break;
    }
    return Math.log(x) * a - x - this.gammaln(a) - Math.log(d);
  }
  /**
   * 規格化第2種不完全ガンマ関数
   * GammaQ(a, x) = Γ(a, x) / Γ(a) = 1 - GammaP(a, x)
   * @param a
   * @param x
   */
  static gammaq(a: number, x: number): number
  {
    return Math.exp(this.loggammaq(a, x));
  }
  /**
   * 誤差関数
   * - 不完全ガンマ関数
   *   - https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0
   *   - https://en.wikipedia.org/wiki/Incomplete_gamma_function
   *   - https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_gamma/igamma.html#math_toolkit.sf_gamma.igamma.implementation
   * @param z
   */
  static erf(z: number): number
  {
    if (Number.isNaN(z)) return Number.NaN;
    if (z == 0) return 0;
    if (z < -0.5) return this.erfc(-z) - 1;
    if (z < 0) return -this.erf(-z);
    // 1/Sqrt(PI)
    const invSqrtPi = 0.56418958354775628694807945156077;
    var zSq = z * z;
    var t = invSqrtPi * z;
    var s = t + t;
    for (var m = 1; m <= 1000; ++m)
    {
      t *= -zSq / m;
      var ps = s;
      s += t / (m + 0.5);
      if (Math.abs(s - ps) < this.EPS3) break;
    }
    return s;
  }
  /**
   * スケーリング相補誤差関数
   * - 不完全ガンマ関数
   *   - https://ja.wikipedia.org/wiki/%E4%B8%8D%E5%AE%8C%E5%85%A8%E3%82%AC%E3%83%B3%E3%83%9E%E9%96%A2%E6%95%B0
   *   - https://en.wikipedia.org/wiki/Incomplete_gamma_function
   *   - https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_gamma/igamma.html#math_toolkit.sf_gamma.igamma.implementation
   * - 連分数の計算法
   *   - πとeの連分数展開とその数値計算法 - Expansions of π and e into Continued Fractions and their Numerical Evaluation
   *     https://ci.nii.ac.jp/naid/110006459103
   * @param z
   */
  static erfcx(z: number): number
  {
    if (Number.isNaN(z)) return Number.NaN;
    var zSq = z * z;
    // z < 0.5 では Erf() から算出
    if (z < 0.5) return Math.exp(zSq) * (1 - this.erf(z));
    var afunc = (m: number) => m * (0.5 - m);
    var bfunc = (m: number) => m + m + zSq + 0.5;
    var a0 = bfunc(0);
    var a1 = afunc(1);
    var b1 = bfunc(1);
    var u = b1 + a1 / a0;
    var v = b1;
    var d = a0 + a1 / b1;
    for (var n = 2; n <= 1000; ++n)
    {
      var alpha = afunc(n);
      var beta = bfunc(n);
      var pu = u;
      var pv = v;
      var pd = d;
      u = beta + alpha / pu;
      v = beta + alpha / pv;
      d = u / v * pd;
      if (Math.abs(d - pd) <= this.EPS3) break;
    }
    // 1/Sqrt(PI)
    const invSqrtPi = 0.56418958354775628694807945156077;
    return invSqrtPi * z / d;
  }
  /**
   * 相補誤差関数
   * @param z
   */
  static erfc(z: number): number
  {
    return z < 0.5 ? 1 - this.erf(z) : Math.exp(-z * z) * this.erfcx(z);
  }
  /**
   * 逆相補誤差関数
   * @param x
   */
  static erfcinv(x: number): number
  {
    if (Number.isNaN(x)) return Number.NaN;
    if (x == 0) return Number.POSITIVE_INFINITY;
    if (x == 1) return 0;
    if (x == 2) return Number.NEGATIVE_INFINITY;
    if (!(0 <= x && x <= 2)) return NaN;
    var minz = x < 1 ? 0.0 : -6.0;
    var maxz = x < 1 ? 28.0 : 0.0;
    var minx = x < 1 ? 1.0 : 2.0;
    var maxx = x < 1 ? 0.0 : 1.0;
    var pivz = 0.5 * (minz + maxz);
    for (var n = 1; n <= 1000; ++n)
    {
      var pivx = this.erfc(pivz);
      if (Math.abs(x - pivx) <= this.EPS3) break;
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
      if (Math.min(Math.abs(pivz - minz), Math.abs(maxz - pivz)) <= this.EPS3) break;
    }
    return pivz;
  }
  /**
   * 正規分布の累積分布関数
   * @param z
   */
  static normcdf(z: number): number
  {
    return 0.5 * this.erfc(-0.70710678118654752440084436210485 * z);
  }
  /**
   * 標準分位関数(プロビット関数)
   * @param x
   */
  static normcdfinv(x: number): number
  {
    return -1.4142135623730950488016887242097 * this.erfcinv(2 * x);
  }
  /**
   * ベータ関数の対数
   * @param x
   * @param y
   */
  static betaln(x: number, y: number): number
  {
    return this.gammaln(x) + this.gammaln(y) - this.gammaln(x + y);
  }
  /**
   * ベータ関数
   * @param x
   * @param y
   */
  static beta(x: number, y: number): number
  {
    if (x > 0 && y > 0) return Math.exp(this.gammaln(x) + this.gammaln(y) - this.gammaln(x + y));
    if (!(x + y > 0)) return this.gamma(x) * this.gamma(y) / this.gamma(x + y);
    if (x > 0) return Math.exp(this.gammaln(x) - this.gammaln(x + y)) * this.gamma(y);
    if (y > 0) return Math.exp(this.gammaln(y) - this.gammaln(x + y)) * this.gamma(x);
    return this.gamma(x) * this.gamma(y) / this.gamma(x + y);
  }
  /**
   * IBeta(a, b, logit) = IBeta(a+n, b, logit) + Exp(logit.logpx * a + logit.lognx * b - betaln(a, b) + ibeta_amod(a, b, n, logit)) / a;
   * IBeta(a+n, b, logit) = IBeta(a, b, logit) - Exp(logit.logpx * a + logit.lognx * b - betaln(a, b) + ibeta_amod(a, b, n, logit)) / a;
   * @param a
   * @param b
   * @param n
   * @param logit
   */
  private static ibeta_amod(a: number, b: number, n: number, logit: Logit)
  {
    var s = 0.0;
    var t = 0.0;
    for (var i = 1; i < n; ++i)
    {
      t += logit.logpx + Math.log((a + b + i - 1) / (a + i));
      s += this.log1pexp(t - s);
    }
    return s;
  }
  /**
   * IBeta(a, b, logit) = IBeta(a, b+n, logit) - Exp(logit.logpx * a + logit.lognx * b - betaln(a, b) + ibeta_bmod(a, b, n, logit)) / b;
   * IBeta(a, b+n, logit) = IBeta(a, b, logit) + Exp(logit.logpx * a + logit.lognx * b - betaln(a, b) + ibeta_bmod(a, b, n, logit)) / b;
   * @param a
   * @param b
   * @param n
   * @param logit
   */
  private static ibeta_bmod(a: number, b: number, n: number, logit: Logit)
  {
    var s = 0.0;
    var t = 0.0;
    for (var i = 1; i < n; ++i)
    {
      t += logit.lognx + Math.log((a + b + i - 1) / (b + i));
      s += this.log1pexp(t - s);
    }
    return s;
  }
  /**
   * 正則ベータ関数 (regularized incomplete beta function / regularized beta function)
   * - 連分数展開 (due to Didonato and Morris)
   *   - Incomplete Beta Functions - Implementation
   *     https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/sf_beta/ibeta_function.html#math_toolkit.sf_beta.ibeta_function.implementation
   *   - Significant Digit Computation of the Incomplete Beta Function Ratios
   *     AR DiDonato, AH Morris Jr - 1988 - dtic.mil
   *     http://www.dtic.mil/dtic/tr/fulltext/u2/a210118.pdf
   *   - Armido R. Didonato and Alfred H. Morris, Jr.. 1992. Algorithm 708: Significant digit computation of the incomplete beta function ratios. ACM Trans. SMath. Softw. 18, 3 (September 1992), 360-373.
   *     DOI: https://doi.org/10.1145/131766.131776
   * - 連分数の計算法
   *   - πとeの連分数展開とその数値計算法 - Expansions of π and e into Continued Fractions and their Numerical Evaluation
   *     https://ci.nii.ac.jp/naid/110006459103
   * @param a
   * @param b
   * @param x
   */
  static ibeta(a: number, b: number, x: number)
  {
    return this.ibetalogit(a, b, Logit.frompx(x));
  }
  /**
   * 正則ベータ関数（但し、x値の代わりに logit(x) = log(x / (1 - x)) で入力）
   * @param a
   * @param b
   * @param _logit
   */
  static ibetalogit(a: number, b: number, _logit: number | Logit)
  {
    var logit = (typeof _logit === "number") ? Logit.fromlogit(_logit) : _logit;
    // 0 < a, 0 < b, 0 <= x, x <= 1
    if (!(a > 0 && b > 0) || Number.isNaN(logit.v)) return Number.NaN;
    var px = logit.px;
    var nx = logit.nx;
    var logpx = logit.logpx;
    var lognx = logit.lognx;
    // $ I_0(a, b) = 0 $
    if (px == 0) return 0;
    // $ I_1(a, b) = 1 $
    if (nx == 0) return 1;
    // $ I_x(1, b) = (1 - x) ^ b $
    if (a == 1)
      return -Math.expm1(logit.lognx * b);
    // $ I_x(a, 1) = x ^ a $
    if (b == 1)
      return Math.exp(logit.logpx * a);
    // $ I_x(1, b) = (1 - x) ^ b $
    if (a <= 260 && a == Math.floor(a))
      return -Math.expm1(logit.lognx * b + this.log1pexp(Math.log(b) + logpx + this.ibeta_amod(1, b, a - 1, logit)));
    // $ I_x(a, 1) = x ^ a $
    if (b <= 260 && b == Math.floor(b))
      return Math.exp(logit.logpx * a + this.log1pexp(Math.log(a) + lognx + this.ibeta_bmod(a, 1, b - 1, logit)));
    var p = a / (a + b);
    var q = b / (a + b);
    /*
    if (100 < a && a <= b && logit.px >= 0.97 * p)
      return ibeta_basym(a, b, logit);
    if (100 < b && b < a && logit.nx <= 1.03 * q)
      return ibeta_basym(a, b, logit);
    */
    return px < (a + 1) / (a + b + 2) ?
      this.ibeta_bup_bfrac(a, b, logit):
      1 - this.ibeta_bup_bfrac(b, a, logit.negate());
  }
  /**
   * BUP + BGRAT
   * @param a
   * @param b
   * @param logit
   */
  private static ibeta_bup_bgrat(a: number, b: number, logit: Logit)
  {
    var px = logit.px;
    var nx = logit.nx;
    var logpx = logit.logpx;
    var lognx = logit.lognx;
    if (a <= 15)
    {
      var anl = 16 - Math.ceil(a);
      var an = a + anl;
      return this.ibeta_bup_bgrat(an, b, logit) + Math.exp(logpx * a + lognx * b - this.betaln(a, b) + this.ibeta_amod(a, b, anl, logit)) / a;
    }
    if (b > 1)
    {
      var bnl = Math.ceil(b) - 1;
      var bn = b - bnl;
      return this.ibeta_bup_bgrat(a, bn, logit) + Math.exp(logpx * a + lognx * bn - this.betaln(a, bn) + this.ibeta_bmod(a, bn, bnl, logit)) / bn;
    }
    return this.ibeta_bgrat(a, b, logit);
  }
  /**
   * BGRAT
   * @param a
   * @param b
   * @param logit
   */
  private static ibeta_bgrat(a: number, b: number, logit: Logit)
  {
    var px = logit.px;
    var nx = logit.nx;
    var logx = logit.logpx;
    var t = a + (b - 1) * 0.5;
    var u = -t * logx;
    var logr = -u + Math.log(u) * b - this.gammaln(b);
    var m = Math.exp(logr + this.gammaln(a + b) - this.gammaln(a)) * Math.pow(t, -b);
    var p = [ 1.0 ];
    var logqb = this.loggammaq(b, u);
    var j = [ Math.exp(this.loggammaq(b, u) - logr) ];
    var c = [ 1.0 ];
    var s = p[0] * j[0];
    for (var n = 1; n < 1000; ++n)
    {
      c.push(c[n - 1] / ((2.0 * n) * (2.0 * n + 1)));
      var sx = 0.0;
      for (var _m = 1; _m < n; ++_m)
      {
        sx += (_m * b - n) * c[_m] * p[n - _m];
      }
      p.push((b - 1) * c[n] + sx / n);
      j.push(((b + 2 * n - 2) * (b + 2 * n - 1) * j[n - 1] + (u + b + 2 * n - 1) * Math.pow(logx * 0.5, 2 * n)) / (4 * t * t));
      var ps = s;
      s += p[n] * j[n];
      if (Math.abs(ps - s) < this.EPS3) break;
    }
    var ms = m * s;
    return ms;
  }
  /**
   * BPSER
   * @param a
   * @param b
   * @param logit
   */
  private static ibeta_bpser(a: number, b: number, logit: Logit)
  {
    var logx = logit.logpx;
    var x = logit.px;
    var t = 1.0;
    var s = 1 / a;
    for (var j = 1; j < 1000; ++j)
    {
      var ps = s;
      t *= x * (j - b) / j;
      s += t / (a + j);
      if (Math.abs(s - ps) < this.EPS3) break;
    }
    return s * Math.exp(logx * a - this.betaln(a, b));
  }
  /**
   * BUP + BFRAC
   * @param a
   * @param b
   * @param logit
   */
  private static ibeta_bup_bfrac(a: number, b: number, logit: Logit)
  {
    var px = logit.px;
    var nx = logit.nx;
    var logpx = logit.logpx;
    var lognx = logit.lognx;
    if (a <= 2)
    {
      var an = 3 - Math.ceil(a);
      return this.ibeta_bup_bfrac(a + an, b, logit) + Math.exp(logpx * a + lognx * b - this.betaln(a, b) + this.ibeta_amod(a, b, an, logit)) / a;
    }
    if (b <= 2)
    {
      var bn = 3 - Math.ceil(b);
      return this.ibeta_bup_bfrac(a, b + bn, logit) - Math.exp(logpx * a + lognx * b - this.betaln(a, b) + this.ibeta_bmod(a, b, bn, logit)) / b;
    }
    return this.ibeta_bfrac(a, b, logit);
  }
  /**
   * BFRAC
   * @param a
   * @param b
   * @param logit
   */
  private static ibeta_bfrac(a: number, b: number, logit: Logit)
  {
    var px = logit.px;
    var nx = logit.nx;
    var logpx = logit.logpx;
    var lognx = logit.lognx;
    var sq = (x: number) => x * x;
    var afunc = (m: number) => (a + (m - 1)) * (a + b + (m - 1)) * m * (b - m) * sq(px / (a + (m + m - 1)));
    var bfunc = (m: number) => m + m * (b - m) * px / (a + (m + m - 1)) + (a + m) * (a - (a + b) * px + 1 + m * (2 - px)) / (a + (m + m + 1));
    var a0 = bfunc(0);
    var a1 = afunc(1);
    var b1 = bfunc(1);
    var u = b1 + a1 / a0;
    var v = b1;
    var d = a0 + a1 / b1;
    for (var n = 2; n <= 1000; ++n)
    {
      var alpha = afunc(n);
      var beta = bfunc(n);
      var pu = u;
      var pv = v;
      var pd = d;
      u = beta + alpha / pu;
      v = beta + alpha / pv;
      d *= u / v;
      if (!(Math.abs(d - pd) > this.EPS3)) break;
    }
    return Math.exp(logpx * a + lognx * b - this.betaln(a, b)) / d;
  }
  private static ibeta_basym(a: number, b: number, logit: Logit)
  {
    const sqrt1_8 = 0.35355339059327376220042218105242; // sqrt(0.125)
    const sqrt2 = 1.4142135623730950488016887242097; // sqrt(2)
    const half_sqrt_pi_inv = 1.1283791670955125738961589031215; // 2 / sqrt(PI)
    var p = a / (a + b);
    var q = b / (a + b);
    var logp = Math.log(p);
    var logq = Math.log(q);
    var phix = -a * (logit.logpx - logp) - b * (logit.lognx - logq);
    var z = Math.sqrt(phix);
    var betagamma = Math.sqrt(a <= b ? (q / a) : (p / b));
    var bg = 1.0;
    var abrate = a <= b ? a / b : b / a;
    var ab = 1.0;
    var al = [];
    var cl = [];
    var el = [ 1 ];
    var ll = [ Math.PI / 8 * this.erfcx(z), sqrt1_8 ];
    var s = 0.0;
    var dfn = (t: number) => this.gammaln(t) - (t - 0.5) * Math.log(t) + t - this.LnSqrt2PI;
    for (var n = 0; n <= 100; ++n)
    {
      ab *= abrate;
      var pm = (n % 2) == 0 ? 1 : -1;
      al.push(2 / (n + 2.0) * (a <= b ? q * (1 + pm * ab) : p * (pm + ab)));
      var bfn = (_n: number, r: number) =>
      {
        if (_n < 1) return 1;
        var bl = [ 1, r * al[1] ];
        for (var j = 2; j <= _n; ++j)
        {
          var bv = 0.0;
          for (var i = 1; i < j; ++i)
          {
            bv += ((j - i) * r - i) * bl[i] * al[j - i];
          }
          bl.push(r * al[j] + bv / j);
        }
        return bl[_n];
      }
      cl.push(bfn(n, -0.5 * (n + 1)) / (n + 1));
      if (n > 0)
      {
        var ev = 0.0;
        for (var i = 0; i < n; ++i)
        {
          ev -= el[i] * cl[n - i];
        }
        el.push(ev);
      }
      if (n > 1)
          ll.push(sqrt1_8 * Math.pow(sqrt2 * z, n - 1) + (n - 1) * ll[n - 2]);
      var ps = s;
      s += el[n] * ll[n] * bg;
      if (Math.abs(s - ps) < Math.abs(s * this.ME100))
          break;
      bg *= betagamma;
    }
    return half_sqrt_pi_inv * Math.exp(dfn(a + b) - dfn(a) - dfn(b) - z * z) * Math.sqrt(2 * (a + b) / (a * b)) * s;
  }
  /**
   * ベータ逆累積分布関数
   * @param a
   * @param b
   * @param p
   */
  static invbeta(a: number, b: number, p: number): number
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
      var dpl = Math.max(p - minp, 0);
      var dph = Math.max(maxp - p, 0);
      pivx = minx + Math.max(Math.min(dpl / (dpl + dph), 0.9375), 0.0625) * (maxx - minx);
      if (Math.max(0, Math.min(Math.abs(pivx - minx), Math.abs(maxx - pivx))) <= this.EPS3) pivx = (minx + maxx) * 0.5;
      if (Math.min(Math.abs(pivx - minx), Math.abs(maxx - pivx)) <= this.EPS3) break;
      var pivp = this.ibeta(a, b, pivx);
      if (Math.abs(p - pivp) <= this.EPS3) break;
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
  /**
   * ベータ逆累積分布関数
   * @param a
   * @param b
   * @param p
   */
  static invbetalogit(a: number, b: number, p: number): number
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
      var dpl = Math.max(p - minp, 0);
      var dph = Math.max(maxp - p, 0);
      pivx = minx + Math.max(Math.min(dpl / (dpl + dph), 0.9375), 0.0625) * (maxx - minx);
      if (Math.max(0, Math.min(Math.abs(pivx - minx), Math.abs(maxx - pivx))) <= this.EPS3) pivx = (minx + maxx) * 0.5;
      if (Math.min(Math.abs(pivx - minx), Math.abs(maxx - pivx)) <= this.EPS3) break;
      var pivp = this.ibetalogit(a, b, pivx);
      if (Math.abs(p - pivp) <= this.EPS3) break;
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
/**
 * ロジット値
 */
class Logit
{
  readonly v: number;
  readonly px: number;
  readonly nx: number;
  readonly logpx: number;
  readonly lognx: number;
  private constructor(logit: number, px: number, nx: number, logpx: number, lognx: number)
  {
    this.v = logit;
    this.px = px;
    this.nx = nx;
    this.logpx = logpx;
    this.lognx = lognx;
  }
  static fromlogit(logit: number): Logit
  {
    return new Logit(
      logit,
      SpecialFunc.delogit(logit),
      SpecialFunc.delogit(-logit),
      SpecialFunc.logdelogit(logit),
      SpecialFunc.logdelogit(-logit),
    );
  }
  static frompx(px: number): Logit
  {
    return new Logit(
      SpecialFunc.enlogit(px),
      px,
      1 - px,
      Math.log(px),
      Math.log1p(-px),
    );
  }
  static fromnx(nx: number): Logit
  {
    return new Logit(
      -SpecialFunc.enlogit(nx),
      1 - nx,
      nx,
      Math.log1p(-nx),
      Math.log(nx),
    );
  }
  negate(): Logit
  {
    return new Logit(
      -this.v,
      this.nx,
      this.px,
      this.lognx,
      this.logpx,
    );
  }
}
interface RateInterval
{
  lowelo: number;
  lowwin: number;
  lowloss: number;
  highelo: number;
  highwin: number;
  highloss: number;
}
class EloRating
{
  private constructor() { /* noop */ }
  static readonly EPS3 = SpecialFunc.EPS3;
  static readonly EloLogitMul = SpecialFunc.LN10 / 400;
  static readonly EloLogitIMul = 400 / SpecialFunc.LN10;
  static winrate2elo(winrate: number): number
  {
    return this.EloLogitIMul * SpecialFunc.enlogit(winrate);
  }
  static elo2winrate(elo: number): number
  {
    return SpecialFunc.delogit(this.EloLogitMul * elo);
  }
  static ibetaelo(a: number, b: number, elo: number): number
  {
    return SpecialFunc.ibetalogit(a, b, this.EloLogitMul * elo);
  }
  static invbetaelo(a: number, b: number, p: number): number
  {
    return this.EloLogitIMul * SpecialFunc.invbetalogit(a, b, p);
  }
  static winlosselo(win: number, loss: number, p: number): RateInterval
  {
    var lowelo = this.invbetaelo(win, loss + 1, p);
    var highelo = this.invbetaelo(win + 1, loss, 1 - p);
    return {
      lowelo: lowelo,
      lowwin: this.elo2winrate(lowelo),
      lowloss: this.elo2winrate(-lowelo),
      highelo: highelo,
      highwin: this.elo2winrate(highelo),
      highloss: this.elo2winrate(-highelo),
    };
  }
}
class SpecialFuncTest
{
  private constructor() { /* noop */ }
  static gammatest(): void
  {
    var elem = document.createElement("pre");
    elem.appendChild(document.createTextNode([
      `GammaLn(0.5) = ${SpecialFunc.gammaln(0.5)}`,
      `GammaLn(1.0) = ${SpecialFunc.gammaln(1.0)}`,
      `GammaLn(1.5) = ${SpecialFunc.gammaln(1.5)}`,
      `GammaLn(2.0) = ${SpecialFunc.gammaln(2.0)}`,
      `GammaLn(2.5) = ${SpecialFunc.gammaln(2.5)}`,
      `GammaLn(3.0) = ${SpecialFunc.gammaln(3.0)}`,
      `GammaLn(3.5) = ${SpecialFunc.gammaln(3.5)}`,
      `GammaLn(4.0) = ${SpecialFunc.gammaln(4.0)}`,
      `Gamma(0.5) = ${SpecialFunc.gamma(0.5)}`,
      `Gamma(1.0) = ${SpecialFunc.gamma(1.0)}`,
      `Gamma(1.5) = ${SpecialFunc.gamma(1.5)}`,
      `Gamma(2.0) = ${SpecialFunc.gamma(2.0)}`,
      `Gamma(2.5) = ${SpecialFunc.gamma(2.5)}`,
      `Gamma(3.0) = ${SpecialFunc.gamma(3.0)}`,
      `Gamma(3.5) = ${SpecialFunc.gamma(3.5)}`,
      `Gamma(4.0) = ${SpecialFunc.gamma(4.0)}`,
      `Gamma(5.0) = ${SpecialFunc.gamma(5.0)}`,
      `Gamma(6.0) = ${SpecialFunc.gamma(6.0)}`,
      `Gamma(7.0) = ${SpecialFunc.gamma(7.0)}`,
      `Gamma(8.0) = ${SpecialFunc.gamma(8.0)}`,
      `Gamma(9.0) = ${SpecialFunc.gamma(9.0)}`,
      `Gamma(10.0) = ${SpecialFunc.gamma(10.0)}`,
      `Gamma(11.0) = ${SpecialFunc.gamma(11.0)}`,
      `Gamma(12.0) = ${SpecialFunc.gamma(12.0)}`,
      `Gamma(13.0) = ${SpecialFunc.gamma(13.0)}`,
      `Gamma(14.0) = ${SpecialFunc.gamma(14.0)}`,
      `Gamma(15.0) = ${SpecialFunc.gamma(15.0)}`,
      `Gamma(16.0) = ${SpecialFunc.gamma(16.0)}`,
      `Gamma(17.0) = ${SpecialFunc.gamma(17.0)}`,
      `Gamma(18.0) = ${SpecialFunc.gamma(18.0)}`,
      `Gamma(19.0) = ${SpecialFunc.gamma(19.0)}`,
      `Gamma(20.0) = ${SpecialFunc.gamma(20.0)}`,
      `Gamma(21.0) = ${SpecialFunc.gamma(21.0)}`,
    ].join("\n")));
    (document.querySelector("#app") || document.body).appendChild(elem);
  }
  static elorationgtest(): void
  {
    // JSON Data: https://gist.github.com/mizar/bb49ac4905742bfdef84f96146873d49
    fetch("https://gist.githubusercontent.com/mizar/bb49ac4905742bfdef84f96146873d49/raw/eloratingtest.json")
    .then((response) => response.json())
    .then((jsondata) =>
    {
      var text = "[\n";
      for (var entry of jsondata)
      {
        var elo = EloRating.winlosselo(
          entry.w + entry.d * 0.5,
          entry.l + entry.d * 0.5,
          entry.r
        );
        text += `${JSON.stringify({
          w: entry.w + entry.d * 0.5,
          l: entry.l + entry.d * 0.5,
          sr: entry.sr,
          r: entry.r,
          low: elo.lowelo,
          tlow: entry.low,
          lowdiff: elo.lowelo - entry.low,
          high: elo.highelo,
          thigh: entry.high,
          highdiff: elo.highelo - entry.high,
        })},\n`;
      }
      text += "{}]";
      var elem = document.createElement("pre");
      elem.appendChild(document.createTextNode(text));
      (document.querySelector("#app") || document.body).appendChild(elem);
    });
  }
}
SpecialFuncTest.gammatest();
SpecialFuncTest.elorationgtest();
