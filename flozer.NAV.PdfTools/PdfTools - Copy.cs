using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class PdfMerger
{
    public static void MergePdfDocuments(string documentPath, string backgroundPath)
    {
        // Open the main document.
        using (PdfReader document = new PdfReader(documentPath))
        {
            // Create a PdfStamper to merge pages into the main document.
            using (PdfWriter writer = PdfWriter.GetInstance(
                    new Document(),
                    new FileStream("merged.pdf", FileMode.Create)))
            {
                PdfStamper stamper = new PdfStamper(writer, document);

                // Open the background PDF for reading.
                using (PdfReader bgReader = new PdfReader(backgroundPath))
                {
                    // Copy all pages from the background PDF into the main document.
                    for (int page = 1; page <= bgReader.NumberOfPages; page++)
                    {
                        Rectangle pageSize = bgReader.GetPageSize(page);  // Background page size
                        Rectangle docPageSize = writer.PageInfo.pageSize;   // Main document page size

                        // Calculate scale to fit within bounds.
                        float scaleX = docPageSize.Width / pageSize.Width;
                        float scaleY = docPageSize.Height / pageSize.Height;

                        float finalScaleX = Math.Min(scaleX, scaleY);
                        float finalScaleY = Math.Min(scaleY, scaleX);

                        // Calculate position to center the background image on the page.
                        float x = (docPageSize.Width - finalScaleX * pageSize.Width) / 2;
                        float y = (docPageSize.Height - finalScaleY * pageSize.Height) / 2;

                        // Copy the background pages.
                        stamper.CopyPages(
                            bgReader,
                            new int[] { page },
                            new Rectangle(x, y, x + finalScaleX * pageSize.Width, y + finalScaleY * pageSize.Height),
                            null);
                    }
                }
            }

            Console.WriteLine("PDF merge completed successfully.");
        }
    }
}
