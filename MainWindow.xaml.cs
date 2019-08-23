using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROC
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public MainWindow()
        {
            InitializeComponent();
            //log4net.Config.XmlConfigurator.Configure();
            log.Info("Waiting for start...");


        }
        public void runbot()
        {
            try
            {
                if (globalVar.isStop)
                    return;
                Bot bot = new Bot();
                bot.GetAdbClass().TakeScreenShot("test.jpg");
                
                bot.StartGame();
                log.Info("Danh sách giả lập đang chạy :");
                //log.Info(bot.GetAdbClass().GetDeviceList());
                //bot.GetAdbClass().GetDeviceList();
                //log.Debug("Emulator detected");
                //bot.ReadScreen();
                while (1 == 1)
                {
                    if (globalVar.isStop)
                        return;
                    //bot.CollectResources();
                    //bot.CollectTroops();

                    bot.Explore();
                    //bot.CollectTribalVillage();

                }

            }
            catch
            {


                // Do what you want with response
                log.Error("Bot đã ngừng");

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            globalVar.isStop = false;
            Task t1 = new Task(() =>
            {
                runbot();
            });
            t1.Start();
            statusContent.Content = "Started";
        }

        private void btnStop(object sender, RoutedEventArgs e)
        {
            globalVar.isStop = true;
            statusContent.Content = "Stopped";
            
        }
    }
    public static class globalVar
    {
        public static bool isStop{ get; set; }
        public static string status { get; set; }
    }
   
}
