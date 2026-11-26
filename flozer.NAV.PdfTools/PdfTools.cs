using System;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

namespace flozer.NAV.PdfTools
{
    public static class PdfTools
    {
        /// <summary>
        /// Adds a PDF background to the first page and another PDF background to all following pages.
        /// </summary>
        /// <param name="FileName1">Path and filename of the PDF file to modify.</param>
        /// <param name="Background1">Path and filename of the PDF background for the first page.</param>
        /// <param name="Background2">Path and filename of the PDF background for all following pages.
        /// If this parameter is empty, Background1 is used for all pages.</param>
        public static void AddBackground(string FileName1, string Background1, string Background2)
        {
            if (string.IsNullOrWhiteSpace(FileName1))
                throw new ArgumentException("The input PDF path must be specified.", nameof(FileName1));
            if (string.IsNullOrWhiteSpace(Background1))
                throw new ArgumentException("The first-page background PDF path must be specified.", nameof(Background1));

            if (string.IsNullOrWhiteSpace(Background2))
                Background2 = Background1;

            if (!File.Exists(FileName1))
                throw new FileNotFoundException("The input PDF file was not found.", FileName1);
            if (!File.Exists(Background1))
                throw new FileNotFoundException("The first-page background PDF file was not found.", Background1);
            if (!File.Exists(Background2))
                throw new FileNotFoundException("The following-pages background PDF file was not found.", Background2);

            string temporaryFileName = Path.Combine(
                Path.GetDirectoryName(Path.GetFullPath(FileName1)),
                Path.GetFileNameWithoutExtension(FileName1) + "." + Guid.NewGuid().ToString("N") + ".pdf");

            try
            {
                using (var document = PdfReader.Open(FileName1, PdfDocumentOpenMode.Modify))
                using (XPdfForm firstPageBackground = XPdfForm.FromFile(Background1))
                using (XPdfForm followingPagesBackground = string.Equals(
                    Background1,
                    Background2,
                    StringComparison.OrdinalIgnoreCase)
                    ? null
                    : XPdfForm.FromFile(Background2))
                {
                    for (int pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
                    {
                        var page = document.Pages[pageIndex];
                        XPdfForm background = pageIndex == 0 || followingPagesBackground == null
                            ? firstPageBackground
                            : followingPagesBackground;

                        using (XGraphics graphics = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend))
                        {
                            graphics.DrawImage(background, 0, 0, page.Width.Point, page.Height.Point);
                        }
                    }

                    document.Save(temporaryFileName);
                }

                File.Replace(temporaryFileName, Path.GetFullPath(FileName1), null);
            }
            finally
            {
                if (File.Exists(temporaryFileName))
                    File.Delete(temporaryFileName);
            }
        }

        /// <summary>
        /// Adds a centered, translucent diagonal text watermark to every page of a PDF.
        /// </summary>
        /// <param name="FileName1">Path and filename of the PDF file to modify.</param>
        /// <param name="WatermarkText">Text to add as a watermark.</param>
        public static void AddWatermark(string FileName1, string WatermarkText)
        {
            if (string.IsNullOrWhiteSpace(FileName1))
                throw new ArgumentException("The input PDF path must be specified.", nameof(FileName1));
            if (string.IsNullOrWhiteSpace(WatermarkText))
                throw new ArgumentException("Watermark text must be specified.", nameof(WatermarkText));
            if (!File.Exists(FileName1))
                throw new FileNotFoundException("The input PDF file was not found.", FileName1);


            string fullFileName = Path.GetFullPath(FileName1);
            string temporaryFileName = Path.Combine(
                Path.GetDirectoryName(fullFileName),
                Path.GetFileNameWithoutExtension(FileName1) + "." + Guid.NewGuid().ToString("N") + ".pdf");

            try
            {
                using (var document = PdfReader.Open(fullFileName, PdfDocumentOpenMode.Modify))
                {
                    //var font = new XFont("Arial", 48, XFontStyle.Bold);
                    //var font = new XFont("Caveat", 48, XFontStyle.Bold);
                    //var font = new XFont("Impact", 48, XFontStyle.Bold);
                    //var font = new XFont("Segoe Print", 48, XFontStyle.Bold);
                    var font = new XFont("Segoe Print", 72, XFontStyle.Bold);

                    //var brush = new XSolidBrush(XColor.FromArgb(40, 128, 128, 128));
                    //var brush = new XSolidBrush(XColor.FromArgb(40, 255, 0, 0)); //Red
                    var brush = new XSolidBrush(XColor.FromArgb(40, 210, 210, 210)); //Grey

                    foreach (var page in document.Pages)
                    {
                        using (XGraphics graphics = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend))
                        {
                            double width = page.Width.Point;
                            double height = page.Height.Point;
                            graphics.RotateAtTransform(-45, new XPoint(width / 2, height / 2));
                            graphics.DrawString(
                                WatermarkText,
                                font,
                                brush,
                                new XRect(0, 0, width, height),
                                XStringFormats.Center);
                        }
                    }

                    document.Save(temporaryFileName);
                }

                File.Replace(temporaryFileName, fullFileName, null);
            }
            finally
            {
                if (File.Exists(temporaryFileName))
                    File.Delete(temporaryFileName);
            }
        }

        /// <summary>
        /// Adds the first page of a PDF file as a footer to every page.
        /// The footer is scaled to fit and aligned to the bottom center of each page.
        /// </summary>
        /// <param name="FileName1">Path and filename of the PDF file to modify.</param>
        /// <param name="FooterFile">Path and filename of the PDF footer file.</param>
        public static void AddFooter(string FileName1, string FooterFile)
        {
            if (string.IsNullOrWhiteSpace(FileName1))
                throw new ArgumentException("The input PDF path must be specified.", nameof(FileName1));
            if (string.IsNullOrWhiteSpace(FooterFile))
                throw new ArgumentException("The footer PDF path must be specified.", nameof(FooterFile));
            if (!File.Exists(FileName1))
                throw new FileNotFoundException("The input PDF file was not found.", FileName1);
            if (!File.Exists(FooterFile))
                throw new FileNotFoundException("The footer PDF file was not found.", FooterFile);

            string fullFileName = Path.GetFullPath(FileName1);
            string temporaryFileName = Path.Combine(
                Path.GetDirectoryName(fullFileName),
                Path.GetFileNameWithoutExtension(FileName1) + "." + Guid.NewGuid().ToString("N") + ".pdf");

            try
            {
                using (var document = PdfReader.Open(fullFileName, PdfDocumentOpenMode.Modify))
                using (XPdfForm footer = XPdfForm.FromFile(FooterFile))
                {
                    foreach (var page in document.Pages)
                    {
                        using (XGraphics graphics = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append))
                        {
                            double scale = Math.Min(
                                page.Width.Point / footer.PointWidth,
                                page.Height.Point / footer.PointHeight);
                            double width = footer.PointWidth * scale;
                            double height = footer.PointHeight * scale;
                            double x = (page.Width.Point - width) / 2;
                            double y = page.Height.Point - height;

                            graphics.DrawImage(footer, x, y, width, height);
                        }
                    }

                    document.Save(temporaryFileName);
                }

                File.Replace(temporaryFileName, fullFileName, null);
            }
            finally
            {
                if (File.Exists(temporaryFileName))
                    File.Delete(temporaryFileName);
            }
        }
    }
}
