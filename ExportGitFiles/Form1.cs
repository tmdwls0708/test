using LibGit2Sharp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using static ICSharpCode.SharpZipLib.Zip.Compression.Deflater;
using System.Text.RegularExpressions;

namespace ExportGitFiles
{
    public partial class Form1 : Form
    {
        #region FrontViewForm
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }

            base.WndProc(ref message);
        }

        public void ShowWindow()
        {
            WinApi.ShowToFront(this.Handle);
        }
        #endregion

        public enum NewLinePosition
        {
            TOP,
            BOTTOM,
            TOP_AND_BOTTOM
        }


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Font = new Font("굴림", 8, FontStyle.Regular);
                dataGridView1.CellContentClick += DataGridView1_CellContentClick;

                SetFormContextMenuStrip();
                SetRichTextBoxContextMenuStrip();
                SetNotifyIconContextMenuStrip();

                this.Width = 796;

                pictureBox1.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory + @"\Image\folder.png");
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox2.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory + @"\Image\folder.png");
                pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;

                GetRepoPath();
                GetSavePath();

                notifyIcon1.Visible = true;

                this.ActiveControl = txtZipPwd;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //그리드뷰 헤더 클리어
                dataGridView1.Columns.Clear();
                dataGridView1.Controls.RemoveByKey("checkboxHeader");

                //소스 경로를 히든필드에 지정
                hdnRepoPath.Text = txtRepoPath.Text.Trim();

                if (String.IsNullOrEmpty(txtRepoPath.Text.Trim()))
                {
                    throw new ArgumentNullException();
                }

                btnSelectAll.Text = "전체선택";
                btnSelectAll.Tag = null;

                DataTable dt = new DataTable();
                dt.Columns.Add("FileName", typeof(string));
                dt.Columns.Add("FilePath", typeof(string));

                dataGridView1.DataSource = null;

                StartProgress();

                Logging("불러오기 실행 중...", Color.Green);

                bool success = true;

                if (rbtnUncommit.Checked)
                    await Task.Run(() => { dt = LoadUncommitFiles(dt, ref success); });
                else
                    await Task.Run(() => { dt = LoadCommitedFiles(dt, ref success); });

                if (!success) return;

                SetGridViewColumns();

                foreach (DataRow row in dt.DefaultView.ToTable(true).Rows)
                {
                    var index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["FileName"].Value = row["FileName"];
                    dataGridView1.Rows[index].Cells["FilePath"].Value = row["FilePath"];
                }

                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];


                await Task.Run(() => { DrawTreeView(btnSelect.Text, dt); });

                Logging("불러오기가 완료되었습니다.", Color.Green);

                string msg = string.Empty;
                Color color;
                if (rbtnUncommit.Checked)
                {
                    msg = $"{rbtnUncommit.Text} Total Count : {dataGridView1.Rows.Count}개";
                    color = Color.Orange;
                }
                else
                {
                    msg = $"{rbtnCommit.Text} Total Count : {dataGridView1.Rows.Count}개, 검색기간 : {dpStart.Value.Date.ToShortDateString()} ~ {dpEnd.Value.Date.ToShortDateString()}";
                    color = Color.Orange;
                }
                Logging(msg, color, "\r\n", NewLinePosition.BOTTOM);
            }
            catch (ArgumentNullException)
            {
                Logging("지정된 경로가 없습니다.", Color.Red);
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
            finally
            {
                StopProgress();
                notifyIcon1.ShowBalloonTip(3000, "Happy JNESS with CYKL", "불러오기 작업이 종료되었습니다.", ToolTipIcon.Info);
            }
        }

        /// <summary>
        /// 데이타그리드뷰 헤더 체크박스 생성
        /// </summary>
        private void SetGridViewColumns()
        {
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Width = 30;
            checkboxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns.Insert(0, checkboxColumn);

            Rectangle rect = dataGridView1.GetCellDisplayRectangle(0, -1, true);
            //rect.X = rect.Location.X + 8;
            //rect.Y = rect.Location.Y + 3;
            rect.X = 50;
            rect.Y = 4;
            rect.Width = rect.Size.Width;
            rect.Height = rect.Size.Height;

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(15, 15);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            dataGridView1.Controls.Add(checkboxHeader);
            dataGridView1.Columns[0].Frozen = true;

            dataGridView1.Columns.Insert(1, new DataGridViewColumn() { Name = "FileName", HeaderText = "FileName", DataPropertyName = "FileName", CellTemplate = new DataGridViewTextBoxCell() });
            dataGridView1.Columns.Insert(2, new DataGridViewColumn() { Name = "FilePath", HeaderText = "FilePath", DataPropertyName = "FilePath", CellTemplate = new DataGridViewTextBoxCell() });

            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        /// <summary>
        /// 각 셀 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //체크박스
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)((DataGridView)sender).Rows[e.RowIndex].Cells[0];

                    if (chk.Value == null)
                        chk.Value = true;
                    else
                        chk.Value = (bool)chk.Value ? false : true;

                    if (chk.Value != null)
                        SetGridViewStyle(e.RowIndex, (bool)chk.Value);
                }
                //Link 버튼 
                else if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    if ((dataGridView1.Rows[e.RowIndex].Cells[3] as DataGridViewButtonCell).Value.ToString() == "-") return;

                    var path = Path.Combine(hdnSavedPath.Text, dataGridView1.Rows[e.RowIndex].Cells["FilePath"].Value.ToString().Replace("htdocs", "web_data"));
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 데이터그리드뷰 체크박스 선택/해제 스타일 변경
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="check"></param>
        private void SetGridViewStyle(int rowIndex, bool check)
        {
            if (check)
            {
                dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView1.Rows[rowIndex].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            }
            else
            {
                dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                dataGridView1.Rows[rowIndex].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Regular);
            }
        }

        /// <summary>
        /// 소스 경로 열기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRepoFind_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtRepoPath.Text;

            if (fbd.ShowDialog() == DialogResult.OK)
                txtRepoPath.Text = fbd.SelectedPath;
        }

        /// <summary>
        /// 저장 경로 열기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFind_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtSavePath.Text;

            if (fbd.ShowDialog() == DialogResult.OK)
                txtSavePath.Text = fbd.SelectedPath;
        }

        /// <summary>
        /// 헤더 체크박스 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool val = ((CheckBox)sender).Checked ? true : false;

                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];

                chk.Value = val;

                SetGridViewStyle(chk.RowIndex, val);
            }
        }

        /// <summary>
        /// 셀 편집 막기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                dataGridView1.Rows[e.RowIndex].ReadOnly = true;
        }

        /// <summary>
        /// 저장하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var sourceDir = txtRepoPath.Text;
                var saveDir = txtSavePath.Text;

                if (String.IsNullOrEmpty(saveDir))
                {
                    throw new ArgumentNullException();
                }

                int cnt = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)row.Cells[0];
                    if (cell.Value == null) continue;
                    if ((bool)cell.Value) cnt++;
                }
                if (cnt == 0)
                {
                    Logging("선택된 행이 없습니다.", Color.Red);
                    return;
                }

                List<DataGridViewRow> list = new List<DataGridViewRow>();

                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCell cell = row.Cells[1] as DataGridViewCell;
                    string fileName = cell.Value?.ToString();

                    DataGridViewCell cell2 = row.Cells[2] as DataGridViewCell;
                    string filePath = cell2.Value?.ToString();

                    if (fileName.Contains(".class") || (fileName.Contains(".properties") && filePath.Contains("classes")))
                        list.Add(row);
                }
                foreach (DataGridViewRow row in list)
                {
                    dataGridView1.Rows.Remove(row);
                }

                //저장 경로를 히든필드에 지정
                hdnSavedPath.Text = txtSavePath.Text.Trim();

                StartProgress();

                Logging("저장 중...", Color.Green, "\r\n", NewLinePosition.BOTTOM);

                DataTable dt = new DataTable();
                dt.Columns.Add("FileName", typeof(string));
                dt.Columns.Add("FilePath", typeof(string));

                int loadFileCount = 0;
                int classFileCount = 0;
                await Task.Run(() =>
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                        if (chk.Value != null)
                        {
                            if ((bool)chk.Value == true)
                            {
                                string path = string.Empty;
                                string sourcePath = string.Empty;
                                string saveFile = string.Empty;
                                string saveFilePath = string.Empty;
                                try
                                {
                                    path = Path.Combine(dataGridView1.Rows[i].Cells["FilePath"].Value.ToString()
                                                       , dataGridView1.Rows[i].Cells["FileName"].Value.ToString()).Replace("/", "\\");

                                    sourcePath = Path.Combine(sourceDir, path);
                                    saveFile = Path.Combine(saveDir, path.Replace("htdocs", "web_data"));
                                    saveFilePath = Path.Combine(saveDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("htdocs", "web_data"));

                                    if (!Directory.Exists(saveFilePath))
                                        Directory.CreateDirectory(saveFilePath);

                                    File.Copy(sourcePath, saveFile, true);

                                    loadFileCount++;

                                    //트리뷰에서 처리할 DataTable
                                    dt.Rows.Add(dataGridView1.Rows[i].Cells["FileName"].Value.ToString(), dataGridView1.Rows[i].Cells["FilePath"].Value.ToString());

                                    this.Invoke(new Action(delegate { Logging(sourcePath, saveFile, true); }));

                                    //class 파일 저장
                                    string filename = dataGridView1.Rows[i].Cells["FileName"].Value.ToString();
                                    int idx = filename.LastIndexOf('.');
                                    string ext = filename.Substring(idx + 1);
                                     
                                    //클래스 파일 처리 
                                    if (ext.ToLower() == "java" && chkClassSave.Checked)
                                    {
                                        string classSourcePath = Path.Combine(sourceDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));
                                        string classSavePath = Path.Combine(saveDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("htdocs", "web_data").Replace("src", "classes"));

                                        if (!Directory.Exists(classSavePath))
                                            Directory.CreateDirectory(classSavePath);

                                        //자바 파일 내용 읽어오기
                                        string content = File.ReadAllText(sourcePath).Replace(" ", "");
                                        Regex regex = new Regex(@"class(.+)extends|class(.+){|class(.+)implements");
                                        Match match = regex.Match(content);

                                        //자바 파일 안에 클래스 명들 뽑아내기 
                                        MatchCollection matches = Regex.Matches(content, @"class(.+)extends|class(.+){|class(.+)implements");
                                        foreach (Match mm in matches)
                                        {
                                            string classFileName = mm.Value.Replace("class", "").Replace("extends", "").Replace("{", "").Replace("implements", "") + ".class";// (ext.ToLower() == "java" ? ".class" : ".properties");

                                            File.Copy(Path.Combine(classSourcePath, classFileName), Path.Combine(classSavePath, classFileName), true);

                                            classFileCount++;

                                            //트리뷰에서 처리할 DataTable
                                            dt.Rows.Add(classFileName, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));

                                            this.Invoke(new Action(delegate
                                            {
                                                Logging(Path.Combine(classSourcePath, classFileName).Replace("/", "\\"), Path.Combine(classSavePath, classFileName).Replace("/", "\\"), true);

                                                //class 파일 저장 시 datagridview에 row 추가
                                                DataGridViewRow _newRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                                                _newRow.Cells[1].Value = classFileName + " (" + filename + ")";
                                                _newRow.Cells[2].Value = dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");
                                                _newRow.DefaultCellStyle.BackColor = Color.White;
                                                _newRow.DefaultCellStyle.ForeColor = Color.Blue;
                                                _newRow.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

                                                bool isAdd = true;
                                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                                {
                                                    string _fileName = row.Cells["FileName"].Value.ToString();
                                                    string _filePath = row.Cells["FilePath"].Value.ToString();

                                                    string subPath = _fileName + _filePath;
                                                    string classPath = classFileName + dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");

                                                    if (subPath == classPath)
                                                        isAdd = false;
                                                }

                                                if (isAdd)
                                                {
                                                    dataGridView1.Rows.Add(_newRow);
                                                }
                                            }));
                                        }

                                    }
                                    // 프로퍼티 파일 처리 
                                    else if (ext.ToLower() == "properties" && chkClassSave.Checked)
                                    {
                                        string classSourcePath = Path.Combine(sourceDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));
                                        string classSavePath = Path.Combine(saveDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("htdocs", "web_data").Replace("src", "classes"));

                                        if (!Directory.Exists(classSavePath))
                                            Directory.CreateDirectory(classSavePath);                                        
                                        
                                        string classFileName = filename;

                                        File.Copy(Path.Combine(classSourcePath, classFileName), Path.Combine(classSavePath, classFileName), true);

                                        classFileCount++;

                                        //트리뷰에서 처리할 DataTable
                                        dt.Rows.Add(classFileName, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));

                                        this.Invoke(new Action(delegate
                                        {
                                            Logging(Path.Combine(classSourcePath, classFileName).Replace("/", "\\"), Path.Combine(classSavePath, classFileName).Replace("/", "\\"), true);

                                            //class 파일 저장 시 datagridview에 row 추가
                                            DataGridViewRow _newRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                                            _newRow.Cells[1].Value = classFileName + " (" + filename + ")";
                                            _newRow.Cells[2].Value = dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");
                                            _newRow.DefaultCellStyle.BackColor = Color.White;
                                            _newRow.DefaultCellStyle.ForeColor = Color.Blue;
                                            _newRow.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

                                            bool isAdd = true;
                                            foreach (DataGridViewRow row in dataGridView1.Rows)
                                            {
                                                string _fileName = row.Cells["FileName"].Value.ToString();
                                                string _filePath = row.Cells["FilePath"].Value.ToString();

                                                string subPath = _fileName + _filePath;
                                                string classPath = classFileName + dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");

                                                if (subPath == classPath)
                                                    isAdd = false;
                                            }

                                            if (isAdd)
                                            {
                                                dataGridView1.Rows.Add(_newRow);
                                            }
                                        }));
                                    }
                                    #region 구버전 클래스 저장 
                                        //if ((ext.ToLower() == "java" || ext.ToLower() == "properties") && chkClassSave.Checked)
                                        //{
                                        //    string classSourcePath = Path.Combine(sourceDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));
                                        //    string classSavePath = Path.Combine(saveDir, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("htdocs", "web_data").Replace("src", "classes"));
                                        //    string className = filename.Substring(0, idx) + (ext.ToLower() == "java" ? ".class" : ".properties");

                                        //    if (!Directory.Exists(classSavePath))
                                        //        Directory.CreateDirectory(classSavePath);

                                        //    File.Copy(Path.Combine(classSourcePath, className), Path.Combine(classSavePath, className), true);

                                        //    classFileCount++;

                                        //    //트리뷰에서 처리할 DataTable
                                        //    dt.Rows.Add(className, dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes"));

                                        //    this.Invoke(new Action(delegate
                                        //    {
                                        //        Logging(Path.Combine(classSourcePath, className).Replace("/", "\\"), Path.Combine(classSavePath, className).Replace("/", "\\"), true);

                                        //        //class 파일 저장 시 datagridview에 row 추가
                                        //        DataGridViewRow _newRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                                        //        _newRow.Cells[1].Value = className;
                                        //        _newRow.Cells[2].Value = dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");
                                        //        _newRow.DefaultCellStyle.BackColor = Color.White;
                                        //        _newRow.DefaultCellStyle.ForeColor = Color.Blue;
                                        //        _newRow.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

                                        //        bool isAdd = true;
                                        //        foreach (DataGridViewRow row in dataGridView1.Rows)
                                        //        {
                                        //            string _fileName = row.Cells["FileName"].Value.ToString();
                                        //            string _filePath = row.Cells["FilePath"].Value.ToString();

                                        //            string subPath = _fileName + _filePath;
                                        //            string classPath = className + dataGridView1.Rows[i].Cells["FilePath"].Value.ToString().Replace("src", "classes");

                                        //            if (subPath == classPath)
                                        //                isAdd = false;
                                        //        }

                                        //        if (isAdd)
                                        //        {
                                        //            dataGridView1.Rows.Add(_newRow);
                                        //        }
                                        //    }));                                                                               
                                        //}   
                                        #endregion
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                });

                if (dataGridView1.Columns["pathLinkButton"] != null)
                    dataGridView1.Columns.Remove("pathLinkButton");

                if (dataGridView1.Columns["pathLinkButton"] == null)
                {
                    DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                    buttonColumn.HeaderText = "Link";
                    buttonColumn.Name = "pathLinkButton";
                    buttonColumn.Text = "열기";
                    buttonColumn.UseColumnTextForButtonValue = true;

                    dataGridView1.Columns.Insert(3, buttonColumn);

                    if (dataGridView1.Rows.Count > 0)
                        dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["FileName"];

                    dataGridView1.Columns[1].Width = 250;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].Width = 50;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                        var isChecked = chk.Value?.ToString();
                        
                        if ((String.IsNullOrEmpty(isChecked) || isChecked.ToLower() == "false") && row.DefaultCellStyle.ForeColor != Color.Blue)
                        {
                            var buttonCell = (DataGridViewButtonCell)row.Cells[3];
                            buttonCell.UseColumnTextForButtonValue = false;
                            buttonCell.Value = "-";
                        }
                    }
                }

                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.Rows[0].Cells[0].Selected = false;

                await Task.Run(() => {
                    DrawTreeView(btnSave.Text, dt); 

                    if (chkZipCompession.Checked)
                    {
                        string zipPath = CreateZipFile();

                        richTextBox1.SelectionColor = Color.Green;
                        richTextBox1.AppendText($"\r\n[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Zip File 저장 성공]\r\n");

                        richTextBox1.SelectionColor = Color.MediumOrchid;
                        richTextBox1.AppendText("[Zip File Path] : ");

                        richTextBox1.SelectionColor = Color.MediumOrchid;
                        richTextBox1.AppendText(zipPath + "\r\n");
                    }
                });
                Logging("저장이 완료되었습니다."
                        , Color.Green
                        , "\r\n"
                        , NewLinePosition.TOP);
                Logging($"Save Files Total Count : { loadFileCount + classFileCount}개\t[불러오기 파일] : {loadFileCount}개\t[Class 파일] : {classFileCount}개"
                        , Color.Orange
                        , "\r\n"
                        , NewLinePosition.BOTTOM);
            }
            catch (ArgumentNullException)
            {
                Logging("지정된 경로가 없습니다.", Color.Red);
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
            finally
            {
                StopProgress();
                notifyIcon1.ShowBalloonTip(3000, "Happy JNESS with CYKL", "저장하기 작업이 종료되었습니다.", ToolTipIcon.Info);
            }
        }

        #region 로그 
        /// <summary>
        /// 로그 남기기
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="source"></param>
        /// <param name="save"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        private void Logging(string source, string save, bool result)
        {
            string now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (result)
            {
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.AppendText($"[{now} - 성공]\r\n");

                richTextBox1.SelectionColor = Color.White;
                richTextBox1.AppendText($"{source.Substring(0, source.LastIndexOf('\\'))}\\");

                richTextBox1.SelectionColor = Color.Yellow;
                richTextBox1.AppendText($"{source.Substring(source.LastIndexOf('\\') + 1)}");

                richTextBox1.SelectionColor = Color.White;
                richTextBox1.AppendText($"\r\n\t\t\t ↓ ↓ ↓    C O P Y   ↓ ↓ ↓\r\n{save.Substring(0, save.LastIndexOf('\\'))}\\");

                richTextBox1.SelectionColor = Color.Yellow;
                richTextBox1.AppendText($"{save.Substring(save.LastIndexOf('\\') + 1)}");

                richTextBox1.SelectionColor = Color.White;
                richTextBox1.AppendText($"\r\n");
            }
            else
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText($"★★★[{now} - 실패] 【{source}】=>【{save}】\r\n");
            }
        }
        public void Logging(string msg, Color color, string newLine = null, NewLinePosition position = default(NewLinePosition))
        {
            richTextBox1.SelectionColor = color;

            switch (position)
            {
                case NewLinePosition.TOP:
                    richTextBox1.AppendText($"{newLine}[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}] { msg }\r\n");
                    break;
                case NewLinePosition.BOTTOM:
                    richTextBox1.AppendText($"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}] { msg + newLine }\r\n");
                    break;
                case NewLinePosition.TOP_AND_BOTTOM:
                    richTextBox1.AppendText($"{newLine}[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}] { msg + newLine }\r\n");
                    break;
                default:
                    richTextBox1.AppendText($"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}] { msg }\r\n");
                    break;
            }
        }
        #endregion

        #region ProgressBar 
        /// <summary>
        /// 프로그레스바 활성화 
        /// </summary>
        private void StartProgress()
        {
            progressBar1.Visible = true;
            txtRepoPath.Enabled = false;
            txtSavePath.Enabled = false;
            btnRepoFind.Enabled = false;
            btnSaveFind.Enabled = false;
            chkClassSave.Enabled = false;
            btnSelect.Enabled = false;
            btnSave.Enabled = false;
            btnSelectAll.Enabled = false;
            btnLogClear.Enabled = false;
            this.ContextMenuStrip.Enabled = false;
            richTextBox1.ContextMenuStrip.Enabled = false;
            rbtnCommit.Enabled = false;
            rbtnUncommit.Enabled = false;
            dataGridView1.Enabled = false;
            dpStart.Enabled = false;
            dpEnd.Enabled = false;
            btnRepoPathSave.Enabled = false;
            btnSavePathSave.Enabled = false;
            chkZipCompession.Enabled = false;
            txtZipPwd.Enabled = false;

            this.DisableCloseButton();
            notifyIcon1.DoubleClick -= notifyIcon1_DoubleClick;
        }

        /// <summary>
        /// 프로그레스바 비활성화
        /// </summary>
        private void StopProgress()
        {
            progressBar1.Visible = false;
            txtRepoPath.Enabled = true;
            txtSavePath.Enabled = true;
            btnRepoFind.Enabled = true;
            btnSaveFind.Enabled = true;
            chkClassSave.Enabled = true;
            btnSelect.Enabled = true;
            btnSave.Enabled = true;
            btnSelectAll.Enabled = true;
            btnLogClear.Enabled = true;
            this.ContextMenuStrip.Enabled = true;
            richTextBox1.ContextMenuStrip.Enabled = true;
            rbtnCommit.Enabled = true;
            rbtnUncommit.Enabled = true;
            dataGridView1.Enabled = true;
            dpStart.Enabled = true;
            dpEnd.Enabled = true;
            btnRepoPathSave.Enabled = true;
            btnSavePathSave.Enabled = true;
            chkZipCompession.Enabled = true;
            txtZipPwd.Enabled = true;

            this.EnableCloseButton();
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
        }
        #endregion

        /// <summary>
        /// 소스 경로 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenRepo_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(txtRepoPath.Text.Trim());
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 저장 경로 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(txtSavePath.Text.Trim());
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 체크박스 전체 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                //if(dataGridView1.Rows.Count > 0)
                //{
                // toggle 버튼 형식
                string tag = btnSelectAll.Tag == null ? "" : btnSelectAll.Tag.ToString();
                bool check = true;
                if (String.IsNullOrEmpty(tag) || tag == "check")
                {
                    btnSelectAll.Text = "선택해제";
                    btnSelectAll.Tag = "unCheck";
                    check = true;
                }
                else
                {
                    btnSelectAll.Text = "전체선택";
                    btnSelectAll.Tag = "check";
                    check = false;
                }

                (dataGridView1.Controls["checkboxHeader"] as CheckBox).Checked = check;

                //foreach (DataGridViewRow row in dataGridView1.Rows)
                //{
                //    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];

                //    chk.Value = check;
                //}
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
                //}                  
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 항상위 옵션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAlwayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }

        /// <summary>
        /// 로깅 텍스트 추가 시 스크롤바 하단으로 옮기기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        /// <summary>
        /// 로깅 박스 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
        }

        #region ContextMenuStrip
        /// <summary>
        /// 리치텍스트박스 컨텍스트 메뉴 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetRichTextBoxContextMenuStrip()
        {
            ContextMenuStrip context = new ContextMenuStrip();
            ToolStripMenuItem item1 = new ToolStripMenuItem();
            item1.Text = "로그삭제";
            item1.Click += richTextBox_Right_Click;
            context.Items.Add(item1);
            ToolStripMenuItem item2 = new ToolStripMenuItem();
            item2.Text = "전체선택";
            item2.Click += richTextBox_Right_Click;
            context.Items.Add(item2);
            ToolStripMenuItem item3 = new ToolStripMenuItem();
            item3.Text = "복사하기";
            item3.Click += richTextBox_Right_Click;
            context.Items.Add(item3);
            richTextBox1.ContextMenuStrip = context;
            context.Show(richTextBox1, richTextBox1.Location);
        }

        /// <summary>
        /// 리치텍스트박스 컨텍스트 메뉴 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox_Right_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if (item.Text == "로그삭제")
                {
                    richTextBox1.Text = null;
                }
                else if (item.Text == "전체선택")
                {
                    richTextBox1.Focus();
                    richTextBox1.SelectAll();
                }
                else if (item.Text == "복사하기")
                {
                    richTextBox1.Copy();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 폼 컨텍스트 메뉴
        /// </summary>
        private void SetFormContextMenuStrip()
        {
            ContextMenuStrip context = new ContextMenuStrip();
            ToolStripMenuItem item1 = new ToolStripMenuItem();
            item1.Name = "0";
            item1.Text = "불러오기";
            item1.Click += Form_Right_Click;
            //item1.ShortcutKeys = Keys.Alt | Keys.R;
            item1.ShortcutKeys = Keys.F1;
            context.Items.Add(item1);
            ToolStripMenuItem item2 = new ToolStripMenuItem();
            item2.Name = "1";
            item2.Text = "전체선택";
            item2.Click += Form_Right_Click;
            item2.ShortcutKeys = Keys.F2;
            context.Items.Add(item2);
            ToolStripMenuItem item3 = new ToolStripMenuItem();
            item3.Name = "2";
            item3.Text = "선택해제";
            item3.Click += Form_Right_Click;
            item3.ShortcutKeys = Keys.F3;
            context.Items.Add(item3);
            ToolStripMenuItem item4 = new ToolStripMenuItem();
            item4.Name = "3";
            item4.Text = "저장하기";
            item4.Click += Form_Right_Click;
            item4.ShortcutKeys = Keys.F4;
            context.Items.Add(item4);

            context.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem item7 = new ToolStripMenuItem();
            item7.Name = "6";
            item7.Text = "트리 보기";
            item7.Click += Form_Right_Click;
            item7.ShortcutKeys = Keys.F5;
            context.Items.Add(item7);
            ToolStripMenuItem item6 = new ToolStripMenuItem();
            item6.Name = "5";
            item6.Text = "class 파일 함께 저장";
            item6.Click += Form_Right_Click;
            item6.ShortcutKeys = Keys.F6;
            context.Items.Add(item6);
            ToolStripMenuItem item5 = new ToolStripMenuItem();
            item5.Name = "4";
            item5.Text = "항상 위";
            item5.Click += Form_Right_Click;
            item5.ShortcutKeys = Keys.F7;
            //item5.ShortcutKeys = (Keys)Shortcut.F1;
            context.Items.Add(item5);

            this.ContextMenuStrip = context;
            context.Show(this, this.Location);
        }

        /// <summary>
        /// 그리드뷰 컨텍스트 메뉴 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Right_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                int idx = int.Parse(item.Name);
                switch (idx)
                {
                    case 0:
                        button1_Click(null, null);
                        break;
                    case 1:
                        (dataGridView1.Controls["checkboxHeader"] as CheckBox).Checked = true;
                        break;
                    case 2:
                        (dataGridView1.Controls["checkboxHeader"] as CheckBox).Checked = false;
                        break;
                    case 3:
                        btnSave_Click(null, null);
                        break;
                    case 4:
                        chkAlwayOnTop.Checked = !chkAlwayOnTop.Checked;
                        break;
                    case 5:
                        chkClassSave.Checked = !chkClassSave.Checked;
                        break;
                    case 6:
                        chkTreeView.Checked = !chkTreeView.Checked;
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 트레이아이콘 컨텍스트 메뉴 
        /// </summary>
        private void SetNotifyIconContextMenuStrip()
        {
            ContextMenuStrip context = new ContextMenuStrip();
            ToolStripMenuItem item1 = new ToolStripMenuItem();
            item1.Name = "0";
            item1.Text = "JNESS - Patch Manager 열기";
            item1.Click += NotifyIcon_Right_Click;
            context.Items.Add(item1);
            ToolStripMenuItem item2 = new ToolStripMenuItem();
            item2.Name = "1";
            item2.Text = "JNESS - Patch Manager 끝내기";
            item2.Click += NotifyIcon_Right_Click;
            context.Items.Add(item2);
            notifyIcon1.ContextMenuStrip = context;
            context.Show(this, this.Location);
        }

        /// <summary>
        /// 트레이아이콘 컨텍스트 메뉴 이벤트 처리 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_Right_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int idx = int.Parse(item.Name);
            switch (idx)
            {
                case 0:
                    notifyIcon1_DoubleClick(null, null);
                    break;
                case 1:
                    Application.Exit();
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Uncommit files, Commit files 옵션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnUncommit.Checked)
                pnlDate.Visible = false;
            else
                pnlDate.Visible = true;
        }

        /// <summary>
        /// 커밋되지 않은 파일 불러오기
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable LoadUncommitFiles(DataTable dt, ref bool result)
        {
            try
            {
                var sourceDir = txtRepoPath.Text;

                using (var repo = new Repository(sourceDir))
                {
                    //if (Directory.Exists(saveDir))
                    //    Directory.Delete(saveDir, true);                    

                    this.Invoke(new Action(delegate { Logging("Repository 탐색 중...", Color.Green); }));
                    int cnt = 0;
                    foreach (var item in repo.RetrieveStatus())
                    {
                        if (item.State == FileStatus.ModifiedInWorkdir
                            || item.State == (FileStatus.ModifiedInWorkdir | FileStatus.ModifiedInIndex)
                            || item.State == FileStatus.NewInWorkdir
                            || item.State == FileStatus.TypeChangeInWorkdir
                            || item.State == FileStatus.RenamedInWorkdir
                            || item.State == FileStatus.ModifiedInIndex)
                        {

                            if (cnt == 0) this.Invoke(new Action(delegate { Logging("데이터 처리 중...", Color.Green); }));
                            cnt++;

                            string filename = string.Empty;
                            string filepath = string.Empty;

                            if (item.FilePath.Contains("/"))
                            {
                                int idx = item.FilePath.LastIndexOf("/");

                                filename = item.FilePath.Substring(idx + 1);
                                filepath = item.FilePath.Substring(0, idx);
                            }
                            else
                            {
                                filename = item.FilePath;
                                filepath = "";
                            }

                            dt.Rows.Add(filename, filepath);
                            #region
                            //var patch = repo.Diff.Compare<Patch>(new List<string>() { item.FilePath });
                            //var blob = repo.Head.Tip[item.FilePath].Target as Blob;
                            //string commitContent;
                            //using (var content = new StreamReader(blob.GetContentStream(), Encoding.UTF8))
                            //{
                            //    commitContent = content.ReadToEnd();
                            //}
                            //string workingContent;
                            //using (var content = new StreamReader(repo.Info.WorkingDirectory + Path.DirectorySeparatorChar + item.FilePath, Encoding.UTF8))
                            //{
                            //    workingContent = content.ReadToEnd();
                            //}

                            //Console.WriteLine("~~~~ Patch file ~~~~");
                            //Console.WriteLine(patch.Content);
                            //Console.WriteLine("\n\n~~~~ Original file ~~~~");
                            //Console.WriteLine(commitContent);
                            //Console.WriteLine("\n\n~~~~ Current file ~~~~");
                            //Console.WriteLine(workingContent);
                            #endregion
                        }
                    }
                }
                result = true;
            }
            catch (RepositoryNotFoundException)
            {
                this.Invoke(new Action(delegate { Logging("올바른 소스 경로를 지정해주세요.", Color.Red); }));
                result = false;
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(delegate { Logging(ex.Message, Color.Red); }));
                result = false;
            }

            return dt;
        }

        /// <summary>
        /// 커밋된 파일 불러오기
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable LoadCommitedFiles(DataTable dt, ref bool result)
        {
            try
            {
                var sourceDir = txtRepoPath.Text;
                using (var repo = new Repository(sourceDir))
                {
                    this.Invoke(new Action(delegate { Logging("Repository 탐색 중...", Color.Green); }));
                    int cnt = 0;

                    DateTime start = dpStart.Value.Date;
                    DateTime end = dpEnd.Value.Date;
                 
                    foreach (Commit commit in repo.Commits)
                    {

                        if (cnt == 0) this.Invoke(new Action(delegate { Logging("데이터 처리 중...", Color.Green); }));
                        cnt++;

                        foreach (var parent in commit.Parents)
                        {
                            try
                            {
                                foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
                                {
                                    if (change.Status == ChangeKind.Unmodified) continue;

                                    DateTime commitDate = commit.Author.When.Date;
                                    if (BoolDateBetween(commitDate, start, end))
                                    {
                                        string filename = string.Empty;
                                        string filepath = string.Empty;

                                        if (change.Path.Contains("/"))
                                        {
                                            int idx = change.Path.LastIndexOf("/");

                                            filename = change.Path.Substring(idx + 1);
                                            filepath = change.Path.Substring(0, idx);
                                        }
                                        else
                                        {
                                            filename = change.Path;
                                            filepath = "";
                                        }
                                        dt.Rows.Add(filename, filepath);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string s = ex.Message;
                            }                            
                        }                    
                    }     
                }
                result = true;
            }
            catch (RepositoryNotFoundException ex)
            {
                this.Invoke(new Action(delegate { Logging("올바른 소스 경로를 지정해주세요.", Color.Red); }));
                result = false;
            }
            catch (LibGit2SharpException ex)
            {
                result = false;
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(delegate { Logging(ex.Message, Color.Red); }));
                result = false;
            }
            return dt;
        }

        /// <summary>
        /// 파일의 커밋 날짜가 검색 범위 내에 있는지 체크
        /// </summary>
        /// <param name="commitDate"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool BoolDateBetween(DateTime commitDate, DateTime start, DateTime end)
        {
            return commitDate >= start && commitDate <= end;
        }

        /// <summary>
        /// 트리뷰 레이아웃 보임/숨김
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTreeView_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTreeView.Checked)
            {
                this.Width = 1207;
            }
            else
            {
                this.Width = 796;
            }
        }

        /// <summary>
        /// 트리뷰 그리기
        /// </summary>
        /// <param name="dt"></param>
        private void DrawTreeView(string eventName, DataTable dt = null)
        {

            this.Invoke(new Action(delegate
            {
                //노드 아이콘
                ImageList img = new ImageList();
                img.Images.Add("directory", Image.FromFile(System.Environment.CurrentDirectory + @"\Image\directory.png"));
                img.Images.Add("file", Image.FromFile(System.Environment.CurrentDirectory + @"\Image\file.png"));
                img.Images.Add("select", Image.FromFile(System.Environment.CurrentDirectory + @"\Image\select.png"));
                img.Images.Add("save", Image.FromFile(System.Environment.CurrentDirectory + @"\Image\save.png"));
                img.Images.Add("egg", Image.FromFile(System.Environment.CurrentDirectory + @"\Image\egg.png"));
                treeView1.ImageList = img;

                int eventNodeIndex = eventName == btnSelect.Text ? 0 : 1;
                string imageName = eventName == btnSelect.Text ? "select" : "save";
                string parentPath = eventName == btnSelect.Text ? hdnRepoPath.Text : hdnSavedPath.Text;

                if (treeView1.Nodes.Find(eventName, false).Length > 0)
                    treeView1.Nodes[eventNodeIndex].Nodes.Clear();
                else
                {
                    TreeNode topNode = new TreeNode();
                    topNode.Name = eventName;
                    topNode.Text = eventName;
                    topNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                    topNode.ImageKey = imageName;
                    topNode.SelectedImageKey = imageName;
                    treeView1.Nodes.Add(topNode);
                }

                TreeNode lastNode = null;
                string subPathAgg;
                foreach (DataRow row in dt.Rows)
                {
                    string childPath = string.Empty;

                    if (eventName == btnSelect.Text)
                    {
                        childPath = row["FilePath"].ToString().Replace("/", "\\");
                    }
                    else
                    {
                        childPath = row["FilePath"].ToString().Replace("/", "\\").Replace("htdocs", "web_data");
                    }

                    string path = Path.Combine(childPath, row["FileName"].ToString());

                    subPathAgg = string.Empty;
                    string[] subPath = path.Split('\\');

                    //루트가 추가되어 있지 않으면 새 노드로 시작
                    if (!treeView1.Nodes[eventNodeIndex].Nodes.ContainsKey(subPath[0]))
                        lastNode = null;

                    for (int i = 0; i < subPath.Length; i++)
                    {
                        subPathAgg += subPath.Length > 2 ? subPath[i] : subPath[i] + '\\';
                        TreeNode[] nodes = treeView1.Nodes[eventNodeIndex].Nodes.Find(subPathAgg, true);
                        if (nodes.Length == 0)
                            if (lastNode == null)
                                lastNode = treeView1.Nodes[eventNodeIndex].Nodes.Add(subPathAgg, subPath[i]);
                            else
                                lastNode = lastNode.Nodes.Add(subPathAgg, subPath[i]);
                        else
                            lastNode = nodes[0];

                        if (i == subPath.Length - 1)
                        {
                            string filename = row["FileName"].ToString();
                            int idx = filename.LastIndexOf('.');
                            string ext = filename.Substring(idx + 1);

                            string lastNodeImage = ext.ToLower() == "class" ? "egg" : "file";

                            lastNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                            lastNode.ImageKey = lastNodeImage;
                            lastNode.SelectedImageKey = lastNodeImage;
                            lastNode.ToolTipText = Path.Combine(parentPath, childPath, row["FileName"].ToString());
                            lastNode.Tag = Path.Combine(parentPath, childPath);
                        }
                    }
                }
                treeView1.ExpandAll();
            }));
        }

        /// <summary>
        /// 파일명 트리노드를 더블 클릭하면 경로 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                System.Diagnostics.Process.Start(e.Node.Tag.ToString());
            }
        }

        /// <summary>
        /// 소스경로 저장하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRepoPathSave_Click(object sender, EventArgs e)
        {
            try
            {
                string regSubkey = @"MadeInSJKim";

                RegistryKey rk = Registry.CurrentUser.OpenSubKey(regSubkey, true);
                if (rk == null)
                {
                    rk = Registry.CurrentUser.CreateSubKey(regSubkey);
                }
                string repoPath = txtRepoPath.Text.Trim();
                rk.SetValue("repoPath", repoPath);

                Logging($"소스경로가 저장되었습니다. Path [{txtRepoPath.Text.Trim()}]", Color.Green);
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
                throw;
            }
        }

        /// <summary>
        /// 소스경로 불러오기 
        /// </summary>
        private void GetRepoPath()
        {
            try
            {
                string regSubkey = @"MadeInSJKim";
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(regSubkey, true);
                if (rk != null) txtRepoPath.Text = rk.GetValue("repoPath")?.ToString();
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 저장경로 저장하기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePathSave_Click(object sender, EventArgs e)
        {
            try
            {
                string regSubkey = @"MadeInSJKim";

                RegistryKey rk = Registry.CurrentUser.OpenSubKey(regSubkey, true);
                if (rk == null)
                {
                    rk = Registry.CurrentUser.CreateSubKey(regSubkey);
                }
                string savedPath = txtSavePath.Text.Trim();
                rk.SetValue("savePath", savedPath);

                Logging($"저장경로가 저장되었습니다. Path [{txtSavePath.Text.Trim()}]", Color.Green);
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 저장경로 불러오기
        /// </summary>
        private void GetSavePath()
        {
            try
            {
                string regSubkey = @"MadeInSJKim";
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(regSubkey, true);
                if (rk != null) txtSavePath.Text = rk.GetValue("savePath")?.ToString();
            }
            catch (Exception ex)
            {
                Logging(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// 트레이아이콘 더블클릭 이벤트 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {     
            if (this.Visible && WindowState != FormWindowState.Minimized)
            {
                this.Hide();
            }
            else
            {
                //this.Show();
                WindowState = FormWindowState.Normal;
                ShowWindow();
            }
        }

        /// <summary>
        /// 폼 X 버튼 눌렀을 때 이벤트 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        /// <summary>
        /// 트레이 아이콘 툴팁 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            //this.Show();            
            WindowState = FormWindowState.Normal;
            ShowWindow();
        }

        /// <summary>
        /// ZIP파일 생성하기
        /// </summary>
        private string CreateZipFile()
        {
            string zipPath = txtSavePath.Text.Trim().Substring(0, txtSavePath.Text.Trim().LastIndexOfAny(new char[] { '\\', '/' }));
            string zipName = txtSavePath.Text.Trim().Substring(txtSavePath.Text.Trim().LastIndexOfAny(new char[] { '\\', '/' }) + 1) + ".zip";

            if (File.Exists(Path.Combine(zipPath, zipName)))
                File.Delete(Path.Combine(zipPath, zipName));

            FastZip zip = new FastZip();
            zip.CreateEmptyDirectories = true;
            zip.Password = txtZipPwd.Text.Trim();
            //zip.UseZip64 = UseZip64.On; 이 옵션을 사용하면 리눅스에서 해제가 안됨.
            zip.CreateZip(Path.Combine(zipPath, zipName), txtSavePath.Text.Trim(), true, "");          

            return Path.Combine(zipPath, zipName);
        }

        private void chkZipCompession_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            if (chk.Checked)
            {
                txtZipPwd.Enabled = true;
                txtZipPwd.Focus();
            }                
            else
                txtZipPwd.Enabled = false;
        }
    }

    public static class FormCloseButtonControl
    {
        [DllImport("user32")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);

        /// <summary>
        /// 닫기 버튼 비활성화
        /// </summary>
        /// <param name="form"></param>
        public static void DisableCloseButton(this Form form)
        {
            EnableMenuItem(GetSystemMenu(form.Handle, false), 0xF060, 1);
        }

        /// <summary>
        /// 닫기 버튼 활성화
        /// </summary>
        /// <param name="form"></param>
        public static void EnableCloseButton(this Form form)
        {
            EnableMenuItem(GetSystemMenu(form.Handle, false), 0xF060, 0);
        }
    }
}
