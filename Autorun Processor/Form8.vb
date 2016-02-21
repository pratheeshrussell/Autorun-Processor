Imports Microsoft.Win32
Imports System.IO

Public Class Form8
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

    Private Sub Form8_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
    End Sub
    Function startup_paths()
        On Error Resume Next
        For Each item In ListView1.Items
            item.remove()
        Next
        get_startupreg("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Run")
        get_startupreg("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce")
        get_startupreg("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunServices")
        get_startupreg("HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunServicesOnce")
        get_startupreg("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run")
        get_startupreg("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunOnce")
        get_startupreg("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunServices")
        get_startupreg("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\RunServicesOnce")

        'get from startup folder
        get_startupfile(Environment.GetFolderPath(Environment.SpecialFolder.Startup))
        get_startupfile(Environment.GetEnvironmentVariable("ALLUSERSPROFILE") & "\Start Menu\Programs\Startup")
    End Function
    Function get_startupreg(ByVal keyname As String)
        'get registry startup items
        Dim fullkey = keyname
        Dim rootKey As RegistryKey
        Dim newkey As RegistryKey
        Dim str(5) As String
        Dim itm As ListViewItem
        If keyname.ToUpper.Contains("HKEY_LOCAL_MACHINE\") Then
            keyname = keyname.ToUpper.Replace("HKEY_LOCAL_MACHINE\", "")
            rootKey = Registry.LocalMachine
            newkey = Registry.LocalMachine.OpenSubKey(keyname, True)
            If newkey Is Nothing Then
                Exit Function
            End If
        ElseIf keyname.ToUpper.Contains("HKEY_CURRENT_USER\") Then
            keyname = keyname.ToUpper.Replace("HKEY_CURRENT_USER\", "")
            rootKey = Registry.CurrentUser
            newkey = Registry.CurrentUser.OpenSubKey(keyname, True)
            If newkey Is Nothing Then
                Exit Function
            End If
        Else
            Exit Function
        End If
        Dim values As String
        rootKey = rootKey.OpenSubKey(keyname, True)
        For Each values In rootKey.GetValueNames()
            str(0) = values
            str(1) = newkey.GetValue(values)
            str(2) = fullkey
            str(3) = "registry"
            itm = New ListViewItem(str)
            ListView1.Items.Add(itm)
        Next
    End Function
    Function get_startupfile(ByVal filepath As String)
        Dim str(5) As String
        Dim itm As ListViewItem
        Dim filename
        Dim filenames() As String = Directory.GetFiles(filepath, "*.*")
        For Each item As String In filenames
            If path.GetExtension(item).ToLower.Contains("lnk") Then
                Dim Obj As Object
                Dim TargetPath
                Obj = CreateObject("WScript.Shell")
                Dim Shortcut As Object
                Shortcut = Obj.CreateShortcut(item)
                TargetPath = Shortcut.TargetPath
                Shortcut.Save()
                filename = path.GetFileName(item)
                str(0) = filename
                str(1) = TargetPath
                str(2) = item
                str(3) = "File"
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
            ElseIf Path.GetExtension(item).ToLower.Contains("exe") Or Path.GetExtension(item).ToLower.Contains("bat") _
            Or Path.GetExtension(item).ToLower.Contains("vbs") Or Path.GetExtension(item).ToLower.Contains("com") Then
                filename = Path.GetFileName(item)
                str(0) = filename
                str(1) = "Nothing"
                str(2) = item
                str(3) = "File"
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
            End If
        Next
    End Function
    Function delete()
        Dim n As Integer
        Dim objectToDelete As String
        Dim filename As String
        n = ListView1.CheckedItems.Count
        If n = 0 Then
            Exit Function
        End If
        For Each item In ListView1.CheckedItems
            objectToDelete = item.SubItems(2).Text()
            filename = item.SubItems(0).Text()
            If item.SubItems(3).Text() = "registry" Then
                deletereg(objectToDelete, filename)
                item.remove()
            Else
                deletefile(objectToDelete, filename)
                item.remove()
            End If
        Next
    End Function
    Function deletereg(ByVal path As String, ByVal name As String)
        Dim regKey1 As Microsoft.Win32.RegistryKey
        Dim hive As String
        Dim delkey
        hive = path.ToLower
        delkey = name.ToLower
        If hive.ToLower.Contains("local_machine") Then
            hive = hive.Replace("hkey_local_machine\", "")
            regKey1 = Registry.LocalMachine.OpenSubKey(hive, True)
            regKey1.DeleteValue(name)
        ElseIf hive.ToLower.Contains("current_user") Then
            hive = hive.Replace("hkey_current_user\", "")
            regKey1 = Registry.CurrentUser.OpenSubKey(hive, True)
            regKey1.DeleteValue(name)
        End If
    End Function
    Function deletefile(ByVal path As String, ByVal name As String)
        If System.IO.File.Exists(path) = True Then
            System.IO.File.SetAttributes(path, FileAttributes.Normal)
            System.IO.File.Delete(path)
        End If
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x
        x = MsgBox("This will permanently remove selected startup entries." & vbCrLf & vbCrLf & "Are you sure you want to do this?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Warning")
        If x = vbYes Then
            delete()
        End If
    End Sub
End Class