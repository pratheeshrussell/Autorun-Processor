Imports System.IO

Public Class Form6
    Dim x
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
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "Picture Files|*.jpeg;*.png;*.bmp;*.jpg;*.gif;*.tif"
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileName <> "" Then TextBox1.Text = OpenFileDialog1.FileName
    End Sub
    Private Sub Form6_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RadioButton1.Checked = True
        Me.ShowInTaskbar = False
        refresh_drives()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        refresh_drives()
    End Sub
    Function refresh_drives()
        ComboBox1.Items.Clear()
        Button4.Visible = False
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim d As IO.DriveInfo
        For Each d In allDrives
            If d.IsReady = True AndAlso d.DriveType = IO.DriveType.Removable Then
                If IO.DriveType.Removable Then
                    ComboBox1.Items.Add(d.Name)
                    ComboBox1.Text = d.Name
                End If
            End If
        Next
        Dim n = ComboBox1.Items.Count
        If n < 1 Then
            Button1.Enabled = False
            Button2.Enabled = False
            GroupBox1.Enabled = False
            TextBox1.Enabled = False
        End If
    End Function
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim file As String
        file = ComboBox1.Text & "\Desktop.ini"
        If System.IO.File.Exists(file) = True Then
            Button4.Visible = True
        Else
            Button4.Visible = False
        End If
    End Sub
    Function background()
        On Error Resume Next
        Dim attribute As System.IO.FileAttributes
        Dim FileToCopy As String
        Dim filepresent = 1
        Dim NewCopy As String
        If RadioButton1.Checked = True Then
            x = "0x00000000"
        ElseIf RadioButton2.Checked = True Then
            x = "0x000000FF"
        ElseIf RadioButton3.Checked = True Then
            x = "0x0000FF00"
        ElseIf RadioButton4.Checked = True Then
            x = "0x00FF0000"
        ElseIf RadioButton5.Checked = True Then
            x = "0x00FFFFFF"
        ElseIf RadioButton6.Checked = True Then
            x = "0x0066FFFF"
        End If
        FileToCopy = TextBox1.Text
        NewCopy = ComboBox1.Text & "\qw#@.jpg"
        If TextBox1.Text = Nothing Then
            filepresent = 0
        End If
        If System.IO.File.Exists(NewCopy) = True Then
            attribute = FileAttributes.Normal
            File.SetAttributes(NewCopy, attribute)
            System.IO.File.Delete(NewCopy)
        End If
        If System.IO.File.Exists(ComboBox1.Text & "\Desktop.ini") = True Then
            attribute = FileAttributes.Normal
            File.SetAttributes(ComboBox1.Text & "\Desktop.ini", attribute)
            System.IO.File.Delete(ComboBox1.Text & "\Desktop.ini")
        End If

        If System.IO.File.Exists(FileToCopy) = True Then
            System.IO.File.Copy(FileToCopy, NewCopy)
        Else
            filepresent = 0
        End If

        Dim TextFile As New StreamWriter(ComboBox1.Text & "\Desktop.ini")
        TextFile.WriteLine("[{BE098140-A513-11D0-A3A4-00C04FD706EC}]")
        TextFile.WriteLine("attribute = 1")
        If filepresent = 1 Then
            TextFile.WriteLine("iconarea_image=qw#@.jpg")
        End If
        TextFile.WriteLine("iconarea_text=" & x)
        TextFile.Close()
        attribute = (FileAttributes.Hidden + FileAttributes.System)
        File.SetAttributes(ComboBox1.Text & "\Desktop.ini", attribute)
        Dim attribute1 As System.IO.FileAttributes = FileAttributes.System
        File.SetAttributes(ComboBox1.Text & "\qw#@.jpg", attribute)
        Button4.Visible = True
        MsgBox("Folder Background Set", 0, "Success! Background Set")
    End Function
    Function delete()
        On Error Resume Next
        Dim FileTodel1 As String
        Dim FileTodel2 As String
        Dim attribute As System.IO.FileAttributes = FileAttributes.Normal
        FileTodel1 = ComboBox1.Text & "\Desktop.ini"
        FileTodel2 = ComboBox1.Text & "\qw#@.jpg"
        If System.IO.File.Exists(FileTodel1) = True Then
            File.SetAttributes(FileTodel1, attribute)
            System.IO.File.Delete(FileTodel1)
        End If
        If System.IO.File.Exists(FileTodel2) = True Then
            File.SetAttributes(FileTodel2, attribute)
            System.IO.File.Delete(FileTodel2)
        End If
        ' If CheckBox1.Checked = True Then
        Button4.Visible = False
        MsgBox("Folder Background Removed.", 0, "Removed")
        ' Else
        ' background()
        ' End If

    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ComboBox1.Text = Nothing Then
            Exit Sub
        End If
        background()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If ComboBox1.Text = Nothing Then
            Exit Sub
        End If
        delete()
    End Sub
End Class