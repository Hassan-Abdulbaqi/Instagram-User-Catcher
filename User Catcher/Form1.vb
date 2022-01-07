Imports System.Text
Imports EO.WebBrowser.DOM


Public Class Form1
  Dim aList As New List(Of String)
  Dim count As Integer
  Dim prob As Integer
  Dim index As Integer = 1
  Dim limit As Integer = 2
  Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    Timer1.Enabled = True
    TextBox2.Enabled = False
    Label8.Text = "Running Please Dont Exit or Mini the app"
    'gxNyb
  End Sub

  Private Sub loadaccounts()

  End Sub

  Private Sub LoadSaved()
    ListBox1.Items.Clear()
    ListBox1.Items.AddRange(Core.SettingsManager.GetAll.ToArray)
  End Sub

  Public Sub ClearUsers()
    My.Settings.UserNames.Clear()
    My.Settings.Save()
  End Sub

  Public Sub SaveUser(ByVal Item As String)
    Core.SettingsManager.AddSetting(Item)
  End Sub

  Public Function WriteInput(ByVal Element As HtmlElement, ByVal Text As String, ByVal DeleteCurrentText As Boolean, Optional ByVal ShowPasswordtype As Boolean = False) As Boolean
    Try
      If Control.IsKeyLocked(Keys.CapsLock) Then

      End If

      If ShowPasswordtype = True Then
        Element.SetAttribute("type", "")
      End If

      Element.Focus()     ' WebView1.EvalScript(String.Format("document.getElementById('{0}').focus();", ElementID))

      If DeleteCurrentText = True Then
        Dim GetCurrentText As String = Element.GetAttribute("value") 'WebView1.EvalScript(String.Format("document.getElementById('{0}').value", ElementID)).ToCharArray

        For Each elementEx As Char In GetCurrentText
          '   System.Windows.Forms.SendKeys.SendWait("{Right}")
          elemfocus()
          System.Windows.Forms.SendKeys.SendWait("{RIGHT}") ' so i added this
          System.Windows.Forms.SendKeys.SendWait("{BKSP}") ' sometimes this Doesnt Delete all the text
          'If GetCurrentText = "" Or GetCurrentText = String.Empty Then

          'End If
          '  WebView1.SendKeyEvent(True, KeyCode.Back, EventFlags.None)
        Next
      End If

      Dim TextArray() As Char = Text.ToCharArray

      For Each elementEx As Char In TextArray
        ' WebView1.SendChar(element)
        System.Windows.Forms.SendKeys.SendWait(elementEx)
      Next

      Dim GetFinalWrite As String = Element.GetAttribute("value") 'WebView1.EvalScript(String.Format("document.getElementById('{0}').value", ElementID))

      Element.Focus()

      Return (Text = GetFinalWrite)

    Catch ex As Exception
      Return False
    End Try
  End Function
  ' its perfect cool. XD
  'There was a problem saving your profile.
  Public Sub look()
    Try
      Dim ps = WebBrowser1.Document.Body.GetElementsByTagName("p")
      Dim btn = WebBrowser1.Document.Body.GetElementsByTagName("Button")

      For Each d As HtmlElement In ps
        If d.InnerText = "Profile saved." Then
          ListBox2.Items.Add(TextBox1.Text & "  " & ListBox3.Items(index))
          next_account()

          Exit Sub
        ElseIf d.InnerText = "There was a problem saving your profile." Then
          prob += 1
          If prob = 2 Then
            next_account()

            prob = 0
            Exit Sub
          End If
          Timer1.Enabled = False
          wait(60)
          Timer1.Enabled = True

          Exit Sub
        ElseIf d.InnerText = "Please wait a few minutes before you try again." Then
          WebBrowser1.Navigate("https://www.instagram.com/")

          wait(10)
          next_account()
          Exit Sub
        End If
      Next
      For Each b As HtmlElement In btn
        If b.InnerText = "OK" Then
          Timer1.Enabled = False


          b.InvokeMember("click")
          next_account()
          Exit Sub

        End If
      Next

      'wait


      Dim validchars As String = "abcdefghijkl_mnopqrstu.vwxyzABCDEFGHIJK_LMNOPQRSTUVWXYZ1234567890._"
      Dim validchars2 As String = "xyi_.k"
      Dim Document As HtmlDocument = WebBrowser1.Document
      Dim sb As New StringBuilder()
      Dim rand As New Random()
      For i As Integer = 1 To limit
        Dim idx As Integer = rand.Next(0, validchars.Length)
        Dim randomChar As Char = validchars(idx)
        sb.Append(randomChar)
      Next i

      Dim randomString = sb.ToString()

      Dim InputElements As HtmlElementCollection = Document.GetElementsByTagName("input")
      Dim user As String = randomString
      Dim UserInput As HtmlElement = Nothing
      For Each elemInput As HtmlElement In InputElements
        If elemInput.Id = "pepUsername" Then
          UserInput = elemInput

          If UserInput IsNot Nothing Then
            If ListBox1.Items.Contains(randomString) Then
              look()
              Exit Sub
            ElseIf randomString.StartsWith(".") Then
              look()
              Exit Sub
            ElseIf randomString.EndsWith(".") Then
              look()
              Exit Sub
            End If
            elemfocus()
            WriteInput(UserInput, user, True)
            SaveUser(randomString)
            ListBox1.Items.Add(randomString)
            webclick("Submit")
            Label2.Text = ListBox1.Items.Count
            TextBox1.Text = randomString
          End If
        End If

      Next
    Catch ex As Exception

    End Try


  End Sub
  Public Sub webclick(ByVal btntxt As String)
    Dim Document As HtmlDocument = WebBrowser1.Document
    Dim ButtonsElements As HtmlElementCollection = Document.GetElementsByTagName("button")
    Dim LoginBUtton As HtmlElement = Nothing
    For Each elemButton As HtmlElement In ButtonsElements
      If elemButton.InnerText = btntxt Then
        LoginBUtton = elemButton
      End If
    Next
    If LoginBUtton IsNot Nothing Then
      LoginBUtton.InvokeMember("click")
    End If
  End Sub
  Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    look()
  End Sub

  Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    Timer1.Enabled = False
    TextBox2.Enabled = True
  End Sub

  Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    Timer2.Enabled = True
    LoadSaved()
    Label2.Text = ListBox1.Items.Count
    ' ClearUsers() ' clear saved data. work 100%
  End Sub
  Public Sub wait(ByVal seconds As Single)
    Static start As Single
    count = seconds
    start = Microsoft.VisualBasic.Timer()
    Do While Microsoft.VisualBasic.Timer() < start + seconds

      count = count - 1
      System.Windows.Forms.Application.DoEvents()

      Timer2.Start()
    Loop
  End Sub
  Public Sub next_account()
    If index = 12 Then
      index = 0
    End If
    'Switch Accounts
    Try
      Dim btn = WebBrowser1.Document.Body.GetElementsByTagName("span")
      For Each Btton As HtmlElement In btn
        If Btton.Style = "width: 24px; height: 24px;" Then
          Btton.InvokeMember("click")
        End If
      Next
      wait(1)
      Dim log = WebBrowser1.Document.Body.GetElementsByTagName("div")
      For Each logout As HtmlElement In log
        If logout.InnerText = "Log Out" Then
          logout.InvokeMember("click")
        End If
      Next
      wait(10)
      Dim Document As HtmlDocument = WebBrowser1.Document
      Dim userelement As HtmlElementCollection = Document.GetElementsByTagName("input")
      For Each put As HtmlElement In userelement
        If put.Name = "username" Then
          WriteInput(put, ListBox3.Items(index), True)
        ElseIf put.Name = "password" Then
          WriteInput(put, "Qazwsx132", True)

        End If
      Next
      webclick("Log In")
      index += 1
      wait(10)
      WebBrowser1.Navigate("https://www.instagram.com/accounts/edit/")
      wait(5)

      Timer1.Enabled = True
    Catch ex As Exception
      MsgBox(ex.Message)
    End Try

  End Sub
  Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
    Label7.Text = "The Account Index:" & index
    Label1.Text = "Wait " & count
    Label2.Text = ListBox1.Items.Count
    If Timer1.Enabled = True Then
      Label8.Text = "Running Please Dont Exit or Mini the app"
    Else
      Label8.Text = "stopped"
    End If
  End Sub


  Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
    next_account()
  End Sub
  Public Sub elemfocus()
    Dim inp = WebBrowser1.Document.Body.GetElementsByTagName("input")
    For Each pp As HtmlElement In inp
      If pp.Id = "pepUsername" Then
        pp.Focus()
      End If
    Next
  End Sub

  Private Sub Button4_Click(sender As Object, e As EventArgs)

  End Sub

  
  Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
    index = ListBox3.SelectedIndex
    Label7.Text = index
  End Sub

  Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
    Try
      limit = TextBox2.Text
    Catch ex As Exception
      MsgBox("Please Enter Integer")
    End Try


  End Sub
End Class
