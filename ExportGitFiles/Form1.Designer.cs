namespace ExportGitFiles
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRepoPath = new System.Windows.Forms.TextBox();
            this.btnRepoFind = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.btnSaveFind = new System.Windows.Forms.Button();
            this.btnOpenRepo = new System.Windows.Forms.Button();
            this.btnOpenSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSavePathSave = new System.Windows.Forms.Button();
            this.btnRepoPathSave = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.hdnRepoPath = new System.Windows.Forms.TextBox();
            this.hdnSavedPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkClassSave = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.chkAlwayOnTop = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogClear = new System.Windows.Forms.Button();
            this.dpStart = new System.Windows.Forms.DateTimePicker();
            this.rbtnUncommit = new System.Windows.Forms.RadioButton();
            this.rbtnCommit = new System.Windows.Forms.RadioButton();
            this.dpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtZipPwd = new System.Windows.Forms.TextBox();
            this.chkZipCompession = new System.Windows.Forms.CheckBox();
            this.chkTreeView = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnlDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.AutoWordSelection = true;
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(10, 479);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(761, 280);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "소스 경로";
            // 
            // txtRepoPath
            // 
            this.txtRepoPath.Location = new System.Drawing.Point(105, 8);
            this.txtRepoPath.Name = "txtRepoPath";
            this.txtRepoPath.Size = new System.Drawing.Size(371, 21);
            this.txtRepoPath.TabIndex = 3;
            // 
            // btnRepoFind
            // 
            this.btnRepoFind.Location = new System.Drawing.Point(482, 8);
            this.btnRepoFind.Name = "btnRepoFind";
            this.btnRepoFind.Size = new System.Drawing.Size(75, 23);
            this.btnRepoFind.TabIndex = 4;
            this.btnRepoFind.Text = "찾기";
            this.btnRepoFind.UseVisualStyleBackColor = true;
            this.btnRepoFind.Click += new System.EventHandler(this.btnRepoFind_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(42, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "저장 경로";
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(105, 35);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(371, 21);
            this.txtSavePath.TabIndex = 6;
            // 
            // btnSaveFind
            // 
            this.btnSaveFind.Location = new System.Drawing.Point(482, 35);
            this.btnSaveFind.Name = "btnSaveFind";
            this.btnSaveFind.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFind.TabIndex = 7;
            this.btnSaveFind.Text = "찾기";
            this.btnSaveFind.UseVisualStyleBackColor = true;
            this.btnSaveFind.Click += new System.EventHandler(this.btnSaveFind_Click);
            // 
            // btnOpenRepo
            // 
            this.btnOpenRepo.Location = new System.Drawing.Point(563, 8);
            this.btnOpenRepo.Name = "btnOpenRepo";
            this.btnOpenRepo.Size = new System.Drawing.Size(75, 23);
            this.btnOpenRepo.TabIndex = 14;
            this.btnOpenRepo.Text = " 열기";
            this.btnOpenRepo.UseVisualStyleBackColor = true;
            this.btnOpenRepo.Click += new System.EventHandler(this.btnOpenRepo_Click);
            // 
            // btnOpenSave
            // 
            this.btnOpenSave.Location = new System.Drawing.Point(563, 35);
            this.btnOpenSave.Name = "btnOpenSave";
            this.btnOpenSave.Size = new System.Drawing.Size(75, 23);
            this.btnOpenSave.TabIndex = 15;
            this.btnOpenSave.Text = "열기";
            this.btnOpenSave.UseVisualStyleBackColor = true;
            this.btnOpenSave.Click += new System.EventHandler(this.btnOpenSave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnSavePathSave);
            this.panel1.Controls.Add(this.btnRepoPathSave);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.txtRepoPath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnRepoFind);
            this.panel1.Controls.Add(this.btnOpenSave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSavePath);
            this.panel1.Controls.Add(this.btnOpenRepo);
            this.panel1.Controls.Add(this.btnSaveFind);
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 67);
            this.panel1.TabIndex = 17;
            // 
            // btnSavePathSave
            // 
            this.btnSavePathSave.Location = new System.Drawing.Point(644, 35);
            this.btnSavePathSave.Name = "btnSavePathSave";
            this.btnSavePathSave.Size = new System.Drawing.Size(99, 23);
            this.btnSavePathSave.TabIndex = 25;
            this.btnSavePathSave.Text = "경로 저장";
            this.btnSavePathSave.UseVisualStyleBackColor = true;
            this.btnSavePathSave.Click += new System.EventHandler(this.btnSavePathSave_Click);
            // 
            // btnRepoPathSave
            // 
            this.btnRepoPathSave.Location = new System.Drawing.Point(644, 8);
            this.btnRepoPathSave.Name = "btnRepoPathSave";
            this.btnRepoPathSave.Size = new System.Drawing.Size(99, 23);
            this.btnRepoPathSave.TabIndex = 24;
            this.btnRepoPathSave.Text = "경로 저장";
            this.btnRepoPathSave.UseVisualStyleBackColor = true;
            this.btnRepoPathSave.Click += new System.EventHandler(this.btnRepoPathSave_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(13, 33);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 20);
            this.pictureBox2.TabIndex = 23;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 20);
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // hdnRepoPath
            // 
            this.hdnRepoPath.Location = new System.Drawing.Point(127, 141);
            this.hdnRepoPath.Name = "hdnRepoPath";
            this.hdnRepoPath.Size = new System.Drawing.Size(88, 21);
            this.hdnRepoPath.TabIndex = 20;
            this.hdnRepoPath.Visible = false;
            // 
            // hdnSavedPath
            // 
            this.hdnSavedPath.Location = new System.Drawing.Point(127, 165);
            this.hdnSavedPath.Name = "hdnSavedPath";
            this.hdnSavedPath.Size = new System.Drawing.Size(88, 21);
            this.hdnSavedPath.TabIndex = 19;
            this.hdnSavedPath.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "▼ Processed Log";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(536, 159);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "불러오기";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(761, 254);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(698, 159);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "저장하기";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkClassSave
            // 
            this.chkClassSave.AutoSize = true;
            this.chkClassSave.Checked = true;
            this.chkClassSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClassSave.Location = new System.Drawing.Point(547, 17);
            this.chkClassSave.Name = "chkClassSave";
            this.chkClassSave.Size = new System.Drawing.Size(135, 16);
            this.chkClassSave.TabIndex = 12;
            this.chkClassSave.Text = "class파일 함께 저장";
            this.chkClassSave.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.SteelBlue;
            this.progressBar1.Location = new System.Drawing.Point(255, 316);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(288, 17);
            this.progressBar1.Step = 5;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 13;
            this.progressBar1.Visible = false;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(617, 159);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 16;
            this.btnSelectAll.Text = "전체선택";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // chkAlwayOnTop
            // 
            this.chkAlwayOnTop.AutoSize = true;
            this.chkAlwayOnTop.Location = new System.Drawing.Point(689, 17);
            this.chkAlwayOnTop.Name = "chkAlwayOnTop";
            this.chkAlwayOnTop.Size = new System.Drawing.Size(68, 16);
            this.chkAlwayOnTop.TabIndex = 18;
            this.chkAlwayOnTop.Text = "항상 위 ";
            this.chkAlwayOnTop.UseVisualStyleBackColor = true;
            this.chkAlwayOnTop.CheckedChanged += new System.EventHandler(this.chkAlwayOnTop_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "▼ Git File List";
            // 
            // btnLogClear
            // 
            this.btnLogClear.Location = new System.Drawing.Point(696, 449);
            this.btnLogClear.Name = "btnLogClear";
            this.btnLogClear.Size = new System.Drawing.Size(75, 23);
            this.btnLogClear.TabIndex = 22;
            this.btnLogClear.Text = "로그 삭제";
            this.btnLogClear.UseVisualStyleBackColor = true;
            this.btnLogClear.Click += new System.EventHandler(this.btnLogClear_Click);
            // 
            // dpStart
            // 
            this.dpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpStart.Location = new System.Drawing.Point(52, 5);
            this.dpStart.Name = "dpStart";
            this.dpStart.Size = new System.Drawing.Size(101, 21);
            this.dpStart.TabIndex = 25;
            // 
            // rbtnUncommit
            // 
            this.rbtnUncommit.AutoSize = true;
            this.rbtnUncommit.Checked = true;
            this.rbtnUncommit.Location = new System.Drawing.Point(17, 20);
            this.rbtnUncommit.Name = "rbtnUncommit";
            this.rbtnUncommit.Size = new System.Drawing.Size(155, 16);
            this.rbtnUncommit.TabIndex = 26;
            this.rbtnUncommit.TabStop = true;
            this.rbtnUncommit.Text = "커밋되지 않은 파일 목록";
            this.rbtnUncommit.UseVisualStyleBackColor = true;
            this.rbtnUncommit.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
            // 
            // rbtnCommit
            // 
            this.rbtnCommit.AutoSize = true;
            this.rbtnCommit.Location = new System.Drawing.Point(17, 44);
            this.rbtnCommit.Name = "rbtnCommit";
            this.rbtnCommit.Size = new System.Drawing.Size(115, 16);
            this.rbtnCommit.TabIndex = 27;
            this.rbtnCommit.Text = "커밋된 파일 목록";
            this.rbtnCommit.UseVisualStyleBackColor = true;
            // 
            // dpEnd
            // 
            this.dpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpEnd.Location = new System.Drawing.Point(205, 5);
            this.dpEnd.Name = "dpEnd";
            this.dpEnd.Size = new System.Drawing.Size(105, 21);
            this.dpEnd.TabIndex = 29;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(5, 11);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(41, 12);
            this.lblStart.TabIndex = 30;
            this.lblStart.Text = "시작일";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(159, 11);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(41, 12);
            this.lblEnd.TabIndex = 31;
            this.lblEnd.Text = "종료일";
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.lblEnd);
            this.pnlDate.Controls.Add(this.lblStart);
            this.pnlDate.Controls.Add(this.dpEnd);
            this.pnlDate.Controls.Add(this.dpStart);
            this.pnlDate.Location = new System.Drawing.Point(138, 35);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(312, 31);
            this.pnlDate.TabIndex = 32;
            this.pnlDate.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtZipPwd);
            this.groupBox1.Controls.Add(this.chkZipCompession);
            this.groupBox1.Controls.Add(this.chkTreeView);
            this.groupBox1.Controls.Add(this.pnlDate);
            this.groupBox1.Controls.Add(this.rbtnCommit);
            this.groupBox1.Controls.Add(this.rbtnUncommit);
            this.groupBox1.Controls.Add(this.chkAlwayOnTop);
            this.groupBox1.Controls.Add(this.chkClassSave);
            this.groupBox1.Location = new System.Drawing.Point(10, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(763, 69);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(557, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 12);
            this.label5.TabIndex = 37;
            this.label5.Text = "- 비밀번호:";
            // 
            // txtZipPwd
            // 
            this.txtZipPwd.Location = new System.Drawing.Point(625, 39);
            this.txtZipPwd.Name = "txtZipPwd";
            this.txtZipPwd.Size = new System.Drawing.Size(122, 21);
            this.txtZipPwd.TabIndex = 36;
            // 
            // chkZipCompession
            // 
            this.chkZipCompession.AutoSize = true;
            this.chkZipCompession.Checked = true;
            this.chkZipCompession.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZipCompession.Location = new System.Drawing.Point(465, 43);
            this.chkZipCompession.Name = "chkZipCompession";
            this.chkZipCompession.Size = new System.Drawing.Size(94, 16);
            this.chkZipCompession.TabIndex = 29;
            this.chkZipCompession.Text = "Zip파일 생성";
            this.chkZipCompession.UseVisualStyleBackColor = true;
            this.chkZipCompession.CheckedChanged += new System.EventHandler(this.chkZipCompession_CheckedChanged);
            // 
            // chkTreeView
            // 
            this.chkTreeView.AutoSize = true;
            this.chkTreeView.Location = new System.Drawing.Point(465, 18);
            this.chkTreeView.Name = "chkTreeView";
            this.chkTreeView.Size = new System.Drawing.Size(76, 16);
            this.chkTreeView.TabIndex = 28;
            this.chkTreeView.Text = "트리 보기";
            this.chkTreeView.UseVisualStyleBackColor = true;
            this.chkTreeView.CheckedChanged += new System.EventHandler(this.chkTreeView_CheckedChanged);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(779, 8);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(404, 751);
            this.treeView1.TabIndex = 35;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "JNESS - Patch Manager";
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1191, 767);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.hdnRepoPath);
            this.Controls.Add(this.hdnSavedPath);
            this.Controls.Add(this.btnLogClear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(796, 806);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JNESS - Patch Manager 1.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnSaveFind;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRepoFind;
        private System.Windows.Forms.TextBox txtRepoPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenSave;
        private System.Windows.Forms.Button btnOpenRepo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox hdnSavedPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAlwayOnTop;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkClassSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnLogClear;
        private System.Windows.Forms.DateTimePicker dpStart;
        private System.Windows.Forms.RadioButton rbtnCommit;
        private System.Windows.Forms.RadioButton rbtnUncommit;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dpEnd;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkTreeView;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox hdnRepoPath;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnSavePathSave;
        private System.Windows.Forms.Button btnRepoPathSave;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox chkZipCompession;
        private System.Windows.Forms.TextBox txtZipPwd;
        private System.Windows.Forms.Label label5;
    }
}

