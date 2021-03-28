namespace Flipdish.Recruiting.WebhookReceiver.StrategyLiquidTemplates
{
    /// <summary>
    /// Strategy contract for Dotliquid templates
    /// </summary>
    public interface ILiquidTemplateStrategy
    {
        /// <summary>
        /// Get the associated liquid template as string
        /// </summary>
        /// <param name="template">Template to parse</param>
        /// <returns>constructed html liquid template</returns>
        string GetTemplate(string templateStr);

        /// <summary>
        /// The corresponding template file name
        /// </summary>
        string TemplateName { get; }

        /// <summary>
        /// The directory where the file is located
        /// </summary>
        string Directory { get; }
    }    
}
