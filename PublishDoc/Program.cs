using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocAsCode.Utility;
using System.IO;
using LibGit2Sharp;

namespace DocAsCode.PublishDoc
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string filesDirectory = "";

            var options = new Option[]
                {
                    new Option("filesDirectory", s => filesDirectory = s, defaultValue: filesDirectory, helpText: "Specify the directory htmls to be published."),
                };

            if (!ConsoleParameterParser.ParseParameters(options, args))
            {
                Console.WriteLine("Error while parsing parameters!");
                return ;
            }

            Publisher.PublishToGithub(filesDirectory);
        }
        
       
    }
}
