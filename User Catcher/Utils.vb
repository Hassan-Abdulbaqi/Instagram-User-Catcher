Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO

Namespace Core
    Public Class Utils

#Region " Center Form To Desktop "

        ' [ Center Form To Desktop ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' Center_Form_To_Desktop(Me)

        Public Shared Sub Center_Form_To_Desktop(ByVal Form As Form)
            Dim Desktop_RES As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
            Form.Location = New Point((Desktop_RES.Bounds.Width - Form.Width) / 2, (Desktop_RES.Bounds.Height - Form.Height) / 2)
        End Sub

#End Region

        Public Shared Function OpenFile(Optional ByVal Filter As String = "All files|*.*") As String
            Dim OpenFileDialog1 As New OpenFileDialog
            OpenFileDialog1.Multiselect = False
            ' OpenFileDialog1.DefaultExt = "txt"
            OpenFileDialog1.FileName = ""
            '  OpenFileDialog1.InitialDirectory = "c:\"
            OpenFileDialog1.Title = "Select file"
            OpenFileDialog1.Filter = Filter

            If Not OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then
                Return OpenFileDialog1.FileName
            End If

            Return Nothing

        End Function


        Public Shared Async Function DownloadImageAsync(ByVal UrlImg As String) As Task(Of Image)
            Try
                Dim WebpImageData() As Byte = New System.Net.WebClient().DownloadData(UrlImg)
                Dim Webpstream As IO.MemoryStream = New IO.MemoryStream(WebpImageData)
                Dim ToBitmap As Bitmap = New Bitmap(Webpstream)
                Return ToBitmap
            Catch ex As Exception
                Exeptions = ex.Message
                Return Nothing
            End Try
        End Function

#Region " My Application Is Already Running "

        ' [ My Application Is Already Running Function ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' MsgBox(My_Application_Is_Already_Running)
        ' If My_Application_Is_Already_Running() Then Application.Exit()

        Public Declare Function CreateMutexA Lib "Kernel32.dll" (ByVal lpSecurityAttributes As Integer, ByVal bInitialOwner As Boolean, ByVal lpName As String) As Integer
        Public Declare Function GetLastError Lib "Kernel32.dll" () As Integer

        Public Shared Function My_Application_Is_Already_Running() As Boolean
            'Attempt to create defualt mutex owned by process
            CreateMutexA(0, True, Process.GetCurrentProcess().MainModule.ModuleName.ToString)
            Return (GetLastError() = 183) ' 183 = ERROR_ALREADY_EXISTS
        End Function

#End Region


#Region " File Funcs "

    Public Shared FileManagerEx As String = String.Empty

    Public Shared Function FileWriteText(ByVal FileDir As String, Optional ByVal ContentText As String = "") As Boolean
      Try
        Dim swEx As New IO.StreamWriter(FileDir, False)
        swEx.Write(ContentText)
        swEx.Close()
        Return True
      Catch ex As Exception
        FileManagerEx = ex.Message
        Return False
      End Try
    End Function

    Public Shared Function FileReadText(ByVal FileDir As String) As String
      Try
        Dim swEx As New IO.StreamReader(FileDir, False)
        Dim ReadAllText As String = swEx.ReadToEnd
        swEx.Close()
        Return ReadAllText
      Catch ex As Exception
        FileManagerEx = ex.Message
        Return String.Empty
      End Try
    End Function

    Public Shared Async Function FileReadTextAsync(ByVal FileDir As String) As Task(Of String)
      Try
        Dim swEx As New IO.StreamReader(FileDir, False)
        Dim ReadAllText As String = Await swEx.ReadToEndAsync()
        swEx.Close()
        Return ReadAllText
      Catch ex As Exception
        FileManagerEx = ex.Message
        Return String.Empty
      End Try
    End Function

#End Region


        Public Shared Exeptions As String = String.Empty


        Public Shared Function GetNum(value As String) As Integer
            Dim text As String = String.Empty
            Dim matchCollection As MatchCollection = Regex.Matches(value, "\d+")
            Dim enumerator As IEnumerator = matchCollection.GetEnumerator()
            While enumerator.MoveNext()
                Dim match As Match = CType(enumerator.Current, Match)
                text += match.ToString()
            End While
            Return Convert.ToInt32(text)
        End Function

        Public Shared Function getID(ByVal url As String) As String 'Function by Stack Overflow Forum. 
            Try
                Dim myMatches As System.Text.RegularExpressions.Match
                Dim MyRegEx As New System.Text.RegularExpressions.Regex("youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase) 'This is where the magic happens/SHOULD work on all normal youtube links including youtu.be
                myMatches = MyRegEx.Match(url)
                If myMatches.Success = True Then
                    Return myMatches.Groups(1).Value
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                Return String.Empty ' Return ex.ToString
            End Try
        End Function

        Public Declare Auto Function GetAsyncKeyState Lib "user32.dll" (vKey As IntPtr) As IntPtr

        Public Shared Function CheckKeyDown(vKey As Keys) As Boolean
            Return 0L <> (CLng(GetAsyncKeyState(CType(CLng(vKey), IntPtr))) And 32768L)
        End Function

#Region "Setttins"

        Declare Function GetPrivateProfileStringA Lib "kernel32" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        Declare Function WritePrivateProfileStringA Lib "kernel32" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

        Private Shared IniFile As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\Settings.ini"


        Public Shared Function ReadIni(ByVal Section As String, ByVal Key As String, Optional ByVal DefaultValue As String = Nothing) As String
            Dim buffer As New System.Text.StringBuilder(260)
            GetPrivateProfileStringA(Section, Key, DefaultValue, buffer, buffer.Capacity, IniFile)
            Return buffer.ToString
        End Function

        Public Shared Function WriteIni(ByVal Section As String, ByVal Key As String, ByVal Value As String) As Boolean
            Return (WritePrivateProfileStringA(Section, Key, Value, IniFile) <> 0)
        End Function

#End Region

#Region " Is Connectivity Avaliable? function "

        ' [ Is Connectivity Avaliable? Function ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' MsgBox(Is_Connectivity_Avaliable())
        ' While Not Is_Connectivity_Avaliable() : Application.DoEvents() : End While

        Private Function Is_Connectivity_Avaliable()

            Dim WebSites() As String = {"Google.com", "Facebook.com", "Microsoft.com"}

            If My.Computer.Network.IsAvailable Then
                For Each WebSite In WebSites
                    Try
                        My.Computer.Network.Ping(WebSite)
                        Return True ' Network connectivity is OK.
                    Catch : End Try
                Next
                Return False ' Network connectivity is down.
            Else
                Return False ' No network adapter is connected.
            End If

        End Function

#End Region

#Region " Sleep "

        ' [ Sleep ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' Sleep(5) : MsgBox("Test")
        ' Sleep(5, Measure.Seconds) : MsgBox("Test")

        Public Enum Measure
            Milliseconds = 1
            Seconds = 2
            Minutes = 3
            Hours = 4
        End Enum

        Public Shared Sub Sleep(ByVal Duration As Int64, Optional ByVal Measure As Measure = Measure.Seconds)

            Dim Starttime = DateTime.Now

            Select Case Measure
                Case Measure.Milliseconds : Do While (DateTime.Now - Starttime).TotalMilliseconds < Duration : Application.DoEvents() : Loop
                Case Measure.Seconds : Do While (DateTime.Now - Starttime).TotalSeconds < Duration : Application.DoEvents() : Loop
                Case Measure.Minutes : Do While (DateTime.Now - Starttime).TotalMinutes < Duration : Application.DoEvents() : Loop
                Case Measure.Hours : Do While (DateTime.Now - Starttime).TotalHours < Duration : Application.DoEvents() : Loop
                Case Else
            End Select

        End Sub

#End Region

#Region " CenterForm function "

        Public Shared Function CenterForm(ByVal ParentForm As Form, ByVal Form_to_Center As Form, ByVal Form_Location As Point) As Point
            Dim FormLocation As New Point
            FormLocation.X = (ParentForm.Left + (ParentForm.Width - Form_to_Center.Width) / 2) ' set the X coordinates.
            FormLocation.Y = (ParentForm.Top + (ParentForm.Height - Form_to_Center.Height) / 2) ' set the Y coordinates.
            Return FormLocation ' return the Location to the Form it was called from.
        End Function

        Public Shared Function CenterControl(ByVal ParentForm As Control, ByVal Form_to_Center As Control, ByVal Form_Location As Point) As Point
            Dim FormLocation As New Point
            FormLocation.X = (ParentForm.Left + (ParentForm.Width - Form_to_Center.Width) / 2) ' set the X coordinates.
            FormLocation.Y = (ParentForm.Top + (ParentForm.Height - Form_to_Center.Height) / 2) ' set the Y coordinates.
            Return FormLocation ' return the Location to the Form it was called from.
        End Function

#End Region

        Public Shared Function GetDataPage(ByVal Url As String) As String
            Try
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
                Dim cookieJar As CookieContainer = New CookieContainer()
                Dim request As HttpWebRequest = CType(WebRequest.Create(Url), HttpWebRequest)
                request.UseDefaultCredentials = True
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.CookieContainer = cookieJar
                request.Accept = "text/html, application/xhtml+xml, */*"
                request.Referer = "https://softwarefuturenet.000webhostapp.com/"
                request.Headers.Add("Accept-Language", "en-GB")
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)"
                request.Host = "softwarefuturenet.000webhostapp.com"
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Dim htmlString As String = String.Empty

                Using reader = New StreamReader(response.GetResponseStream())
                    htmlString = reader.ReadToEnd()
                End Using

                Return htmlString
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

    End Class
End Namespace

