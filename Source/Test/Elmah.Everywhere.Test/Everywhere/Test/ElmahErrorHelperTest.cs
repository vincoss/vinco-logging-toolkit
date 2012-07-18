using System;
using System.Linq;
using Xunit;
using System.Collections.Specialized;
using System.Collections.Generic;


namespace Elmah.Everywhere.Test
{
    public class ElmahErrorHelperTest
    {
        [Fact]
        public void LogException_ErrorInfo_Test()
        {
            // Arrange
            TestableElmahErrorHelper helper = new TestableElmahErrorHelper();

            ErrorInfo info = new ErrorInfo();

            // Act
            helper.LogException(info);

            // Assert
            Assert.NotNull(helper.Error);
        }

        [Fact]
        public void ToError_Test()
        {
            // Arrange
            ErrorInfo info = GetInfo();

            // Act
            Error error = ElmahErrorHelper.ToError(GetInfo());

            // Assert
            Assert.Equal(info.ApplicationName, error.ApplicationName);
            Assert.Equal(info.Host, error.HostName);
            Assert.Equal(info.Type, error.Type);
            Assert.Equal(info.Source, error.Source);
            Assert.Equal(info.Message, error.Message);
            Assert.Equal(info.BuildMessage(), error.Detail);
            Assert.Equal(info.Date.ToString(), error.Time.ToString());
            Assert.Equal(info.User, error.User);
            Assert.Equal(info.StatusCode, error.StatusCode);

            CompareCollections(info.ErrorDetails.Single(x => x.Name == "Cookies").Items, error.Cookies);
            CompareCollections(info.ErrorDetails.Single(x => x.Name == "Form").Items, error.Form);
            CompareCollections(info.ErrorDetails.Single(x => x.Name == "QueryString").Items, error.QueryString);
            CompareCollections(info.ErrorDetails.Single(x => x.Name == "ServerVariables").Items, error.ServerVariables);
        }

        private static void CompareCollections(IEnumerable<KeyValuePair<string,string>> info, NameValueCollection error)
        {
            foreach (string key in error.Keys)
            {
                string expected = info.Single(x => x.Key == key).Value;
                string actual = error[key];

                Assert.Equal(expected, actual);
            }
        }

        private static  ErrorInfo GetInfo()
        {
            var info = new ErrorInfo(new Exception("Test Exception"))
                       {
                           ApplicationName = "ApplicationName",
                           Host = "Host",
                           Type = "Type",
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

        class TestableElmahErrorHelper : ElmahErrorHelper
        {
            public Error Error;

            protected override void LogInternal(Error error, object context)
            {
                Error = error;
            }
        }
    }
}
