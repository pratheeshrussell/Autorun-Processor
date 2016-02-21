Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Public Class Form14
    Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Select Case (keyData)
            Case Keys.Alt Or Keys.F4
                Return True
        End Select
        Return MyBase.ProcessDialogKey(keyData)
    End Function
#Region "keyhook"
    Public Declare Function UnhookWindowsHookEx Lib "user32" _
     (ByVal hHook As Integer) As Integer

    Public Declare Function SetWindowsHookEx Lib "user32" _
      Alias "SetWindowsHookExA" (ByVal idHook As Integer, _
      ByVal lpfn As KeyboardHookDelegate, ByVal hmod As Integer, _
      ByVal dwThreadId As Integer) As Integer

    Private Declare Function GetAsyncKeyState Lib "user32" _
      (ByVal vKey As Integer) As Integer

    Private Declare Function CallNextHookEx Lib "user32" _
      (ByVal hHook As Integer, _
      ByVal nCode As Integer, _
      ByVal wParam As Integer, _
      ByVal lParam As KBDLLHOOKSTRUCT) As Integer

    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure

    Private Declare Function GetModuleHandleA _
    Lib "kernel32.dll" (ByVal fakezero As IntPtr) _
    As IntPtr
    ' Low-Level Keyboard Constants
    Private Const HC_ACTION As Integer = 0
    Private Const LLKHF_EXTENDED As Integer = &H1
    Private Const LLKHF_INJECTED As Integer = &H10
    Private Const LLKHF_ALTDOWN As Integer = &H20
    Private Const LLKHF_UP As Integer = &H80

    ' Virtual Keys
    Public Const VK_TAB = &H9
    Public Const VK_CONTROL = &H11
    Public Const VK_ESCAPE = &H1B
    Public Const VK_DELETE = &H2E
    Public Const VK_LWIN = &H5B
    Public Const VK_RWIN = &H5C
    Public Const VK_MENU = &H12
    Private Const WH_KEYBOARD_LL As Integer = 13&
    Public KeyboardHandle As Integer

    ' Implement this function to block as many
    ' key combinations as you'd like
    Public Function IsHooked( _
      ByRef Hookstruct As KBDLLHOOKSTRUCT) As Boolean

        If (Hookstruct.vkCode = VK_ESCAPE) And _
          CBool(GetAsyncKeyState(VK_CONTROL) _
          And &H8000) Then

            Call HookedState("Ctrl + Esc blocked")
            Return True
        End If

        If (Hookstruct.vkCode = VK_TAB) And _
          CBool(Hookstruct.flags And _
          LLKHF_ALTDOWN) Then
            Call HookedState("Alt + Tab blockd")
            Return True
        End If
        If (Hookstruct.vkCode = VK_LWIN) Then
            Return True
        End If

        If (Hookstruct.vkCode = VK_RWIN) Then
            Return True
        End If



        If (Hookstruct.vkCode = VK_ESCAPE) And _
          CBool(Hookstruct.flags And _
            LLKHF_ALTDOWN) Then
            Call HookedState("Alt + Escape blocked")
            Return True
        End If
        ' MsgBox("Hookstruct.vkCode: " & Hookstruct.vkCode)
    End Function
    Private Sub HookedState(ByVal Text As String)
        ' MsgBox(Text)
    End Sub
    Public Function KeyboardCallback(ByVal Code As Integer, _
      ByVal wParam As Integer, _
      ByRef lParam As KBDLLHOOKSTRUCT) As Integer

        If (Code = HC_ACTION) Then
            If (IsHooked(lParam)) Then
                Return 1
            End If
        End If
        Return 0

    End Function


    Public Delegate Function KeyboardHookDelegate( _
      ByVal Code As Integer, _
      ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) _
                   As Integer

    <MarshalAs(UnmanagedType.FunctionPtr)> _
    Private callback As KeyboardHookDelegate

    Public Sub HookKeyboard()
        callback = New KeyboardHookDelegate(AddressOf KeyboardCallback)

        KeyboardHandle = SetWindowsHookEx( _
WH_KEYBOARD_LL, _
callback, _
GetModuleHandleA(IntPtr.Zero), _
0)
        Call CheckHooked()
    End Sub

    Public Sub CheckHooked()
        If (Hooked()) Then
            ' "Keyboard hooked"
        Else
            'MsgBox("Keyboard hook failed: " & Err.LastDllError)
        End If
    End Sub

    Private Function Hooked()
        Hooked = KeyboardHandle <> 0
    End Function

    Public Sub UnhookKeyboard()
        If (Hooked()) Then
            Call UnhookWindowsHookEx(KeyboardHandle)
        End If
    End Sub
#End Region

    Private Sub PictureBox1_MouseHover(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseHover
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("shutdownglow")
    End Sub
    Private Sub PictureBox1_leave(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("shutdown")
    End Sub
    Private Sub PictureBox1_down(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseDown
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("shutdownpress")
    End Sub
    Private Sub PictureBox1_up(ByVal sender As System.Object, _
ByVal e As System.EventArgs) Handles PictureBox1.MouseUp
        PictureBox1.Image = My.Resources.ResourceManager.GetObject("shutdown")
    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0")
    End Sub


    Private Sub Form14_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Shutdown Computer")
        Me.ShowInTaskbar = False
        With Me
            .MaximizeBox = False
            .MinimizeBox = False
            .TopMost = True
            .Opacity = 0.7
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .WindowState = System.Windows.Forms.FormWindowState.Maximized
        End With
        Label4.Hide()
        Label5.Hide()
        TextBox4.Hide()
        TextBox5.Hide()
        Button4.Hide()
        HookKeyboard() 'hook keyboard
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\Screenlock", True)
        If key Is Nothing Then
            lclose()
        End If
        TextBox1.Text = key.GetValue("unlock")
        If TextBox1.Text Is Nothing Then
            TextBox1.Text = 1
        End If



        If TextBox1.Text = 1 Then
            Label3.Hide()
            TextBox3.Hide()
            Label2.Hide()
            ComboBox1.Hide()
            Label2.Hide()
            Button2.Hide()
            LinkLabel1.Hide()
        ElseIf TextBox1.Text = 2 Then
            Label3.Show()
            TextBox3.Show()
            Label2.Hide()
            ComboBox1.Hide()
            Label2.Hide()
            Button2.Hide()
            LinkLabel1.Show()
            isdisablesafeboot()
        ElseIf TextBox1.Text = 3 Then
            TextBox2.Text = GetCpuID()
            Label2.Show()
            Label3.Show()
            ComboBox1.Show()
            TextBox3.Show()
            LinkLabel1.Show()
            Button2.Show()
            isdisablesafeboot()
            getremdrive()
        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        lclose()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\", True)
        Dim ans As Object = key1.GetValue("answer")
        If TextBox5.Text.ToLower Is Nothing Then
            Exit Sub
        End If
        If ans.tolower = TextBox5.Text.ToLower Then
            lclose()
        End If
    End Sub
    Function GetCpuID()
        If TextBox1.Text = 3 Then
            Dim wmi, cpu
            Dim x = Nothing
            wmi = GetObject("winmgmts:")
            For Each cpu In wmi.InstancesOf("Win32_Processor")
                x = cpu.ProcessorID
            Next
            GetCpuID = x
        Else
            GetCpuID = ""
        End If

       
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = 1 Then
            lclose()
        ElseIf TextBox1.Text = 2 Then
            Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\Screenlock", True)
            Dim pass As Object = key1.GetValue("password")
            If TextBox3.Text = Nothing Then
                Label1.Text = "Enter A Password"
                Exit Sub
            End If
            If pass = Form1.encrypt_pass(TextBox3.Text.ToLower) Then
                lclose()
            Else
                Label1.Text = "Password Wrong"
            End If
        ElseIf TextBox1.Text = 3 Then
            unlock3()
        End If
    End Sub
    Function isdisablesafeboot()
        Dim regKey1 As Microsoft.Win32.RegistryKey
        regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor\Screenlock", True)
        Dim x
        x = regKey1.GetValue("disablesafe")
        If x = 1 Then
            Dim regKey As RegistryKey
            regKey = Registry.LocalMachine.OpenSubKey("System\CurrentControlSet\Control\safeboot", True)
            If regKey IsNot Nothing Then
                regKey = Registry.LocalMachine.OpenSubKey("System\CurrentControlSet\Control", True)
                regKey.DeleteSubKeyTree("SafeBoot")
                regKey.Close()
            End If
            
        End If
    End Function
    Function unlock3()
        If ComboBox1.Text = Nothing Then
            Label1.Text = "Select Drive"
            Exit Function
        End If
        If TextBox3.Text = Nothing Then
            Label1.Text = "Enter Password"
            Exit Function
        End If
        Dim path = ComboBox1.Text & "Aplock.lock"
        If System.IO.File.Exists(path) = True Then
        Else
            Exit Function
        End If

        Dim id
        Dim pass
        Dim x = 0
        Dim readkey() As String = System.IO.File.ReadAllLines(path)
        For Each line As String In readkey
            x = x + 1
            If x = 1 Then
                id = line
            ElseIf x = 2 Then
                pass = line
            End If
        Next
        If TextBox2.Text = id Then
            If pass.tolower = Form1.encrypt_pass(TextBox3.Text.ToLower) Then
                System.IO.File.Delete(path)
                lclose()
            Else
                Label1.Text = "Password Wrong"
            End If
        Else
            Label1.Text = "No Key File Found"
        End If
    End Function

    Function getremdrive()
        ComboBox1.Items.Clear()
        For Each d As System.IO.DriveInfo In My.Computer.FileSystem.Drives
            If d.DriveType = IO.DriveType.Removable Then
                ComboBox1.Items.Add(d.Name)
                ComboBox1.Text = d.Name
            End If
        Next
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        getremdrive()
    End Sub
    Function lclose()
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor", True)
        UnhookKeyboard() 'unhook keyboard
        Shell("REGEDIT /s safeboot.reg", vbHide) 'restore safemode
        key.DeleteSubKey("Screenlock")
        Me.Close()
    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.TopMost = True
    End Sub
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Label4.Show()
        Label5.Show()
        TextBox4.Show()
        TextBox5.Show()
        Button4.Show()
        Dim key1 As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\autorun processor\", True)
        Dim quest As Object = key1.GetValue("question")
        TextBox4.Text = quest
    End Sub
End Class


