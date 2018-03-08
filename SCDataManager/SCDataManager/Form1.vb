Imports System.IO

Public Class Form1
    Private Const BufferSize As UInteger = &H10000

    Public MainAdress As UInteger
    Private hockvalue() As UInteger = {2018915346, 4041129114}


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WinAPI.AdjustToken()

        'WinAPI.Write(841236768, CUInt(123))
    End Sub
    Private Sub RedemptionButton1_Click(sender As Object, e As EventArgs)
        If WinAPI.CheckProcess() = False Then
            MsgBox("스타꺼짐")
        End If

        If BackgroundWorker1.IsBusy = False Then
            MsgBox("작동을 시작합니다.")
            BackgroundWorker1.RunWorkerAsync()
        Else
            MsgBox("이미 동작중입니다")
        End If
    End Sub
    'Private Sub SearchMemory()
    '    Dim Memory As New WinAPI.MemoryReader(0)
    '    While (Memory.Position < CUInt(4294967295))
    '        Dim value As UInteger = Memory.ReadUInt32()
    '        ' RichTextBox1.AppendText(Hex(value) & " ")
    '        If value = 123456789 Then
    '            MsgBox(Hex(Memory.Position))
    '            Exit Sub
    '        End If
    '    End While
    'End Sub
    Private Sub SearchMemory()
        Dim Memory As New WinAPI.MemoryReader(0) With {
            .Position = 0
            }


        Dim checkindex As Integer = 0
        While (Memory.Position < CUInt(2147483648))
            Dim values() As Byte = Memory.ReadBytes(BufferSize)
            Dim memoryste As New MemoryStream(values)
            Dim stremawriter As New BinaryReader(memoryste)

            Dim check As Boolean = False
            For i = 0 To BufferSize / 4 - 1
                Dim value As UInteger = stremawriter.ReadUInt32

                If value = hockvalue(checkindex) Then
                    checkindex += 1
                Else
                    checkindex = 0
                End If


                If checkindex = hockvalue.Length Then
                    MainAdress = Memory.Position - BufferSize + i * 4 - 4
                    Exit Sub
                End If

            Next
            'Label1.Text = "0x" & Format("{0:D8}", Hex(Memory.Position))
            'RichTextBox1.AppendText("0x" & Hex(Memory.Position) & " ")
            stremawriter.Close()
            memoryste.Close()

        End While
    End Sub


    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim oldtime As DateTime = System.DateTime.Now
        SearchMemory()
        Dim newtime As DateTime = System.DateTime.Now
        Dim time As TimeSpan = newtime.Subtract(oldtime)
        MsgBox(time.ToString & "만큼 걸렸습니다." & vbCrLf & Hex(MainAdress) & "에서 발견되었습니다.")
        WinAPI.Write(CUInt(MainAdress + 8), CUInt(123))
    End Sub


    '데이터 확인용 데스값
    '명령 데스값
    '명령 인자1,2,3,4....

    '무조건 

    'S 베이스 오프셋 단서를 올림
    'P 시작시에 베이스 오프셋을 찾고 확인용 데스값에 1 대입
    'S 확인용 데스값이 1이 되면 베이스 오프셋을 모ㅜㄷ 초기화
    '1이 확인 되면 게임
End Class
