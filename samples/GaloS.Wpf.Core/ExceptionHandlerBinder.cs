﻿using System.Diagnostics;
using System.Text;
using System.Windows;

namespace GaloS.Wpf.Core
{
    internal static class ExceptionHandlerBinder
    {
        public static void Bind()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionInfoFormatFriendly(e.Exception, e.ToString());
            e.Handled = true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionInfoFormatFriendly(e.ExceptionObject as Exception, e.ToString());
        }

        /// <summary>h
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        private static string ExceptionInfoFormatFriendly(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now);
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);

                sb.AppendLine("【异常方法】：" + ex.TargetSite);

            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            var str = sb.ToString();
            Trace.TraceError(str);
            return str;
        }
    }
}
