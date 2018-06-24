using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace merge_video
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void merge_video_files(string[] filenames, string output_filename)
        {
            Process p = new Process();
            p.StartInfo.FileName = "ffmpeg.exe";

            string[] lines = new string[filenames.Length];
            for (int i = 0; i < lines.Length; i++)
            {

                string command = string.Format("file '{0}'", filenames[i]);
                lines[i] = command;
            }

            System.IO.File.WriteAllLines("temp.txt", lines, Encoding.GetEncoding("gbk"));

            string file_list_tag = filenames[0];
            for(int i = 1; i < filenames.Length; i++)
            {
                file_list_tag += "|" + filenames[i];
            }

            p.StartInfo.Arguments = string.Format("-safe 0 -f concat -i temp.txt -c copy \"{0}\" -y", output_filename);
            p.Start();
            p.WaitForExit();
            
            System.IO.File.WriteAllLines("result.txt", new string[]{ p.StartInfo.Arguments});
        }

        private void add_new_file(string filename)
        {
            string title = System.IO.Path.GetFileName(filename);

            ListViewItem listViewItem = new ListViewItem(title);
            listViewItem.SubItems.Add(filename);
            listView1.Items.Add(listViewItem);
        }

        private void File_dragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void File_dragDrop(object sender, DragEventArgs e)
        {
            string filename = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            add_new_file(filename);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listView1.Items.Count == 0)
            {
                MessageBox.Show("列表为空");
                return;
            }
            string[] file_list = new string[listView1.Items.Count];
            for(int i = 0; i < file_list.Length; i++)
            {
                ListViewItem item = listView1.Items[i];
                file_list[i] = item.SubItems[1].Text;
            }
            string output_filename = file_list[0] + "_merge.mp4";
            saveFileDialog1.FileName = output_filename;
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                output_filename = saveFileDialog1.FileName;
                merge_video_files(file_list, output_filename);
                MessageBox.Show("保存至：" + output_filename);
            }
            
        }
    }
}
