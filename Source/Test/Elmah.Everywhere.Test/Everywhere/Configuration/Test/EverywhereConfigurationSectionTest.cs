using Xunit;


namespace Elmah.Everywhere.Configuration.Test
{
    public class EverywhereConfigurationSectionTest
    {
        [Fact]
        public void SECTION_KEY_Test()
        {
            // Assert
            Assert.Equal("everywhere/settings", EverywhereConfigurationSection.SECTION_KEY);
        }

        [Fact]
        public void Section_Test()
        {
            EverywhereConfigurationSection section = new EverywhereConfigurationSection();
            section.RemoteLogUri = "RemoteLogUri";
            section.Token = "Token";
            section.ApplicationName = "ApplicationName";
            section.Host = "Host";

            // Assert
            Assert.Equal("RemoteLogUri", section.RemoteLogUri);
            Assert.Equal("Token", section.Token);
            Assert.Equal("ApplicationName", section.ApplicationName);
            Assert.Equal("Host", section.Host);
        }
    }
}
