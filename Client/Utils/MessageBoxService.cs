using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client.Utils
{
    public static class MessageBoxService
    {
        public static void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error");
        }

        public static MessageBoxResult ShowQuestion(string question)
        {
            return MessageBox.Show(question, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
