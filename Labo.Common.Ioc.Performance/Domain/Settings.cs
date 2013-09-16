namespace Labo.Common.Ioc.Performance.Domain
{
    public class Settings : ISettings
    {
        private readonly IConfigurationManager m_ConfigurationManager;

        public Settings(IConfigurationManager configurationManager)
        {
            m_ConfigurationManager = configurationManager;
        }
    }
}