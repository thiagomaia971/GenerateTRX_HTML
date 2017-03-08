using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerarTestes.App
{
    public partial class GerarTest : Form
    {

        public string SourceDll { get; set; }
        public string MyProperty { get; set; }
        public GerarTest()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void ButtonFileDllTest(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fullName = openFileDialog1.FileName;

                MessageBox.Show(fullName);
            }
        }

        private void ButtonGerarTRX(object sender, EventArgs e)
        {
            var pathCSPROJ = openFileDialog1.FileName;
            var fileResultTest = GetFileResultTest();
            var exe = "./MSTest.exe";
            var mensagemRetorno = $"The file {pathCSPROJ}\\ResultTest.trx was created.";

            var comando = string.Format("/testcontainer:{0} /resultsfile:{1} ", pathCSPROJ, fileResultTest);

            ExecutarCommandLine(exe, comando, mensagemRetorno);

        }

        private void ButtonGerarHTML(object sender, EventArgs e)
        {
            var pathCSPROJ = GetPathCSPROJ();
            var fileResultTest = GetFileResultTest();
            var exe = "./specflow.exe";
            var mensagemRetorno = $"The file HTML is created in the same folder of the ResultTest.trx";

            var comando = string.Format("mstestexecutionreport {0} /testResult:{1}", pathCSPROJ, fileResultTest);

            ExecutarCommandLine(exe, comando, mensagemRetorno);
            fileResultTest = fileResultTest.Replace("trx", "html");

            File.Move(Path.GetFullPath("./") + "\\TestResult.html", fileResultTest);
        }

        private static void ExecutarCommandLine(string exe, string comando, string mensagemRetorno)
        {
            using (Process processo = new Process())
            {
                processo.StartInfo.FileName = exe;

                processo.StartInfo.Arguments = comando;

                processo.StartInfo.RedirectStandardOutput = true;
                processo.StartInfo.UseShellExecute = false;
                processo.StartInfo.CreateNoWindow = true;

                processo.Start();
                processo.WaitForExit();

                string saida = processo.StandardOutput.ReadToEnd();

                MessageBox.Show(mensagemRetorno);
            }
        }

        private string GetFileResultTest()
        {
            var pathCSPROJ = GetPathCSPROJ().Split('\\').ToList();
            pathCSPROJ.RemoveRange(pathCSPROJ.Count - 2, 2);

            var pathResultTest = string.Join("\\", pathCSPROJ);

            pathResultTest += "\\TestResults\\ResultTest.trx";

            return pathResultTest;
        }

        private string GetPathCSPROJ()
        {
            var path = openFileDialog1.FileName.Split(new string[] { "bin" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            var b = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            path += b + ".csproj";

            return path;
        }
    }
}
