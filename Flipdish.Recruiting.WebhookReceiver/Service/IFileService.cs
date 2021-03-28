namespace Flipdish.Recruiting.WebhookReceiver.Service
{
    /// <summary>
    /// FileService Contract
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Contract to convert contents of a file to a string
        /// </summary>
        /// <param name="directory">The directory where the file is located</param>
        /// <param name="fileName">The file name to convert</param>
        /// <returns><see cref="string"/>ified file contents</returns>
        string GetFileContents(string directory, string fileName);
    }
}
