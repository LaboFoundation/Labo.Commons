namespace Labo.Common.Ioc.Performance.Domain
{
    public class Application : IApplication
    {
        private readonly IController m_Controller;

        public Application(IController controller)
        {
            m_Controller = controller;
        }
    }
}