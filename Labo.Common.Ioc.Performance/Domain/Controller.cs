namespace Labo.Common.Ioc.Performance.Domain
{
    public class Controller : IController
    {
        private readonly IErrorHandler m_ErrorHandler;

        public Controller(IErrorHandler errorHandler)
        {
            m_ErrorHandler = errorHandler;
        }
    }
}