Public Class Form4
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

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim leftpos As Long
        Dim toppos As Long
        leftpos = (My.Computer.Screen.WorkingArea.Right) - Me.Width
        toppos = (My.Computer.Screen.WorkingArea.Bottom) - Me.Height
        Me.Location = New Point(leftpos, toppos)
        Me.ShowInTaskbar = False
        Me.Opacity = Form1.Opacity
        Me.TopMost = False
    End Sub
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
    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        f5.Show()
    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.Hide()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form1.PictureBox5_Click(sender, New System.EventArgs)
        Me.Hide()
    End Sub
End Class