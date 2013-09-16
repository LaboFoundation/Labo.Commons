namespace Labo.Common.Ioc.Performance.Domain
{
    using System;

    public class Settings : ISettings
    {
        private readonly IConfigurationManager m_ConfigurationManager;

        private readonly DateTime m_DateTime;

        private readonly DateTime m_DateTime1;

        public Settings(IConfigurationManager configurationManager, DateTime dateTime, DateTime dateTime1 = default(DateTime))
        {
            m_ConfigurationManager = configurationManager;
            m_DateTime = dateTime;
            m_DateTime1 = dateTime1;
        }
    }
}