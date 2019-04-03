using NReco.VideoConverter;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Convertidor : Form
    {
        string video;

        public Convertidor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Proceso();
        }

        async void Proceso()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Multiselect = true;

                string formats = "*.webm; *.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                          " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro";

                string[] exts = formats.Split(';');
                string filter = string.Empty;
                foreach (string ext in exts)
                {

                    filter += "Video Files (" + ext.Replace("*", "").Trim() + ")|" + ext + "|";
                }

                openFileDialog.Filter = filter.Remove(filter.Length - 1, 1);
                openFileDialog.ShowDialog();

                foreach (var item in openFileDialog.SafeFileNames)
                {
                    dataGridView.Rows.Add(item);
                }
                   
                foreach (var item in openFileDialog.FileNames)
                {
                    ActivarBarra();
                    video = item;
                    await Task.Run(() => ConvertirVideo(video));
                    DesactivarBarra();
                    dataGridView.Rows.Remove(dataGridView.Rows[0]);
                }
                MessageBox.Show("Todos Los Videos Se Han Convertido");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ha Ocurrido un error convirtiendo el video: " + ex.Message);
            }
        }

        void ConvertirVideo(string video)
        {
            try
            {
                string path2 = Application.StartupPath;
                var Convertidor = new FFMpegConverter();
                string nombre =  Guid.NewGuid().ToString().Substring(0,8);
                Convertidor.ConvertMedia(video, nombre + ".mp4", "mp4");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Este Video no se puede convertir: " + video + ex.Message);
            }
        }

        async void ActivarBarra()
        {
            progressBar.MarqueeAnimationSpeed = 30;
            progressBar.Style = ProgressBarStyle.Marquee;
        }

        async void DesactivarBarra()
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;
        }

        private void Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
