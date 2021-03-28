using System.IO;

namespace Flipdish.Recruiting.WebhookReceiver.Service
{
    /// <summary>
    /// File Service
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// <see cref="IFileService.GetFileContents(string, string)"/>
        /// </summary>
        public string GetFileContents(string directory, string fileName)
        {
            var templateFilePath = Path.Combine(directory, fileName);
            return new StreamReader(templateFilePath).ReadToEnd();
        }
    }
}
