using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Threading;
using System.Windows;

namespace QrCodeServer
{
    class Program
    {
        static string baseanswer,basecss;
        static string[] prefixes = { "http://192.168.1.35:25000/", "http://localhost:25000/" };
        public static Application app = new Application();
        [STAThread]
        static void Main(string[] args)
        {



            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Hello World!");

            baseanswer = File.ReadAllText(@"C:\Path\Def.html");
            basecss = File.ReadAllText(@"C:\Path\Def.css");

            _ = Task.Run(() => reloadwebsite());
            // _ = Task.Run(() => QRForm());
            var appthread = new Thread(() => { app.Run(new Window1()); });



           

            HttpListener listener = new HttpListener();
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    //Console.WriteLine($"Adding Task to task pool : {DateTime.Now}");

                    _ = Task.Run(() => Respond(context));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                    
                }

            }
            listener.Stop();
        }



            static void reloadwebsite()
        {
            while (true)
            {
                if (Console.ReadKey(true).KeyChar == 'r')
                {
                    Window1.AddToMessageHistory("reloaded website(s)");
                    Console.WriteLine("reloaded website(s)");
                    baseanswer = File.ReadAllText(@"C:\Path\Def.html");
                    basecss = File.ReadAllText(@"C:\Path\Def.css");

                }
            }


        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task Respond(HttpListenerContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Console.WriteLine($"Task actually started : {DateTime.Now}");

            Console.WriteLine("got request");

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            var request = context.Request;
            var body = new StreamReader(request.InputStream).ReadToEnd();

            //Log the request
            #region log 
            
            Console.WriteLine($"Response Data:\r\n " +
                $"a: {body}" 
               );
            foreach (var key in request.QueryString.AllKeys)
            {
                Console.WriteLine($"{key}: {request.QueryString.Get(key)}");

            }
            foreach (var header in request.Headers.AllKeys)
            {
                Console.WriteLine($"{header}: {request.QueryString.Get(header)}");

            }



            #endregion
            // Construct a response.
            string responseString = GenerateResponse(context);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();

            return;
        }
        static string GenerateResponse(HttpListenerContext context)
        {
            var returnhtml = baseanswer;
            var css = basecss;
            var js = new string("");
            if (context.Request.QueryString.Count>3)
            {
                returnhtml= "<HTML><BODY> Too Much querry maximum of 1 query if allowed \r\n for more info go to /help/</BODY></HTML>";
            }



            returnhtml=returnhtml.Replace(@"#$#$CSS#$#$", css);
            returnhtml = returnhtml.Replace(@"#$#$JS#$#$", js);

            return returnhtml;
        }
       
      /*  static void QRForm()
        {
            Form myform = new Form();
            Button mybutton = new Button()
            {
                Text = "Hello",
                Location = new System.Drawing.Point(10, 10)
            };
            mybutton.Click += (o, s) =>
            {
                MessageBox.Show("world");
            };
            
            myform.Controls.Add(mybutton);
            myform.ShowDialog();
            while (myform.Created)
            {

            }
        }*/


    }
    
}
