Imports Microsoft.Win32

Public Class Form11
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
    Private Sub Form11_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.ShowInTaskbar = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Enabled = False Then
            setpass2()
        Else
            setpass1()
        End If
        Me.Hide()
    End Sub
    Function preliminary()
        Dim regKey As Object
        regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("passwd")
        If regKey Is Nothing Then
            TextBox1.Enabled = False
        Else
            TextBox1.Enabled = True
        End If
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
        TextBox3.Text = Nothing
    End Function
    Function setpass1()
        'another pass exist
        Dim regpass As Object
        Dim pass
        Dim oldpass = Form1.encrypt_pass(TextBox1.Text)
        regpass = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("passwd")
        If regpass = oldpass Then
        Else
            MsgBox("Old Password Wrong!")
            Exit Function
        End If
        If TextBox2.Text = TextBox3.Text Then
            pass = Form1.encrypt_pass(TextBox2.Text)
            Dim regKey1 As Microsoft.Win32.RegistryKey
            regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True)
            regKey1.DeleteValue("passwd")
            regKey1.SetValue("passwd", pass)
        Else
            MsgBox("Passwords Doesnot Match")
        End If
    End Function
    Function setpass2()
        'no pass
        Dim pass
        If TextBox2.Text = TextBox3.Text Then
            pass = Form1.encrypt_pass(TextBox2.Text)
            Dim regKey1 As Microsoft.Win32.RegistryKey
            regKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True)
            regKey1.SetValue("passwd", pass)
            Form1.CheckBox9.Checked = True
        Else
            MsgBox("Passwords Doesnot Match")
            Form1.CheckBox9.Checked = False
        End If
    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Form1.CheckBox9.Checked = False
        Me.Hide()
        Dim regKey As Object
        regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\autorun processor", True).GetValue("passwd")
        If regKey Is Nothing Then
            Form1.CheckBox9.Checked = False
            Me.Hide()
        End If
    End Sub
End Class