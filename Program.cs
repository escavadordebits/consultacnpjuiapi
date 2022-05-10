using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SapConsultaCep
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var Login = new Login();

            Login.ConectaSap();

            System.Windows.Forms.Application.Run();
        }
    }
}
