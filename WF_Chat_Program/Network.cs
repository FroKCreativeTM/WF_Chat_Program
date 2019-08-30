using System;
using System.Text;              // 문자열 처리
using System.Threading;         // 스레드 처리
using System.Net;               // 네트워크 처리
using System.Net.Sockets;       // 소켓 처리
using System.IO;                // ver 2.0 ~ 3.0 추가 부분

namespace WF_Chat_Program
{
    class NetWork
    {
        Form1 wnd = null;           // 채팅 창 변수
        //  Socket server = null;       // 서버 소켓(접속을 받는 소켓) 변수
        //                              // 서버일 경우 통신 소켓
        //  Socket client = null;       // 클라이언트 소켓(접속, 통신) 변수
        Thread th = null;           // 스레드 처리

        // 3.0
        TcpListener server = null;
        TcpClient client = null;

        // 2.0 
        NetworkStream netStream = null;     // 네트워크 스트림
        StreamReader streamReader = null;   // 읽기 문자 전용 스트림
        StreamWriter streamWriter = null;   // 쓰기 문자 전용 스트림

        // Network 생성자
        public NetWork(Form1 wnd)
        {
            this.wnd = wnd; // Network 클래스에도 Form1 멤버 사용 허용
        }

        // 채팅 서버 시작 : 클라이언트 접속을 받고 메시지를 수신
        public void ServerStart()
        {
            try
            {
                // 서버 포트 번호를 7000번으로 지정
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 7000);
                // 서버에 소켓을 할당한다
                // 이 때 IPv4를 사용하며(1번째 인자.)
                // 양방향 통신을 지향하고
                // TCP 통신을 한다.
                // server = new Socket(AddressFamily.InterNetwork,
                //    SocketType.Stream, ProtocolType.Tcp);

                server = new TcpListener(iPEndPoint);

                // 소켓과 서버 ip, 포트번호를 바인딩해준다.
                // server.Bind(iPEndPoint);
                // 클라이언트 서버연결 대기
                // server.Listen(10);

                server.Start();     // 채탱 서버 실행

                // 채팅창(txt_info)에 메시지 추가
                wnd.Add_MSG("채팅 서버 시작...");

                // 클라이언트가 접속시 활성화
                // client = server.Accept();

                // 접속한 클라이언트의 ip주소를 출력
                // IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                // wnd.Add_MSG(ip.Address + "접속...");

                // 채팅 클라이언트가 접속하면 통신 소켓 반환
                client = server.AcceptTcpClient();

                // ver 2.0에서 추가된 부분, 서버에서 송수신을 위한 스트림 객체 형성
                // (3.0으로 수정완료)
                netStream = client.GetStream();
                streamReader = new StreamReader(netStream);
                streamWriter = new StreamWriter(netStream);

                // 상대방의 메시지를 수신하는 Receiver 메서드의 스레드로 생성
                th = new Thread(new ThreadStart(Receive));
                th.Start();

            }
            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        // 채팅 서버를 중단시키는 데 쓰이는 메소드
        public void ServerStop()
        {
            try
            {
                if (client != null)
                {
                    // 클라이언트가 접속되어있는 상태라면
                    if (client.Connected)
                    {
                        // ver 2.0으로 추가된 부분(3.0으로 수정완료)
                        if (streamReader != null) streamReader.Close();
                        if (streamWriter != null) streamWriter.Close();
                        if (netStream != null) netStream.Close();

                        // 클라이언트 소켓을 닫는다.
                        client.Close();
                        if (th.IsAlive) // Receive 스레드가 실행중이라면
                            th.Abort(); // 스레드 종료
                        server.Stop();  // 서버 소켓 닫기
                    }
                }
                // server.Close(); // 서버 소켓 종료
            }
            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        // 채팅 서버와 연결하는 메소드
        public bool Connect(string ip)
        {
            try
            {
                // 접속할 채팅 서버의 주소와 포트 번호 지정
                // IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 7000);

                // 클라이언트에도 서버와 마찬가지로
                // IPv4, 양방향 (연결기반 바이트) 통신, TCP 프로토콜을 넣어준다.
                // client = new Socket(AddressFamily.InterNetwork,
                //    SocketType.Stream,
                //    ProtocolType.Tcp);

                client = new TcpClient(ip, 7000);
                wnd.Add_MSG(ip + "서버에 접속 성공...");

                // 채팅 서버에 접속을 시도한다.
                // client.Connect(iPEndPoint);

                // ver 2.0으로 추가된 부분 (3.0으로 수정완료)
                netStream = client.GetStream();
                streamReader = new StreamReader(netStream);
                streamWriter = new StreamWriter(netStream);

                // 채팅 문자열을 수신하는 메소드(Receive)를 스레드로 생성하고 시작
                th = new Thread(new ThreadStart(Receive));
                th.Start();

                return true;
            }
            catch (Exception ex)        // 채팅 서버에 접속 실패시
            {
                wnd.Add_MSG(ex.Message);
                return false;
            }
        }

        // 채팅 서버와 연결 종료하는 메소드
        public void DisConnect()
        {
            try
            {
                if (client != null)
                {
                    if (client.Connected)
                    {   // 채팅 서버와 연결되어 있을 시
                        if (streamReader != null) streamReader.Close();
                        if (streamWriter != null) streamWriter.Close();
                        if (netStream != null) netStream.Close();

                        client.Close();
                    }
                    if (th.IsAlive)             // Receive 스레드 정지
                        th.Abort();
                }
                wnd.Add_MSG("채팅 서버 연결 종료!");
            }
            // 채팅 서버 연결 해제 혹은 스레드 종료시 예외가 발생하면
            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        // 상대방의 데이터를 수신하는 메소드
        public void Receive()
        {
            string msg = null;
            // 상대방과 연결이 되어 있다면
            try
            {
                // 클라이언트에 소켓이 할당이 되어있고
                // 동시에 연결이 되어있어야만 데이터를 받을 수 있다.
                //while(client != null && client.Connected)
                //{
                    // Receive 메소드를 사용해서 바이트 단위로 데이터를 읽는다.
                    // byte[] data = ReceiveData();

                    // 2.0으로 바뀐 코드
                    // 라인 단위로 문자열을 읽어오자.
                    // (3.0 버전으로 수정 완료)
                    do
                    {
                        msg = streamReader.ReadLine();
                        // wnd.Add_MSG("[상대방]" + Encoding.Default.GetString(data));
                        wnd.Add_MSG("[상대방]" + msg);
                    } while (msg != null);
                //}
            }

            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        // 상대방에게 데이터를 송신하는 메소드
        public void Send(string msg)
        {
            try
            {
                if (client.Connected)
                {
                    // 바이트 배열로 보내면 null이 보내지는 경우도 발견됨
                    // byte[] data = Encoding.Default.GetBytes(msg);
                    // SendData(data);

                    // 2.0으로 바뀐 코드
                    streamWriter.WriteLine(msg);
                    streamWriter.Flush();           // 버퍼를 비워주자.
                }
                else
                    wnd.Add_MSG("메시지 전송 실패");
            }
            catch (Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        /*
         * 3.0이 되면서 Network Stream과 헬퍼 클래스를 사용하기 때문에 쓰지 않는 
         * 메소드 들이다.
         * 
        private void SendData(byte[] data)
        {
            try
            {
                int nOffset = 0;                // 버퍼 내 위치
                int nSize = data.Length;        // 전송할 바이트 배열의 크기
                int nLeftData = nSize;          // 남은 데이터의 양
                int nSendData = 0;              // 전송된 데이터의 양


                byte[] data_size = new byte[4]; // 전송할 실제 데이터의 크기 전달
                data_size = BitConverter.GetBytes(nSize);   // 정수 형태로 데이터 크기 전달
                nSendData = client.Send(data_size);

                // 실제 데이터 전송
                while (nOffset < nSize)
                {
                    nSendData = client.Send(data, nOffset, nLeftData, SocketFlags.None);
                    nOffset += nSendData;
                    nLeftData -= nSendData;
                }
            }

            catch(Exception ex)
            {
                wnd.Add_MSG(ex.Message);
            }
        }

        private byte[] ReceiveData()
        {
            try
            {
                int nOffset = 0;                // 버퍼 내 위치
                int nSize = 0;                  // 수신할 데이터 크기
                int nLeftData = 0;              // 남은 데이터 크기
                int nRecv_data = 0;             // 수신한 데이터 크기

                // 수신할 데이터 알아내기
                byte[] data_size = new byte[4];
                nRecv_data = client.Receive(data_size, 0, 4, SocketFlags.None);
                nSize = BitConverter.ToInt32(data_size, 0);
                nLeftData = nSize;
                byte[] data = new byte[nSize];  // 바이트 배열 생성

                while (nOffset < nSize)
                {   // 상대방이 전송한 데이터를 읽어옴
                    nRecv_data = client.Receive(data, nOffset, nLeftData, SocketFlags.None);
                    if (nRecv_data == 0) break;
                    nOffset += nRecv_data;
                    nRecv_data -= nRecv_data;
                }
                return data;
            }
            catch(Exception ex)
            {
                wnd.Add_MSG(ex.Message);
                return null;
            }
        }
        */
    }
}
