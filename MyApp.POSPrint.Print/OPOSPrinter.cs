using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace MyApp.POSPrint.Print
{
    public class OPOSPrinter
    {
        PosExplorer posExplorer;

        public OPOSPrinter()
        {
            posExplorer = new PosExplorer();
        }

        public string GetPOSList()
        {
            string div = "";
            DeviceCollection devices = posExplorer.GetDevices(DeviceType.PosPrinter);
            foreach (DeviceInfo d in devices)
            {
                div += d.ServiceObjectName;
                div += "\n";
            }

            return div;
        }

        public PosPrinter GetPOSPrinterInstance(string oposName)
        {
            DeviceCollection devices = posExplorer.GetDevices(DeviceType.PosPrinter);
            DeviceInfo device = null;
            foreach (DeviceInfo d in devices)
            {
                if (d.ServiceObjectName == oposName)
                {
                    device = d;
                    break;
                }
            }

            var posCommon = (PosCommon)posExplorer.CreateInstance(device);

            posCommon.Open();
            posCommon.Claim(1000);
            posCommon.DeviceEnabled = true;

            PosPrinter posPrinter = posCommon as PosPrinter;
            posPrinter.AsyncMode = false;

            return posPrinter;
        }

        public void PrintText(PosPrinter posPrinter, PrinterStation CurrentStation, string printText)
        {
            StringBuilder str = new StringBuilder();
            str.Append("ESC@");

            str.Append("ESC|2CESC|cA"); // Double-wide and center
            str.Append(printText);
            str.Append(((char)10).ToString());  // Line break

            string finalText = str.ToString().Replace("ESC", ((char)27).ToString()) + "\x1B|1lF";

            finalText +=
                ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString();

            posPrinter.PrintNormal(CurrentStation, finalText);

            posPrinter.CutPaper(95);

            posPrinter.Close();
        }

        public void PrintBarcode(PosPrinter posPrinter, PrinterStation CurrentStation, string printText)
        {
            posPrinter.PrintBarCode(PrinterStation.Receipt, printText,
                       BarCodeSymbology.Code128, posPrinter.RecLineHeight,
                       posPrinter.RecLineWidth, PosPrinter.PrinterBarCodeLeft,
                       BarCodeTextPosition.Below);

            string finalText1 =
                ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString() + ((char)10).ToString();

            posPrinter.PrintNormal(CurrentStation, finalText1);

            posPrinter.CutPaper(95);

            posPrinter.Close();
        }

    }
}
