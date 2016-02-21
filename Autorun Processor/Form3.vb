Imports Microsoft.Win32
Public Class Form3

    Dim attribute As System.IO.FileAttributes = IO.FileAttributes.Normal
    Public closeme = 0
    Dim f5 As New Form5
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
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim leftpos As Long
        Dim toppos As Long
        leftpos = (My.Computer.Screen.WorkingArea.Right) - Me.Width
        toppos = (My.Computer.Screen.WorkingArea.Bottom) - Me.Height
        Me.Location = New Point(leftpos, toppos)
        Me.TopMost = True
        Me.ShowInTaskbar = False
    End Sub
    Private Sub label1_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label1.MouseHover
        Label1.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub Label1_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label1.MouseLeave
        Label1.Image = My.Resources.ResourceManager.GetObject("mainnav")
    End Sub
    Private Sub Label1_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label1.MouseDown
        Label1.Image = My.Resources.ResourceManager.GetObject("button_press")
    End Sub
    Private Sub Label1_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label1.MouseUp
        Label1.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub label2_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseHover
        Label2.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub Label2_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseLeave
        Label2.Image = My.Resources.ResourceManager.GetObject("mainnav")
    End Sub
    Private Sub Label2_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseDown
        Label2.Image = My.Resources.ResourceManager.GetObject("button_press")
    End Sub
    Private Sub Label2_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseUp
        Label2.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub label3_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseHover
        Label3.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub Label3_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseLeave
        Label3.Image = My.Resources.ResourceManager.GetObject("mainnav")
    End Sub
    Private Sub Label3_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseDown
        Label3.Image = My.Resources.ResourceManager.GetObject("button_press")
    End Sub
    Private Sub Label3_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseUp
        Label3.Image = My.Resources.ResourceManager.GetObject("button_hover")
    End Sub
    Private Sub label5_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label5.MouseHover
        Me.Cursor = Cursors.Hand
    End Sub
    Private Sub label5_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label5.MouseLeave
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        f5.show()
    End Sub
    Friend Function close1()
        Me.Hide()
    End Function
    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        Form1.action_delete(Label7.Text)
        Me.Hide()
    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Form1.action_rename(Label7.Text)
        Me.Hide()
    End Sub
    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        Me.Hide()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Form1.TextBox1.Text.ToString.Contains(Label7.Text.Replace("autorun.inf", "")) Then
            Me.Hide()
        End If
        If Form1.TextBox1.Text.ToString.Contains("125") Then
            Me.Hide()
        End If
    End Sub
End Class