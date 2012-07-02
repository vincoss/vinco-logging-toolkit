using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xunit;
using System.Linq;


namespace Elmah.Everywhere.Test
{
    public class ErrorInfoTest
    {
        [Fact]
        public void Default_Constructor_Test()
        {
            // Assert
            ErrorInfo error = new ErrorInfo();
        }

        [Fact]
        public void Constructor_Throws_If_Parameter_Null_Test()
        {
            Assert.Throws<ArgumentNullException>(() =>
                                                     {
                                                         new ErrorInfo(null);
                                                     });
        }

        [Fact]
        public void Constructor_Test()
        {
            // Arrange
            Exception exception = new Exception("Test Exception");

            // Act
            ErrorInfo error = new ErrorInfo(exception);

            // Assert
            Assert.NotNull(error.Id);
            Assert.Null(error.Host);
            Assert.Equal(exception.GetType().FullName, error.Type);
            Assert.Equal(error.Source, exception.Source);
            Assert.Equal(exception.Message, error.Message);
            Assert.NotNull(error.Detail);
            Assert.Null(error.ApplicationName);
            Assert.Null(error.User);
            Assert.Equal(0, error.StatusCode);
            Assert.Equal(DateTime.Now.ToString(), error.Date.ToString());
            Assert.Equal(exception, error.Exception);
        }

        [Fact]
        public void Details_Not_Null_Test()
        {
            // Act
            ErrorInfo error = new ErrorInfo();

            // Assert
            Assert.False(error.Details.Any());
        }

        [Fact]
        public void Appenders_Is_Null_Test()
        {
            // Act
            ErrorInfo error = new ErrorInfo();

            // Assert
            Assert.Null(error.Appenders);
        }

        [Fact]
        public void Properties_Is_Null_Test()
        {
            // Act
            ErrorInfo error = new ErrorInfo();

            // Assert
            Assert.Null(error.Properties);
        }

        [Fact]
        public void ToString_Returns_Exception_Message_Test()
        {
            // Act
            ErrorInfo error = new ErrorInfo(new Exception("Test Error"));

            // Assert
            Assert.Equal(error.Exception.Message, error.ToString());
        }

        [Fact]
        public void BuildMessage_Returns_Detail_Message_If_Details_Are_Empty_Test()
        {
            // Arrange
            ErrorInfo error = new ErrorInfo(new Exception());

            // Act
            string detail = error.BuildMessage();

            // Assert
            Assert.Equal(error.Detail, detail);
        }

        [Fact]
        public void BuildMessage_Appends_Details()
        {
            // Arrange
            ErrorInfo error = new ErrorInfo(new Exception());
            var pair = new List<KeyValuePair<string, string>>();
            pair.Add(new KeyValuePair<string, string>("Key", "Value"));

            error.AddDetail("Append Test", pair);

            // Act
            string detail = error.BuildMessage();

            // Assert
            Assert.True(detail.Contains("Append Test"));
        }

        [Fact]
        public void DetailInfo_Test()
        {
            // Arrange
            ErrorInfo.DetailInfo detail = new ErrorInfo.DetailInfo("Test", new List<KeyValuePair<string, string>>());

            // Assert
            Assert.Equal("Test", detail.Name);
            Assert.NotNull(detail.Items);
        }

        [Fact]
        public void AddDetail_Test()
        {
            // Arrange
            ErrorInfo error = new ErrorInfo();

            error.AddDetail("Test", "Key", "Value");

            // Act
            ErrorInfo.DetailInfo detail = error.Details.First();

            // Assert
            Assert.Equal("Test", detail.Name);
            Assert.Equal("Key", detail.Items[0].Key);
            Assert.Equal("Value", detail.Items[0].Value);
        }
    }
}
