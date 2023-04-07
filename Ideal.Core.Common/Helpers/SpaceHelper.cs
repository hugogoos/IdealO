namespace Ideal.Core.Common.Helpers
{
    /// <summary>
    /// 区域计算帮助
    /// </summary>
    public class SpaceHelper
    {
        /// <summary>
        /// 是否在区域内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static bool IsPtInPoly(in Point2D point, in Point2D[] pts)
        {
            if (pts == null || pts.Length == 0)
            {
                return false;
            }

            var N = pts.Length;
            var boundOrVertex = true; // 如果点位于多边形的顶点或边上，也算做点在多边形内，直接返回true
            var intersectCount = 0;// cross points count of x
            var precision = 2e-10; // 浮点类型计算时候与0比较时候的容差
            Point2D p1, p2;// neighbour bound vertices
            var p = point; // 当前点
            p1 = pts[0];// left vertex
            for (var i = 1; i <= N; ++i)
            {// check all rays
                if (p.X == p1.X && p.Y == p1.Y)
                {
                    return boundOrVertex;// p is an vertex
                }
                p2 = pts[i % N];// right vertex
                if (p.X < Math.Min(p1.X, p2.X) || p.X > Math.Max(p1.X, p2.X))
                {
                    // ray is outside of our interests
                    p1 = p2;
                    continue;// next ray left point
                }
                if (p.X > Math.Min(p1.X, p2.X) && p.X < Math.Max(p1.X, p2.X))
                {
                    // ray is crossing over by the algorithm (common part of)
                    if (p.Y <= Math.Max(p1.Y, p2.Y))
                    {// x is before of ray
                        if (p1.X == p2.X && p.Y >= Math.Min(p1.Y, p2.Y))
                        {
                            // overlies on a horizontal ray
                            return boundOrVertex;
                        }
                        if (p1.Y == p2.Y)
                        {// ray is vertical
                            if (p1.Y == p.Y)
                            {// overlies on a vertical ray
                                return boundOrVertex;
                            }
                            else
                            {// before ray
                                ++intersectCount;
                            }
                        }
                        else
                        {// cross point on the left side
                            var xinters = ((p.X - p1.X) * (p2.Y - p1.Y)
                                    / (p2.X - p1.X)) + p1.Y;
                            // cross point of y
                            if (Math.Abs(p.Y - xinters) < precision)
                            {
                                // overlies on a ray
                                return boundOrVertex;
                            }
                            if (p.Y < xinters)
                            {// before ray
                                ++intersectCount;
                            }
                        }
                    }
                }
                else
                {// special case when ray is crossing through the vertex
                    if (p.X == p2.X && p.Y <= p2.Y)
                    {// p crossing over p2
                        var p3 = pts[(i + 1) % N]; // next vertex
                        if (p.X >= Math.Min(p1.X, p3.X)
                                && p.X <= Math.Max(p1.X, p3.X))
                        {
                            // p.x lies between p1.x & p3.x
                            ++intersectCount;
                        }
                        else
                        {
                            intersectCount += 2;
                        }
                    }
                }
                p1 = p2;// next ray left point
            }
            if (intersectCount % 2 == 0)
            {// 偶数在多边形外
                return false;
            }
            else
            { // 奇数在多边形内
                return true;
            }
        }

        /// <summary>
        /// 是否在区域内
        /// </summary>
        /// <param name="current">当前判断点</param>
        /// <param name="points">区域集合</param>
        /// <returns></returns>
        public static bool IsInAreaByCrossingNumber(in Point2D current, in Point2D[] points)
        {
            if (points == null || points.Length == 0)
            {
                return false;
            }

            var cn = 0;
            for (var i = 0; i < points.Length - 1; i++)
            {
                if ((points[i].Y <= current.Y && points[i + 1].Y > current.Y) || (points[i].Y > current.Y && points[i + 1].Y <= current.Y))
                {
                    var vt = (current.Y - points[i].Y) / (points[i + 1].Y - points[i].Y);
                    if (current.X < points[i].X + (vt * (points[i + 1].X - points[i].X)))
                    {
                        ++cn;
                    }
                }
            }

            return (cn & 1) == 1;
        }

        /// <summary>
        /// 是否在区域内
        /// </summary>
        /// <param name="current">当前判断点</param>
        /// <param name="points">区域集合</param>
        /// <returns></returns>
        public static bool IsInAreaByWindingNumber(in Point2D current, in Point2D[] points)
        {
            if (points == null || points.Length == 0)
            {
                return false;
            }

            var wn = 0;
            for (var i = 0; i < points.Length - 1; i++)
            {
                if (points[i].Y <= current.Y)
                {
                    if (points[i + 1].Y > current.Y)
                    {
                        if (IsLeft(current, points[i], points[i + 1]))
                        {
                            ++wn;
                        }
                    }
                }
                else
                {
                    if (points[i + 1].Y <= current.Y)
                    {
                        if (IsRight(current, points[i], points[i + 1]))
                        {
                            --wn;
                        }
                    }
                }
            }

            return wn > 0;
        }

        /// <summary>
        /// current 在线段左侧
        /// </summary>
        /// <param name="current">点坐标</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        /// <returns></returns>
        private static bool IsLeft(in Point2D current, in Point2D start, in Point2D end)
        {
            return JudgeDirection(current, start, end) > 0;
        }

        /// <summary>
        /// current 在线段右侧 
        /// </summary>
        /// <param name="current">点坐标</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        private static bool IsRight(in Point2D current, in Point2D start, in Point2D end)
        {
            return JudgeDirection(current, start, end) < 0;
        }

        /// <summary>
        /// 判断点与线段关系
        /// </summary>
        /// <param name="current">点坐标</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        /// <returns>0: current 在线段上 小于0: current 在线段右侧   大于0: current 在线段左侧 </returns>
        private static double JudgeDirection(in Point2D current, in Point2D start, in Point2D end)
        {
            return ((end.X - start.X) * (current.Y - start.Y)) - ((current.X - start.X) * (end.Y - start.Y));
        }

        /// <summary>
        /// 获取定位相对左下角为原点坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static (double X, double Y) TransforCoordinateLowerLeft(double x, double y, in MapInfo mapInfo)
        {
            var point_x = mapInfo.ImageCenterPixelX + mapInfo.OriginOffsetPixelX + (x / mapInfo.Scale);
            var point_y = mapInfo.ImageCenterPixelY - mapInfo.OriginOffsetPixelY + (y / mapInfo.Scale);
            return (point_x, point_y);
        }

        /// <summary>
        /// 计算两点距离
        /// </summary>
        /// <param name="start">start</param>
        /// <param name="end">p2</param>
        /// <returns></returns>
        public static double CalculateTwoPointsDistance(in Point2D start, in Point2D end)
        {
            var x = start.X - end.X;
            var y = start.Y - end.Y;
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        /// <summary>
        /// 计算两点速度
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double CalculateSpeed(double distance, long start, long end)
        {
            if (start == end)
            {
                return 0;
            }

            //1.75m/s-2m/s不等，平均跑动速度为走动速度的3-5倍
            //1- 2 m/s 
            //28 千米/时(公里/h)=7.777777784 米/秒(m/s)
            //最快的人，最快的人类速度记录者为尤塞恩·博尔特44.72千米每小时
            var time = (end - start) * 0.001; ;
            return distance / time;
        }

        /// <summary>
        /// 已知直角三角形两点坐标和一边长，求另一点坐标
        /// </summary>
        /// <param name="a">a点</param>
        /// <param name="b">b点</param>
        /// <param name="bc">bc长度</param>
        /// <param name="abRigth">位于ab方向右边</param>
        /// <returns></returns>
        public static (double X, double Y) CalculateCPoint(Point2D a, Point2D b, double bc, bool abRigth)
        {
            //结果：
            //x1 = bX + [L * (aY - bY)] / √[(aX - bX)² +(aY – bY)²]
            //y1 = bY - [L * (aX - bX)] / √[(aX - bX)² +(aY – bY)²]
            //和
            //x1 = bX - [L * (aY - bY)] / √[(aX - bX)² +(aY – bY)²]
            //y1 = bY + [L * (aX - bX)] / √[(aX - bX)² +(aY – bY)²]

            var acLength = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            if (abRigth)
            {
                var cX = b.X - (bc * (a.Y - b.Y) / acLength);
                var cY = b.Y + (bc * (a.X - b.X) / acLength);
                return (cX, cY);
            }
            else
            {
                var cX = b.X + (bc * (a.Y - b.Y) / acLength);
                var cY = b.Y - (bc * (a.X - b.X) / acLength);
                return (cX, cY);
            }
        }

        /// <summary>
        /// 计算中点坐标
        /// </summary>
        /// <param name="a">a点</param>
        /// <param name="b">b点</param>
        /// <returns></returns>
        public static (double X, double Y) CalculateMidPoint(Point2D a, Point2D b)
        {
            //公式：（(x1+x2)/2, (y1+y2)/2）

            var cX = (a.X + b.X) / 2;
            var cY = (a.Y + b.Y) / 2;
            return (cX, cY);
        }

        /// <summary>
        /// 区域坐标字符串转区域坐标集合
        /// </summary>
        /// <param name="source"></param>
        /// <param name="splitSapn1"></param>
        /// <param name="splitSapn2"></param>
        /// <returns></returns>
        public static Point2D[] ConvertToPoint2Ds(string source, string splitSapn1 = ",", string splitSapn2 = "|")
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Array.Empty<Point2D>();
            }

            var list = new List<Point2D>();
            var strSpan = source.AsSpan();
            var splitSapn = splitSapn1.AsSpan();
            var barSplitTapn = splitSapn2.AsSpan();

            while (true)
            {
                var n = strSpan.IndexOf(splitSapn);
                if (n == -1)
                {
                    if (!strSpan.IsWhiteSpace())
                    {
                        var pt = ConvertToPoint2D(strSpan, barSplitTapn);
                        if (pt.X != int.MaxValue && pt.Y != int.MaxValue)
                        {
                            list.Add(pt);
                        }
                    }

                    break;
                }

                var point = ConvertToPoint2D(strSpan[..n], barSplitTapn);
                if (point.X != int.MaxValue && point.Y != int.MaxValue)
                {
                    list.Add(point);
                }

                strSpan = strSpan[(n + splitSapn.Length)..];
            }

            return list.ToArray();
        }

        private static Point2D ConvertToPoint2D(ReadOnlySpan<char> sourceSapn, ReadOnlySpan<char> splitSapn)
        {
            if (sourceSapn.IsWhiteSpace())
            {
                return new Point2D(int.MaxValue, int.MaxValue);
            }

            try
            {
                var n = sourceSapn.IndexOf(splitSapn);
                if (n == -1)
                {
                    return new Point2D(int.MaxValue, int.MaxValue);
                }

                if (!double.TryParse(sourceSapn[..n], out var x))
                {
                    x = int.MaxValue;
                }

                if (!double.TryParse(sourceSapn[(n + splitSapn.Length)..], out var y))
                {
                    y = int.MaxValue;
                }

                return new Point2D(x, y);
            }
            catch (Exception)
            {
                return new Point2D(int.MaxValue, int.MaxValue);
            }
        }

        /// <summary>
        /// 多边形扩展或收缩
        /// </summary>
        /// <param name="points">多边形顶点集合</param>
        /// <param name="expand">扩展大小，为负则收缩</param>
        /// <returns>扩展或收缩后的多边形</returns>
        public static Point2D[] ExpandArea(Point2D[] points, double expand)
        {
            if (points == null || points.Length < 3)
            {
                return Array.Empty<Point2D>();
            }

            var vectorPoints = new Point2D[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var n = i == points.Length - 1 ? 0 : i + 1;
                var reduce = new Point2D(points[n].X - points[i].X, points[n].Y - points[i].Y);
                vectorPoints[i] = reduce;
            }

            var unitVectorPoints = new Point2D[vectorPoints.Length];
            for (var i = 0; i < vectorPoints.Length; i++)
            {
                var rideDouble = (vectorPoints[i].X * vectorPoints[i].X) + (vectorPoints[i].Y * vectorPoints[i].Y);
                var value = 1.0 / Math.Sqrt(rideDouble);
                var ride = new Point2D(vectorPoints[i].X * value, vectorPoints[i].Y * value);
                unitVectorPoints[i] = ride;
            }

            var newPoints = new Point2D[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var startIndex = i == 0 ? points.Length - 1 : i - 1;
                var endIndex = i;
                var sina = (unitVectorPoints[startIndex].X * unitVectorPoints[endIndex].Y) - (unitVectorPoints[startIndex].Y * unitVectorPoints[endIndex].X);
                var length = expand / sina;
                var vector = new Point2D(unitVectorPoints[endIndex].X - unitVectorPoints[startIndex].X, unitVectorPoints[endIndex].Y - unitVectorPoints[startIndex].Y);
                var ride = new Point2D(vector.X * length, vector.Y * length);
                var plus = new Point2D(points[i].X + ride.X, points[i].Y + ride.Y);
                newPoints[i] = plus;
            }

            return newPoints;
        }
    }

    /// <summary>
    /// 地图信息
    /// </summary>
    public class MapInfo
    {
        /// <summary>
        /// 描述:地图相对坐标原点偏移 x
        /// 默认值:
        /// </summary>
        public double OriginOffsetPixelX { get; set; }

        /// <summary>
        /// 描述:地图相对坐标原点偏移 y
        /// 默认值:
        /// </summary>
        public double OriginOffsetPixelY { get; set; }

        /// <summary>
        /// 描述:图纸中⼼原点 x
        /// 默认值:
        /// </summary>
        public double ImageCenterPixelX { get; set; }

        /// <summary>
        /// 描述:图纸中⼼原点 y
        /// 默认值:
        /// </summary>
        public double ImageCenterPixelY { get; set; }

        /// <summary>
        /// 描述:坐标比例尺
        /// 默认值:
        /// </summary>
        public double Scale { get; set; } = 0;
    }

    /// <summary>
    /// 二维坐标点
    /// </summary>
    public readonly struct Point2D
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// x轴
        /// </summary>
        public double X { get; }

        /// <summary>
        /// y轴
        /// </summary>
        public double Y { get; }
    }
}
