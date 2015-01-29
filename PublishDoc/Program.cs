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
            Publisher.PublishToGithub(args[0]);
        }
        
       
    }
}
