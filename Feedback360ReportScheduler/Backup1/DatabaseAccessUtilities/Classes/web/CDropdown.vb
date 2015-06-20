Imports System.Web.UI.WebControls

Public Class CDropdown

    Public Shared Sub SetText(ByVal dd As ListControl, ByVal text As String)

        text = Trim(LCase(text))
        Dim i As ListItem
        For Each i In dd.Items
            If Trim(LCase(i.Text)) = text Then
                dd.SelectedIndex = -1
                i.Selected = True
                Exit Sub
            End If
        Next
    End Sub
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As Integer) As ListItem
        Return Add(dd, text, val.ToString)
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String, ByVal val As String) As ListItem
        Return Add(dd, New ListItem(text, val))
    End Function
    Public Shared Function Add(ByVal dd As ListControl, ByVal text As String) As ListItem
        Return Add(dd, New ListItem(text, text))
    End Function
    Public Shared Sub AddEnums(ByVal dd As ListControl, ByVal enumType As Type)
        For Each text As String In [Enum].GetNames(enumType)
            Dim value As Integer = CInt([Enum].Parse(enumType, text))
            Add(dd, text, value)
        Next
    End Sub

    Private Shared Function Add(ByVal dd As ListControl, ByVal li As ListItem) As ListItem
        dd.Items.Add(li)
        Return li
    End Function
    Public Shared Function GetInt(ByVal dd As ListControl) As Integer
        Try
            Return CTextbox.GetInteger(dd.SelectedValue)
        Catch ex As Exception
            Return Integer.MinValue
        End Try
    End Function
    Public Shared Function GetGuid(ByVal dd As ListControl) As Guid
        Try
            If String.IsNullOrEmpty(dd.SelectedValue) Then Return Guid.Empty
            Return New Guid(dd.SelectedValue)
        Catch ex As Exception
            Return Guid.Empty
        End Try
    End Function
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As String)
        If TypeOf dd Is DropDownList OrElse TypeOf dd Is RadioButtonList Then
            Try
                dd.SelectedValue = val
            Catch ex As Exception
            End Try
        Else
            For Each i As ListItem In dd.Items
                If i.Value = val Then i.Selected = True
            Next
        End If
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Guid)
        SetValue(dd, val.ToString)
    End Sub
    Public Shared Sub SetValue(ByVal dd As ListControl, ByVal val As Integer)
        SetValue(dd, val.ToString)
    End Sub
    Public Shared Sub SetValues(ByVal dd As ListControl, ByVal vals As ICollection)
        For Each i As Object In vals
            SetValue(dd, i.ToString)
        Next
    End Sub
    Public Shared Sub SelectAll(ByVal dd As ListControl)
        SelectAll(dd, True)
    End Sub
    Public Shared Sub SelectAll(ByVal dd As ListControl, ByVal value As Boolean)
        For Each i As ListItem In dd.Items
            i.Selected = value
        Next
    End Sub
    Public Shared Function SelectedValues(ByVal dd As ListControl) As List(Of String)
        Dim list As New List(Of String)()
        For Each i As ListItem In dd.Items
            If i.Selected Then list.Add(i.Value)
        Next
        Return list
    End Function
    Public Shared Function SelectedInts(ByVal dd As ListControl) As List(Of Integer)
        Dim list As New List(Of Integer)()
        For Each i As ListItem In dd.Items
            If i.Selected Then list.Add(Integer.Parse(i.Value))
        Next
        Return list
    End Function
    Public Shared Sub Remove(ByVal dd As ListControl, ByVal val As Integer)
        Remove(dd, val.ToString)
    End Sub
    Public Shared Sub Remove(ByVal dd As ListControl, ByVal val As String)
        Dim found As ListItem = dd.Items.FindByValue(val)
        If Not IsNothing(found) Then dd.Items.Remove(found)
    End Sub

    Public Shared Function BlankItem(ByVal dd As ListControl) As ListItem
        Return BlankItem(dd, String.Empty)
    End Function
    Public Shared Function BlankItem(ByVal dd As ListControl, ByVal text As String) As ListItem
        Return BlankItem(dd, text, String.Empty)
    End Function
    Public Shared Function BlankItem(ByVal dd As ListControl, ByVal text As String, ByVal val As String) As ListItem
        Return Insert(dd, New ListItem(text, val))
    End Function

    'Private
    Private Shared Function Insert(ByVal dd As ListControl, ByVal li As ListItem) As ListItem
        dd.Items.Insert(0, li)
        Return li
    End Function

End Class
