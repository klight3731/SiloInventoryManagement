using SiloInventoryManagement.DAL;
using System;
using System.Windows;

namespace SiloInventoryManagement.UI
{
    public static class ErrorHelper
    {
        private static ExceptionsDBA _errorDBA = new ExceptionsDBA();

        public static void DisplayError(Exception ex)
        {
            try
            {
                MessageBox.Show(ex.StackTrace,
                                "An error occured while processing your request",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                //if we're getting an error here I'm going to assume there is a connection problem.  At least display the error below
                //string blah = "";
                throw (e);
            }
            finally
            {
                
            }
        }

        public static void LogAndDisplayError(Exception ex)
        {
            try
            {
                LogError(ex);
            }
            catch (Exception e)
            {
                //if we're getting an error here I'm going to assume there is a connection problem.  At least display the error below
                //string blah = "";
                throw (e);
            }
            finally
            {
                MessageBox.Show(string.Format("Source: {0}{1}{2}Message: {3}{4}{5}Inner Exception Message: {6}{7}{8}Stack Trace: {9}",
                                                ex.Source, Environment.NewLine,
                                                Environment.NewLine,
                                                ex.Message,
                                                Environment.NewLine,
                                                Environment.NewLine,
                                                ex.InnerException == null ? "" : ex.InnerException.Message,
                                                Environment.NewLine,
                                                Environment.NewLine,
                                                ex.StackTrace),
                                "An error occured while processing your request",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void LogError(Exception ex)
        {
            _errorDBA.Insert(ex.Message, ex.StackTrace);
        }
    }
}
