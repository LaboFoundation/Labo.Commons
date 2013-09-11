namespace Labo.Common.Ioc.Tests.Performance.Domain
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger m_Logger;

        public ErrorHandler(ILogger logger)
        {
            this.m_Logger = logger;
        }

        public ILogger Logger { get { return this.m_Logger; } }
    }
}