using System;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private int i = 0;

        public struct Employee
        {
            public string NCNSS;       // National Insurance Number
            public string Nom;         // Name
            public string Prenom;      // Surname
            public string Fonction;    // Job Title
            public string TypeEmploye; // Employee Type
            public DateTime dateNais;  // Date of Birth
            public DateTime DateEmb;   // Employment Date
            public double Salaire;
            public bool Sex;           // Gender (true for Male, false for Female)
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Ecole Racine - TSDI - 2023 - 2024 -";
            // Add items to the dropdown lists
            comboBox1.Items.AddRange(new string[] { "Ingénieur", "Technicien", "Agent" });
            comboBox2.Items.AddRange(new string[] { "Développeur", "Designer", "Directeur", "Comptable", "Chef de projet" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                comboBox1.SelectedItem == null ||
                comboBox2.SelectedItem == null ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Champ Vide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double salaire;
            if (!double.TryParse(textBox6.Text, out salaire))
            {
                MessageBox.Show("Veuillez entrer un montant valide pour le salaire.", "Valeur Invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the employee is already in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["NCNSS"].Value != null && row.Cells["NCNSS"].Value.ToString() == textBox1.Text)
                {
                    MessageBox.Show("Cet employé est déjà ajouté.", "Employé Existants", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Create an Employee object from the input data
            Employee employee = new Employee
            {
                NCNSS = textBox1.Text,
                Nom = textBox2.Text,
                Prenom = textBox3.Text,
                dateNais = dateTimePicker1.Value,
                DateEmb = DateTime.Now, // Set the employment date to now or use another date picker if needed
                Fonction = comboBox1.SelectedItem.ToString(),
                TypeEmploye = comboBox2.SelectedItem.ToString(),
                Salaire = salaire,
                Sex = checkBox1.Checked // Assign gender based on checkBox1
            };

            DialogResult result = MessageBox.Show("Voulez-vous ajouter cet employé ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                i++;
                dataGridView1.Rows.Add(employee.NCNSS, employee.Nom, employee.Prenom, employee.Sex ? "Male" : "Female", employee.dateNais.ToShortDateString(), employee.DateEmb.ToShortDateString(), employee.Fonction, employee.TypeEmploye, employee.Salaire.ToString("F2"));
                MessageBox.Show("Employé ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                string newNcnss = Prompt.ShowDialog("Modifier le NCNSS:", "Modifier le NCNSS");
                string newNom = Prompt.ShowDialog("Modifier le Nom:", "Modifier le Nom");
                string newPrenom = Prompt.ShowDialog("Modifier le Prénom:", "Modifier le Prénom");
                string newSalaireStr = Prompt.ShowDialog("Modifier le salaire:", "Modifier le salaire");

                double newSalaire;
                if (!double.TryParse(newSalaireStr, out newSalaire))
                {
                    MessageBox.Show("Veuillez entrer un montant valide pour le salaire.", "Valeur Invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedRow.Cells["NCNSS"].Value = newNcnss;
                selectedRow.Cells["Nom"].Value = newNom;
                selectedRow.Cells["Prenom"].Value = newPrenom;
                selectedRow.Cells["Salaire"].Value = newSalaire.ToString("F2");

                MessageBox.Show("Modification réussie.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne à modifier.", "Aucune Ligne Sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Cells["Salaire"].Value != null)
                {
                    double value;
                    if (double.TryParse(row.Cells["Salaire"].Value.ToString(), out value))
                    {
                        total += value;
                    }
                }
            }
            textBox5.Text = total.ToString("F2") + " DH";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear input fields and reset checkboxes
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // Validate NCNSS input to ensure only digits are entered
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Veuillez entrer uniquement des chiffres.", "Caractères Invalides", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Manage checkbox states
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // Manage checkbox states
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and a decimal point in the salary field
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}

