Imports System.Collections.Specialized

Namespace Core

  Public Class SettingsManager

    Public Shared Function GetAll() As List(Of String)
      Try
        Dim SavedTask As StringCollection = My.Settings.UserNames
        Dim TaskList As List(Of String)
        If SavedTask Is Nothing Then
          TaskList = New List(Of String)
        Else
          TaskList = SavedTask.Cast(Of String)().ToList()
        End If
        Return TaskList
      Catch ex As Exception
        Return Nothing
      End Try
    End Function

    Public Shared Function AddSetting(ByVal Uname As String)
      Dim SavedTask As StringCollection = My.Settings.UserNames
      Dim TaskList As List(Of String)
      If SavedTask Is Nothing Then
        TaskList = New List(Of String)
      Else
        TaskList = SavedTask.Cast(Of String)().ToList()
      End If

      TaskList.Add(Uname)
      Dim collection As StringCollection = New StringCollection()
      collection.AddRange(TaskList.ToArray())
      My.Settings.UserNames = collection
      My.Settings.Save()
      Return True
    End Function

    Public Shared Function RemoveByText(ByVal Uname As String)
      Dim SavedTask As StringCollection = My.Settings.UserNames
      Dim TaskList As List(Of String)
      If SavedTask Is Nothing Then
        TaskList = New List(Of String)
      Else
        TaskList = SavedTask.Cast(Of String)().ToList()
      End If

      Try
        For Each Tasks As String In TaskList
          Dim TaskInfo As String() = Tasks.Split("|")
          If Not TaskInfo.Count = 0 Then
            If TaskInfo(0) = Uname Then
              TaskList.Remove(Tasks)
            End If
          End If
        Next
      Catch ex As Exception

      End Try

      Dim collection As StringCollection = New StringCollection()
      collection.AddRange(TaskList.ToArray())
      My.Settings.UserNames = collection
      My.Settings.Save()
      Return True
    End Function

  End Class

End Namespace