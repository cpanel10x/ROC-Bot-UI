using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using Tesseract;
using System.Net;
using System.Text;

namespace ROC
{
    public class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AdbClass adbClass;
        private Point resolution;
        private String resolutionPath;


        public Bot()
        {

            log.Info("Đang kết nối đến giả lập...");
            this.adbClass = new AdbClass();
            this.resolution = this.adbClass.GetScreenSize();


            this.resolutionPath = "resources/Game/resolutions/" + resolution.X + 'x' + resolution.Y + '/';
            if (!Directory.Exists(this.resolutionPath))
            {
                log.Error("Resource folder not found");
                throw new FileNotFoundException();
            }
            else
            {
                log.Debug("Resource folder used : " + this.resolutionPath);
            }
        }

        public AdbClass GetAdbClass()
        {
            return this.adbClass;
        }

        public void StartGame()
        {
            int i = 0;

            log.Info("Đang mở Game");
            try
            {
                this.TouchImage("App.jpg", 0.1);
                log.Debug("Game launched");

            }
            catch
            {
                log.Debug("Game already started ?");
            }
            while (1 == 1)
            {
                if (globalVar.isStop)
                    return;
                i++;
                if (i == 10)
                    throw new TimeoutException();
                try
                {
                    Rectangle EventQuest = FindImage("EventQuest.jpg", 0.4);
                    this.GoToTown();
                    break;
                }
                catch (ImageNotFound)
                {
                    log.Debug("Game starting...");
                    this.ClearWindows();
                    //adbClass.TouchPoint(new Point(1633, 184));
                    Thread.Sleep(5000);
                }
            }
            log.Debug("Game started!");
            PushNotice("Game started!!");
            //this.GoToTown();
        }

        public void ReadScreen()
        {
            try
            {
                log.Info("OCR read screen");
                TesseractEnviornment.CustomSearchPath = "resources/Tesseract";
                var engine = new TesseractEngine(@"./resources/Tesseract/tessdata", "eng", EngineMode.Default);     //var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
                var conv = new BitmapToPixConverter();
                var img = conv.Convert(ImageUtil.RemoveNoise(ImageUtil.SetGrayscale(adbClass.GetAdbScreen())));
                var page = engine.Process(img);
                log.Info("OCR : " + page.GetText());
            }
            catch (Exception e)
            {
                log.Debug(e);
            }
        }

        public void CollectResources()
        {
            log.Debug("Trying to collect resources...");
            String[] ResourcesFiles =
            {
                "Help.jpg",
                "AskHelp.jpg",
                "Food.jpg",
                "Gold.jpg",
                "Rock.jpg",
                "Wood.jpg",
            };

            foreach (string resource in ResourcesFiles)
            {
                try
                {
                    log.Info("Collecting " + resource.Split('.')[0]);
                    this.TouchImage(resource, 0.4);
                }
                catch
                {
                    log.Info("Can't find " + resource.Split('.')[0]);
                }
            }
            log.Info("Collection terminated");
        }
        public void ClearWindows()
        {
            log.Debug("Trying to clear windows...");
            String[] WindowsFiles =
            {
                "CloseWindows.jpg",
                "CloseWindows2.jpg",
            };

            foreach (string wins in WindowsFiles)
            {
                try
                {
                    log.Debug("Closing " + wins.Split('.')[0]);
                    this.TouchImage(wins, 0.1);
                }
                catch
                {
                    log.Debug("Can't find " + wins.Split('.')[0]);
                }
            }
            log.Debug("Done Clear Windows");
        }
        public void CheckVerify()
        {
            log.Debug("Check Verify...");
            String[] VerifyFiles =
                {   
                    "Verify.jpg",
                    "Verify2.jpg",
                };
            foreach (string resource in VerifyFiles)
            {
                try
                {
                    Rectangle VerifyButton = FindImage(resource, 0.2);
                    PushNotice("Phát hiện yêu cầu Verify lúc " + DateTime.Now.ToString("HH:mm:ss"));
                }
                catch (ImageNotFound)
                {
                    log.Debug("Không có verify");
                }
            }
        }
        public void CollectTroops()
        {
            log.Debug("Trying to collect troops...");
            String[] ResourcesFiles =
            {
                "Stable1.jpg",
                "Barracks1.jpg",
                "Archery1.jpg",
                "Siege1.jpg",
            };

            foreach (string resource in ResourcesFiles)
            {
                try
                {
                    log.Info("Collecting " + resource.Split('.')[0]);
                    this.TouchImage(resource, 0.4);
                }
                catch
                {
                    log.Info("Can't find " + resource.Split('.')[0]);
                }
            }
            log.Info("Collection terminated");
        }

        public void Explore()
        {
            while (1 == 1)
            {
                if (globalVar.isStop)
                    return;
                try
                {
                    log.Info("Kiểm tra xem có thằng nào rảnh không...");
                    
                    this.TouchImage("ExploreNotif.jpg", 0.5, 0, 100);
                }
                catch
                {
                    log.Info("Không có thằng nào rảnh");
                    this.CheckVerify();
                    return;
                }
                try
                {

                    log.Info("Có thằng đang rảnh, bắt đầu sai nó đi.");
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreMenu.jpg", 0.3);
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreButton.jpg", 0.2);
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreButton.jpg", 0.2);
                    Thread.Sleep(3000);
                    this.TouchImage("SendButton.jpg", 0.2);
                    PushNotice("Đã gửi 1 thằng đi lúc " + DateTime.Now.ToString("HH:mm:ss"));
                    Thread.Sleep(3000);
                    this.GoToTown();
                    Thread.Sleep(3000);
                    
                }
                catch
                {
                    //adbClass.TouchPoint(new Point(1633, 184));
                    //try
                    //{
                    //    //Rectangle map = this.FindImage("CloseWindows.jpg", 0.4);
                    //    log.Info("clear windows");
                    //    this.TouchImage("CloseWindows.jpg", 0.1);
                    //    Thread.Sleep(3000);
                    //    this.TouchImage("CloseWindows2.jpg", 0.1);

                    //}
                    //catch (ImageNotFound)
                    //{
                    //    this.GoToTown();
                    //}
                this.ClearWindows();
                this.GoToTown();
                }
            }
        }
        public void PushNotice(string mess)
        {
            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            string apiToken = "866266174:AAENyet9NF7hMgBslcdLgsykrmamo2-dZU0";
            string chatId = "894875684";
            urlString = String.Format(urlString, apiToken, chatId, mess);
            WebRequest request = WebRequest.Create(urlString);
            Stream rs = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(rs);
            string line = "";
            StringBuilder sb = new StringBuilder();
        }
        public void GoToTown()
        {
            int numError = 0;
            try
            {
                log.Debug("Is home?");
                Rectangle map = this.FindImage("Map.jpg", 0.2);
                log.Debug("Already in town ! Recenter Home");
                this.TouchImage("Map.jpg", 0.2);
                Thread.Sleep(1000);
                this.TouchImage("Home.jpg", 0.2);
                log.Info("Đã về nhà");
            }
            catch (ImageNotFound)
            {
                try
                {
                    this.TouchImage("Home.jpg", 0.2);
                    //adbClass.TouchPoint(new Point(90, 1100));
                }
                catch
                {
                    log.Error("I'm lost");
                    PushNotice("Lạc lối");
                    this.ClearWindows();
                    if (numError++ < 5)
                        this.GoToTown();
                    else
                        throw new ImageNotFound();
                   
                }
            }
        }

        public void CollectTribalVillage()
        {
            try
            {
                while (1 == 1)
                {
                    log.Info("Trying to collect gift from tribal villages...");
                    this.GoToMessages();
                    Thread.Sleep(2000);
                    this.FindImage("ExplorationReportSelected.jpg", 0.4);
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationTribalVillage.jpg", 0.3, 90, 0);
                    Thread.Sleep(5000);
                    adbClass.TouchPoint(new Point(this.resolution.X / 2, this.resolution.Y / 2));
                    Thread.Sleep(2000);
                    this.GoToTown();
                    Thread.Sleep(2000);
                }
            }
            catch
            {
                try
                {
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationReportNotSelected.jpg", 0.3);
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationTribalVillage.jpg", 0.3, 90, 0);
                    Thread.Sleep(5000);
                    adbClass.TouchPoint(new Point(this.resolution.X / 2, this.resolution.Y / 2));
                    Thread.Sleep(2000);
                    this.GoToTown();
                    Thread.Sleep(2000);
                }
                catch
                {
                    log.Info("Nothing to collect");
                    adbClass.TouchPoint(new Point(1850, 75));
                    return;
                }
            }
        }

        public void GoToMessages()
        {
            adbClass.TouchPoint(new Point(1800, 950));
        }

        private Rectangle FindImage(string path, Double tolerance)
        {
            log.Debug("Trying to find image on screen  : " + this.resolutionPath + path);
            Bitmap needle = ImageUtil.OpenImage(this.resolutionPath + path);
            Bitmap screen = adbClass.GetAdbScreen();
            Rectangle image = ImageUtil.SearchBitmap(needle, screen, tolerance);
            if (image.Width != 0)
                return image;
            else
                //GetAdbClass().TakeScreenShot("Error_" + DateTime.Now.ToFileTime() + ".jpg");
                throw new ImageNotFound();
                
        }

        private void TouchImage(string path, Double tolerance)
        {

            adbClass.TouchRectangle(this.FindImage(path, tolerance));
        }

        private void TouchImage(string path, Double tolerance, int x, int y)
        {
            adbClass.TouchRectangle(this.FindImage(path, tolerance), x, y);
        }

    }


    class ImageNotFound : Exception
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageNotFound()
        {
            log.Debug("Image not found");

        }
    }
}