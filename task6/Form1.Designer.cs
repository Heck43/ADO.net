namespace task6;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }


    private void InitializeComponent()
    {
        dgvClients = new DataGridView();
        tabControl1 = new TabControl();
        tabAdd = new TabPage();
        lblAddName = new Label();
        txtAddFirstName = new TextBox();
        lblAddLastName = new Label();
        txtAddLastName = new TextBox();
        lblAddBirth = new Label();
        dtpAddBirthDate = new DateTimePicker();
        lblAddEmail = new Label();
        txtAddEmail = new TextBox();
        lblAddPhone = new Label();
        txtAddPhone = new TextBox();
        btnAdd = new Button();
        tabUpdate = new TabPage();
        lblUpId = new Label();
        txtUpdateId = new TextBox();
        lblUpEmail = new Label();
        txtUpdateEmail = new TextBox();
        lblUpPhone = new Label();
        txtUpdatePhone = new TextBox();
        btnUpdate = new Button();
        tabDelete = new TabPage();
        lblDelId = new Label();
        txtDeleteId = new TextBox();
        btnDelete = new Button();
        panelTop = new Panel();
        btnShowAll = new Button();
        btnShow21 = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
        tabControl1.SuspendLayout();
        tabAdd.SuspendLayout();
        tabUpdate.SuspendLayout();
        tabDelete.SuspendLayout();
        panelTop.SuspendLayout();
        SuspendLayout();
        // 
        // dgvClients
        // 
        dgvClients.AllowUserToAddRows = false;
        dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvClients.Dock = DockStyle.Fill;
        dgvClients.Location = new Point(0, 240);
        dgvClients.Name = "dgvClients";
        dgvClients.ReadOnly = true;
        dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvClients.Size = new Size(880, 360);
        dgvClients.TabIndex = 0;
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabAdd);
        tabControl1.Controls.Add(tabUpdate);
        tabControl1.Controls.Add(tabDelete);
        tabControl1.Dock = DockStyle.Top;
        tabControl1.Location = new Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(880, 190);
        tabControl1.TabIndex = 2;
        // 
        // tabAdd
        // 
        tabAdd.Controls.Add(lblAddName);
        tabAdd.Controls.Add(txtAddFirstName);
        tabAdd.Controls.Add(lblAddLastName);
        tabAdd.Controls.Add(txtAddLastName);
        tabAdd.Controls.Add(lblAddBirth);
        tabAdd.Controls.Add(dtpAddBirthDate);
        tabAdd.Controls.Add(lblAddEmail);
        tabAdd.Controls.Add(txtAddEmail);
        tabAdd.Controls.Add(lblAddPhone);
        tabAdd.Controls.Add(txtAddPhone);
        tabAdd.Controls.Add(btnAdd);
        tabAdd.Location = new Point(4, 24);
        tabAdd.Name = "tabAdd";
        tabAdd.Size = new Size(872, 162);
        tabAdd.TabIndex = 0;
        tabAdd.Text = "Добавить клиента";
        // 
        // lblAddName
        // 
        lblAddName.AutoSize = true;
        lblAddName.Location = new Point(15, 20);
        lblAddName.Name = "lblAddName";
        lblAddName.Size = new Size(34, 15);
        lblAddName.TabIndex = 0;
        lblAddName.Text = "Имя:";
        // 
        // txtAddFirstName
        // 
        txtAddFirstName.Location = new Point(15, 40);
        txtAddFirstName.Name = "txtAddFirstName";
        txtAddFirstName.Size = new Size(140, 23);
        txtAddFirstName.TabIndex = 1;
        // 
        // lblAddLastName
        // 
        lblAddLastName.AutoSize = true;
        lblAddLastName.Location = new Point(170, 20);
        lblAddLastName.Name = "lblAddLastName";
        lblAddLastName.Size = new Size(61, 15);
        lblAddLastName.TabIndex = 2;
        lblAddLastName.Text = "Фамилия:";
        // 
        // txtAddLastName
        // 
        txtAddLastName.Location = new Point(170, 40);
        txtAddLastName.Name = "txtAddLastName";
        txtAddLastName.Size = new Size(140, 23);
        txtAddLastName.TabIndex = 3;
        // 
        // lblAddBirth
        // 
        lblAddBirth.AutoSize = true;
        lblAddBirth.Location = new Point(325, 20);
        lblAddBirth.Name = "lblAddBirth";
        lblAddBirth.Size = new Size(93, 15);
        lblAddBirth.TabIndex = 4;
        lblAddBirth.Text = "Дата рождения:";
        // 
        // dtpAddBirthDate
        // 
        dtpAddBirthDate.Format = DateTimePickerFormat.Short;
        dtpAddBirthDate.Location = new Point(325, 40);
        dtpAddBirthDate.Name = "dtpAddBirthDate";
        dtpAddBirthDate.Size = new Size(140, 23);
        dtpAddBirthDate.TabIndex = 5;
        // 
        // lblAddEmail
        // 
        lblAddEmail.AutoSize = true;
        lblAddEmail.Location = new Point(480, 20);
        lblAddEmail.Name = "lblAddEmail";
        lblAddEmail.Size = new Size(39, 15);
        lblAddEmail.TabIndex = 6;
        lblAddEmail.Text = "Email:";
        // 
        // txtAddEmail
        // 
        txtAddEmail.Location = new Point(480, 40);
        txtAddEmail.Name = "txtAddEmail";
        txtAddEmail.Size = new Size(160, 23);
        txtAddEmail.TabIndex = 7;
        // 
        // lblAddPhone
        // 
        lblAddPhone.AutoSize = true;
        lblAddPhone.Location = new Point(655, 20);
        lblAddPhone.Name = "lblAddPhone";
        lblAddPhone.Size = new Size(58, 15);
        lblAddPhone.TabIndex = 8;
        lblAddPhone.Text = "Телефон (+7, 10-15 цифр):";
        // 
        // txtAddPhone
        // 
        txtAddPhone.Location = new Point(655, 40);
        txtAddPhone.Name = "txtAddPhone";
        txtAddPhone.Size = new Size(150, 23);
        txtAddPhone.TabIndex = 9;
        // 
        // btnAdd
        // 
        btnAdd.Location = new Point(15, 80);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(180, 35);
        btnAdd.TabIndex = 10;
        btnAdd.Text = "Добавить клиента";
        btnAdd.Click += BtnAdd_Click;
        // 
        // tabUpdate
        // 
        tabUpdate.Controls.Add(lblUpId);
        tabUpdate.Controls.Add(txtUpdateId);
        tabUpdate.Controls.Add(lblUpEmail);
        tabUpdate.Controls.Add(txtUpdateEmail);
        tabUpdate.Controls.Add(lblUpPhone);
        tabUpdate.Controls.Add(txtUpdatePhone);
        tabUpdate.Controls.Add(btnUpdate);
        tabUpdate.Location = new Point(4, 24);
        tabUpdate.Name = "tabUpdate";
        tabUpdate.Size = new Size(872, 162);
        tabUpdate.TabIndex = 1;
        tabUpdate.Text = "Обновить клиента";
        // 
        // lblUpId
        // 
        lblUpId.AutoSize = true;
        lblUpId.Location = new Point(15, 20);
        lblUpId.Name = "lblUpId";
        lblUpId.Size = new Size(68, 15);
        lblUpId.TabIndex = 0;
        lblUpId.Text = "ID клиента:";
        // 
        // txtUpdateId
        // 
        txtUpdateId.Location = new Point(15, 40);
        txtUpdateId.Name = "txtUpdateId";
        txtUpdateId.Size = new Size(100, 23);
        txtUpdateId.TabIndex = 1;
        // 
        // lblUpEmail
        // 
        lblUpEmail.AutoSize = true;
        lblUpEmail.Location = new Point(130, 20);
        lblUpEmail.Name = "lblUpEmail";
        lblUpEmail.Size = new Size(80, 15);
        lblUpEmail.TabIndex = 2;
        lblUpEmail.Text = "Новый Email:";
        // 
        // txtUpdateEmail
        // 
        txtUpdateEmail.Location = new Point(130, 40);
        txtUpdateEmail.Name = "txtUpdateEmail";
        txtUpdateEmail.Size = new Size(180, 23);
        txtUpdateEmail.TabIndex = 3;
        // 
        // lblUpPhone
        // 
        lblUpPhone.AutoSize = true;
        lblUpPhone.Location = new Point(330, 20);
        lblUpPhone.Name = "lblUpPhone";
        lblUpPhone.Size = new Size(99, 15);
        lblUpPhone.TabIndex = 4;
        lblUpPhone.Text = "Новый Телефон:";
        // 
        // txtUpdatePhone
        // 
        txtUpdatePhone.Location = new Point(330, 40);
        txtUpdatePhone.Name = "txtUpdatePhone";
        txtUpdatePhone.Size = new Size(180, 23);
        txtUpdatePhone.TabIndex = 5;
        // 
        // btnUpdate
        // 
        btnUpdate.Location = new Point(15, 80);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(180, 35);
        btnUpdate.TabIndex = 6;
        btnUpdate.Text = "Обновить клиента";
        btnUpdate.Click += BtnUpdate_Click;
        // 
        // tabDelete
        // 
        tabDelete.Controls.Add(lblDelId);
        tabDelete.Controls.Add(txtDeleteId);
        tabDelete.Controls.Add(btnDelete);
        tabDelete.Location = new Point(4, 24);
        tabDelete.Name = "tabDelete";
        tabDelete.Size = new Size(872, 162);
        tabDelete.TabIndex = 2;
        tabDelete.Text = "Удалить клиента";
        // 
        // lblDelId
        // 
        lblDelId.AutoSize = true;
        lblDelId.Location = new Point(15, 20);
        lblDelId.Name = "lblDelId";
        lblDelId.Size = new Size(68, 15);
        lblDelId.TabIndex = 0;
        lblDelId.Text = "ID клиента:";
        // 
        // txtDeleteId
        // 
        txtDeleteId.Location = new Point(15, 40);
        txtDeleteId.Name = "txtDeleteId";
        txtDeleteId.Size = new Size(120, 23);
        txtDeleteId.TabIndex = 1;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(15, 80);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(160, 35);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Удалить клиента";
        btnDelete.Click += BtnDelete_Click;
        // 
        // panelTop
        // 
        panelTop.Controls.Add(btnShowAll);
        panelTop.Controls.Add(btnShow21);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 190);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(880, 50);
        panelTop.TabIndex = 1;
        // 
        // btnShowAll
        // 
        btnShowAll.Location = new Point(15, 10);
        btnShowAll.Name = "btnShowAll";
        btnShowAll.Size = new Size(190, 30);
        btnShowAll.TabIndex = 0;
        btnShowAll.Text = "Показать всех клиентов";
        btnShowAll.Click += BtnShowAll_Click;
        // 
        // btnShow21
        // 
        btnShow21.Location = new Point(215, 10);
        btnShow21.Name = "btnShow21";
        btnShow21.Size = new Size(210, 30);
        btnShow21.TabIndex = 1;
        btnShow21.Text = "Клиенты старше 21 года";
        btnShow21.Click += BtnShow21_Click;
        // 
        // Form1
        // 
        ClientSize = new Size(880, 600);
        Controls.Add(dgvClients);
        Controls.Add(panelTop);
        Controls.Add(tabControl1);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Управление клиентами (PostgreSQL)";
        ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
        tabControl1.ResumeLayout(false);
        tabAdd.ResumeLayout(false);
        tabAdd.PerformLayout();
        tabUpdate.ResumeLayout(false);
        tabUpdate.PerformLayout();
        tabDelete.ResumeLayout(false);
        tabDelete.PerformLayout();
        panelTop.ResumeLayout(false);
        ResumeLayout(false);
    }


    private DataGridView dgvClients;
    private TabControl tabControl1;
    private TabPage tabAdd;
    private TabPage tabUpdate;
    private TabPage tabDelete;
    private Panel panelTop;
    private Button btnShowAll;
    private Button btnShow21;

    private TextBox txtAddFirstName;
    private TextBox txtAddLastName;
    private DateTimePicker dtpAddBirthDate;
    private TextBox txtAddEmail;
    private TextBox txtAddPhone;
    private Button btnAdd;
    private Label lblAddName;
    private Label lblAddLastName;
    private Label lblAddBirth;
    private Label lblAddEmail;
    private Label lblAddPhone;

    private TextBox txtUpdateId;
    private TextBox txtUpdateEmail;
    private TextBox txtUpdatePhone;
    private Button btnUpdate;
    private Label lblUpId;
    private Label lblUpEmail;
    private Label lblUpPhone;

    private TextBox txtDeleteId;
    private Button btnDelete;
    private Label lblDelId;
}
