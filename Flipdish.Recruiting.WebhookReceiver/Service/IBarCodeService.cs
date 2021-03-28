using System.IO;

namespace Flipdish.Recruiting.WebhookReceiver.Service
{
	/// <summary>
	/// Contract for NetBarcode services
	/// </summary>
	public interface IBarCodeService
	{
		/// <summary>
		/// Retrieve base64 ean barcode
		/// </summary>
		/// <param name="barcodeNumbers">Barcode Data/></param>
		/// <returns></returns>
		Stream GetBase64EAN13Barcode(string barcodeNumbers);

	}
}
