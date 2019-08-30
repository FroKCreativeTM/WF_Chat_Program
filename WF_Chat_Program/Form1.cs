using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

/*
 * Ver : 3.0
 * 프로젝트 명 : 1 : 1 채팅 프로그램
 * 작성자 : 차승철
 * 학번 : 18017089
 * 3.0 버젼 내용 : 헬퍼 클래스를 이용한 1대 1 채팅 프로그램
 */

/*
 * Ver : 1.0 
 * 내용 : 소켓 바이트 배열 단위의 문자 전송
 * 문제점 : null이 보내지는 경우, 예외처리가 힘듬
 * 
 * ver : 2.0 
 * 내용 : Network stream을 이용해서 소켓 방식 채팅보다 구현이 용이하다.
 *          + ver 1.0의 문제점 해결
 */

namespace WF_Chat_Program
{
    public partial class Form1 : Form
    {
        private NetWork net = null;
        private Thread server_th = null;        // 채팅서버 쓰레드 선언

        public Form1()
        {
            InitializeComponent();
            net = new NetWork(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 채팅 서버를 시작시키는 스레드를 생성
            server_th = new Thread(new ThreadStart(net.ServerStart));
            // 채팅 서버 시작
            server_th.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 클라이언트 수행중일 시
                if (btn_connect.Text == "접속중...")
                {
                    // 채팅 서버와 연결되어 있으면 연결 끊기
                    net.DisConnect();
                }
                else
                {
                    // 채팅 서버 실행 중지
                    net.ServerStop();
                    // 만약 서버 쓰레드가 살아있다면
                    if (server_th.IsAlive)
                        server_th.Abort();          // ServerStart 스레드 종료
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_connect_Click(object sender, EventArgs e)
        {
            // 채팅 서버에 접속한 경우
            if(btn_connect.Text == "연결")
            {
                // 접속할 서버 ip 가져오기
                string ip = txt_ip.Text.Trim();
                // 접속할 서버의 ip를 입력하지 않았다면
                if(ip == "")
                {
                    MessageBox.Show("아이피 번호를 입력하세요!", "IP Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                // 채팅 서버에 접속을 시도한다.
                if (!net.Connect(ip))
                {
                    MessageBox.Show("서버 아이피가 틀렸거나\r\n" +
                        "서버가 작동중이지 않습니다.", "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                // 접속이 성공하면 채팅이 시작된다.
                else
                    btn_connect.Text = "접속중...";
            }
            // 채팅을 종료하는 경우
            else
            {
                // 채팅 서버와 연결을 끊음
                net.DisConnect();
                btn_connect.Text = "연결";
            }
        }

        public void Add_MSG(string msg)
        {
            // 채팅 문자열 입력
            txt_info.AppendText(msg + "\r\n");
            // 텍스트 박스의 내용을 현재 캐럿 위치까지 스크롤
            txt_info.ScrollToCaret();
            // txt_input 텍스트 박스에 초점을 맞춤
            txt_input.Focus();
        }

        private void Txt_input_KeyDown(object sender, KeyEventArgs e)
        {
            // 엔터키를 눌러서 상대방에게 메시지를 전송한다.
            if (e.KeyCode == Keys.Enter)
            {
                // input창의 텍스트를 string형에 임시 저장한다.
                string msg = txt_input.Text.Trim();
                Add_MSG("[본인]" + msg);
                net.Send(msg);          // 메시지 전송
                txt_input.Text = "";    // 다음 전송을 위해 비워둔다.
                txt_input.Focus();
            }
        }
    }
}
