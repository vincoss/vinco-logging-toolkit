using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Elmah.Everywhere.Appenders;


namespace Elmah.Everywhere.Test
{
    public class ErrorInfoTest
    {
        [Fact]
        public void Shall_Have_Default_Constructor_Test()
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
            Assert.False(error.ErrorDetails.Any());
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
        public void BuildMessage_Returns_Error_Detail_Message_If_ErrorDetails_Are_Empty_Test()
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
            ErrorInfo.DetailInfo detail = error.ErrorDetails.First();

            // Assert
            Assert.Equal("Test", detail.Name);
            Assert.Equal("Key", detail.Items[0].Key);
            Assert.Equal("Value", detail.Items[0].Value);
        }

        [Fact]
        public void ShouldAppend_Appender_Data_Test()
        {
            // Arrange
            var appenders = new List<Type>
                                {
                                    typeof(FirstAppender),
                                    typeof(SecondAppender)
                                };

            ErrorInfo error = new ErrorInfo(new Exception("Test-Exception"));
            error.Appenders = appenders;
            error.EnsureAppenders();

            // Act
            string detail = error.BuildMessage();

            // Assert
            Assert.True(detail.Contains("FirstAppender"));
            Assert.True(detail.Contains("SecondAppender"));
            Assert.True(detail.Contains("FirstKey"));
            Assert.True(detail.Contains("FirstValue"));
            Assert.True(detail.Contains("SecondKey"));
            Assert.True(detail.Contains("SecondValue"));
        }

        public class FirstAppender : BaseAppender
        {
            public override void Append(ErrorInfo error)
            {
                error.AddDetail(this.Name, "FirstKey", "FirstValue");
            }

            public override int Order
            {
                get { return 0; }
            }

            public override string Name
            {
                get { return "FirstAppender"; }
            }
        }

        public class SecondAppender : BaseAppender
        {
            public override void Append(ErrorInfo error)
            {
                error.AddDetail(this.Name, "SecondKey", "SecondValue");
            }

            public override int Order
            {
                get { return 1; }
            }

            public override string Name
            {
                get { return "SecondAppender"; }
            }
        }
    }
}
