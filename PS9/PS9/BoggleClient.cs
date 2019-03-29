using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS9
{
    public partial class BoggleClient : Form, IBoggleService
    {
        public BoggleClient()
        {
            InitializeComponent();
        }

        public event Action EnterGame;
        public event Action CancelGame;
        public event Action<string> SubmitWord;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void BoggleClient_Load(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        public string ObtainUsername()
        {
            throw new NotImplementedException();
        }

        public string ObtainDesiredServer()
        {
            throw new NotImplementedException();
        }

        public void SetTimeLimit()
        {
            throw new NotImplementedException();
        }

        public void SetRemainingTime()
        {
            throw new NotImplementedException();
        }

        public void SetPlayerScore(int score)
        {
            throw new NotImplementedException();
        }

        public void SetOpponentScore(int oppScore)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentPlayedWords(List<string> words)
        {
            throw new NotImplementedException();
        }
    }
}
