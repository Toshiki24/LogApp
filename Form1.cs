using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogApp
{
    public partial class Form1 : Form
    {
        private const string FOLDER_NAME = "Data";
        private object obj = new object();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string todayData = string.Empty;
            string filepath = string.Empty;
            string path = @"C:\Users\Toshi\Desktop\new\Data\2021\07\04.txt";
            // ファイルパスを取得する
            filepath = fileCreate();
            // CSVを読み込む
            using (StreamReader reader = new StreamReader(filepath))
            {
                todayData = reader.ReadToEnd();
            }
 
            //現在の状況(計測中か否か)
            //時間(今回の計測時間)
            //合計時間(本日の合計計測時間)
            //コメント(フリーフォーマット)

        }

        /// <summary>
        /// 今日のファイルパスを取得する
        /// </summary>
        /// <returns>ファイルパス</returns>
        private string fileCreate()
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, FOLDER_NAME);
            string year = DateTime.Now.ToString("yyyy"); // 年
            string manth = DateTime.Now.ToString("MM");  // 月
            string day = DateTime.Now.ToString("dd") + ".txt";    // 日

            // フォルダが存在しない場合は作成
            if (!Directory.Exists(folderPath))
            {
                // フォルダーの作成
                DirectoryCreate(folderPath);

            }

            // 現在の年のディレクトリ
            string yearDirectory = Path.Combine(folderPath, year);
            if (!Directory.Exists(yearDirectory))
            {
                // フォルダーの作成
                DirectoryCreate(yearDirectory);
            }

            // 現在の月のディレクトリ
            string manthDirectory = Path.Combine(yearDirectory, manth);
            if (!Directory.Exists(manthDirectory))
            {
                // フォルダーの作成
                DirectoryCreate(manthDirectory);
            }
            // ファイル読み込み
            string filePath = Path.Combine(manthDirectory, day);
            if (!File.Exists(filePath))
            {
                // ファイルの作成
                File.Create(filePath).Close();

                // アクセス権を変更する
                FileSystemAccessRule rule = new FileSystemAccessRule(
                  new NTAccount("everyone"),
                  FileSystemRights.Modify,
                  AccessControlType.Allow);

                FileSecurity security = File.GetAccessControl(filePath);
                security.AddAccessRule(rule);
                File.SetAccessControl(filePath, security);
            }
            return filePath;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        /// <summary>
        /// ディレクトを作成してアクセス権を付与する
        /// </summary>
        /// <param name="path">ディレクトリを作成するパス</param>
        private void DirectoryCreate(string path)
        {
            Directory.CreateDirectory(path);

            FileSystemAccessRule rule = new FileSystemAccessRule(
              new NTAccount("everyone"),
              FileSystemRights.FullControl,
              InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
              PropagationFlags.None,
              AccessControlType.Allow);

            DirectorySecurity security = Directory.GetAccessControl(path);
            security.SetAccessRule(rule);
            Directory.SetAccessControl(path, security);
        }
    }
}
