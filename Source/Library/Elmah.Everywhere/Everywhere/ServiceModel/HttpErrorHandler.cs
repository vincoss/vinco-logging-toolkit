using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;
using Elmah.Everywhere.Diagnostics;


namespace Elmah.Everywhere.ServiceModel
{
    public class HttpErrorHandler : IErrorHandler
    {
        #region IErrorHandler Members

        public bool HandleError(Exception error)
        {
            if (error == null)
            {
                return false;
            }
            if (error is FaultException)
            {
                var sb = new StringBuilder();
                AppendFaultExceptionDetail(error, sb);
            }
            Diagnostics.ExceptionHandler.Report(error, null);
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }
        
        #endregion

        #region FaultException helper methods

        private static void AppendFaultExceptionDetail(Exception exception, StringBuilder sb)
        {
            try
            {
                MethodInfo writeGenericFaultExceptionDetailMethodInfo = typeof(HttpErrorHandler).GetMethod("GetGenericFaultExceptionDetail", BindingFlags.NonPublic | BindingFlags.Static);

                Type faultExceptionType = exception.GetType();

                writeGenericFaultExceptionDetailMethodInfo.MakeGenericMethod(faultExceptionType.GetGenericArguments()).Invoke(null, new object[] { exception, sb });

                XElement element = XElement.Parse(sb.ToString());
                sb.Clear();
                sb.AppendLine();
                foreach (XElement node in element.Nodes())
                {
                    sb.AppendLine(string.Format("{0} : {1}", node.Name.LocalName, node.Value));
                }
                exception.Data[exception.GetType()] = sb.ToString();
            }
            catch (Exception ex)
            {
                sb.AppendLine(string.Format("Get_FaultException_Detail_Fail : {0}", ex.ToString()));
            }
        }

        private static void GetGenericFaultExceptionDetail<T>(FaultException faultException, StringBuilder sb)
        {
            var faultExceptionWithDetail = (FaultException<T>)faultException;

            var dataContractSerializer = new DataContractSerializer(typeof(T));

            using (var writer = new StringWriter(sb))
            {
                var xmlWriter = XmlWriter.Create(writer);
                dataContractSerializer.WriteObject(xmlWriter, faultExceptionWithDetail.Detail);
            }
        } 

        #endregion
    }
}
