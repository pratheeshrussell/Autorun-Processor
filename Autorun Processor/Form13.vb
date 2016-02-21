Imports Microsoft.Win32
Imports System.IO

Public Class Form13
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
    Private Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
    Private Const KEYEVENTF_KEYUP As Long = &H2
    Private Const VK_LWIN As Long = &H5B ' left WIN key
    Private Const VK_RWIN As Long = &H5C ' right WIN key
    Function loader()
        Me.ShowInTaskbar = False
        Label4.Hide()
        Label5.Hide()
        ComboBox2.Hide()
        TextBox2.Hide()
        Button5.Hide()
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor", True)
        key.CreateSubKey("Screenlock")
        RadioButton1.Checked = True
    End Function
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'cancel
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor", True)
        key.DeleteSubKey("Screenlock")
        Me.Hide()
    End Sub
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Label3.Hide()
        TextBox1.Hide()
        ComboBox1.Hide()
        Label2.Hide()
        Button3.Hide()
        CheckBox1.Checked = False
        CheckBox1.Enabled = False
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Label3.Show()
        TextBox1.Show()
        ComboBox1.Hide()
        Label2.Hide()
        Button3.Hide()
        CheckBox1.Checked = True
        CheckBox1.Enabled = True
    End Sub
    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        Label3.Show()
        TextBox1.Show()
        ComboBox1.Show()
        Label2.Show()
        Button3.Show()
        CheckBox1.Checked = True
        CheckBox1.Enabled = True
        getremdrive()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'sec ques set
        Label4.Hide()
        Label5.Hide()
        ComboBox2.Hide()
        TextBox2.Hide()
        Button5.Hide()
        Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\", True)
        Dim p As Object = key1.GetValue("question")
        Dim q As Object = key1.GetValue("answer")
        If p IsNot Nothing Then
            key1.DeleteValue("question")
        End If
        If p IsNot Nothing Then
            key1.DeleteValue("answer")
        End If
        key1.SetValue("question", ComboBox2.Text)
        key1.SetValue("answer", TextBox2.Text)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'sec ques 
        Label4.Show()
        Label5.Show()
        ComboBox2.Show()
        TextBox2.Show()
        Button5.Show()
        Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\", True)
        Dim quest As Object = key1.GetValue("question")
        Dim ans As Object = key1.GetValue("answer")
        If quest Is Nothing Then
        Else
            ComboBox2.Text = quest.ToString
        End If
        If ans Is Nothing Then
        Else
            TextBox2.Text = ans.ToString
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        getremdrive()
    End Sub
    Function getremdrive()
        ComboBox1.Items.Clear()
        For Each d As System.IO.DriveInfo In My.Computer.FileSystem.Drives
            If d.DriveType = IO.DriveType.Removable Then
                ComboBox1.Items.Add(d.Name)
                ComboBox1.Text = d.Name
            End If
        Next
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\Screenlock", True)
        Dim FileToDelete As String
        FileToDelete = "safeboot.reg"
        If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.Delete(FileToDelete)
        End If
        Shell("REGEDIT /e safeboot.reg HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\SafeBoot", vbHide) 'backup safemode
        If CheckBox1.Checked = True Then
            key.SetValue("disablesafe", 1)
        Else
            key.SetValue("disablesafe", 0)
        End If
        Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\", True)
        Dim quest As Object = key1.GetValue("question")
        Dim ans As Object = key1.GetValue("answer")
        If quest Is Nothing Then
            MsgBox("Set A Secret Question")
            Exit Sub
        End If
        If quest Is Nothing Then
            MsgBox("Set A Secret Question")
            Exit Sub
        End If

        If RadioButton1.Checked = True Then
            key.SetValue("unlock", 1)
        ElseIf RadioButton2.Checked = True Then
            If TextBox1.Text = Nothing Then
                MsgBox("Set A Password")
                Exit Sub
            Else
                key.SetValue("unlock", 2)
                key.SetValue("password", Form1.encrypt_pass(TextBox1.Text.ToLower))
            End If
        ElseIf RadioButton3.Checked = True Then
            If TextBox1.Text = Nothing Then
                MsgBox("Set A Password")
                Exit Sub
            Else
                If ComboBox1.Text = Nothing Then
                    MsgBox("Select A Removable Drive")
                    Exit Sub
                Else
                    key.SetValue("unlock", 3)
                    unlockop3()
                End If
            End If
        End If

        Form1.WindowState = FormWindowState.Minimized
        keybd_event(VK_LWIN, 0, 0, 0)                ' set left win key down
        keybd_event(&H4D, 0, 0, 0)
        keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, 0)  ' set left win key up

        Dim f14 As New Form14
        f14.Show()
        Me.Hide()
    End Sub
    Function unlockop3()
        Dim path = ComboBox1.Text
        Dim pass = Form1.encrypt_pass(TextBox1.Text.ToLower)
        Dim k
        k = GetCpuID()
        Dim FILE_NAME As String = path & "Aplock.lock"
        If System.IO.File.Exists(FILE_NAME) = True Then
            System.IO.File.Delete(FILE_NAME)
        End If
        Dim i As Integer
        Dim aryText(4) As String
        aryText(0) = k
        aryText(1) = pass
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
        For i = 0 To 1
            objWriter.WriteLine(aryText(i))
        Next
        objWriter.Close()
        Dim attribute As System.IO.FileAttributes = FileAttributes.Hidden + FileAttributes.System
        File.SetAttributes(FILE_NAME, attribute)
    End Function
    Function GetCpuID()
        'create our variables
        Dim wmi, cpu
        Dim x = Nothing
        'use GetObject to get the WMI instance
        wmi = GetObject("winmgmts:")
        'loop through all CPU's found
        For Each cpu In wmi.InstancesOf("Win32_Processor")
            'write out the processor ID
            x = cpu.ProcessorID
        Next
        GetCpuID = x
    End Function
End Class