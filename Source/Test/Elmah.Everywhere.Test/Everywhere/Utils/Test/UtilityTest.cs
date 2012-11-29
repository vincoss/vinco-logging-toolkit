using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Collections.Specialized;


namespace Elmah.Everywhere.Utils.Test
{
    public class UtilityTest
    {
        [Fact]
        public void SerializeXml_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         Utility.SerializeXml(null);
                                                     });
        }

        [Fact]
        public void DeserializeXml_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         Utility.DeserializeXml(null);
                                                     });
        }

        [Fact]
        public void Can_SerializeXml_And_Deserialize_Test()
        {
            // Arrange
            ErrorInfo expected = GetInfo();

            string xml = Utility.SerializeXml(expected);

            // Act
            ErrorInfo actual = Utility.DeserializeXml(xml);

            // Assert
            Assert.Equal(expected.ApplicationName, actual.ApplicationName);
            Assert.Equal(expected.Host, actual.Host);
            Assert.Equal(expected.ErrorType, actual.ErrorType);
            Assert.Equal(expected.Source, actual.Source);
            Assert.Equal(expected.Message, actual.Message);
            Assert.Equal(expected.Detail, actual.Detail);
            Assert.Equal(expected.Date.ToString(), actual.Date.ToString());
            Assert.Equal(expected.User, actual.User);
            Assert.Equal(expected.StatusCode, actual.StatusCode);

            CompareCollections(expected.ErrorDetails.Single(x => x.Name == "Cookies").Items, actual.ErrorDetails.Single(x => x.Name == "Cookies").Items);
            CompareCollections(expected.ErrorDetails.Single(x => x.Name == "Form").Items, actual.ErrorDetails.Single(x => x.Name == "Form").Items);
            CompareCollections(expected.ErrorDetails.Single(x => x.Name == "QueryString").Items, actual.ErrorDetails.Single(x => x.Name == "QueryString").Items);
            CompareCollections(expected.ErrorDetails.Single(x => x.Name == "ServerVariables").Items, actual.ErrorDetails.Single(x => x.Name == "ServerVariables").Items);
        }

        [Fact]
        public void GetExceptionString_Returns_String_Empty_If_Exception_Is_Null_Test()
        {
            // Assert
            Assert.Equal("", Utility.GetExceptionString(null));
        }

        private static void CompareCollections(IEnumerable<KeyValuePair<string, string>> first, IEnumerable<KeyValuePair<string, string>> second)
        {
            foreach (var pair in second)
            {
                string expected = first.Single(x => x.Key == pair.Key).Value;

                Assert.Equal(expected, pair.Value);
            }
        }

        private static ErrorInfo GetInfo()
        {
            var info = new ErrorInfo(new Exception("Test Exception"))
            {
                ApplicationName = "ApplicationName",
                Host = "Host",
                ErrorType = "ErrorType",
                Source = "Source",
                Message = "Message",
                Detail = "Detail",
                Date = DateTime.Now,
                User = "User",
                StatusCode = 1000
            };

            info.AddDetail("Cookies", "Key", "Value");
            info.AddDetail("Form", "Key", "Value");
            info.AddDetail("QueryString", "Key", "Value");
            info.AddDetail("ServerVariables", "Key", "Value");

            return info;
        }
    }
}
