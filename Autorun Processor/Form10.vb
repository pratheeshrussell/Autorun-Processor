Imports Microsoft.Win32

Public Class Form10
#Region " ClientAreaMove Handling "
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub
#End Region
    Private Sub PictureBox1_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseHover
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("green_close_hover")
    End Sub
    Private Sub PictureBox1_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("green_close_normal")
    End Sub
    Private Sub PictureBox1_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseDown
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("green_close_press")
    End Sub
    Private Sub PictureBox1_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseUp
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("green_close_hover")
    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.Hide()
    End Sub
    Function preliminary()
        Me.ShowInTaskbar = False
        Me.Opacity = Form1.Opacity
        read_hosts()
    End Function
    Function read_hosts()
        TextBox1.Text = Nothing
        Dim path = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\etc\hosts"
        Dim readupdate() As String = System.IO.File.ReadAllLines(path)
        For Each line As String In readupdate
            TextBox1.Text = TextBox1.Text & line & vbCrLf
        Next
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim path = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\etc\hosts"
        If System.IO.File.Exists(path) = True Then
            Dim objWriter As New System.IO.StreamWriter(path)
            objWriter.Write(TextBox1.Text)
            objWriter.Close()
        Else
            MsgBox("File Does Not Exist")
        End If
        read_hosts()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        clear_host()
        Dim FILE_NAME As String = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\etc\hosts"
        Dim i As Integer
        Dim aryText(50) As String

        aryText(0) = "# Copyright (c) 1993-1999 Microsoft Corp."
        aryText(1) = "#"
        aryText(2) = "# This is a sample HOSTS file used by Microsoft TCP/IP for Windows."
        aryText(3) = "#"
        aryText(4) = "# This file contains the mappings of IP addresses to host names. Each"
        aryText(5) = "# entry should be kept on an individual line. The IP address should"
        aryText(6) = "# be placed in the first column followed by the corresponding host name."
        aryText(7) = "# The IP address and the host name should be separated by at least one"
        aryText(8) = "# space."
        aryText(9) = "#"
        aryText(10) = "# Additionally, comments (such as these) may be inserted on individual"
        aryText(11) = "# lines or following the machine name denoted by a '#' symbol."
        aryText(12) = "#"
        aryText(13) = "# For example:"
        aryText(14) = "#"
        aryText(15) = "#      102.54.94.97     rhino.acme.com          # source server"
        aryText(16) = "#       38.25.63.10     x.acme.com              # x client host"
        aryText(17) = ""
        aryText(18) = "127.0.0.1       localhost"
        aryText(19) = "127.0.0.1 	mpa.one.microsoft.com"

        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)

        For i = 0 To 19
            objWriter.WriteLine(aryText(i))
        Next
        objWriter.Close()
        read_hosts()
    End Sub
    Function clear_host()
        Dim path = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\drivers\etc\hosts"
        If System.IO.File.Exists(path) = True Then
            Dim objWriter As New System.IO.StreamWriter(path)
            objWriter.Write("")
            objWriter.Close()
        Else
        End If

    End Function
End Class