namespace Labo.Common.Ioc.Performance.Domain
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger m_Logger;

        public ErrorHandler(ILogger logger)
        {
            m_Logger = logger;
        }

        public ILogger Logger { get { return m_Logger; } }
    }
}