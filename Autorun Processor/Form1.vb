Imports System.Reflection
Imports System.IO
Imports Microsoft.Win32
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32.SafeHandles
Imports System.Diagnostics
Imports System.Security.Cryptography
Imports System.Threading

Public Class Form1
    Private mobjController As AgentObjects.Agent
    Private mobjCharacter As AgentObjects.IAgentCtlCharacter
    Public watchfolder As FileSystemWatcher
    Dim startsetting = 0 'startup
    Dim watcher_enabled = 0
    Dim startuppath 'startup folder
    Dim check = Nothing
    Dim attribute As System.IO.FileAttributes = IO.FileAttributes.Normal
    Dim f2 As New Form2
    Dim f4 As New Form4
    Dim f5 As New Form5
    Dim f6 As New Form6
    Dim f7 As New Form7
    Dim f8 As New Form8
    Dim f9 As New Form9
    Dim f10 As New Form10
    Dim f11 As New Form11
    Dim f13 As New Form13
    Dim f14 As New Form14
    Private paths As New List(Of String)
    Private safeHandles As New List(Of SafeFileHandle)
    Dim isanimate = 0
    Dim picturebox7select = 0
    Dim picturebox8select = 0
    Dim picturebox9select = 0
    Dim picturebox10select = 0
    Private mouseOffset As Point
    Private isMouseDown As Boolean = False
    Private Sub Form1_MouseDown(ByVal sender As Object, _
    ByVal e As MouseEventArgs) Handles MyBase.MouseDown
        Dim xOffset As Integer
        Dim yOffset As Integer
        If e.Button = MouseButtons.Left Then
            xOffset = -e.X - SystemInformation.FrameBorderSize.Width
            yOffset = -e.Y - SystemInformation.CaptionHeight - _
            SystemInformation.FrameBorderSize.Height
            mouseOffset = New Point(xOffset, yOffset)
            isMouseDown = True
        End If
    End Sub
    Private Sub Form1_MouseMove(ByVal sender As Object, _
    ByVal e As MouseEventArgs) Handles MyBase.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub
    Private Sub Form1_MouseUp(ByVal sender As Object, _
    ByVal e As MouseEventArgs) Handles MyBase.MouseUp
        ' Changes the isMouseDown field so that the form does
        ' not move unless the user is pressing the left mouse button.
        If e.Button = MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub
    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Private Const DBT_DEVTYP_VOLUME As Integer = &H2  '

    Private Structure DEV_BROADCAST_VOLUME
        Dim Dbcv_Size As Integer
        Dim Dbcv_Devicetype As Integer
        Dim Dbcv_Reserved As Integer
        Dim Dbcv_Unitmask As Integer
        Dim Dbcv_Flags As Short
    End Structure
    'INTERFACE DESIGN
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
        If CheckBox2.CheckState = CheckState.Checked Then
            Me.Visible = False
            Me.ni.Visible = True
        Else
            Me.Close()
        End If
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
        PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_mousedown")
        Me.WindowState = FormWindowState.Minimized
        Me.ni.Visible = True
        Me.ShowInTaskbar = False
    End Sub

    Private Sub PictureBox4_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox4.MouseHover
        PictureBox4.Image = My.Resources.ResourceManager.GetObject("scan_normal")
    End Sub
    Private Sub PictureBox4_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox4.MouseLeave
        PictureBox4.Image = My.Resources.ResourceManager.GetObject("scan_hover")
    End Sub
    Private Sub PictureBox4_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox4.MouseDown
        PictureBox4.Image = My.Resources.ResourceManager.GetObject("scan_pressed")
    End Sub
    Private Sub PictureBox4_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox4.MouseUp
        PictureBox4.Image = My.Resources.ResourceManager.GetObject("scan_normal")
    End Sub
    Private Sub PictureBox5_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox5.MouseHover
        PictureBox5.Image = My.Resources.ResourceManager.GetObject("ball_green.hover")
    End Sub
    Private Sub PictureBox5_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox5.MouseLeave
        PictureBox5.Image = My.Resources.ResourceManager.GetObject("ball_green.normal.back")
    End Sub
    Public Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
        f2.Show()
    End Sub
    Private Sub PictureBox6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox6.Click
        f2.Show()
    End Sub
    Private Sub PictureBox6_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox6.MouseHover
        PictureBox6.Image = My.Resources.ResourceManager.GetObject("ball red hover")
    End Sub
    Private Sub PictureBox6_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox6.MouseLeave
        PictureBox6.Image = My.Resources.ResourceManager.GetObject("ball red norm")
    End Sub
    Private Sub label3_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseHover
        Timer2.Enabled = True
        Timer2.Start()
    End Sub
    Private Sub label3_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label3.MouseLeave
        Timer2.Enabled = False
        no_threat()
    End Sub
    Private Sub PictureBox7_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox7.MouseHover
        If picturebox7select = 1 Then
        Else
            PictureBox7.Image = My.Resources.ResourceManager.GetObject("on mouse hover")
            picturebox7select = 0
        End If
    End Sub
    Private Sub PictureBox7_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox7.MouseLeave
        If picturebox7select = 1 Then
        Else
            PictureBox7.Image = My.Resources.ResourceManager.GetObject("on normal")
            picturebox7select = 0
        End If
    End Sub
    Private Sub PictureBox8_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox8.MouseHover
        If picturebox8select = 1 Then
        Else
            PictureBox8.Image = My.Resources.ResourceManager.GetObject("off mouse hover")
            picturebox8select = 0
        End If
    End Sub
    Private Sub PictureBox8_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox8.MouseLeave
        If picturebox8select = 1 Then
        Else
            PictureBox8.Image = My.Resources.ResourceManager.GetObject("off normal")
            picturebox8select = 0
        End If
    End Sub
    Private Sub PictureBox10_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox10.MouseHover
        If picturebox10select = 1 Then
        Else
            PictureBox10.Image = My.Resources.ResourceManager.GetObject("on mouse hover")
            picturebox10select = 0
        End If
    End Sub
    Private Sub PictureBox10_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox10.MouseLeave
        If picturebox10select = 1 Then
        Else
            PictureBox10.Image = My.Resources.ResourceManager.GetObject("on normal")
            picturebox10select = 0
        End If
    End Sub
    Private Sub PictureBox9_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox9.MouseHover
        If picturebox9select = 1 Then
        Else
            PictureBox9.Image = My.Resources.ResourceManager.GetObject("off mouse hover")
            picturebox9select = 0
        End If
    End Sub
    Private Sub PictureBox9_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox9.MouseLeave
        If picturebox9select = 1 Then
        Else
            PictureBox9.Image = My.Resources.ResourceManager.GetObject("off normal")
            picturebox9select = 0
        End If
    End Sub
    Private Sub label2_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseHover
        Me.Cursor = Cursors.Hand
    End Sub
    Private Sub label2_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label2.MouseLeave
        Me.Cursor = Cursors.Arrow
    End Sub
    Private Sub label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        f5.Show()
    End Sub
    Private Sub label9_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label9.MouseHover
        Label9.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label9_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label9.MouseLeave
        Label9.Image = My.Resources.ResourceManager.GetObject("scan_normal1")
    End Sub
    Private Sub label9_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label9.MouseDown
        Label9.Image = My.Resources.ResourceManager.GetObject("scan_pressed1")
    End Sub
    Private Sub label9_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label9.MouseUp
        Label9.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label10_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label10.MouseHover
        Label10.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label10_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label10.MouseLeave
        Label10.Image = My.Resources.ResourceManager.GetObject("scan_normal1")
    End Sub
    Private Sub label10_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label10.MouseDown
        Label10.Image = My.Resources.ResourceManager.GetObject("scan_pressed1")
    End Sub
    Private Sub label10_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label10.MouseUp
        Label10.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub PictureBox11_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox11.MouseHover
        PictureBox11.Image = My.Resources.ResourceManager.GetObject("ball_yellow_hover")
    End Sub
    Private Sub PictureBox11_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox11.MouseLeave
        PictureBox11.Image = My.Resources.ResourceManager.GetObject("ball_yellow_normal")
    End Sub
    Private Sub label12_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label12.MouseHover
        Label12.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label12_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label12.MouseLeave
        Label12.Image = My.Resources.ResourceManager.GetObject("scan_normal1")
    End Sub
    Private Sub label12_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label12.MouseDown
        Label12.Image = My.Resources.ResourceManager.GetObject("scan_pressed1")
    End Sub
    Private Sub label12_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label12.MouseUp
        Label12.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label13_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label13.MouseHover
        Label13.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label13_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label13.MouseLeave
        Label13.Image = My.Resources.ResourceManager.GetObject("scan_normal1")
    End Sub
    Private Sub label13_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label13.MouseDown
        Label13.Image = My.Resources.ResourceManager.GetObject("scan_pressed1")
    End Sub
    Private Sub label13_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label13.MouseUp
        Label13.Image = My.Resources.ResourceManager.GetObject("scan_hover1")
    End Sub
    Private Sub label14_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label14.MouseHover
        Label14.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label14_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label14.MouseLeave
        Label14.Image = My.Resources.ResourceManager.GetObject("tool_button_norm")
    End Sub
    Private Sub label14_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label14.MouseDown
        Label14.Image = My.Resources.ResourceManager.GetObject("tool_button_ click")
    End Sub
    Private Sub label14_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label14.MouseUp
        Label14.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label15_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label15.MouseHover
        Label15.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label15_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label15.MouseLeave
        Label15.Image = My.Resources.ResourceManager.GetObject("tool_button_norm")
    End Sub
    Private Sub label15_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label15.MouseDown
        Label15.Image = My.Resources.ResourceManager.GetObject("tool_button_ click")
    End Sub
    Private Sub label15_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label15.MouseUp
        Label15.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label16_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label16.MouseHover
        Label16.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label16_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label16.MouseLeave
        Label16.Image = My.Resources.ResourceManager.GetObject("tool_button_norm")
    End Sub
    Private Sub label16_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label16.MouseDown
        Label16.Image = My.Resources.ResourceManager.GetObject("tool_button_ click")
    End Sub
    Private Sub label16_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label16.MouseUp
        Label16.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label17_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label17.MouseHover
        Label17.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label17_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label17.MouseLeave
        Label17.Image = My.Resources.ResourceManager.GetObject("tool_button_norm")
    End Sub
    Private Sub label17_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label17.MouseDown
        Label17.Image = My.Resources.ResourceManager.GetObject("tool_button_ click")
    End Sub
    Private Sub label17_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label17.MouseUp
        Label17.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label18_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label18.MouseHover
        Label18.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub label18_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label18.MouseLeave
        Label18.Image = My.Resources.ResourceManager.GetObject("tool_button_norm")
    End Sub
    Private Sub label18_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label18.MouseDown
        Label18.Image = My.Resources.ResourceManager.GetObject("tool_button_ click")
    End Sub
    Private Sub label18_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles Label18.MouseUp
        Label18.Image = My.Resources.ResourceManager.GetObject("tool_button_hover")
    End Sub
    Private Sub PictureBox11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox11.Click
        f2.Show()
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ProgressBar1.PerformStep()
        If ProgressBar1.Value >= ProgressBar1.Maximum Then
            Timer1.Enabled = False
            ProgressBar1.Hide()
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        GroupBox3.Show()
        GroupBox4.Hide()
        Label9.Hide()
        TabControl1.Hide()
        Label8.Hide()
        Label10.Hide()
        ListView2.Visible = False
        GroupBox2.Show()
        Label7.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        Label18.Hide()
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox6.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        GroupBox1.Hide()
        PictureBox4.Hide()
        Label1.Hide()
        Label12.Hide()
        Label13.Hide()
        refresh_drives()
        TrackBar1.Hide()
        ListView3.Hide()
        CheckBox8.Hide()
        Label6.Hide()
        Dim regVersion As RegistryKey
        Dim keyValue As String

        keyValue = "Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"
        regVersion = Registry.CurrentUser.OpenSubKey(keyValue, False)
        Dim intVersi As Integer = 0
        If (Not regVersion Is Nothing) Then
            intVersi = regVersion.GetValue("NoDriveTypeAutoRun", 0)
            If intVersi = 255 Then
                PictureBox8.Image = My.Resources.ResourceManager.GetObject("off normal")
                PictureBox7.Image = My.Resources.ResourceManager.GetObject("on selected")
                picturebox7select = 1
                picturebox8select = 0
            Else
                PictureBox8.Image = My.Resources.ResourceManager.GetObject("off-select")
                PictureBox7.Image = My.Resources.ResourceManager.GetObject("on normal")
                picturebox8select = 1
                picturebox7select = 0
            End If
            regVersion.Close()
        End If

        keyValue = "SYSTEM\CurrentControlSet\Services\Cdrom"
        regVersion = Registry.LocalMachine.OpenSubKey(keyValue, False)
        intVersi = 0
        If (Not regVersion Is Nothing) Then
            intVersi = regVersion.GetValue("AutoRun", 0)
            If intVersi = 0 Then
                PictureBox9.Image = My.Resources.ResourceManager.GetObject("off normal")
                PictureBox10.Image = My.Resources.ResourceManager.GetObject("on selected")
                picturebox10select = 1
                picturebox9select = 0
            Else
                PictureBox9.Image = My.Resources.ResourceManager.GetObject("off-select")
                PictureBox10.Image = My.Resources.ResourceManager.GetObject("on normal")
                picturebox10select = 0
                picturebox9select = 1
            End If
            regVersion.Close()
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        GroupBox3.Hide()
        Label9.Hide()
        Label8.Hide()
        Label7.Hide()
        Label10.Hide()
        Label18.Hide()
        ListView2.Visible = False
        GroupBox2.Hide()
        PictureBox4.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        Label12.Hide()
        Label13.Hide()
        ListView3.Hide()
        CheckBox8.Hide()
        Label6.Hide()
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            TabControl1.Hide()
            f12.Show()
            f12.TextBox2.Text = "setting"
        Else
            tools("setting")
           End If
    End Sub
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ListView3.Hide()
        CheckBox8.Hide()
        TabControl1.Hide()
        Label12.Hide()
        Label13.Hide()
        Label6.Hide()
        Label8.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        Label18.Hide()
        CheckBox6.Hide()
        GroupBox3.Hide()
        GroupBox4.Hide()
        Label9.Hide()
        Label10.Hide()
        ListView2.Visible = False
        GroupBox2.Hide()
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        Label7.Show()
        GroupBox1.Hide()
        PictureBox4.Show()

        Label1.Hide()
        TrackBar1.Hide()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        GroupBox3.Hide()
        GroupBox4.Hide()
        ListView2.Visible = True
        TabControl1.Hide()
        Label9.Show()
        Label8.Show()
        Label18.Hide()
        Label10.Show()
        GroupBox2.Hide()
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox6.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        Label12.Hide()
        Label13.Hide()
        Label7.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        GroupBox1.Hide()
        PictureBox4.Hide()
        Label1.Hide()
        TrackBar1.Hide()
        ListView3.Hide()
        CheckBox8.Hide()
        Label6.Hide()
        Label8.Text = "Find And Fix Problems Caused By Malware"
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        GroupBox3.Hide()
        GroupBox4.Hide()
        TabControl1.Hide()
        ListView2.Visible = False
        Label9.Hide()
        Label8.Hide()
        Label18.Hide()
        Label10.Hide()
        GroupBox2.Hide()
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox6.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        Label7.Hide()
        GroupBox1.Hide()
        PictureBox4.Hide()
        Label1.Hide()
        TrackBar1.Hide()
        ListView3.Show()
        CheckBox8.Show()
        Label6.Show()
        Label12.Show()
        Label13.Show()
        If CheckBox8.Checked = True Then
            ListView3.Enabled = True
            Label12.Enabled = True
            Label13.Enabled = True
        Else
            ListView3.Enabled = False
            Label12.Enabled = False
            Label13.Enabled = False
        End If
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        'f6.Show()
        GroupBox3.Hide()
        TabControl1.Hide()
        GroupBox4.Hide()
        ListView2.Visible = False
        Label9.Hide()
        Label8.Hide()
        Label10.Hide()
        GroupBox2.Hide()
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox6.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        Label7.Hide()
        Label12.Hide()
        Label13.Hide()
        Label18.Show()
        GroupBox1.Hide()
        PictureBox4.Hide()
        Label1.Hide()
        TrackBar1.Hide()
        ListView3.Hide()
        CheckBox8.Hide()
        Label6.Hide()
        Label14.Show()
        Label15.Show()
        Label16.Show()
        Label17.Show()
    End Sub
    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click
        ProgressBar1.Show()
        ProgressBar1.Step = 20
        ProgressBar1.Value = 0
        Timer1.Enabled = True
        custom_scan()
    End Sub
    Private Sub Ni_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
    Handles ni.MouseClick
        If e.Button = MouseButtons.Left Then
            Me.ShowInTaskbar = True
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
            Me.ni.Visible = False
            PictureBox2.Image = My.Resources.ResourceManager.GetObject("minimize_normal")
            Me.Button1_Click(sender, New System.EventArgs())
        ElseIf e.Button = MouseButtons.Right Then
            ni.ContextMenuStrip.Show()
        End If
    End Sub
    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        On Error Resume Next
        Dim l As Integer
        l = ListView4.Items.Count
        Dim j
        If l = 0 Then
        Else
            For Each item In ListView4.Items
                j = item.ToString.ToLower.Replace("listviewitem:", "")
                j = j.replace(" ", "")
                j = j.replace("{", "")
                j = j.replace("}", "")
                Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName(j)
                For Each p As Process In pProcess
                    p.Kill()
                Next
            Next
        End If
    End Sub
    Private Sub OpeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpeToolStripMenuItem.Click
        Me.Visible = True
        Me.WindowState = FormWindowState.Normal
        Me.ni.Visible = False
    End Sub
    Private Sub ScanToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScanToolStripMenuItem.Click
        custom_scan()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "close"
        Else
            tools("close")
        End If
    End Sub








    'vaccination
    Private Sub PictureBox7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox7.Click
        'usb autorun on
        On Error Resume Next
        PictureBox8.Image = My.Resources.ResourceManager.GetObject("off normal")
        PictureBox7.Image = My.Resources.ResourceManager.GetObject("on selected")
        Dim regVersion As RegistryKey
        regVersion = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\policies\Explorer\NoDriveTypeAutoRun", True)
        Dim intVersion As Integer = 255
        regVersion.SetValue("NoDriveTypeAutoRun", intVersion)
        regVersion.Close()
        regVersion = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True)
        intVersion = 255
        regVersion.SetValue("NoDriveTypeAutoRun", intVersion)
        regVersion.Close()
        picturebox7select = 1
        picturebox8select = 0

        Dim regKey1 As Microsoft.Win32.RegistryKey
        regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\IniFileMapping\Autorun.inf", True)
        Dim newkey As RegistryKey
        If regKey1 Is Nothing Then
            'MsgBox("Not Found")
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\IniFileMapping", True)
            newkey = key.CreateSubKey("Autorun.inf")
        End If
        Dim keyx As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\IniFileMapping\Autorun.inf", True)
        keyx.SetValue("", "@SYS:DoesNotExist") '(name, value)
    End Sub

    Private Sub PictureBox8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox8.Click
        'usb autorun off
        On Error Resume Next
        PictureBox8.Image = My.Resources.ResourceManager.GetObject("off-select")
        PictureBox7.Image = My.Resources.ResourceManager.GetObject("on normal")
        Dim regVersion As RegistryKey
        regVersion = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\policies\Explorer\NoDriveTypeAutoRun", True)
        Dim intVersion As Integer = 91
        regVersion.SetValue("NoDriveTypeAutoRun", intVersion)
        regVersion.Close()
        regVersion = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True)
        intVersion = 91
        regVersion.SetValue("NoDriveTypeAutoRun", intVersion)
        regVersion.Close()
        picturebox8select = 1
        picturebox7select = 0
        Dim delKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\IniFileMapping\Autorun.inf", True)
        delKey.DeleteValue("")
    End Sub

    Private Sub PictureBox10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox10.Click
        'cd autorun on
        On Error Resume Next
        PictureBox9.Image = My.Resources.ResourceManager.GetObject("off normal")
        PictureBox10.Image = My.Resources.ResourceManager.GetObject("on selected")
        Dim regVersion As RegistryKey
        regVersion = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Cdrom", True)
        Dim intVersion As Integer = 0
        regVersion.SetValue("AutoRun", intVersion)
        regVersion.Close()
        picturebox10select = 1
        picturebox9select = 0
    End Sub
    Private Sub PictureBox9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox9.Click
        ' cd autorun off
        On Error Resume Next
        PictureBox9.Image = My.Resources.ResourceManager.GetObject("off-select")
        PictureBox10.Image = My.Resources.ResourceManager.GetObject("on normal")
        Dim regVersion As RegistryKey
        regVersion = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Cdrom", True)
        Dim intVersion As Integer = 1
        regVersion.SetValue("AutoRun", intVersion)
        regVersion.Close()
        picturebox10select = 0
        picturebox9select = 1
        MsgBox("Please Restart Computer For Changes To Take Effect", 0, "Success!")
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        refresh_drives()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        immunize()
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        removeimmunity()
    End Sub



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim processes() As Process = System.Diagnostics.Process.GetProcesses()
        Dim p
        For Each proc As Process In processes
            Try
                If proc.ProcessName.ToLower = "autorun processor" Then
                    startuppath = proc.MainModule.FileName
                    startuppath = startuppath.tolower.replace("autorun processor.exe", "")

                    Exit For
                Else
                    startuppath = CurDir() 'assign current folder
                End If
                ' proc.MainModule.FileName
            Catch ex As Exception
            End Try
        Next

        'startuppath = CurDir() 'assign current folder

        Dim regKey1 As Microsoft.Win32.RegistryKey
        regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor\Screenlock", True)
        If regKey1 Is Nothing Then
        Else
            f14.Show()
        End If
        TextBox1.Text = "0"
        TabControl1.Hide()
        TextBox1.Hide()
        Label18.Hide()
        Label7.Show()
        Label8.Hide()
        Label9.Hide()
        Label10.Hide()
        Label12.Hide()
        Label13.Hide()
        Label14.Hide()
        Label15.Hide()
        Label16.Hide()
        Label17.Hide()
        ListView1.Columns.Add("path", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("file handle", 150, HorizontalAlignment.Left)
        ListView1.Visible = False
        ListView2.Columns.Add("Problem", 200, HorizontalAlignment.Left)
        ListView2.Columns.Add("Found In", 0, HorizontalAlignment.Left)
        ListView2.Columns.Add("Path", 100, HorizontalAlignment.Left)
        ListView2.Columns.Add("Key Or File", 150, HorizontalAlignment.Left)
        ListView2.Columns.Add("Action", 100, HorizontalAlignment.Left)
        ListView2.Visible = False
        'Timer2.Enabled = True
        'Timer2.Start()
        f2.ListView1.Columns.Add("Threat Name", 100, HorizontalAlignment.Left)
        f2.ListView1.Columns.Add("Threat Type", 150, HorizontalAlignment.Left)
        f2.ListView1.Columns.Add("Path", 200, HorizontalAlignment.Left)
        f2.ListView1.Columns.Add("File pointed", 250, HorizontalAlignment.Left)
        'startup
        f8.ListView1.Columns.Add("Name", 175, HorizontalAlignment.Left)
        f8.ListView1.Columns.Add("Program Path", 300, HorizontalAlignment.Left)
        f8.ListView1.Columns.Add("Path", 0, HorizontalAlignment.Left)
        f8.ListView1.Columns.Add("Found In", 0, HorizontalAlignment.Left)
        'taskman
        f9.ListView1.Columns.Add("Name", 150, HorizontalAlignment.Left)
        f9.ListView1.Columns.Add("Path", 200, HorizontalAlignment.Left)
        f9.ListView1.Columns.Add("Pid", 75, HorizontalAlignment.Left)
        f9.ListView1.Columns.Add("Priority", 50, HorizontalAlignment.Left)
        f9.ListView1.Columns.Add("Memory", 75, HorizontalAlignment.Left)
        GroupBox2.Hide()
        GroupBox3.Hide()
        GroupBox4.Hide()
        TrackBar1.Minimum = 50
        TrackBar1.Maximum = 100
        TrackBar1.TickFrequency = 5
        CheckBox1.Hide()
        CheckBox2.Hide()
        CheckBox3.Hide()
        CheckBox4.Hide()
        CheckBox5.Hide()
        CheckBox6.Hide()
        CheckBox7.Hide()
        CheckBox9.Hide()
        Label1.Hide()
        TrackBar1.Hide()
        GroupBox1.Hide()
        ProgressBar1.Hide()
        PictureBox4.Show()
        PictureBox6.Hide()
        PictureBox5.Show()
        ListView3.Columns.Add("Events", 1000, HorizontalAlignment.Left)
        ListView3.Hide()
        CheckBox8.Hide()
        Label6.Hide()
        CheckBox7.Enabled = False
        Me.ni.Visible = False
        Me.BackgroundImage = My.Resources.ResourceManager.GetObject("autorun processor")
        check_registry()
    End Sub
    Function check_registry()
        Dim regKey1 As Microsoft.Win32.RegistryKey
        Dim x
        regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True)
        Dim newkey As RegistryKey
        If regKey1 Is Nothing Then
            'MsgBox("Not Found")
            Me.WindowState = FormWindowState.Normal
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
            newkey = key.CreateSubKey("autorun processor")
            newkey.SetValue("tray exit", "1")
            newkey.SetValue("opacity", "95")
            newkey.SetValue("real time", "1")
            newkey.SetValue("Delete Pointed", "1")
            newkey.SetValue("preventexe", "1")
            newkey.SetValue("other notification", "2")
            RadioButton1.Checked = True
            RadioButton5.Checked = True
            newkey.SetValue("on detection", "1")
            TrackBar1.Value = 95
            CheckBox2.Checked = True
            CheckBox3.Checked = True
            CheckBox4.Checked = True
            CheckBox5.Checked = True
            CheckBox6.Checked = True
            CheckBox7.Checked = False
            x = MsgBox("Do You Want To Start Autorun Processor At Computer Startup?" & Chr(13) & "This Setting Can Also Be Modified Through Settings Option", MsgBoxStyle.YesNo, "Startup")
            startsetting = 1
            If x = MsgBoxResult.Yes Then
                CheckBox1.Checked = True
            Else
                CheckBox1.Checked = False
            End If
           
            MsgBox("This Seems To Be Your First Launch ,So You Are Requested" & Chr(13) & "To Customize The Settings According To Your Need", MsgBoxStyle.OkOnly, "Welcome")
        Else
            'MsgBox("Found")
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
            Me.ni.Visible = True
            ni.BalloonTipText = "Autorun Processor"
            ni.ShowBalloonTip(3000)
            Dim regKey As Object = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("tray exit")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "tray exit", "1")
                CheckBox2.Checked = True
            ElseIf regKey = 1 Then
                CheckBox2.Checked = True
            ElseIf regKey = 0 Then
                CheckBox2.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("hidden notify")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "hidden notify", "1")
                CheckBox4.Checked = True
            ElseIf regKey = 1 Then
                CheckBox4.Checked = True
            ElseIf regKey = 0 Then
                CheckBox4.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("on detection")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "on detection", "1")
                regKey.SetValue("on detection", "1")
                RadioButton1.Checked = True
            ElseIf regKey = 1 Then
                RadioButton1.Checked = True
            ElseIf regKey = 2 Then
                RadioButton2.Checked = True
            ElseIf regKey = 3 Then
                RadioButton3.Checked = True
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("opacity")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "opacity", "95")
                TrackBar1.Value = 95
            Else
                TrackBar1.Value = regKey
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("Delete Pointed")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "Delete Pointed", "1")
                CheckBox5.Checked = True
            ElseIf regKey = 1 Then
                CheckBox5.Checked = True
            ElseIf regKey = 0 Then
                CheckBox5.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("preventexe")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "preventexe", "1")
                CheckBox6.Checked = True
            ElseIf regKey = 1 Then
                CheckBox6.Checked = True
            ElseIf regKey = 0 Then
                CheckBox6.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("checkvirus")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "checkvirus", "0")
                CheckBox7.Checked = False
            ElseIf regKey = 1 Then
                CheckBox7.Checked = True
            ElseIf regKey = 0 Then
                CheckBox7.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("real time")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "real time", "1")
                CheckBox3.Checked = True
            ElseIf regKey = 1 Then
                CheckBox3.Checked = True
            ElseIf regKey = 0 Then
                CheckBox3.Checked = False
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("other notification")
            If regKey Is Nothing Then
                My.Computer.Registry.SetValue("HKEY_Local_Machine\SOFTWARE\autorun processor", "other notification", "2")
                regKey.SetValue("other notification", "2")
                RadioButton5.Checked = True
            ElseIf regKey = 1 Then
                RadioButton4.Checked = True
            ElseIf regKey = 2 Then
                RadioButton5.Checked = True
            ElseIf regKey = 3 Then
                RadioButton6.Checked = True
            End If

            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("passwd")
            If regKey Is Nothing Then
                CheckBox9.Checked = False
            Else
                CheckBox9.Checked = True
            End If

            regKey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True).GetValue("Autorun Processor")
            If regKey Is Nothing Then
                CheckBox1.Checked = False
            Else
                CheckBox1.Checked = True
            End If

        End If

        Me.Opacity = TrackBar1.Value / 100
        startsetting = 1
    End Function
    Function usbwinset()
        On Error Resume Next
        Dim regKey As Object
        regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", True)
        If regKey Is Nothing Then
            Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control", True)
            key.CreateSubKey("StorageDevicePolicies")
        End If

        regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", True).GetValue("WriteProtect")
        If regKey Is Nothing Then
            CheckBox10.Checked = False
        ElseIf regKey = 1 Then
            CheckBox10.Checked = True
        ElseIf regKey = 0 Then
            CheckBox10.Checked = False
        End If

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True).GetValue("NoCDBurning")
        If regKey Is Nothing Then
            CheckBox11.Checked = False
        ElseIf regKey = 1 Then
            CheckBox11.Checked = True
        ElseIf regKey = 0 Then
            CheckBox11.Checked = False
        End If

        regKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\UsbStor", True).GetValue("Start")
        If regKey Is Nothing Then
            CheckBox12.Checked = False
        ElseIf regKey = 4 Then
            CheckBox12.Checked = True
        ElseIf regKey = 3 Then
            CheckBox12.Checked = False
        End If

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True).GetValue("ShowSuperHidden")
        If regKey Is Nothing Then
            CheckBox13.Checked = False
        ElseIf regKey = 1 Then
            CheckBox13.Checked = True
        ElseIf regKey = 0 Then
            CheckBox13.Checked = False
        End If

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True).GetValue("Hidden")
        If regKey Is Nothing Then
        ElseIf regKey = 1 Then
            RadioButton8.Checked = True
        ElseIf regKey = 2 Then
            RadioButton7.Checked = True
        End If
    End Function
    'set settings to registry
    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        'usb write protect
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", True)
        If CheckBox10.Checked = True Then
            regkey.setvalue("WriteProtect", 1)
        Else
            regkey.deletevalue("WriteProtect")
        End If
    End Sub

    Private Sub CheckBox11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox11.CheckedChanged
        'no cd burn
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", True)
        If CheckBox11.Checked = True Then
            regkey.setvalue("NoCDBurning", 1)
        Else
            regkey.deletevalue("NoCDBurning")
        End If
    End Sub

    Private Sub CheckBox12_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox12.CheckedChanged
        'no rem drives
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\UsbStor", True)
        If CheckBox12.Checked = True Then
            regkey.setvalue("Start", 4)
        Else
            regkey.setvalue("Start", 3)
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox13.CheckedChanged
        'show super hidden
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True)
        If CheckBox13.Checked = True Then
            regkey.setvalue("ShowSuperHidden", 1)
        Else
            regkey.setvalue("ShowSuperHidden", 0)
        End If
    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        'show hidden
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True)
        regkey.setvalue("Hidden", 1)
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        'dont show hidden
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim regkey As Object = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True)
        regkey.setvalue("Hidden", 2)
    End Sub
    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        'password protect settings
        If startsetting = 0 Then
            Exit Sub
        End If
        On Error Resume Next
        Dim regKey As Object
        regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("passwd")
        Dim regkey1 As Object = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True)
        If CheckBox9.Checked = False Then
            regkey1.deletevalue("passwd")
        Else
            If regKey = Nothing Then
                f11.Show()
                f11.preliminary()
            End If
        End If
    End Sub
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("on detection", "1")
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("on detection", "2")
    End Sub
    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("on detection", "3")
    End Sub
    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("other notification", "1")

        'enable agent
        mobjController = New AgentObjects.Agent
        With mobjController
            .Connected = True
            .Characters.Load("merlin", "merlin.acs")
            mobjCharacter = .Characters("merlin")
        End With
        Dim leftpos As Long
        Dim toppos As Long
        leftpos = (My.Computer.Screen.WorkingArea.Right) - 200
        toppos = (My.Computer.Screen.WorkingArea.Bottom) - 200
        mobjCharacter.MoveTo(leftpos, toppos)
        With mobjCharacter
            .Show()
            .Width = 200
            .Height = 200
        End With
        isanimate = 1
    End Sub
    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("other notification", "2")
        If isanimate = 1 Then
            mobjCharacter.Hide()
        End If
        isanimate = 0
    End Sub
    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("other notification", "3")
        If isanimate = 1 Then
            mobjCharacter.Hide()
        End If
        isanimate = 0
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        On Error Resume Next
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
        Dim regkey As RegistryKey
        Dim dem = """" & CurDir() & "\Autorun Processor.exe" & """"
        If CheckBox1.Checked = True Then
            regkey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True).GetValue("Autorun Processor")
            If regKey Is Nothing Then
                key.SetValue("Autorun Processor", dem)
            End If
        Else
            key.DeleteValue("Autorun Processor")
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.CreateSubKey("autorun processor")
        If CheckBox2.Checked = True Then
            newkey.SetValue("tray exit", "1")
        Else
            newkey.SetValue("tray exit", "0")
        End If
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If startsetting = 0 Then
            If CheckBox3.Checked = True Then
                custom_scan()
            End If
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        If CheckBox3.Checked = True Then
            newkey.SetValue("real time", "1")
            custom_scan()
        Else
            newkey.SetValue("real time", "0")
        End If
    End Sub
    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        If CheckBox4.Checked = True Then
            newkey.SetValue("hidden notify", "1")
        Else
            newkey.SetValue("hidden notify", "0")
        End If
    End Sub
    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        If CheckBox5.Checked = True Then
            newkey.SetValue("Delete Pointed", "1")
            CheckBox7.Enabled = True
        Else
            newkey.SetValue("Delete Pointed", "0")
            CheckBox7.CheckState = CheckState.Unchecked
            CheckBox7.Enabled = False
        End If
    End Sub
    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        If CheckBox6.Checked = True Then
            newkey.SetValue("preventexe", "1")
        Else
            newkey.SetValue("preventexe", "0")
            unlockeverything()
        End If
    End Sub
    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If startsetting = 0 Then
            Exit Sub
        End If
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        If CheckBox7.Checked = True Then
            newkey.SetValue("checkvirus", "1")
        Else
            newkey.SetValue("checkvirus", "0")
        End If
    End Sub
    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            ListView3.Enabled = True
            Label12.Enabled = True
            Label13.Enabled = True
            watch_alldrives()
        Else
            ListView3.Enabled = False
            Label12.Enabled = False
            Label13.Enabled = False
            end_watch()
        End If
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Form1.ActiveForm.Opacity = TrackBar1.Value / 100
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software", True)
        Dim newkey As RegistryKey = key.OpenSubKey("autorun processor", True)
        newkey.SetValue("opacity", TrackBar1.Value)
    End Sub
    'detect drive
    Protected Overrides Sub WndProc(ByRef M As System.Windows.Forms.Message)
        'These are the required subclassing codes for detecting device based removal and arrival.
        'If M.Msg = WM_DEVICECHANGE Then
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim d As IO.DriveInfo
        Select Case M.WParam
            'Check if a device was added.
            Case DBT_DEVICEARRIVAL
                Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)
                If DevType = DBT_DEVTYP_VOLUME Then
                    Dim Vol As New DEV_BROADCAST_VOLUME
                    Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))
                    If Vol.Dbcv_Flags = 0 Then
                        For i As Integer = 0 To 20
                            If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then
                                Dim Usb = Chr(65 + i) + ":\"
                                For Each d In allDrives
                                    If d.IsReady = True AndAlso d.Name = Usb AndAlso (d.TotalSize / (1024 * 1024)) > 20 Then
                                        notification("USB Drive Detected")
                                        If CheckBox3.CheckState = CheckState.Checked Then
                                            scan(Usb.ToString)
                                        End If
                                        If CheckBox6.Checked = True Then
                                            lock_exe(Usb.ToString)
                                        End If
                                        If CheckBox8.Checked = True Then
                                            enable_watcher(Usb.ToString)
                                        End If
                                        'MsgBox("Looks like a USB device was plugged in!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb) '.ToString)
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End If
                End If

            Case DBT_DEVICEREMOVECOMPLETE
                Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)
                If DevType = DBT_DEVTYP_VOLUME Then
                    Dim Vol As New DEV_BROADCAST_VOLUME
                    Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))
                    If Vol.Dbcv_Flags = 0 Then
                        For i As Integer = 0 To 20
                            If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then '--->normal drives
                                Dim Usb = Chr(65 + i) + ":\"
                                notification("USB Drive Removed")
                                usb_removed(Usb.ToString)
                                ' MsgBox("Looks like a volume device was removed!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString)
                                Exit For
                            End If

                            If Math.Pow(2, i) + Math.Pow(2, i + 1) = Vol.Dbcv_Unitmask Then '----->u3 drives
                                Dim Usb As String = Chr(65 + i) + ":\"
                                Dim usb1 As String = Chr(65 + i + 1) + ":\"
                                usb_removed(Usb.ToString)
                                notification("USB Drive Removed")
                                usb_removed(Usb.ToString)
                                usb_removed(usb1.ToString)
                                'MsgBox("Looks like a volume device was removed!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString & vbNewLine & vbNewLine & "The drive letter is: " & usb1.ToString)
                                Exit For
                            End If
                        Next
                    End If
                End If

        End Select
        ' End If
        MyBase.WndProc(M)
    End Sub
    Function usb_removed(ByVal path)
        Dim removed = path
        Dim item1 As ListViewItem
        For Each items In f2.ListView1.Items
            If items.SubItems(2).Text().contains(path) Then
                items.remove()
            End If
        Next
        no_threat()
    End Function
    Function read_autorun(ByVal path)
        Dim FILE_NAME As String = path
        FILE_NAME = FILE_NAME.ToLower
        unlockafile(path)
        Dim readautorun() As String = System.IO.File.ReadAllLines(FILE_NAME)
        Dim x
        Dim y

        y = FILE_NAME.Replace("autorun.inf", "")
        For Each line As String In readautorun
            line = line.ToLower
            If line.Contains(" ") Then
                line = line.Replace(" ", Nothing)
            End If
            If line.Contains("open=") Then
                line = line.Replace("open=", Nothing)
                x = line
            End If
        Next
        read_autorun = y.toupper & x
    End Function
    Function custom_scan()
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim d As IO.DriveInfo
        For Each d In allDrives
            If d.IsReady = True AndAlso d.DriveType = IO.DriveType.Removable Then
                If IO.DriveType.Removable Then
                    scan(d.Name)
                End If
            End If
        Next
    End Function
    Function scan(ByVal path)
        TextBox1.Text = "0"
        Dim myDirectory As DirectoryInfo
        Dim aFile As FileInfo
        Dim adir As DirectoryInfo
        Dim str(5) As String
        Dim itm As ListViewItem
        Dim n = f2.ListView1.Items.Count
        Dim item1 As ListViewItem
        'hidden
        myDirectory = New DirectoryInfo(path)
        For Each aFile In myDirectory.GetFiles
            If (aFile.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                'MsgBox("hidden file found")
                str(0) = aFile.Name
                str(1) = "Hidden File"
                str(2) = aFile.FullName
                str(3) = "N/A"
                itm = New ListViewItem(str)
                'f2.ListView1.Items.Add(itm)
                If CheckBox4.CheckState = CheckState.Checked Then
                    f4.Show()
                End If
                If n = 0 Then
                    f2.ListView1.Items.Add(itm)
                Else
                    item1 = f2.ListView1.FindItemWithText(str(2))
                    If (item1 IsNot Nothing) Then
                    Else
                        f2.ListView1.Items.Add(itm)
                    End If
                End If
            End If
        Next

        For Each adir In myDirectory.GetDirectories
            If (adir.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                '  MsgBox("hidden folder found" & adir.Name)
                str(0) = adir.Name
                str(1) = "Hidden Folder"
                str(2) = adir.FullName
                str(3) = "N/A"
                itm = New ListViewItem(str)
                If CheckBox4.CheckState = CheckState.Checked Then
                    f4.Show()
                End If
                If n = 0 Then
                    f2.ListView1.Items.Add(itm)
                Else
                    item1 = f2.ListView1.FindItemWithText(str(2))
                    If (item1 IsNot Nothing) Then
                    Else
                        f2.ListView1.Items.Add(itm)
                    End If
                End If
            End If
        Next

        'autorun
        If System.IO.File.Exists(path & "autorun.inf") = True Then
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(path & "autorun.inf")
            If fileDetail.Length > 0 Then
                notification("Autorun File Detected")
                ' MsgBox("autorun detected")
                str(0) = "Autorun.inf"
                str(1) = "AutorunFile"
                str(2) = path & "autorun.inf"
                str(3) = read_autorun(path & "autorun.inf")
                itm = New ListViewItem(str)
                'lock(str(2)) 'locked
                PictureBox5.Hide()
                PictureBox11.Hide()
                PictureBox6.Show()
                Me.BackgroundImage = My.Resources.ResourceManager.GetObject("Danger")
                If n = 0 Then
                    f2.ListView1.Items.Add(itm)
                Else
                    item1 = f2.ListView1.FindItemWithText(str(2))
                    If (item1 IsNot Nothing) Then
                    Else
                        f2.ListView1.Items.Add(itm)
                    End If
                End If
            End If
            treatment(path)
        Else
            no_threat()
            If CheckBox6.Checked = True Then
                lock_exe(path)
            Else
                customscan_unlocking(path)
        End If
        End If

    End Function
    Function treatment(ByVal path As String)
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim n As Integer
        n = f2.ListView1.Items.Count
        'show hidden file notification
        If CheckBox4.CheckState = CheckState.Checked Then
            For Each items In f2.ListView1.Items
                If items.SubItems(1).Text() = "Hidden File" Then
                    f4.Show()
                End If
            Next
            For Each items In f2.ListView1.Items
                If items.SubItems(1).Text() = "Hidden Folder" Then
                    f2.Refresh()
                    f4.Show()
                End If
            Next
        End If
        'take action for pointed file
        Dim x = 0
        x = pointed_filedelete(path & "autorun.inf")
        Do Until x = 1

        Loop
        'take action
        If RadioButton1.Checked = True Then
            action_delete(path & "autorun.inf")
            notification("Autorun File Deleted")
        ElseIf RadioButton2.Checked = True Then
            action_rename(path & "autorun.inf")
            notification("Autorun File Renamed")
        ElseIf RadioButton3.Checked = True Then
            Dim f3 As New Form3
            Threading.Thread.Sleep(1000)
            If n > 0 Then
                For Each items In f2.ListView1.Items
                    If items.SubItems(1).Text() = "AutorunFile" Then
                        f3.Show()
                        f3.Label7.Text = items.SubItems(2).Text()
                    End If
                Next
            End If
        End If
        If CheckBox6.Checked = True Then
            lock_exe(path)
        Else
            customscan_unlocking(path)
        End If
       
        no_threat()
    End Function
    Function no_threat()
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim n As Integer
        n = f2.ListView1.Items.Count
        If n = 0 Then
            PictureBox6.Hide()
            PictureBox5.Show()
            Me.BackgroundImage = My.Resources.ResourceManager.GetObject("autorun processor")
            TextBox1.Text = "125"
            Label3.Text = "    No Threats Detected    "
            PictureBox5.Show()
            Me.BackgroundImage = My.Resources.ResourceManager.GetObject("autorun processor")
            Exit Function
        End If
        If n > 0 Then
            For Each items In f2.ListView1.Items
                If items.SubItems(1).Text() = "AutorunFile" Then

                    Label3.Text = "    Threats Have Been Detected    "
                    Exit Function
                End If
            Next
        End If
        If n > 0 Then
            For Each items In f2.ListView1.Items
                If items.SubItems(1).Text() = "Hidden File" Then
                    TextBox1.Text = "125"
                    Label3.Text = "    Hidden Objects Detected    "
                    PictureBox6.Hide()
                    PictureBox5.Hide()
                    PictureBox11.Show()
                    Me.BackgroundImage = My.Resources.ResourceManager.GetObject("main_window_yellow")
                    Exit Function
                End If
            Next
        End If
        If n > 0 Then
            For Each items In f2.ListView1.Items
                If items.SubItems(1).Text() = "Hidden Folder" Then
                    TextBox1.Text = "125"
                    Label3.Text = "    Hidden Objects Detected    "
                    PictureBox6.Hide()
                    PictureBox5.Hide()
                    PictureBox11.Show()
                    Me.BackgroundImage = My.Resources.ResourceManager.GetObject("main_window_yellow")
                    Exit Function
                End If
            Next
        End If
    End Function
    Function remove_current_autorun()
        Dim keyName As String = "Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2"
        Dim rootKey As RegistryKey
        rootKey = Registry.CurrentUser
        Dim subk As String
        Dim tmp As RegistryKey
        Dim tmp1 As RegistryKey
        rootKey = rootKey.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2", True)
        For Each subk In rootKey.GetSubKeyNames()
            'MessageBox.Show(subk)
            tmp = rootKey.OpenSubKey(subk, True)
            tmp1 = rootKey.OpenSubKey(subk & "\shell", True)
            If tmp1 Is Nothing Then
            Else
                If tmp.Name.Length > 90 Then
                    tmp.DeleteSubKeyTree("shell")
                End If
            End If
        Next
    End Function
    Function lock_exe(ByVal path As String) 'lock all exe
        Dim filenames() As String = Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories)
        For Each item As String In filenames
            lock(item)
        Next
    End Function
    Function pointed_filedelete(ByVal path As String)
        Dim FileToDelete As String
        Dim x = 0
        Dim ret = 0
        remove_current_autorun()
        FileToDelete = read_autorun(path)
        x = unlockafile(FileToDelete)
        'System.IO.File.SetAttributes(FileToDelete, attribute)
        Do Until x = 1
        Loop
        If CheckBox5.Checked = True Then
            If CheckBox7.Checked = True Then
                ret = scan_virus(FileToDelete) 'vir scan
                If ret = 1 Then
                    System.IO.File.Delete(FileToDelete)
                    notification("Virus File Found And Deleted")
                End If
            Else
                action_delete(FileToDelete)
                notification("Pointed File Deleted")
            End If
        Else

            If CheckBox6.Checked = True Then
            Else
                lock(FileToDelete) 'locked
            End If
        End If
        pointed_filedelete = 1
    End Function
    Function action_delete(ByVal filetodelete As String)
        remove_current_autorun()
        Dim x1 = 0
        unlockafile(filetodelete.ToLower) 'unlock
        If System.IO.File.Exists(filetodelete) = True Then
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(filetodelete)
            If fileDetail.Length > 0 Then
               System.IO.File.SetAttributes(filetodelete, attribute)
                System.IO.File.Delete(filetodelete)
              End If
        End If
        Dim n As Integer
        n = f2.ListView1.Items.Count
        If n > 0 Then
            For Each items In f2.ListView1.Items
                If items.SubItems(2).Text() = filetodelete Then
                    items.remove()
                End If
            Next
        End If
        If TextBox1.Text.ToLower.Contains(filetodelete.ToLower.Replace("autorun.inf", "")) Then
        Else
            TextBox1.Text = TextBox1.Text & filetodelete.ToLower.Replace("autorun.inf", "")
        End If
        
        'no_threat()
    End Function
    Function action_rename(ByVal renamefile As String)
        Dim x As String
        Dim y As String
        Dim FileToDelete As String
        Dim n As Integer
        n = f2.ListView1.Items.Count
        remove_current_autorun()
        x = renamefile
        x = x.ToLower
        y = x.Replace(".inf", ".ren")
        FileToDelete = y
        If System.IO.File.Exists(FileToDelete) = True Then
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(FileToDelete)
            If fileDetail.Length > 0 Then
                System.IO.File.SetAttributes(FileToDelete, attribute)
                System.IO.File.Delete(FileToDelete)
            End If
        End If
        unlockafile(x)
        System.IO.File.SetAttributes(x, attribute)
        My.Computer.FileSystem.RenameFile(x, "autorun.ren")
        If n > 0 Then
            For Each items In f2.ListView1.Items
                If items.SubItems(2).Text() = renamefile Then
                    items.remove()
                End If
            Next
        End If
        If TextBox1.Text.ToLower.Contains(renamefile.ToLower.Replace("autorun.inf", "")) Then
        Else
            TextBox1.Text = TextBox1.Text & renamefile.ToLower.Replace("autorun.inf", "")
        End If
        'no_threat()
    End Function
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Label3.Text = Label3.Text.Substring(1) & Label3.Text.Substring(0, 1)
    End Sub
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        f7.Show()
    End Sub
    'usb vaccination
    Function immunize()
        Dim x = ComboBox1.Text.ToUpper & "AUTORUN.INF"
        Dim di As DirectoryInfo = New DirectoryInfo(x)

        If System.IO.File.Exists(ComboBox1.Text & "AUTORUN.INF") = True Then
            Dim fileDetail As IO.FileInfo
            fileDetail = My.Computer.FileSystem.GetFileInfo(ComboBox1.Text & "AUTORUN.INF")
            If fileDetail.Length > 0 Then
                Dim attribute As System.IO.FileAttributes = IO.FileAttributes.Normal
                System.IO.File.SetAttributes(x, attribute)
                System.IO.File.Delete(ComboBox1.Text & "AUTORUN.INF")
            Else
                Label11.Text = "Immunized"
                Label11.ForeColor = Color.Green
                Button5.Enabled = False
                Button6.Enabled = True
                Exit Function
            End If
        Else
        End If
        di.Create()
        FileSystem.SetAttr(x, DirectCast((FileAttribute.System + FileAttribute.Hidden + FileAttribute.ReadOnly), FileAttribute))
        Shell("cmd /c mkdir \\.\" & x & "\prn", vbHide)
        Label1.Text = "Immunized"
        Label11.ForeColor = Color.Green
        Button5.Enabled = False
        Button6.Enabled = True

    End Function
    Function removeimmunity()
        Dim x = ComboBox1.Text & "AUTORUN.INF"
        Dim di As DirectoryInfo = New DirectoryInfo(x)

        Try
            If System.IO.File.Exists(ComboBox1.Text & "AUTORUN.INF") = True Then
                Dim fileDetail As IO.FileInfo
                fileDetail = My.Computer.FileSystem.GetFileInfo(ComboBox1.Text & "AUTORUN.INF")
                If fileDetail.Length = 0 Then
                    MsgBox("The USB Drive Was Not Immunized By Autorun Processor" & Chr(13) & "Use The Program That Immunized It To Remove The Immunization")
                    Exit Function
                End If
            End If
            FileSystem.SetAttr(x, DirectCast((FileAttribute.Normal), FileAttribute))
            Shell("cmd /c rmdir \\.\" & x & "\prn", vbHide)
            Threading.Thread.Sleep(1000)
            Shell("cmd /c rmdir " & x, vbHide)
            Threading.Thread.Sleep(500)
            checkremoval()
        Catch ex As Exception
        End Try

    End Function
    Function checkremoval()
        Threading.Thread.Sleep(500)
        Dim x = ComboBox1.Text & "AUTORUN.INF"
        Dim di As DirectoryInfo = New DirectoryInfo(x)
        If di.Exists Then
            MsgBox("The USB Drive Was Not Immunized By Autorun Processor" & Chr(13) & "Use The Program That Immunized It To Remove The Immunization")
        Else
            Label11.Text = "Not Immunized"
            Label11.ForeColor = Color.Red
            Button6.Enabled = False
            Button5.Enabled = True
        End If
    End Function
    Function refresh_drives()
        ComboBox1.Items.Clear()
        ComboBox1.Text = Nothing
        Button5.Enabled = False
        Button6.Enabled = False
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
    End Function
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim myDirectory As DirectoryInfo
        Dim adir As DirectoryInfo
        Dim n = ComboBox1.Items.Count
        If n > 0 Then
            myDirectory = New DirectoryInfo(ComboBox1.Text)
            For Each adir In myDirectory.GetDirectories
                If (adir.Name.ToUpper = "AUTORUN.INF") Then
                    Label11.Show()
                    Label11.Text = "Immunized"
                    Label11.ForeColor = Color.Green
                    Button5.Enabled = False
                    Button6.Enabled = True
                    Return
                Else
                    If System.IO.File.Exists(ComboBox1.Text & "AUTORUN.INF") = True Then
                        Dim fileDetail As IO.FileInfo
                        fileDetail = My.Computer.FileSystem.GetFileInfo(ComboBox1.Text & "AUTORUN.INF")
                        If fileDetail.Length > 0 Then
                            Label11.Show()
                            Label11.Text = "Not Immunized"
                            Label11.ForeColor = Color.Red
                            Button6.Enabled = False
                            Button5.Enabled = True
                        Else
                            Label11.Show()
                            Label11.Text = "Seems To Be Immunized By Some Other Program"
                            Label11.ForeColor = Color.Green
                            Button5.Enabled = False
                            Button6.Enabled = True
                        End If
                    End If
                    Label11.Show()
                    Label11.Text = "Not Immunized"
                    Label11.ForeColor = Color.Red
                    Button6.Enabled = False
                    Button5.Enabled = True
                End If
            Next
        Else
            Label11.Show()
            Label11.Text = "No Removable Drives Detected"
            Label11.ForeColor = Color.Black
            Button5.Enabled = False
            Button6.Enabled = False
        End If
    End Sub
    'notifications
    Function notification(ByVal text As String)
        If RadioButton4.Checked = True Then
            mobjCharacter.Speak(text)
        ElseIf RadioButton5.Checked = True Then
            If ni.Visible = True Then
                ni.BalloonTipText = text
                ni.ShowBalloonTip(750)
            End If
        ElseIf RadioButton6.Checked = True Then

        End If
    End Function
    'drive watcher
    Private Sub Label13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label13.Click
        'export to text file
        Dim MyFolderBrowser As New System.Windows.Forms.FolderBrowserDialog
        Dim savetextin = Nothing
        Dim time = DateTime.Now.TimeOfDay.ToString
        time = time.Replace(":", "_")
        time = time.Substring(0, time.Length - 8)
        Dim l
        MyFolderBrowser.Description = "Select the Path To Save Log File"
        ' Sets the root folder where the browsing starts from 
        MyFolderBrowser.RootFolder = Environment.SpecialFolder.DesktopDirectory
        Dim dlgResult As DialogResult = MyFolderBrowser.ShowDialog()
        If dlgResult = Windows.Forms.DialogResult.OK Then
            savetextin = MyFolderBrowser.SelectedPath & "\" & "APlog_" & time & ".txt"
        Else
            Exit Sub
        End If
        Dim fs As FileStream = Nothing
        If (Not File.Exists(savetextin)) Then
            fs = File.Create(savetextin)
            fs.Close()
            writetofile(savetextin)
        Else
            l = MsgBox("File Already Exists Do You Want To Replace File?", MsgBoxStyle.YesNo, "")
            If l = vbYes Then
                File.Delete(savetextin)
                writetofile(savetextin)
            Else
                MsgBox("Cannot Save File", MsgBoxStyle.OkOnly, "Error")
            End If

        End If
    End Sub
    Function writetofile(ByVal path As String)
        Using sw As StreamWriter = New StreamWriter(path)
            sw.Write("# Autorun Processor Drive Watcher Log" & vbCrLf)
            sw.Write("# Date:" & DateTime.Now.Date.ToString.Replace("12:00:00 AM", "") & vbCrLf)
            sw.Write("# Time:" & DateTime.Now.Hour.ToString & "." & DateTime.Now.Minute.ToString & vbCrLf)
            For Each items In ListView3.Items
                sw.Write(items.SubItems(0).Text())
                sw.Write(vbCrLf)
            Next
        End Using
    End Function
    Function end_watch()
        If watcher_enabled = 1 Then
            watchfolder.EnableRaisingEvents = False
            watcher_enabled = 0
        End If
    End Function
    Function watch_alldrives()
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim d As IO.DriveInfo
        For Each d In allDrives
            If d.IsReady = True AndAlso d.DriveType = IO.DriveType.Removable Then
                If IO.DriveType.Removable Then
                    enable_watcher(d.Name)
                End If
            End If
        Next
    End Function
    Function enable_watcher(ByVal path As String)
        watchfolder = New System.IO.FileSystemWatcher()
        'this is the path we want to monitor
        watchfolder.IncludeSubdirectories = True
        watchfolder.Path = path
        watchfolder.NotifyFilter = IO.NotifyFilters.LastAccess
        watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                   IO.NotifyFilters.FileName
        watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                   IO.NotifyFilters.Attributes
        watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                   IO.NotifyFilters.DirectoryName
        watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                 IO.NotifyFilters.LastWrite
        AddHandler watchfolder.Changed, AddressOf logchange
        AddHandler watchfolder.Created, AddressOf logchange
        AddHandler watchfolder.Deleted, AddressOf logchange
        ' add the rename handler as the signature is different
        AddHandler watchfolder.Renamed, AddressOf logrename
        watchfolder.EnableRaisingEvents = True
        watcher_enabled = 1
    End Function
    Private Delegate Sub updateListbox(ByVal newList As String)
    Private Sub updateListHandler(ByVal listText As String)
        Dim time
        time = DateTime.Now.TimeOfDay
        If time = check Then
        Else
            ListView3.Items.Add(listText)
            check = DateTime.Now.TimeOfDay
        End If

    End Sub
    Private Sub logchange(ByVal source As Object, ByVal e As  _
                      System.IO.FileSystemEventArgs)
        Dim x As String = Nothing
        If e.ChangeType = IO.WatcherChangeTypes.Changed Then
            x = (e.FullPath & _
                                      " has been modified")
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Created Then
            x = (e.FullPath & _
                                     " has been created" & vbCrLf)
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
            x = (e.FullPath & _
                                        " has been deleted" & vbCrLf)
        End If
        If Me.InvokeRequired Then 'are we running on a secondary thread
            Dim d As New updateListbox(AddressOf updateListHandler)
            Me.Invoke(d, New Object() {x})
        Else
            updateListHandler(x)
        End If
    End Sub
    Public Sub logrename(ByVal source As Object, ByVal e As  _
                             System.IO.RenamedEventArgs)
        Dim x As String
        x = (e.OldName & _
                      " has been renamed to " & e.Name & vbCrLf)

        If Me.InvokeRequired Then 'are we running on a secondary thread
            Dim d As New updateListbox(AddressOf updateListHandler)
            Me.Invoke(d, New Object() {x})
        Else
            updateListHandler(x)
        End If
    End Sub
    'virus scan
    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click
        Dim t As Thread
        unlockeverything()
        t = New Thread(AddressOf Me.unlockeverything)
        t.Start()
        t = New Thread(AddressOf Me.getfiles)
        t.Start()
    End Sub
    Function getfiles()
        Threading.Thread.Sleep(100)
        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim isvirus
        Dim d As IO.DriveInfo
        If Me.InvokeRequired Then 'are we running on a secondary thread
            Dim j As New updateListbox(AddressOf updateListHandler)
            Me.Invoke(j, New Object() {"Scan Started"})
        Else
            updateListHandler("Scan Started")
        End If
        For Each d In allDrives
            If d.IsReady = True AndAlso d.DriveType = IO.DriveType.Removable Then
                If IO.DriveType.Removable Then
                    'get files
                    Dim filenames() As String = Directory.GetFiles(d.Name, "*.exe", SearchOption.AllDirectories)
                    For Each item As String In filenames
                        isvirus = scan_virus(item)
                        If isvirus = 1 Then
                            'file is virus
                            Threading.Thread.Sleep(5)
                            System.IO.File.SetAttributes(item, attribute)
                            System.IO.File.Delete(item)
                            If Me.InvokeRequired Then 'are we running on a secondary thread
                                Dim j As New updateListbox(AddressOf updateListHandler)
                                Threading.Thread.Sleep(5)
                                Me.Invoke(j, New Object() {"Virus Detected And Deleted: " & item})
                            Else
                                Threading.Thread.Sleep(5)
                                updateListHandler("Virus Detected And Deleted" & item)
                            End If
                        End If
                    Next
                    If CheckBox6.Checked = True Then
                        lock_exe(d.Name)
                    End If
                End If
            End If
        Next

        If Me.InvokeRequired Then 'are we running on a secondary thread
            Dim j As New updateListbox(AddressOf updateListHandler)
            Me.Invoke(j, New Object() {"Scan Finished"})
        Else
            updateListHandler("Scan Finished")
        End If

    End Function
    Function scan_virus(ByVal file As String)
        On Error Resume Next
        'get md5
        scan_virus = 0
        Dim fileMD5 As String
        fileMD5 = GenerateFileMD5(file)
        'check with virus md5
        Dim FILE_NAME As String = startuppath & "\virusbase.update" '---->path1
        FILE_NAME = FILE_NAME.ToLower
        Dim readupdate() As String = System.IO.File.ReadAllLines(FILE_NAME)
        For Each line As String In readupdate
            If line = fileMD5 Then
                scan_virus = 1
                Exit Function
            End If
        Next
    End Function
    Function GenerateFileMD5(ByVal filePath As String)
        On Error Resume Next
        Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        Dim f As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Delete, 8192)
        f = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Delete, 8192)
        md5.ComputeHash(f)
        f.Dispose()
        f.Close()
        Dim hash As Byte() = md5.Hash
        Dim buff As StringBuilder = New StringBuilder
        Dim hashByte As Byte
        For Each hashByte In hash
            buff.Append(String.Format("{0:X2}", hashByte))
        Next
        Dim md5string As String
        md5string = buff.ToString()
        Return md5string
    End Function
    'xp troubleshooting
    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click
        Label8.Text = "Initializing............."
        If System.IO.File.Exists(startuppath & "\up1.update") = True Then
            scan_cbmal()
        Else
            MsgBox("File Not Found", 0, "Error")
            Label8.Text = "Find And Fix Problems Caused By Malware"
        End If

    End Sub
    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click
        action_cbmal()
    End Sub
    Function scan_cbmal()
        Label8.Text = "Finding............."
        Dim FILE_NAME As String = startuppath & "\up1.update"
        Dim returned
        FILE_NAME = FILE_NAME.ToLower
        Dim readupdate() As String = System.IO.File.ReadAllLines(FILE_NAME)
        Dim str(5) As String
        Dim itm As ListViewItem
        Dim x = 0
        For Each line As String In readupdate
            x = x + 1
            If x = 1 Then
                str(0) = line
            ElseIf x = 2 Then
                str(1) = line
            ElseIf x = 3 Then
                str(2) = line
            ElseIf x = 4 Then
                str(3) = line
            ElseIf x = 5 Then
                str(4) = line
                If str(1).ToLower.Contains("registry") Then
                    returned = if_regexist(str(2), str(3), str(4))
                    If returned = 1 Then
                        itm = New ListViewItem(str)
                        ListView2.Items.Add(itm)
                        x = 0
                    Else
                        x = 0
                    End If
                ElseIf str(1).ToLower.Contains("file") Then
                    returned = if_fileexist(str(2), str(3))
                    If returned = 1 Then
                        itm = New ListViewItem(str)
                        ListView2.Items.Add(itm)
                        x = 0
                    Else
                        x = 0
                    End If
                End If
            End If
        Next
        'enable all checkboxes
        For Each item In ListView2.Items
            item.checked = True
        Next
        Dim n = ListView2.Items.Count
        If n = 0 Then
            Label8.Text = "Search Complete ,Nothing Detected"
        Else
            Label8.Text = "Problems Detected...Fix Them As Soon As Possible"
        End If
    End Function
    Function if_regexist(ByVal value As String, ByVal value1 As String, ByVal value2 As String)
        Dim readValue As String
        readValue = My.Computer.Registry.GetValue(value, value1, Nothing)
        If readValue <> "" Then
            'if_regexist = 1 'key exist
            If value2.ToLower.Contains("delete") Then
                if_regexist = 1
                Exit Function
            End If
            If readValue = value2 Then
                if_regexist = 0
            Else
                if_regexist = 1
            End If
        Else
            if_regexist = 0 'key not found
        End If
    End Function
    Function if_fileexist(ByVal value As String, ByVal value1 As String)
        Dim MyFile As New FileInfo(value & "\" & value1)
        If MyFile.Exists() Then
            if_fileexist = 1
        Else
            if_fileexist = 0
        End If
    End Function
    Function action_cbmal()
        On Error Resume Next
        Label8.Text = "Fixing Problems.................."
        Dim n = ListView2.CheckedItems.Count
        If n = 0 Then
            MsgBox("Nothing Checked", 0, "Error")
        Else
            For Each item In ListView2.CheckedItems
                If item.SubItems(1).Text.ToLower.Contains("registry") Then
                    action_regkey(item.SubItems(2).Text(), item.SubItems(3).Text(), item.SubItems(4).Text())
                    item.Remove()
                ElseIf item.SubItems(1).Text.ToLower.Contains("file") Then
                    action_file(item.SubItems(2).Text(), item.SubItems(3).Text())
                    item.Remove()
                End If
            Next
        End If
        Label8.Text = "Find And Fix Problems Caused By Malware"
    End Function
    Function action_regkey(ByVal value As String, ByVal value1 As String, ByVal value2 As String)
        Dim regKey1 As Microsoft.Win32.RegistryKey
        Dim hive As String
        Dim delkey
        If value2.ToLower.Contains("delete") Then
            hive = value.ToLower
            delkey = value1.ToLower
            If hive.ToLower.Contains("local_machine") Then
                hive = hive.Replace("hkey_local_machine\", "")
                regKey1 = Registry.LocalMachine.OpenSubKey(hive, True)
                regKey1.DeleteValue(value1)
            ElseIf hive.ToLower.Contains("current_user") Then
                hive = hive.Replace("hkey_current_user\", "")
                regKey1 = Registry.CurrentUser.OpenSubKey(hive, True)
                regKey1.DeleteValue(value1)
            ElseIf hive.ToLower.Contains("users") Then
                hive = hive.Replace("hkey_users\", "")
                regKey1 = Registry.Users.OpenSubKey(hive, True)
                regKey1.DeleteValue(value1)
            ElseIf hive.ToLower.Contains("classes_root") Then
                hive = hive.Replace("hkey_classes_root\", "")
                regKey1 = Registry.ClassesRoot.OpenSubKey(hive, True)
                regKey1.DeleteValue(value1)
            End If
        Else
            My.Computer.Registry.SetValue(value, value1, value2)
        End If
    End Function
    Function action_file(ByVal value As String, ByVal value1 As String)
        Dim FileToDelete As String
        FileToDelete = value & "\" & value1
        If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.SetAttributes(FileToDelete, attribute)
            System.IO.File.Delete(FileToDelete)
        End If
    End Function
    'locking and unlocking functions
    Function unlockafile(ByVal path As String)
        Dim handleno
        For Each items In ListView1.Items
            If items.SubItems(0).Text().tolower = path.ToLower Then
                handleno = items.SubItems(1).Text()
                UnlockAll(handleno)
                'MsgBox(path.ToLower)
                items.remove()
            End If
        Next
        unlockafile = 1
    End Function
    Function unlockeverything()
        If safeHandles IsNot Nothing Then
            For Each sfh As SafeFileHandle In safeHandles
                UnlockFile(sfh)
            Next
            safeHandles.Clear()
        End If
    End Function
    Function lock(ByVal path As String)
        '-------------->declare path here
        ' Lock the file, put the returned handle into our list of safe file handles:
        Dim sfh As SafeFileHandle = LockFile(path)
        If sfh IsNot Nothing And sfh.IsInvalid = False Then
            safeHandles.Add(sfh)
        Else
            Debug.WriteLine("didn't get a handle for: " & path)
        End If

    End Function
    Private Delegate Sub updateListview1(ByVal newList As String)
    Private Sub updateListHandler1(ByVal listText As String)
        Me.ListView1.Items.Add(listText)
    End Sub
    Private Function LockFile(ByVal path As String) As SafeFileHandle
        Dim sfh As SafeFileHandle = API.CreateFile(path, 3, 0, 0, 3, 0, IntPtr.Zero)
        Dim str(5) As String
        str(0) = path.ToLower
        str(1) = sfh.DangerousGetHandle
        Dim itm As ListViewItem
        itm = New ListViewItem(str)

        If Me.InvokeRequired Then 'are we running on a secondary thread
            Dim j As New updateListview1(AddressOf updateListHandler1)
            Me.Invoke(j, New Object() {"Scan Finished"})
        Else
            updateListHandler1("Scan Finished")
        End If
        '  ListView1.Items.Add(itm)

        If API.LockFile(sfh, 0, 0, 1024, 1024) Then ' locks a weird portion of the file - I think it locks 4398046512128 bytes?
            Debug.WriteLine("Locked: " & path)
        Else
            Debug.WriteLine("failed to lock: " & path)
        End If
        Return sfh
    End Function
    Private Sub customscan_unlocking(ByVal drivename As String)
        Dim handleno 'unlockes all files in a drive
        For Each items In ListView1.Items
            If items.SubItems(0).Text().tolower.contains(drivename.ToLower) Then
                handleno = items.SubItems(1).Text()
                UnlockAll(handleno)
                items.remove()
            End If
        Next
    End Sub
    Private Sub UnlockAll(ByVal unlockme As Integer)
        If safeHandles IsNot Nothing Then
            For Each sfh As SafeFileHandle In safeHandles
                If sfh.DangerousGetHandle = unlockme Then
                    UnlockFile(sfh)
                End If
            Next
            safeHandles.Clear()
        End If
    End Sub
    Private Sub UnlockFile(ByVal sfh As SafeFileHandle)
        On Error Resume Next
        If API.UnLockFile(sfh, 0, 0, 1024, 1024) Then
            Debug.WriteLine("unlocked a file.")
        Else
            Debug.WriteLine("failed to unlock a file")
        End If
        sfh.Dispose() ' calls closehandle internally - locks will be removed even if unlock failed.

    End Sub
    'pass encrypt
    Function encrypt_pass(ByVal text As String)
        Dim eax, ebx, ecx, edx, ebp, part1, part2, part3, part4 As Long
        Dim name1 As String
        name1 = text
        ebp = &H6A
        For Each singlechar In name1
            ecx = AscW(singlechar)
            ebp = ebp + ecx * 2
        Next
        part1 = ebp

        For Each singlechar In name1
            ecx = AscW(singlechar)
            ecx = ecx * 2
            edx = ecx * 9
            ebp = edx + ebp
        Next
        part2 = ebp

        For Each singlechar In name1
            ecx = AscW(singlechar)
            eax = ecx * 5
            eax = eax * 2 + ecx
            eax = eax * 2 + 1
        Next
        part3 = eax

        For Each singlechar In name1
            ecx = AscW(singlechar)
            ebx = ecx * 4 + &H1D
        Next
        part4 = ebx

        encrypt_pass = part1 & part2 & part3 & part4
    End Function
    'tools
    Private Sub Label14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label14.Click
        'open task killer
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "taskkiller"
        Else
            tools("taskkiller")
        End If
    End Sub

    Private Sub Label15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label15.Click
        'startup item
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "startup"
        Else
            tools("startup")
        End If
    End Sub

    Private Sub Label16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label16.Click
        'screen lock
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "screenlock"
        Else
            tools("screenlock")
        End If
    End Sub

    Private Sub Label17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label17.Click
        'style usb
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "style usb"
        Else
            tools("style usb")
        End If
    End Sub
    Private Sub Label18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label18.Click
        'other options
        If CheckBox9.Checked = True Then
            Dim f12 As New Form12
            f12.Show()
            f12.TextBox2.Text = "others"
        Else
            tools("others")
        End If
    End Sub
    Function tools(ByVal tool As String)
        If tool = "close" Then
            Me.Close()
        ElseIf tool = "style usb" Then
            f6.Show()
            f6.refresh_drives()
            f6.TopMost = True
            f6.TopMost = False
        ElseIf tool = "startup" Then
            f8.Show()
            f8.startup_paths()
            f8.TopMost = True
            f8.TopMost = False
        ElseIf tool = "taskkiller" Then
            f9.Show()
            f9.Timer1.Enabled = True
            f9.TopMost = True
            f9.TopMost = False
        ElseIf tool = "others" Then
            f10.Show()
            f10.preliminary()
            f10.TopMost = True
            f10.TopMost = False
        ElseIf tool = "setting" Then
            TabControl1.Show()
            GroupBox4.Show()
            CheckBox1.Show()
            CheckBox2.Show()
            CheckBox3.Show()
            CheckBox4.Show()
            CheckBox5.Show()
            CheckBox7.Show()
            CheckBox9.Show()
            CheckBox6.Show()
            GroupBox1.Show()
            Label1.Show()
            TrackBar1.Show()
            Me.TabControl1.SelectedTab = TabPage1
            Me.TabPage1.Select()
            usbwinset()
        ElseIf tool = "screenlock" Then
            f13.Show()
            f13.loader()
        End If
    End Function
End Class


Public Class API
    'own the handle of the file to this program
    <DllImport("kernel32")> Public Shared Function _
        CreateFile(ByVal FileName As String, _
        ByVal DesiredAccess As Integer, _
        ByVal ShareMode As Integer, _
        ByVal SecurityAttributes As IntPtr, _
        ByVal CreationDisposition As Integer, _
        ByVal FlagsAndAttributes As Integer, _
        ByVal hTemplateFile As IntPtr) As SafeFileHandle
    End Function

    'lock the files from edit,delete,cut,copy
    <DllImport("kernel32")> Public Shared Function _
      LockFile(ByVal hFile As SafeFileHandle, _
      ByVal dwFileOffsetLow As Integer, _
      ByVal dwFileOffsetHigh As Integer, _
      ByVal nNumberOfBytesToLockLow As Integer, _
      ByVal nNumberOfBytesToLockHigh As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    'lock the files from edit,delete,cut,copy
    <DllImport("kernel32")> Public Shared Function _
      UnLockFile(ByVal hFile As SafeFileHandle, _
      ByVal dwFileOffsetLow As Integer, _
      ByVal dwFileOffsetHigh As Integer, _
      ByVal nNumberOfBytesToLockLow As Integer, _
      ByVal nNumberOfBytesToLockHigh As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
End Class

