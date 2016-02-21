Imports System.IO

Public Class Form2
    Dim attribute As System.IO.FileAttributes = IO.FileAttributes.Normal

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
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("close_red mousehover")
    End Sub
    Private Sub PictureBox1_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("close_red normal")
    End Sub
    Private Sub PictureBox1_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseDown
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("close_red mousedown")
    End Sub
    Private Sub PictureBox1_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseUp
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("close_red mousehover")
    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.Hide()
    End Sub
    Private Sub PictureBox2_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox2.MouseHover
        PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_mousehover")
    End Sub
    Private Sub PictureBox2_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox2.MouseLeave
        PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_normal")
    End Sub
    Private Sub PictureBox2_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox2.MouseDown
        PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_mousedown")
    End Sub
    Private Sub PictureBox2_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox2.MouseUp
        PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_mousehover")
    End Sub
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Opacity = Form1.Opacity

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim n As Integer
        Dim i As Integer
        Dim x
        Dim FileToDelete As String
        n = ListView1.CheckedItems.Count

        If n = 0 Then
            MsgBox("Nothing Checked", 0, "Error")
            Exit Sub
        End If

        For Each item In ListView1.CheckedItems
            x = item.SubItems(2).Text()
            FileToDelete = x
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
            If System.IO.File.Exists(FileToDelete) Then
                If fileDetail.Length > 0 Then
                    Form1.unlockafile(FileToDelete)
                    System.IO.File.SetAttributes(FileToDelete, attribute)
                End If
                If System.IO.File.Exists(FileToDelete) = True Then
                    fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
                    If fileDetail.Length > 0 Then
                        Form1.unlockafile(FileToDelete)
                        System.IO.File.Delete(FileToDelete)
                    End If
                End If
            End If
            item.Remove()
        Next
        no_threat()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim n = ListView1.CheckedItems.Count
        Dim x
        Dim ask
        If n = 0 Then
            MsgBox("Nothing Checked", 0, "Error")
        Else
            For Each item In ListView1.CheckedItems
                If item.SubItems(1).Text = "Hidden File" Then
                    x = item.SubItems(2).Text()
                    Dim fileDetail As IO.FileInfo
                    fileDetail = My.Computer.FileSystem.GetFileInfo(x)
                    If File.Exists(x) Then
                        If fileDetail.Length > 0 Then
                            System.IO.File.SetAttributes(x, attribute)
                        Else
                            If fileDetail.Name = "autorun.inf" Then
                                ask = MsgBox("The File Seems To Be A Immunization File So It Wont Be Made Visible", MsgBoxStyle.OkOnly, "Sorry")
                                '  If ask = vbYes Then

                                'End If
                            Else
                            System.IO.File.SetAttributes(x, attribute)
                        End If
                        End If
                    End If
                    item.Remove()

                ElseIf item.SubItems(1).Text = "Hidden Folder" Then
                    x = item.SubItems(2).Text()
                    System.IO.File.SetAttributes(x, attribute)
                    Shell("cmd /c attrib -s -h -a -r " & x & "/*.* /s /d", vbHide)
                    item.Remove()
                End If
            Next
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim n As Integer
        n = ListView1.Items.Count
        Dim FileToDelete As String
        If n > 0 Then
            For Each items In ListView1.Items
                FileToDelete = items.SubItems(2).Text()
                If System.IO.File.Exists(FileToDelete) = True Then
                    Dim fileDetail As IO.FileInfo
                    Form1.unlockafile(FileToDelete)
                    fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
                    If fileDetail.Length > 0 Then
                        System.IO.File.SetAttributes(FileToDelete, attribute)
                        System.IO.File.Delete(FileToDelete)
                    End If
                End If
                items.Remove()
            Next
        End If
        no_threat()
    End Sub
    Private Sub UnhideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnhideToolStripMenuItem.Click
        Dim n = ListView1.SelectedItems.Count
        Dim x
        If n = 0 Then
        Else
            For Each item In ListView1.SelectedItems
                If item.SubItems(1).Text = "Hidden File" Then
                    x = item.SubItems(2).Text()
                    Dim fileDetail As IO.FileInfo
                    fileDetail = My.Computer.FileSystem.GetFileInfo(x)
                    If File.Exists(x) Then
                        If fileDetail.Length > 0 Then
                            System.IO.File.SetAttributes(x, attribute)
                        End If
                    End If
                    item.Remove()

                ElseIf item.SubItems(1).Text = "Hidden Folder" Then
                    x = item.SubItems(2).Text()
                    System.IO.File.SetAttributes(x, attribute)
                    Shell("cmd /c attrib -s -h -a -r " & x & "/*.* /s /d", vbHide)
                    item.Remove()
                End If
            Next
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim n As Integer
        Dim i As Integer
        Dim x
        Dim FileToDelete As String
        n = ListView1.SelectedItems.Count
        Dim j As Integer

        If n = 0 Then
            Exit Sub
        End If

        For Each item In ListView1.CheckedItems
            x = item.SubItems(2).Text()
            FileToDelete = x
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
            If System.IO.File.Exists(FileToDelete) Then
                If fileDetail.Length > 0 Then
                    System.IO.File.SetAttributes(FileToDelete, attribute)
                End If
                If System.IO.File.Exists(FileToDelete) = True Then
                    fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
                    If fileDetail.Length > 0 Then
                        Form1.unlockafile(FileToDelete)
                        System.IO.File.Delete(FileToDelete)
                    End If
                End If
            End If
            item.Remove()
        Next
        no_threat()
    End Sub
    Function no_threat()
        Form1.no_threat()
    End Function
End Class