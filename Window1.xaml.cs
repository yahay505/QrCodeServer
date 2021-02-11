using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
//using System.Threading;
using System.Timers;

namespace QrCodeServer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    public delegate void Notify();
    public partial class Window1 : Window
    {
        public static Window1 mainwindow;
        public static event Notify startfadeevent;
        public Window1()
        {
            mainwindow = this;
            InitializeComponent();

        }
        protected virtual void fadeevent() //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            startfadeevent?.Invoke();
            Console.WriteLine($"{(startfadeevent.GetInvocationList().Length)}");
        }


        static public void AddToMessageHistory(string _message) 
            => Program.app.Dispatcher.BeginInvoke(new Action(delegate () { Window1.mainwindow.maintextlog.Text += $"\r\n {_message}"; Window1.mainwindow.scroller.ScrollToBottom(); }));

    }
}
