﻿namespace WF_Chat_Program
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
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.btn_connect = new System.Windows.Forms.Button();
            this.txt_info = new System.Windows.Forms.TextBox();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(13, 13);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(638, 21);
            this.txt_ip.TabIndex = 0;
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(658, 13);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 1;
            this.btn_connect.Text = "연결";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.Btn_connect_Click);
            // 
            // txt_info
            // 
            this.txt_info.Location = new System.Drawing.Point(13, 41);
            this.txt_info.Multiline = true;
            this.txt_info.Name = "txt_info";
            this.txt_info.ReadOnly = true;
            this.txt_info.Size = new System.Drawing.Size(720, 335);
            this.txt_info.TabIndex = 2;
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(12, 382);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(721, 21);
            this.txt_input.TabIndex = 3;
            this.txt_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Txt_input_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 419);
            this.Controls.Add(this.txt_input);
            this.Controls.Add(this.txt_info);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.txt_ip);
            this.Name = "Form1";
            this.Text = "1:1 채팅 프로그램";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ip;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox txt_info;
        private System.Windows.Forms.TextBox txt_input;
    }
}

