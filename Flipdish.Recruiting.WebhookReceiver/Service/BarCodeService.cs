using NetBarcode;
using System.IO;

namespace Flipdish.Recruiting.WebhookReceiver.Service
{
	/// <summary>
	/// Handler for <see cref="NetBarcode"/> operations
	/// </summary>
	public class BarCodeService : IBarCodeService
    {
        /// <summary>
        /// <see cref="IBarCodeService.GetBase64EAN13Barcode(string)"/>
        /// </summary>
        /// <param name="barcodeNumbers"></param>
        /// <returns></returns>
        public Stream GetBase64EAN13Barcode(string barcodeNumbers)
        {
            //TODO: Should be configurable, add appsettings equivalent
            //TODO: Add exception middleware
            var barcode = new Barcode(barcodeNumbers, showLabel: true, width: 130, height: 110, labelPosition: LabelPosition.BottomCenter);
            var bytes = barcode.GetByteArray();
            return new MemoryStream(bytes);
        }
    }
}
