namespace Labo.Common.Ioc.Performance.Domain
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger m_Logger;
        private readonly ISettings m_Settings;

        public ErrorHandler(ILogger logger, ISettings settings)
        {
            m_Logger = logger;
            m_Settings = settings;
        }

        public ILogger Logger { get { return m_Logger; } }
    }
}