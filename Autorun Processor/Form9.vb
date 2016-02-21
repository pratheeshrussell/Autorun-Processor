Imports System.Threading

Public Class Form9
    Dim str(5) As String
    Dim itm As ListViewItem
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
        Timer1.Enabled = False
    End Sub
    Private Sub Form9_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
        Me.Opacity = Form1.Opacity
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        getprocess()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error Resume Next
        Dim n = ListView1.CheckedItems.Count
        Dim x
        If n = 0 Then
        Else
            For Each item In ListView1.CheckedItems
                x = item.SubItems(2).Text()
                Dim p As Process = Process.GetProcessById(x)
                If p Is Nothing Then Return
                If Not p.CloseMainWindow() Then p.Kill()
                p.WaitForExit()
                p.Close()
                item.remove()
            Next
        End If

    End Sub
    Function getprocess()
        Dim processes() As Process = System.Diagnostics.Process.GetProcesses()
        Dim p
        For Each proc As Process In processes
            Try
                str(0) = proc.ProcessName
                str(1) = proc.MainModule.FileName
                str(2) = proc.Id.ToString
                str(3) = proc.BasePriority.ToString()
                str(4) = proc.WorkingSet64.ToString()
                p = check(proc.Id.ToString)
                If p = 1 Then
                    itm = New ListViewItem(str)
                    ListView1.Items.Add(itm)
                End If
            Catch ex As Exception
            End Try
        Next
        recheck()
    End Function
    Function check(ByVal pid As String)
        Dim n = ListView1.Items.Count
        Dim x
        If n = 0 Then
            check = 1
            Exit Function
        End If
        For Each item In ListView1.Items
            x = ListView1.FindItemWithText(pid)
            If x Is Nothing Then
                check = 1
            Else
                check = 0
            End If
        Next
    End Function
    Function recheck()
        Dim n = ListView1.Items.Count
        Dim x
        Dim processes() As Process = System.Diagnostics.Process.GetProcesses()
        Dim p
        If n = 0 Then
        Else
            For Each item In ListView1.Items
                x = item.SubItems(2).Text
                For Each proc As Process In processes
                    If proc.Id.ToString = x Then
                        p = 1
                        Exit For
                    Else
                        p = 0
                    End If
                Next
                If p = 0 Then
                    item.remove()
                End If
            Next
        End If
    End Function
    Function dofirst()
        Me.getprocess()
        Timer1.Enabled = True
    End Function
   Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        getprocess()
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        getprocess() 'realtime
    End Sub
   
    Private Sub KillProcessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KillProcessToolStripMenuItem.Click
        On Error Resume Next
        Dim n = ListView1.SelectedItems.Count
        Dim x
        If n = 0 Then
        Else
            For Each item In ListView1.SelectedItems
                x = item.SubItems(2).Text()
                Dim p As Process = Process.GetProcessById(x)
                If p Is Nothing Then Return
                If Not p.CloseMainWindow() Then p.Kill()
                p.WaitForExit()
                p.Close()
                item.remove()
            Next
        End If
        getprocess()
    End Sub
    Private Sub RepeatedKillToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepeatedKillToolStripMenuItem.Click
        On Error Resume Next
        Dim n = ListView1.SelectedItems.Count
        Dim x
        If n = 0 Then
        Else
            For Each item In ListView1.SelectedItems
                x = item.SubItems(0).Text()
                Form1.ListView4.Items.Add(x)
                item.remove()
            Next
        End If
    End Sub
End Class