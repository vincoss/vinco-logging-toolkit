namespace WindowsForms_Sample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMakeUIException = new System.Windows.Forms.Button();
            this.btnAsyncUIException = new System.Windows.Forms.Button();
            this.btnServiceCallException = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMakeUIException
            // 
            this.btnMakeUIException.Location = new System.Drawing.Point(51, 12);
            this.btnMakeUIException.Name = "btnMakeUIException";
            this.btnMakeUIException.Size = new System.Drawing.Size(180, 23);
            this.btnMakeUIException.TabIndex = 0;
            this.btnMakeUIException.Text = "Make UI Exception";
            this.btnMakeUIException.UseVisualStyleBackColor = true;
            this.btnMakeUIException.Click += new System.EventHandler(this.btnMakeUIException_Click);
            // 
            // btnAsyncUIException
            // 
            this.btnAsyncUIException.Location = new System.Drawing.Point(51, 41);
            this.btnAsyncUIException.Name = "btnAsyncUIException";
            this.btnAsyncUIException.Size = new System.Drawing.Size(180, 23);
            this.btnAsyncUIException.TabIndex = 1;
            this.btnAsyncUIException.Text = "Async UI Exception";
            this.btnAsyncUIException.UseVisualStyleBackColor = true;
            this.btnAsyncUIException.Click += new System.EventHandler(this.btnAsyncUIException_Click);
            // 
            // btnServiceCallException
            // 
            this.btnServiceCallException.Location = new System.Drawing.Point(51, 70);
            this.btnServiceCallException.Name = "btnServiceCallException";
            this.btnServiceCallException.Size = new System.Drawing.Size(180, 23);
            this.btnServiceCallException.TabIndex = 2;
            this.btnServiceCallException.Text = "Make Service Call Exception";
            this.btnServiceCallException.UseVisualStyleBackColor = true;
            this.btnServiceCallException.Click += new System.EventHandler(this.btnServiceCallException_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnServiceCallException);
            this.Controls.Add(this.btnAsyncUIException);
            this.Controls.Add(this.btnMakeUIException);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMakeUIException;
        private System.Windows.Forms.Button btnAsyncUIException;
        private System.Windows.Forms.Button btnServiceCallException;
    }
}

