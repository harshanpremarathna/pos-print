using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.POSPrint.Print
{
    public class RawPrinter
    {
        #region Helper function

        private String FeedPaper(int pNumLines)
        {
            StringBuilder sequence = new StringBuilder();

            sequence.Append((char)27);

            sequence.Append((char)100);

            sequence.Append((char)pNumLines);

            return sequence.ToString();
        }

        private String CutPaper()
        {
            // 66 or 65

            StringBuilder sequence = new StringBuilder();

            sequence.Append((char)29);

            sequence.Append((char)86);

            sequence.Append((char)65);

            sequence.Append((char)0);

            return sequence.ToString();
        }

        private StringBuilder SetCenter(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)97);
            str.Append((char)1);
            return str;
        }

        private StringBuilder SetLeft(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)97);
            str.Append((char)0);
            return str;
        }

        private StringBuilder SetDoubleWidth(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)33);
            str.Append((char)32);
            return str;
        }

        private StringBuilder SetDoubleHeight(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)33);
            str.Append((char)16);
            return str;
        }

        private StringBuilder ClearPrintMode(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)33);
            str.Append((char)0);
            return str;
        }

        private StringBuilder SetEmphasized(StringBuilder str)
        {
            str.Append((char)27);
            str.Append((char)33);
            str.Append((char)8);
            return str;
        }

        #endregion

        public bool PrintText(string printername, string printText)
        {
            StringBuilder str = new StringBuilder();
            str.Append((char)27); str.Append((char)64); // ESC@

            str.Append("\u001D" + "!" + "\u0011");


            str.Append(printText);
            str.Append(((char)10).ToString());  // Line break

            str.Append(FeedPaper(3));
            str.Append(CutPaper());

            return RawPrinterHelper.SendStringToPrinter(printername, str.ToString());
        }

        public bool PrintBarcode(string printername, string printText)
        {
            StringBuilder str = new StringBuilder();
            str.Append((char)27); str.Append((char)64); // ESC@

            //str = SetDoubleWidth(str); str = SetDoubleHeight(str); str = SetCenter(str);    // Set double
            //str.Append(printText);
            str.Append(((char)10).ToString());
            //str = ClearPrintMode(str);

            byte[] SetBarcodeHeight = new byte[] { 0x1D, 0x68, 0x25 };
            // Set Barcode width
            byte[] SetBarcodeWidth = new byte[] { 0x1D, 0x77, 0x03 };
            // Begin barcode printing
            byte[] EAN13BarCodeStart = new byte[] { 0x1D, 0x6B, 67, 13 };
            str.Append(ASCIIEncoding.ASCII.GetString(SetBarcodeHeight));
            str.Append(ASCIIEncoding.ASCII.GetString(SetBarcodeWidth));
            str.Append(string.Format("{0}{1}", ASCIIEncoding.ASCII.GetString(EAN13BarCodeStart), printText));

            str.Append(FeedPaper(3));
            str.Append(CutPaper());


            // https://www.codeproject.com/Questions/89230/Print-Barcode-together-with-TEXT

            return RawPrinterHelper.SendStringToPrinter(printername, str.ToString());
        }
    }
}
