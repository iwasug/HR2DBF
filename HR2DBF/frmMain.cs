using Ionic.Zip;
using SocialExplorer.IO.FastDBF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace HR2DBF
{
    public partial class frmMain : Form
    {
        private string FolderHasil = Application.StartupPath + "\\HASIL";
        private string FolderTemp = Application.StartupPath + "\\TEMP";
        private string FolderTempBln = Application.StartupPath + "\\TEMPBLN";
        //private string FL = "DT|WT|PL|ST|SLP|RGL|DBT|EDC|CIN|CRT";
        private DataTable KONV = new DataTable();
        private DataTable KONVBLN = new DataTable();
        private bool ProsesHarian = true;
        private string FileProses = "";
        DateTime start = DateTime.Now;
        //private string FL = "CA|ST|PL|WT|DT|SLP|RGL|DBT|CIN|CRT|NTB|WU|TL";
        public frmMain()
        {
            InitializeComponent();
            this.Text = "HR2DBF v." + Assembly.GetEntryAssembly().GetName().Version.ToString();
            txtFolder.Text = Properties.Settings.Default.Folder;
            KONV.Columns.Add("FILE", typeof(string));
            KONV.Columns.Add("KOMA", typeof(string));
            KONV.Rows.Add("ST", "|");
            KONV.Rows.Add("PL", ",");
            KONV.Rows.Add("WT", "|");
            KONV.Rows.Add("DT", "|");
            KONV.Rows.Add("SLP", ",");
            KONV.Rows.Add("RGL", ",");
            KONV.Rows.Add("DBT", ",");
            KONV.Rows.Add("CIN", ",");
            KONV.Rows.Add("CRT", ",");
            KONV.Rows.Add("NTB", "|");
            KONV.Rows.Add("WU", ",");
            KONV.Rows.Add("TL", ",");
            KONV.Rows.Add("CA", ",");
            KONV.Rows.Add("PBK", ",");
            KONV.Rows.Add("HARIAN", ",");
            KONV.Rows.Add("PR", "|");
            KONVBLN.Columns.Add("FILE", typeof(string));
            KONVBLN.Columns.Add("KOMA", typeof(string));
            //KONVBLN.Rows.Add("CONST", ","); //OK
            KONVBLN.Rows.Add("BYR", ","); //OK
            //KONVBLN.Rows.Add("BKL", ",");//OK
            //KONVBLN.Rows.Add("HPC", ",");//OK
            //KONVBLN.Rows.Add("HARIAN", ",");//OK
            //KONVBLN.Rows.Add("HP", ",");//OK
            //KONVBLN.Rows.Add("LPM", ",");//OK
           // KONVBLN.Rows.Add("MSTRAN", ",");//OK
           // KONVBLN.Rows.Add("MTRAN", ",");//OK
           // KONVBLN.Rows.Add("PROD", ",");//OK
            //KONVBLN.Rows.Add("RET", ",");//OK
            //KONVBLN.Rows.Add("RKL", ",");//OK
           // KONVBLN.Rows.Add("TOKO", ",");//OK
          //  KONVBLN.Rows.Add("FILET", ",");//OK
           // KONVBLN.Rows.Add("STMAST", ",");//OK
            lbCount = "-";
        }

        private void checkOto_CheckedChanged(object sender)
        {
            if(checkOto.Checked)
            {
                //TimerProses.Start();
                mulai();
            }
            else
            {
                TimerProses.Stop();
            }
        }

        int curent;
        private void mulai()
        {
            curent = 1200;
            TimerProses.Interval = 1000;
            TimerProses.Start();
        }

        private void TimerProses_Tick(object sender, EventArgs e)
        {
            curent = curent - 1;
            lblTimer.Value2 = curent.ToString();
            lblTimer.Refresh();
            if (curent == 0)
            {
                //Proses();
            }
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = Properties.Settings.Default.Folder;
            fd.ShowDialog();
            txtFolder.Text = fd.SelectedPath;
            Properties.Settings.Default.Folder = txtFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void Proses()
        {
            if (!Directory.Exists(FolderHasil))
            {
                Directory.CreateDirectory(FolderHasil);
            }
            if (!Directory.Exists(FolderTemp))
            {
                Directory.CreateDirectory(FolderTemp);
            }
            if (!Directory.Exists(FolderTempBln))
            {
                Directory.CreateDirectory(FolderTempBln);
            }
            if(radioHarian.Checked)
            {
                ProsesHarian = true;
            }
            else if(radioBulanan.Checked)
            {
                ProsesHarian = false;
            }
            else
            {
                MessageBox.Show("TYPE PROSES BELUM DI TENTUKAN", "IWA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(txtFolder.Text == "")
            {
                MessageBox.Show("FOLDER HARIAN / BULANAN BELUM DI ISI", "IWA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(ProsesHarian)
            {
                pgBar2.Maximum = KONV.Rows.Count;
            }
            else
            {
                pgBar2.Maximum = KONVBLN.Rows.Count;
            }
            FileProses = txtFolder.Text;
            groupSetting.Enabled = false;
            start = DateTime.Now;
            TimerProses.Stop();
            bgWork.RunWorkerAsync();
        }

        private void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
            var files = Directory.GetFiles(FileProses, "HR*.*", SearchOption.AllDirectories).Union(Directory.GetFiles(FileProses, "FR*.*", SearchOption.AllDirectories)).Union(Directory.GetFiles(FileProses, "CR*.*", SearchOption.AllDirectories));
            if (ProsesHarian)
            {
                files = Directory.GetFiles(FileProses, "HR*.*", SearchOption.AllDirectories).Union(Directory.GetFiles(FileProses, "FR*.*", SearchOption.AllDirectories)).Union(Directory.GetFiles(FileProses, "CR*.*", SearchOption.AllDirectories));
            }
            else
            {
                files = Directory.GetFiles(FileProses, "*.IDT", SearchOption.AllDirectories);
            }
            double a = files.Count();
            double b = 0;
            foreach (string file in files)
            {
                b += 1;
                lbCount = "(" + a.ToString() + " \\ " + b.ToString() + ")";
                bgWork.ReportProgress(int.Parse(Math.Round((double)b / (double)a * 100).ToString()));
                string fileName = Path.GetFileName(file);
                if (fileName.Length != 12)
                {
                    goto Lanjut;
                }
                lbFile = fileName;
                if (File.Exists(FolderHasil + "\\" + fileName))
                {
                    File.Delete(FolderHasil + "\\" + fileName);
                }
                if (!File.Exists(FolderTemp + "\\" + fileName))
                {
                    File.Copy(file, FolderTemp + "\\" + fileName);
                }
                if (ProsesHarian)
                {
                    string THN = fileName.Substring(2, 2);
                    string BLN = fileName.Substring(4, 2);
                    string TGL = fileName.Substring(6, 2);
                    string KDTK = "";
                    string KOMA = "";
                    if (fileName.Substring(0, 1) == "H")
                    {
                        KDTK = "T" + fileName.Substring(9, 3);
                    }
                    else if (fileName.Substring(0, 1) == "F")
                    {
                        KDTK = "F" + fileName.Substring(9, 3);
                    }
                    else if (fileName.Substring(0, 1) == "C")
                    {
                        KDTK = "R" + fileName.Substring(9, 3);
                    }
                    lbProg = 0;
                    foreach (DataRow C in KONV.Rows)
                    {
                        lbProg += 1;
                        string P2 = C[0].ToString();
                        KOMA = C[1].ToString();
                        string P3 = "";
                        if (P2 == "TL" || P2 == "WU")
                        {
                            P3 = P2 + THN.Substring(1, 1) + BLN + TGL + KDTK.Substring(0, 1) + "." + KDTK.Substring(1, 3);
                        }
                        else if (P2 == "HARIAN")
                        {
                            P3 = P2 + ".CSV";
                        }
                        else
                        {
                            P3 = P2 + BLN + TGL + KDTK.Substring(0, 1) + "." + KDTK.Substring(1, 3);
                        }
                        lbStatus = P3;
                        if (UnzipFile(file, P3, FolderTemp))
                        {

                            //string newPath = Path.ChangeExtension(FolderTemp + "\\" + P3, "CSV");
                            string FLCSV = Path.ChangeExtension(FolderTemp + "\\" + P3, ".CSV");
                            //string FLCSV = "";

                            if (P2 != "HARIAN")
                            {
                                try
                                {
                                    if (File.Exists(FLCSV))
                                    {
                                        File.Delete(FLCSV);
                                    }
                                    File.Move(FolderTemp + "\\" + P3, Path.ChangeExtension(FolderTemp + "\\" + P3, ".CSV"));
                                }
                                catch (Exception)
                                {

                                }
                            }
                            using (DataTable DT = GETCSVTABLE(FLCSV, KOMA))
                            {
                                if (CSV2DBF(DT, P2, P3))
                                {
                                    //MessageBox.Show(FLCSV);
                                    if (P2 == "HARIAN")
                                    {
                                        string HDBF = Path.ChangeExtension(FolderTemp + "\\" + P3, ".DBF");
                                        if (Zip(FolderTemp + "\\" + fileName, HDBF))
                                        {
                                            try
                                            {
                                                if (File.Exists(FLCSV))
                                                {
                                                    File.Delete(FLCSV);
                                                }
                                                if (File.Exists(FolderTemp + "\\" + P3))
                                                {
                                                    File.Delete(FolderTemp + "\\" + P3);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                            
                                        }
                                    }
                                    else
                                    {
                                        if (Zip(FolderTemp + "\\" + fileName, FolderTemp + "\\" + P3))
                                        {
                                            if (File.Exists(FLCSV))
                                            {
                                                File.Delete(FLCSV);
                                            }
                                            if (File.Exists(FolderTemp + "\\" + P3))
                                            {
                                                File.Delete(FolderTemp + "\\" + P3);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (File.Exists(file))
                    {
                        File.Move(FolderTemp + "\\" + fileName, FolderHasil + "\\" + fileName);
                        File.Delete(file);
                        File.Delete(FolderTemp + "\\" + fileName);
                    }
                    clearFolder(FolderTemp);
                }
                else
                {
                    string THN = fileName.Substring(4, 2);
                    string BLN = fileName.Substring(6, 2);
                    string KDTK = fileName.Substring(0, 4);
                    string KOMA = "";
                    bool isDBF = false;
                    if(!File.Exists(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDT"))
                    {
                        File.Copy(file, FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDT");
                    }
                    if (UnzipAll(file, FolderTempBln))
                    {
                        if (UnzipAll(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM", FolderTempBln))
                        {
                            //File.Delete(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM");
                            lbProg = 0;
                            foreach (DataRow C in KONVBLN.Rows)
                            {
                                lbProg += 1;
                                string P2 = C[0].ToString();
                                KOMA = C[1].ToString();
                                string P3 = "";
                                if (P2 == "CONST" || P2 == "TOKO" || P2 == "MSTRAN" || P2 == "MTRAN" || P2 == "STMAST" || P2 == "HARIAN")
                                {
                                    P3 = P2 + "." + KDTK.Substring(1, 3);
                                    isDBF = false;
                                }
                                else if (P2 == "BKL" || P2 == "RET" || P2 == "RKL")
                                {
                                    P3 = P2 + THN + BLN + KDTK.Substring(0, 1) + "." + KDTK.Substring(1, 3);
                                    isDBF = false;
                                }
                                else if (P2 == "BYR")
                                {
                                    P3 = P2 + BLN + THN + KDTK.Substring(0, 1) + "." + KDTK.Substring(1, 3);
                                    isDBF = false;
                                }
                                else if (P2 == "PROD")
                                {
                                    P3 = P2 + THN + BLN + "." + KDTK.Substring(1, 3);
                                    isDBF = true;
                                }
                                else if (P2 == "FILET")
                                {
                                    P3 = KDTK + THN + BLN + "." + KDTK.Substring(1, 3);
                                    isDBF = true;
                                }
                                else if (P2 == "LPM")
                                {
                                    P3 = P2 + KDTK + "." + KDTK.Substring(1, 3);
                                    isDBF = true;
                                }
                                else if (P2 == "HPC" || P2 == "HP")
                                {
                                    string A = "";
                                    int B = int.Parse(BLN);
                                    if (B <= 9)
                                    {
                                        A = B.ToString();
                                    }
                                    else if (B == 10)
                                    {
                                        A = "A";
                                    }
                                    else if (B == 11)
                                    {
                                        A = "B";
                                    }
                                    else if (B == 12)
                                    {
                                        A = "C";
                                    }
                                    P3 = P2 + KDTK + A + "F." + KDTK.Substring(1, 3);
                                    isDBF = false;
                                }
                                lbStatus = P3;
                                if (File.Exists(FolderTempBln + "\\" + P3))
                                {
                                    if (P2 == "HP")
                                    {
                                        Zip(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM", FolderTempBln + "\\" + P3);
                                    }
                                    string FLCSV = Path.ChangeExtension(FolderTempBln + "\\" + P3, ".CSV");
                                    if (File.Exists(FLCSV))
                                    {
                                        File.Delete(FLCSV);
                                    }
                                    File.Move(FolderTempBln + "\\" + P3, Path.ChangeExtension(FolderTempBln + "\\" + P3, ".CSV"));
                                    using (DataTable DT = GETCSVTABLE(FLCSV, KOMA))
                                    {
                                        if (CSV2DBF(DT, P2, P3))
                                        {
                                            string FileZIP = "";
                                            if (P2 == "HP")
                                            {
                                                isDBF = true;
                                            }
                                            if (isDBF)
                                            {
                                                File.Move(FolderTempBln + "\\" + P3, Path.ChangeExtension(FolderTempBln + "\\" + P3, ".DBF"));
                                                FileZIP = Path.ChangeExtension(FolderTempBln + "\\" + P3, ".DBF");
                                            }
                                            else
                                            {
                                                FileZIP = FolderTempBln + "\\" + P3;
                                            }
                                            //MessageBox.Show(FolderHasil + "\\" + fileName.Substring(0, 9) + "IDM");
                                            if (P2 == "HPC")
                                            {
                                                File.Move(FLCSV, Path.ChangeExtension(FLCSV, "." + KDTK.Substring(1, 3) + ""));
                                            }
                                            if(P2 == "CONST" || P2 == "TOKO" || P2 == "MSTRAN" || P2 == "MTRAN" || P2 == "STMAST" || P2 == "HARIAN")
                                            {
                                                if (Zip(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM", FileZIP))
                                                {
                                                    if (File.Exists(FileZIP))
                                                    {
                                                        File.Delete(FileZIP);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Zip(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDT", FileZIP))
                                                {
                                                    if (File.Exists(FileZIP))
                                                    {
                                                        File.Delete(FileZIP);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (File.Exists(file))
                    {
                        if (File.Exists(FolderHasil + "\\" + fileName.Substring(0, 9) + "IDT"))
                        {
                            File.Delete(FolderHasil + "\\" + fileName.Substring(0, 9) + "IDT");
                        }
                        if (Zip(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDT", FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM"))
                        {
                            if (File.Exists(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM"))
                            {
                                File.Delete(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDM");
                            }
                        }
                        File.Move(FolderTempBln + "\\" + fileName.Substring(0, 9) + "IDT", FolderHasil + "\\" + fileName.Substring(0, 9) + "IDT");
                        File.Delete(file);
                    }
                    clearFolder(FolderTempBln);
                }
            Lanjut:
                //lblFile.Refresh();
                lbFile = file;
            }
        }

        private string _lbFile;
        private string lbFile
        {
            get
            {
                return this._lbFile;
            }
            set
            {
                this._lbFile = value;
                this.UpdateFile();
            }
        }
        private void UpdateFile()
        {
            bool invokeRequired = this.InvokeRequired;
            if (invokeRequired)
            {
                this.Invoke(new MethodInvoker(this.UpdateFile));
            }
            else
            {
                lblFile.Text = lbFile;
            }
        }

        private string _lbStatus;
        private string lbStatus
        {
            get
            {
                return this._lbStatus;
            }
            set
            {
                this._lbStatus = value;
                this.UpdateStatus();
            }
        }
        private void UpdateStatus()
        {
            bool invokeRequired = this.InvokeRequired;
            if (invokeRequired)
            {
                this.Invoke(new MethodInvoker(this.UpdateStatus));
            }
            else
            {
                lblStatus.Text = lbStatus;
            }
        }

        private int _lbProg;
        private int lbProg
        {
            get
            {
                return this._lbProg;
            }
            set
            {
                this._lbProg = value;
                this.UpdateProg();
            }
        }
        private void UpdateProg()
        {
            bool invokeRequired = this.InvokeRequired;
            if (invokeRequired)
            {
                this.Invoke(new MethodInvoker(this.UpdateProg));
            }
            else
            {
                pgBar2.Value = lbProg;
            }
        }

        private string _lbCount;
        private string lbCount
        {
            get
            {
                return this._lbCount;
            }
            set
            {
                this._lbCount = value;
                this.UpdateCount();
            }
        }
        private void UpdateCount()
        {
            bool invokeRequired = this.InvokeRequired;
            if (invokeRequired)
            {
                this.Invoke(new MethodInvoker(this.UpdateCount));
            }
            else
            {
                //pgBar2.Value = lbProg;
                lblCount.Text = lbCount;
            }
        }



        private void bgWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeSpan timeDiff = DateTime.Now - start;
            lbFile = String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                timeDiff.Days, timeDiff.Hours, timeDiff.Minutes, timeDiff.Seconds);
            lbStatus = "";
            groupSetting.Enabled = true;
        }

        private void btnProses_Click(object sender, EventArgs e)
        {
            Proses();
        }

        private void Log(string Message)
        {
            StreamWriter sw = null;
            string log = AppDomain.CurrentDomain.BaseDirectory + "\\TRACELOG.TXT";
            string x = AppDomain.CurrentDomain.BaseDirectory + "\\TRACE.txt";
            try
            {
                if (File.Exists(x))
                {
                    MessageBox.Show(Message, "IWA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (File.Exists(log))
                {
                    long length = new FileInfo(log).Length;
                    if (length >= 10485760)
                    {
                        File.Delete(log);
                    }
                }
                sw = new StreamWriter(log, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }

        private bool UnzipFile(string filezip, string file, string folder)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(filezip))
                {
                    ZipEntry e = zip[file];
                    e.Extract(folder, ExtractExistingFileAction.OverwriteSilently);
                }
                return true;
            }
            catch (Exception err)
            {
                Log(err.ToString());
                return false;
            }
        }

        private bool Zip(string FileZip, string File)
        {
            try
            {
                using (ZipFile zip = new ZipFile(FileZip))
                {
                    zip.UpdateFile(File, "");
                    zip.Save();
                }
                return true;
            }
            catch (Exception err)
            {
                Log(err.ToString());
                return false;
            }
        }

        public bool UnzipAll(string file, string folder)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(file))
                {
                    foreach (ZipEntry e in zip)
                    {
                        try
                        {
                            e.Extract(folder, ExtractExistingFileAction.OverwriteSilently);
                        }
                        catch (Exception)
                        {
                            //Write ex.ToString() to file and move on...
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool GetFileRes(string NamaFile, string NamaFileAs, string ToFolder)
        {
            try
            {
                Assembly Asm = Assembly.GetExecutingAssembly();
                Stream strm = Asm.GetManifestResourceStream("HR2DBF.FileDBF." + NamaFile);
                BinaryReader br = new BinaryReader(strm);
                FileStream fs = File.Create(ToFolder + "\\" + NamaFileAs);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(br.ReadBytes(checked((int)br.BaseStream.Length)));
                bw.Flush();
                fs.Close();
                bw.Close();
                return true;
            }
            catch (Exception err)
            {
                Log(err.ToString());
                return false;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lbStatus = "-";
            lbFile = "-";
            lbCount = "-";
        }

        private bool CSV2DBF(DataTable DT, string FL, string NAMA)
        {
            try
            {

                //string FILE = Path.Combine(FolderTemp, NAMA);
                string FILE = "";
                string FILEHRN = "";
                string Folder = "";
                if (FL == "HARIAN")
                {
                    if (ProsesHarian)
                    {
                        FILEHRN = "HARIAN.DBF";
                        FILE = Path.Combine(FolderTemp, FILEHRN);
                        Folder = FolderTemp;
                    }
                    else
                    {
                        FILEHRN = NAMA;
                        FILE = Path.Combine(FolderTempBln, FILEHRN);
                        Folder = FolderTempBln;
                    }
                }
                else
                {
                    if (ProsesHarian)
                    {
                        FILE = Path.Combine(FolderTemp, NAMA);
                    }
                    else
                    {
                        FILE = Path.Combine(FolderTempBln, NAMA);
                    }
                }
                string fileName = Path.GetFileName(FILE);
                if (File.Exists(FILE))
                {
                    File.Delete(FILE);
                }
                switch (FL)
                {
                    //private string FL = "CA|ST|PL|WT|DT|SLP|RGL|DBT|CIN|CRT|NTB|WU|TL";
                    #region HARIAN
                    case "HARIAN":
                        if (GetFileRes("HRN.DBF", FILEHRN, Folder))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header);
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                //orec[1] = DateTime.Parse(dr[1].ToString()).ToString("yyyy-MM-dd");
                                orec[1] = DateTime.Parse(dr[1].ToString().Substring(6, 4) + "-" + dr[1].ToString().Substring(3, 2) + "-" + dr[1].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                if (radioBulanan.Checked == false)
                                {
                                    orec[40] = dr[40].ToString();
                                }
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region PL
                    case "PL":
                        if (GetFileRes("PL.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                //orec[1] = DateTime.Parse(dr[1].ToString()).ToString("yyyy-MM-dd");
                                orec[1] = DateTime.Parse(dr[1].ToString().Substring(6, 4) + "-" + dr[1].ToString().Substring(3, 2) + "-" + dr[1].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region CA
                    case "CA":
                        if (GetFileRes("CANCEL.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                if (dr[16].ToString() == "")
                                {
                                    orec[16] = null;
                                }
                                else
                                {
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    //MessageBox.Show(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2));
                                    orec[16] = DateTime.Parse(dr[16].ToString().Substring(6, 4) + "-" + dr[16].ToString().Substring(3, 2) + "-" + dr[16].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region ST
                    case "ST":
                        if (GetFileRes("ST.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                orec[42] = dr[42].ToString();
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                orec[46] = dr[46].ToString();
                                orec[47] = dr[47].ToString();
                                orec[48] = dr[48].ToString();
                                orec[49] = dr[49].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region WT
                    case "WT":
                        if (GetFileRes("WT.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                //MessageBox.Show(dr[24].ToString());
                                orec[20] = DateTime.Parse(dr[20].ToString().Substring(6, 4) + "-" + dr[20].ToString().Substring(3, 2) + "-" + dr[20].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[21] = DateTime.Parse(dr[21].ToString().Substring(6, 4) + "-" + dr[21].ToString().Substring(3, 2) + "-" + dr[21].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                //orec[20] = DateTime.Parse(dr[20].ToString()).ToString("yyyy-MM-dd");
                                //orec[21] = DateTime.Parse(dr[21].ToString()).ToString("yyyy-MM-dd");
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                if (dr[24].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[24] = null;
                                }
                                else
                                {
                                    //orec[24] = DateTime.Parse(dr[24].ToString()).ToString("yyyy-MM-dd");
                                    orec[24] = DateTime.Parse(dr[24].ToString().Substring(6, 4) + "-" + dr[24].ToString().Substring(3, 2) + "-" + dr[24].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region DT
                    case "DT":
                        if (GetFileRes("DT.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                //orec[13] = DateTime.Parse(dr[13].ToString()).ToString("yyyy-MM-dd");
                                orec[13] = DateTime.Parse(dr[13].ToString().Substring(6, 4) + "-" + dr[13].ToString().Substring(3, 2) + "-" + dr[13].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[14] = DateTime.Parse(dr[14].ToString()).ToString("HH:mm:ss");
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region SLP
                    case "SLP":
                        if (GetFileRes("SLP.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                if (dr[4].ToString() == "")
                                {
                                    orec[4] = null;
                                }
                                else
                                {
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    //MessageBox.Show(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2));
                                    orec[4] = DateTime.Parse(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                if (dr[31].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[31] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    orec[31] = DateTime.Parse(dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(3, 2) + "-" + dr[31].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                orec[42] = dr[42].ToString();
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region RGL
                    case "RGL":
                        if (GetFileRes("RGL.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region DBT
                    case "DBT":
                        if (GetFileRes("DBT.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                if (dr[4].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[4] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    orec[4] = DateTime.Parse(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                if (dr[18].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[18] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[18] = DateTime.Parse(dr[18].ToString()).ToString("yyyy-MM-dd");
                                    orec[18] = DateTime.Parse(dr[18].ToString().Substring(6, 4) + "-" + dr[18].ToString().Substring(3, 2) + "-" + dr[18].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[19].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[19] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    orec[19] = DateTime.Parse(dr[19].ToString()).ToString("HH:mm:ss");
                                }
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region CIN
                    case "CIN":
                        if (GetFileRes("CIN.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                if (dr[9].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[9] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[9] = DateTime.Parse(dr[9].ToString()).ToString("yyyy-MM-dd");
                                    orec[9] = DateTime.Parse(dr[9].ToString().Substring(6, 4) + "-" + dr[9].ToString().Substring(3, 2) + "-" + dr[9].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[10] = DateTime.Parse(dr[10].ToString()).ToString("HH:mm:ss");
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region CRT
                    case "CRT":
                        if (GetFileRes("CRT.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                if (dr[4].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[4] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    orec[4] = DateTime.Parse(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region NTB
                    case "NTB":
                        if (GetFileRes("NT.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                //MessageBox.Show(dr[0].ToString());
                                if (dr[3].ToString() == "")
                                {

                                    orec[3] = null;
                                }
                                else
                                {
                                    //orec[3] = DateTime.Parse(dr[3].ToString()).ToString("yyyy-MM-dd");
                                    orec[3] = DateTime.Parse(dr[3].ToString().Substring(6, 4) + "-" + dr[3].ToString().Substring(3, 2) + "-" + dr[3].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region WU
                    case "WU":
                        if (GetFileRes("WU.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                if (dr[14].ToString() == "")
                                {
                                    orec[14] = null;
                                }
                                else
                                {
                                    //orec[14] = DateTime.Parse(dr[14].ToString()).ToString("yyyy-MM-dd");
                                    orec[14] = DateTime.Parse(dr[14].ToString().Substring(6, 4) + "-" + dr[14].ToString().Substring(3, 2) + "-" + dr[14].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region TL
                    case "TL":
                        if (GetFileRes("TL.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                if (dr[9].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[9] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[9].ToString().Substring(6,4) +"-" + dr[9].ToString().Substring(6, 4) + "-" + dr[9].ToString().Substring(0, 2));
                                    //30-11-2015
                                    orec[9] = dr[9].ToString();
                                }
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                if (dr[19].ToString().ToUpper() == "1")
                                {
                                    orec[15] = dr[15].ToString() + "," + dr[16].ToString() + "," + dr[17].ToString();
                                    orec[16] = dr[18].ToString();
                                    orec[17] = dr[19].ToString();
                                    orec[18] = dr[20].ToString();
                                    orec[19] = dr[21].ToString();
                                    //MessageBox.Show(dr[22].ToString());
                                    if (dr[22].ToString() == "")
                                    {
                                        //MessageBox.Show("OK");
                                        orec[20] = null;
                                    }
                                    else
                                    {
                                        //MessageBox.Show(dr[14].ToString());
                                        //MessageBox.Show(dr[20].ToString().Substring(6,4) +"-" + dr[20].ToString().Substring(6, 4) + "-" + dr[20].ToString().Substring(0, 2));
                                        //30-11-2015
                                        //orec[20] = DateTime.Parse(dr[22].ToString()).ToString("yyyy-MM-dd");
                                        //orec[20] = DateTime.Parse(dr[22].ToString().Substring(6, 4) + "-" + dr[22].ToString().Substring(3, 2) + "-" + dr[22].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                        //MessageBox.Show(dr[22].ToString());
                                        orec[20] = dr[22].ToString();
                                        //orec[20] = DateTime.Parse(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                    }
                                    orec[21] = dr[23].ToString();
                                    orec[22] = dr[24].ToString();
                                    orec[23] = dr[25].ToString();
                                    orec[24] = dr[26].ToString();
                                    orec[25] = dr[27].ToString();
                                    orec[26] = dr[28].ToString();
                                }
                                else
                                {
                                    orec[15] = dr[15].ToString() + "," + dr[16].ToString();
                                    orec[16] = dr[17].ToString();
                                    orec[17] = dr[18].ToString();
                                    orec[18] = dr[19].ToString();
                                    orec[19] = dr[20].ToString();
                                    //MessageBox.Show(dr[22].ToString());
                                    if (dr[21].ToString() == "")
                                    {
                                        //MessageBox.Show("OK");
                                        orec[20] = null;
                                    }
                                    else
                                    {
                                        //MessageBox.Show(dr[14].ToString());
                                        //MessageBox.Show(dr[20].ToString().Substring(6,4) +"-" + dr[20].ToString().Substring(6, 4) + "-" + dr[20].ToString().Substring(0, 2));
                                        //30-11-2015
                                        //orec[20] = DateTime.Parse(dr[22].ToString()).ToString("yyyy-MM-dd");
                                        //orec[20] = DateTime.Parse(dr[22].ToString().Substring(6, 4) + "-" + dr[22].ToString().Substring(3, 2) + "-" + dr[22].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                        //MessageBox.Show(dr[22].ToString());
                                        //MessageBox.Show(dr[26].ToString());
                                        orec[20] = dr[21].ToString();
                                        //orec[20] = DateTime.Parse(dr[4].ToString().Substring(6, 4) + "-" + dr[4].ToString().Substring(3, 2) + "-" + dr[4].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                    }
                                    orec[21] = dr[22].ToString();
                                    orec[22] = dr[23].ToString();
                                    orec[23] = dr[24].ToString();
                                    orec[24] = dr[25].ToString();
                                    orec[25] = dr[26].ToString();
                                    //orec[26] = dr[27].ToString();
                                }
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region CONST
                    case "CONST":
                        if (GetFileRes("CONST.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                //orec[1] = DateTime.Parse(dr[1].ToString()).ToString("yyyy-MM-dd");
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                if (dr[5].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[5] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    orec[5] = DateTime.Parse(dr[5].ToString().Substring(6, 4) + "-" + dr[5].ToString().Substring(3, 2) + "-" + dr[5].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[6].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[6] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    orec[6] = DateTime.Parse(dr[6].ToString().Substring(6, 4) + "-" + dr[6].ToString().Substring(3, 2) + "-" + dr[6].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[7].ToString() == "")
                                {
                                    //MessageBox.Show("OK");
                                    orec[7] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[31].ToString().Substring(6,4) +"-" + dr[31].ToString().Substring(6, 4) + "-" + dr[31].ToString().Substring(0, 2));
                                    //30-11-2015
                                    //orec[4] = DateTime.Parse(dr[4].ToString()).ToString("yyyy-MM-dd");
                                    orec[7] = DateTime.Parse(dr[7].ToString().Substring(6, 4) + "-" + dr[7].ToString().Substring(3, 2) + "-" + dr[7].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[8] = dr[8].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region MSTRAN
                    case "MSTRAN":
                        if (GetFileRes("MSTRANV2.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                //orec[5] = "";
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                if (dr[20].ToString() == "")
                                {
                                    orec[20] = null;
                                }
                                else
                                {
                                    orec[20] = DateTime.Parse(dr[20].ToString().Substring(6, 4) + "-" + dr[20].ToString().Substring(3, 2) + "-" + dr[20].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[21].ToString() == "")
                                {
                                    orec[21] = null;
                                }
                                else
                                {
                                    //MessageBox.Show(dr[21].ToString().Substring(6, 4) + "-" + dr[21].ToString().Substring(3, 2) + "-" + dr[21].ToString().Substring(0, 2));
                                    orec[21] = DateTime.Parse(dr[21].ToString().Substring(6, 4) + "-" + dr[21].ToString().Substring(3, 2) + "-" + dr[21].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                //MessageBox.Show(dr[22].ToString());
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                //orec[24] = dr[24].ToString();
                                //MessageBox.Show(dr[24].ToString());
                                //MessageBox.Show(dr[25].ToString());
                                //MessageBox.Show(dr[26].ToString());
                                if (dr[24].ToString() == "")
                                {
                                    orec[24] = null;
                                }
                                else
                                {
                                    orec[24] = DateTime.Parse(dr[24].ToString().Substring(6, 4) + "-" + dr[24].ToString().Substring(3, 2) + "-" + dr[24].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region MTRAN
                    case "MTRAN":
                        if (GetFileRes("MTRAN.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                if (dr[13].ToString() == "")
                                {
                                    orec[13] = null;
                                }
                                else
                                {
                                    orec[13] = DateTime.Parse(dr[13].ToString().Substring(6, 4) + "-" + dr[13].ToString().Substring(3, 2) + "-" + dr[13].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region STMAST
                    case "STMAST":
                        if (GetFileRes("STMAST.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                if (dr[19].ToString() == "")
                                {
                                    orec[19] = null;
                                }
                                else
                                {
                                    orec[19] = DateTime.Parse(dr[19].ToString().Substring(6, 4) + "-" + dr[19].ToString().Substring(3, 2) + "-" + dr[19].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                orec[42] = dr[42].ToString();
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                orec[46] = dr[46].ToString();
                                orec[47] = dr[47].ToString();
                                orec[48] = dr[48].ToString();
                                orec[49] = dr[49].ToString();
                                orec[50] = dr[50].ToString();
                                orec[51] = dr[51].ToString();
                                orec[52] = dr[52].ToString();
                                orec[53] = dr[53].ToString();
                                orec[54] = dr[54].ToString();
                                orec[55] = dr[55].ToString();
                                orec[56] = dr[56].ToString();
                                orec[57] = dr[57].ToString();
                                orec[58] = dr[58].ToString();
                                orec[59] = dr[59].ToString();
                                orec[60] = dr[60].ToString();
                                orec[61] = dr[61].ToString();
                                orec[62] = dr[62].ToString();
                                orec[63] = dr[63].ToString();
                                orec[64] = dr[64].ToString();
                                orec[65] = dr[65].ToString();
                                orec[66] = dr[66].ToString();
                                orec[67] = dr[67].ToString();
                                orec[68] = dr[68].ToString();
                                orec[69] = dr[69].ToString();
                                orec[70] = dr[70].ToString();
                                orec[71] = dr[71].ToString();
                                orec[72] = dr[72].ToString();
                                orec[73] = dr[73].ToString();
                                orec[74] = dr[74].ToString();
                                orec[75] = dr[75].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region LPM
                    case "LPM":
                        if (GetFileRes("LPM.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                //MessageBox.Show(DateTime.Parse(dr[1].ToString().Substring(6, 4) + "-" + dr[1].ToString().Substring(3, 2) + "-" + dr[1].ToString().Substring(0, 2)).ToString("yyyy-MM-dd"));
                                if (dr[1].ToString() == "")
                                {
                                    orec[1] = null;
                                }
                                else
                                {
                                    orec[1] = DateTime.Parse(dr[1].ToString().Substring(6, 4) + "-" + dr[1].ToString().Substring(3, 2) + "-" + dr[1].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();



                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region PRODMAST
                    case "PROD":
                        if (GetFileRes("PROD.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                if (dr[28].ToString() == "")
                                {
                                    orec[28] = null;
                                }
                                else
                                {
                                    orec[28] = DateTime.Parse(dr[28].ToString().Substring(6, 4) + "-" + dr[28].ToString().Substring(3, 2) + "-" + dr[28].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                if (dr[42].ToString() == "")
                                {
                                    orec[42] = null;
                                }
                                else
                                {
                                    orec[42] = DateTime.Parse(dr[42].ToString().Substring(6, 4) + "-" + dr[42].ToString().Substring(3, 2) + "-" + dr[42].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region FILET
                    case "FILET":
                        if (GetFileRes("FILET.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header);
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                orec[42] = dr[42].ToString();
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                orec[46] = dr[46].ToString();
                                orec[47] = dr[47].ToString();
                                orec[48] = dr[48].ToString();
                                orec[49] = dr[49].ToString();
                                orec[50] = dr[50].ToString();
                                orec[51] = dr[51].ToString();
                                orec[52] = dr[52].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region TOKO
                    case "TOKO":
                        if (GetFileRes("TOKO.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                if (dr[21].ToString() == "")
                                {
                                    orec[21] = null;
                                }
                                else
                                {
                                    orec[21] = DateTime.Parse(dr[21].ToString().Substring(6, 4) + "-" + dr[21].ToString().Substring(3, 2) + "-" + dr[21].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                if (dr[33].ToString() == "")
                                {
                                    orec[33] = null;
                                }
                                else
                                {
                                    orec[33] = DateTime.Parse(dr[33].ToString().Substring(6, 4) + "-" + dr[33].ToString().Substring(3, 2) + "-" + dr[33].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[34].ToString() == "")
                                {
                                    orec[34] = null;
                                }
                                else
                                {
                                    orec[34] = DateTime.Parse(dr[34].ToString().Substring(6, 4) + "-" + dr[34].ToString().Substring(3, 2) + "-" + dr[34].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[35] = dr[35].ToString();
                                if (dr[36].ToString() == "")
                                {
                                    orec[36] = null;
                                }
                                else
                                {
                                    orec[36] = DateTime.Parse(dr[36].ToString().Substring(6, 4) + "-" + dr[36].ToString().Substring(3, 2) + "-" + dr[36].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[37].ToString() == "")
                                {
                                    orec[37] = null;
                                }
                                else
                                {
                                    orec[37] = DateTime.Parse(dr[37].ToString().Substring(6, 4) + "-" + dr[37].ToString().Substring(3, 2) + "-" + dr[37].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[38] = dr[38].ToString();
                                orec[39] = dr[39].ToString();
                                orec[40] = dr[40].ToString();
                                orec[41] = dr[41].ToString();
                                orec[42] = dr[42].ToString();
                                orec[43] = dr[43].ToString();
                                orec[44] = dr[44].ToString();
                                orec[45] = dr[45].ToString();
                                orec[46] = dr[46].ToString();
                                orec[47] = dr[47].ToString();
                                orec[48] = dr[48].ToString();
                                orec[49] = dr[49].ToString();
                                orec[50] = dr[50].ToString();
                                orec[51] = dr[51].ToString();
                                orec[52] = dr[52].ToString();
                                orec[53] = dr[53].ToString();
                                orec[54] = dr[54].ToString();
                                orec[55] = dr[55].ToString();
                                orec[56] = dr[56].ToString();
                                orec[57] = dr[57].ToString();
                                orec[58] = dr[58].ToString();
                                orec[59] = dr[59].ToString();
                                orec[60] = dr[60].ToString();
                                orec[61] = dr[61].ToString();
                                orec[62] = dr[62].ToString();
                                orec[63] = dr[63].ToString();
                                orec[64] = dr[64].ToString();
                                orec[65] = dr[65].ToString();
                                orec[66] = dr[66].ToString();
                                orec[67] = dr[67].ToString();
                                orec[68] = dr[68].ToString();
                                orec[69] = dr[69].ToString();
                                orec[70] = dr[70].ToString();
                                orec[71] = dr[71].ToString();
                                orec[72] = dr[72].ToString();
                                orec[73] = dr[73].ToString();
                                orec[74] = dr[74].ToString();
                                orec[75] = dr[75].ToString();
                                orec[76] = dr[76].ToString();
                                orec[77] = dr[77].ToString();
                                orec[78] = dr[78].ToString();
                                orec[79] = dr[79].ToString();
                                orec[80] = dr[80].ToString();
                                orec[81] = dr[81].ToString();
                                orec[82] = dr[82].ToString();
                                orec[83] = dr[83].ToString();
                                orec[84] = dr[84].ToString();
                                if (dr[85].ToString() == "")
                                {
                                    orec[85] = null;
                                }
                                else
                                {
                                    orec[85] = DateTime.Parse(dr[85].ToString().Substring(6, 4) + "-" + dr[85].ToString().Substring(3, 2) + "-" + dr[85].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[86].ToString() == "")
                                {
                                    orec[86] = null;
                                }
                                else
                                {
                                    orec[86] = DateTime.Parse(dr[86].ToString().Substring(6, 4) + "-" + dr[86].ToString().Substring(3, 2) + "-" + dr[86].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[87].ToString() == "")
                                {
                                    orec[87] = null;
                                }
                                else
                                {
                                    orec[87] = DateTime.Parse(dr[87].ToString().Substring(6, 4) + "-" + dr[87].ToString().Substring(3, 2) + "-" + dr[87].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[88] = dr[88].ToString();
                                orec[89] = dr[89].ToString();
                                orec[90] = dr[90].ToString();
                                orec[91] = dr[91].ToString();
                                orec[92] = dr[92].ToString();
                                orec[93] = dr[93].ToString();
                                orec[94] = dr[94].ToString();
                                orec[95] = dr[95].ToString();
                                orec[96] = dr[96].ToString();
                                orec[97] = dr[97].ToString();
                                orec[98] = dr[98].ToString();
                                orec[99] = dr[99].ToString();
                                orec[100] = dr[100].ToString();
                                orec[101] = dr[101].ToString();
                                if (dr[102].ToString() == "")
                                {
                                    orec[102] = null;
                                }
                                else
                                {
                                    orec[102] = DateTime.Parse(dr[102].ToString().Substring(6, 4) + "-" + dr[102].ToString().Substring(3, 2) + "-" + dr[102].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[103] = dr[103].ToString();
                                orec[104] = dr[104].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region RKL
                    case "RKL":
                        if (GetFileRes("RKL.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                if (dr[11].ToString() == "")
                                {
                                    orec[11] = null;
                                }
                                else
                                {
                                    orec[11] = DateTime.Parse(dr[11].ToString().Substring(6, 4) + "-" + dr[11].ToString().Substring(3, 2) + "-" + dr[11].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[12].ToString() == "")
                                {
                                    orec[12] = null;
                                }
                                else
                                {
                                    orec[12] = DateTime.Parse(dr[12].ToString().Substring(6, 4) + "-" + dr[12].ToString().Substring(3, 2) + "-" + dr[12].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region RET
                    case "RET":
                        if (GetFileRes("RET.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                if (dr[11].ToString() == "")
                                {
                                    orec[11] = null;
                                }
                                else
                                {
                                    orec[11] = DateTime.Parse(dr[11].ToString().Substring(6, 4) + "-" + dr[11].ToString().Substring(3, 2) + "-" + dr[11].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[12].ToString() == "")
                                {
                                    orec[12] = null;
                                }
                                else
                                {
                                    orec[12] = DateTime.Parse(dr[12].ToString().Substring(6, 4) + "-" + dr[12].ToString().Substring(3, 2) + "-" + dr[12].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region BKL
                    case "BKL":
                        if (GetFileRes("BKL.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                if (dr[11].ToString() == "")
                                {
                                    orec[11] = null;
                                }
                                else
                                {
                                    orec[11] = DateTime.Parse(dr[11].ToString().Substring(6, 4) + "-" + dr[11].ToString().Substring(3, 2) + "-" + dr[11].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                if (dr[12].ToString() == "")
                                {
                                    orec[12] = null;
                                }
                                else
                                {
                                    orec[12] = DateTime.Parse(dr[12].ToString().Substring(6, 4) + "-" + dr[12].ToString().Substring(3, 2) + "-" + dr[12].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                }
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region HP
                    case "HP":
                        if (GetFileRes("HP.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region pbk
                    case "PBK":
                        if (GetFileRes("PBK.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = DateTime.Parse(dr[7].ToString().Substring(6, 4) + "-" + dr[7].ToString().Substring(3, 2) + "-" + dr[7].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;

                    #endregion
                    #region PR
                    case "PR":
                        if (GetFileRes("PR.DBF", NAMA, FolderTemp))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = dr[0].ToString();
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                orec[25] = dr[25].ToString();
                                orec[26] = dr[26].ToString();
                                orec[27] = dr[27].ToString();
                                orec[28] = dr[28].ToString();
                                orec[29] = dr[29].ToString();
                                orec[30] = dr[30].ToString();
                                orec[31] = dr[31].ToString();
                                orec[32] = dr[32].ToString();
                                orec[33] = dr[33].ToString();
                                orec[34] = dr[34].ToString();
                                orec[35] = dr[35].ToString();
                                orec[36] = dr[36].ToString();
                                orec[37] = dr[37].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                    #endregion
                    #region BYR
                    case "BYR":
                        //MessageBox.Show(NAMA);
                        if (GetFileRes("BAYAR.DBF", NAMA, FolderTempBln))
                        {
                            var odbf = new DbfFile(Encoding.GetEncoding(1252));
                            odbf.Open(FILE, FileMode.Open);
                            var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                            orec.Clear();
                            int A = DT.Rows.Count;
                            int B = 0;
                            foreach (DataRow dr in DT.Rows)
                            {
                                B += 1;
                                lbCount = A.ToString() + "/" + B.ToString();
                                //progressBar1.Value += 1;
                                Application.DoEvents();
                                orec.AllowDecimalTruncate = true;
                                orec.AllowIntegerTruncate = true;
                                orec[0] = DateTime.Parse(dr[0].ToString().Substring(6, 4) + "-" + dr[0].ToString().Substring(3, 2) + "-" + dr[0].ToString().Substring(0, 2)).ToString("yyyy-MM-dd");
                                orec[1] = dr[1].ToString();
                                orec[2] = dr[2].ToString();
                                orec[3] = dr[3].ToString();
                                orec[4] = dr[4].ToString();
                                orec[5] = dr[5].ToString();
                                orec[6] = dr[6].ToString();
                                orec[7] = dr[7].ToString();
                                orec[8] = dr[8].ToString();
                                orec[9] = dr[9].ToString();
                                orec[10] = dr[10].ToString();
                                orec[11] = dr[11].ToString();
                                orec[12] = dr[12].ToString();
                                orec[13] = dr[13].ToString();
                                orec[14] = dr[14].ToString();
                                orec[15] = dr[15].ToString();
                                orec[16] = dr[16].ToString();
                                orec[17] = dr[17].ToString();
                                orec[18] = dr[18].ToString();
                                orec[19] = dr[19].ToString();
                                orec[20] = dr[20].ToString();
                                orec[21] = dr[21].ToString();
                                orec[22] = dr[22].ToString();
                                orec[23] = dr[23].ToString();
                                orec[24] = dr[24].ToString();
                                odbf.Write(orec, true);
                            }
                            orec.Clear();
                            odbf.Close();
                        }
                        break;
                        #endregion

                }
                return true;
            }
            catch (Exception err)
            {
                Log(err.ToString());
                //MessageBox.Show(err.ToString());
                return false;
            }
        }

        private void bgWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgBar1.Value = e.ProgressPercentage;
            //lblProsesInduk.Text = "Progress...." + pgBarInduk.Value.ToString() + " %";
        }

        private void clearFolder(string FolderName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    clearFolder(di.FullName);
                    di.Delete();
                }
            }
            catch (Exception)
            {

            }
        }

        private DataTable GETCSVTABLE(string filename, string koma)
        {
            FileInfo file = new FileInfo(filename);
            DataTable tbl = new DataTable();
            try
            {
                if (File.Exists(file.DirectoryName + "//schema.ini"))
                {
                    File.Delete(file.DirectoryName + "//schema.ini");
                }
                using (StreamReader reader = new StreamReader(filename))
                {
                    //FileStream fs = new FileStream(file.DirectoryName + "//schema.ini", FileMode.Append, FileAccess.Write);
                    using (FileStream fs = new FileStream(file.DirectoryName + "//schema.ini", FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine("[" + file.Name + "]");
                        sw.WriteLine("Format = Delimited(" + koma + ")");
                        if (file.Name.Substring(0, 2) != "TL")
                        {
                            char K = Char.Parse(koma);
                            string B = reader.ReadLine();
                            string[] A = B.Split(K);
                            //MessageBox.Show(B);
                            int D = 1;
                            foreach (string C in A)
                            {
                                sw.WriteLine("Col" + D.ToString() + "=" + C + " Char Width 100");
                                //MessageBox.Show(C);
                                D += 1;
                            }
                        }
                        else
                        {
                            char K = Char.Parse(koma);
                            string B = reader.ReadLine();
                            string[] A = B.Split(K);
                            //MessageBox.Show(B);
                            int D = 1;
                            foreach (string C in A)
                            {
                                sw.WriteLine("Col" + D.ToString() + "=" + C + " Char Width 100");
                                //MessageBox.Show(C);
                                D += 1;
                            }
                            sw.WriteLine("Col28 = A Char Width 100");
                            sw.WriteLine("Col29 = B Char Width 100");
                            sw.WriteLine("Col30 = C Char Width 100");
                            sw.WriteLine("Col31 = D Char Width 100");
                        }
                        sw.Close();
                    }
                }
                if (koma == ",")
                {
                    var fileContents = File.ReadAllText(filename);
                    fileContents = fileContents.Replace("'", "");
                    File.WriteAllText(filename, fileContents);
                }
                using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + file.DirectoryName + "\";Extended Properties =\"Text;HDR=YES;\""))
                {
                    using (OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", file.Name), con))
                    {
                        con.Open();
                        using (OleDbDataAdapter adp = new OleDbDataAdapter(cmd))
                        {
                            adp.Fill(tbl);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Log(err.ToString() + " - " + filename + " - ( " + koma + " ) + ");
            }
            return tbl;
        }
    }
}
