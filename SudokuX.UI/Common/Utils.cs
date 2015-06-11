using System;
using System.Windows.Media;

namespace SudokuX.UI.Common
{
    public static class Utils
    {
        /// <summary>
        /// Given H,S,L,A values in range of 0-1,
        /// Returns a Color (RGB struct) with RGB components in the range of 0-255.
        /// </summary>
        /// <param name="hue">The hue (0.0 - 1.0).</param>
        /// <param name="sat">The saturation (0.0 - 1.0).</param>
        /// <param name="lum">The luminance (0.0 - 0.5 - 1.0).</param>
        /// <param name="alpha">The alpha (0.0 - 1.0).</param>
        /// <returns></returns>
        public static Color FromHsla(double hue, double sat, double lum, double alpha = 1.0)
        {
            if (alpha > 1.0)
                alpha = 1.0;

            if (hue >= 1.0 || hue < 0.0)
            {
                hue = hue - Math.Floor(hue);
            }
            var r = lum;
            var g = lum;
            var b = lum;
            var v = (lum <= 0.5) ? (lum * (1.0 + sat)) : (lum + sat - lum * sat);
            if (v > 0)
            {
                var m = lum + lum - v;
                var sv = (v - m) / v;
                hue *= 6.0;
                var sextant = (int)hue % 6;
                var fract = hue - sextant;
                var vsf = v * sv * fract;
                var mid1 = m + vsf;
                var mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Color.FromArgb(
                  Convert.ToByte(alpha * 255.0f),
                  Convert.ToByte(r * 255.0f),
                  Convert.ToByte(g * 255.0f),
                  Convert.ToByte(b * 255.0f));
        }

    }
}
