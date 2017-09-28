using Scangram.Common.DocumentDetection.Contracts;
using OpenCvSharp;

namespace Scangram.Common.DocumentDetection.Postprocessor
{
    class BlackWhiteImagePostProcessor : IImagePostProcessor
    {
        public void PostProcessImage(ref Mat image, Mat sourceImage)
        {
            image = image.CvtColor(ColorConversionCodes.BGR2GRAY);

            image = image.Threshold(132, 255, ThresholdTypes.Binary);

            // TODO Check, what works better
            // image = image.CvtColor(ColorConversionCodes.BGR2GRAY);

            // using (var detector = SimpleBlobDetector.Create())
            // {
            //     var keypoints = detector.Detect(hardImage);
            //     Cv2.DrawKeypoints(hardImage, keypoints, image, new Scalar(0, 0, 255), DrawMatchesFlags.DrawRichKeypoints);
            // }

            var contours = Cv2.FindContoursAsArray(image, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            foreach (var contour in contours)
            {
                if (contour.Length < 5)
                {
                    continue;
                }

                var ellipse = Cv2.FitEllipse(contour);
                Cv2.Ellipse(image, ellipse, new Scalar(0, 255, 0), 2);
            }
        }
    }
}