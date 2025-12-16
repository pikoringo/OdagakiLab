using DlibDotNet;
using Intel.RealSense;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MainSystem
{
    public class RealSenseCamera : IDisposable
    {
        private Pipeline pipeline;
        private Intrinsics colorIntrinsics;
        private bool streaming = false;
        private ShapePredictor shapePredictor;
        private FrontalFaceDetector faceDetector;
        private bool c3Enabled = false;

        // Event to send computed target to main form
        public event EventHandler<TargetEventArgs> TargetComputed;

        // Event to send frames to the form
        public event Action<Bitmap> FrameReady;
        public Mat LatestColorFrame { get; private set; }
        public DepthFrame LatestDepthFrame { get; private set; }
        public Intrinsics ColorIntrinsics { get; private set; }
        public Intrinsics DepthIntrinsics { get; private set; }
        private bool intrinsicsInitialized = false;



        public void Start(string shapePredictorPath = null)
        {
            if (streaming) return;

            if (!string.IsNullOrEmpty(shapePredictorPath))
            {
                if (!System.IO.File.Exists(shapePredictorPath))
                    throw new System.IO.FileNotFoundException("Shape predictor not found", shapePredictorPath);

                faceDetector = Dlib.GetFrontalFaceDetector();
                shapePredictor = ShapePredictor.Deserialize(shapePredictorPath);
            }

            pipeline = new Pipeline();
            var config = new Config();
            config.EnableStream(Intel.RealSense.Stream.Color, 640, 480, Format.Bgr8, 30);
            config.EnableStream(Intel.RealSense.Stream.Depth, 640, 480, Format.Z16, 30);

            PipelineProfile profile = pipeline.Start(config);
            colorIntrinsics = profile.GetStream(Intel.RealSense.Stream.Color).As<VideoStreamProfile>().GetIntrinsics(); // <-- store here

            streaming = true;
            Task.Run(() => StreamLoop());
        }

        private void StreamLoop()
        {
            var colorProfile = pipeline.ActiveProfile.GetStream(Intel.RealSense.Stream.Color).As<VideoStreamProfile>();
            var colorIntrinsics = colorProfile.GetIntrinsics();

            while (streaming)
            {
                using (var frames = pipeline.WaitForFrames())
                {
                    if (frames == null) { Console.WriteLine("Frames null"); continue; }
                    
                    using (var colorFrame = frames.ColorFrame)
                    using (var depthFrame = frames.DepthFrame)
                    {
                        if (colorFrame == null)
                            continue;

                        // Update intrinsics once
                        if (!intrinsicsInitialized)
                        {
                            var colorStreamProfile = colorFrame.Profile.As<VideoStreamProfile>();
                            var depthStreamProfile = depthFrame.Profile.As<VideoStreamProfile>();
                            ColorIntrinsics = colorStreamProfile.GetIntrinsics();
                            DepthIntrinsics = depthStreamProfile.GetIntrinsics();
                            intrinsicsInitialized = true;
                        }

                        // Save the latest frames
                        LatestColorFrame = new Mat(colorFrame.Height, colorFrame.Width, MatType.CV_8UC3);
                        colorFrame.CopyTo(LatestColorFrame.Data);
                        LatestDepthFrame = depthFrame;

                        using (var colorMat = new Mat(colorFrame.Height, colorFrame.Width, MatType.CV_8UC3))
                        {
                            colorFrame.CopyTo(colorMat.Data);
                            Cv2.Flip(colorMat, colorMat, FlipMode.Y);

                            if (c3Enabled && shapePredictor != null && faceDetector != null)
                            {
                                //Console.WriteLine("LocateC3 flag is true, running C3 detection");
                                using (var bitmap = BitmapConverter.ToBitmap(colorMat))
                                {
                                    string tempFile = Path.GetTempFileName() + ".png";
                                    bitmap.Save(tempFile);

                                    using (var dlibImg = Dlib.LoadImage<RgbPixel>(tempFile))
                                    {
                                        var faces = faceDetector.Operator(dlibImg);

                                        foreach (var face in faces)
                                        {
                                            var shape = shapePredictor.Detect(dlibImg, face);

                                            // Get nasion and ears
                                            var nasion = shape.GetPart(27);
                                            var leftEar = shape.GetPart(1);
                                            var rightEar = shape.GetPart(15);

                                            // Draw directly on colorMat
                                            Cv2.Circle(colorMat, new OpenCvSharp.Point(nasion.X, nasion.Y), 5, Scalar.Red, -1);
                                            Cv2.Circle(colorMat, new OpenCvSharp.Point(leftEar.X, leftEar.Y), 5, Scalar.Blue, -1);
                                            Cv2.Circle(colorMat, new OpenCvSharp.Point(rightEar.X, rightEar.Y), 5, Scalar.Blue, -1);

                                            // Compute target midpoint / angle / XYZ like your console code
                                            var midpoint = new OpenCvSharp.Point(
                                                (leftEar.X + rightEar.X) / 2,
                                                (leftEar.Y + rightEar.Y) / 2
                                            );

                                            float depthValue = LatestDepthFrame.GetDistance(nasion.X, nasion.Y);
                                            float[] nasion3D = DeprojectPixelToPoint(DepthIntrinsics, new float[] { nasion.X, nasion.Y }, depthValue);

                                            double earAngle = Math.Atan((rightEar.Y - leftEar.Y) / (double)(rightEar.X - leftEar.X));
                                            double targetB = 150 * Math.Cos(earAngle);
                                            double targetAngle = 36 - (earAngle * 180 / Math.PI);
                                            double inRadians = targetAngle * Math.PI / 180;
                                            double targetX = midpoint.X - (targetB * Math.Tan(inRadians));
                                            double targetY = midpoint.Y - targetB;

                                            float TdepthValue = LatestDepthFrame.GetDistance((int)targetX, (int)targetY);
                                            float[] target3D = DeprojectPixelToPoint(DepthIntrinsics, new float[] { (float)targetX, (float)targetY }, TdepthValue);

                                            double adjustedX = Math.Round(Map(target3D[0], -2.5, 3, 200, 800), 0);
                                            double adjustedY = Math.Round(Map(target3D[2], 0, 0, 0, 0), 0); // optional
                                            double adjustedZ = Math.Round(Map(target3D[1], 3.0, -3.0, -70, 670), 0);

                                            Cv2.Circle(colorMat, new OpenCvSharp.Point(targetX, targetY), 5, Scalar.Blue, -1);

                                            TargetComputed?.Invoke(this, new TargetEventArgs(adjustedX, adjustedY, adjustedZ, targetAngle, (int)targetX, (int)targetY));
                                            //Console.WriteLine(adjustedX);
                                        }
                                    }
                                }
                            }


                            // Draw example grid (like your previous code)
                            float xMin = -2.5f, xMax = 2.5f;
                            float yMin = -2.0f, yMax = 2.0f;
                            float gridStep = 0.1f;
                            float fixedZ = 1.0f;
                            float scaleFactor = 5.0f;

                            // Vertical grid lines (X axis)
                            for (float xVal = xMin; xVal <= xMax; xVal += gridStep)
                            {
                                float[] point = { xVal, 0f, fixedZ };
                                var pixel = ProjectPointToPixel(colorIntrinsics, point);
                                int px = (int)Math.Round((double)pixel[0]);
                                int py = (int)Math.Round((double)pixel[1]);

                                if (px >= 0 && px < colorMat.Width && py >= 0 && py < colorMat.Height)
                                {
                                    Cv2.Circle(colorMat, new OpenCvSharp.Point(px, py), 3, Scalar.Green, -1);
                                    Cv2.PutText(colorMat, (xVal * scaleFactor).ToString("0.0"),
                                        new OpenCvSharp.Point(px - 20, py - 10),
                                        HersheyFonts.HersheySimplex, 0.4, Scalar.Green, 1);
                                }
                            }

                            // Horizontal grid lines (Y axis)
                            for (float yVal = yMin; yVal <= yMax; yVal += gridStep)
                            {
                                float[] point = { 0f, yVal, fixedZ };
                                var pixel = ProjectPointToPixel(colorIntrinsics, point);
                                int px = (int)Math.Round((double)pixel[0]);
                                int py = (int)Math.Round((double)pixel[1]);

                                if (px >= 0 && px < colorMat.Width && py >= 0 && py < colorMat.Height)
                                {
                                    Cv2.Circle(colorMat, new OpenCvSharp.Point(px, py), 3, Scalar.Blue, -1);
                                    Cv2.PutText(colorMat, (yVal * scaleFactor).ToString("0.0"),
                                        new OpenCvSharp.Point(px + 5, py + 15),
                                        HersheyFonts.HersheySimplex, 0.4, Scalar.Blue, 1);
                                }
                            }

                            // Draw origin
                            float[] origin = { 0f, 0f, fixedZ };
                            var originPixel = ProjectPointToPixel(colorIntrinsics, origin);
                            int ox = (int)Math.Round((double)originPixel[0]);
                            int oy = (int)Math.Round((double)originPixel[1]);
                            Cv2.Line(colorMat, new OpenCvSharp.Point(0, oy), new OpenCvSharp.Point(colorMat.Width - 1, oy), Scalar.Green, 1);
                            Cv2.Line(colorMat, new OpenCvSharp.Point(ox, 0), new OpenCvSharp.Point(ox, colorMat.Height - 1), Scalar.Blue, 1);
                            Cv2.PutText(colorMat, "(0,0)", new OpenCvSharp.Point(ox + 5, oy - 5), HersheyFonts.HersheySimplex, 0.5, Scalar.Yellow, 1);

                            // Convert Mat to Bitmap and fire event
                            using (var bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(colorMat))
                            {
                                FrameReady?.Invoke((Bitmap)bmp.Clone());
                            }
                        }
                    }
                }
            }
        }
        private float[] ProjectPointToPixel(Intrinsics intr, float[] point)
        {
            float[] pixel = new float[2];
            if (point[2] > 0)
            {
                pixel[0] = point[0] * intr.fx / point[2] + intr.ppx;
                pixel[1] = point[1] * intr.fy / point[2] + intr.ppy;
            }
            else
            {
                pixel[0] = pixel[1] = float.NaN;
            }
            return pixel;
        }

        public void Stop()
        {
            if (!streaming) return;
            streaming = false;
            pipeline?.Stop();
            pipeline?.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
        public void EnableC3(bool enable = true)
        {
            c3Enabled = enable;
            
        }
        private float[] DeprojectPixelToPoint(Intrinsics intrinsics, float[] pixel, float depth)
        {
            float[] point = new float[3];
            if (depth > 0)
            {
                point[0] = (pixel[0] - intrinsics.ppx) * depth / intrinsics.fx;
                point[1] = (pixel[1] - intrinsics.ppy) * depth / intrinsics.fy;
                point[2] = depth;
            }
            else
            {
                point[0] = point[1] = point[2] = float.NaN;
            }
            return point;
        }

        private double Map(double value, double sourceMin, double sourceMax, double targetMin, double targetMax)
        {
            return (targetMax - targetMin) * (value - sourceMin) / (sourceMax - sourceMin) + targetMin;
        }

    }
    public class TargetEventArgs : EventArgs
    {
        public double AdjustedX { get; }
        public double AdjustedY { get; }
        public double AdjustedZ { get; }
        public double AdjustedAngle { get; }
        public int PixelX { get; }
        public int PixelY { get; }

        public TargetEventArgs(double x, double y, double z, double angle, int px, int py)
        {
            AdjustedX = x;
            AdjustedY = y;
            AdjustedZ = z;
            AdjustedAngle = angle;
            PixelX = px;
            PixelY = py;
        }
    }
}
