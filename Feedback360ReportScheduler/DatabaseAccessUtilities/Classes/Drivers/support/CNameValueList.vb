<Serializable(), CLSCompliant(True)> _
Public Class CNameValueList : Inherits List(Of CNameValue)

#Region "Constructors"
    'Standard
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of CNameValue))
        MyBase.New(collection)
    End Sub

    'Alternative
    Public Sub New(ByVal dictionary As Dictionary(Of String, Object))
        MyBase.New()
        If IsNothing(dictionary) Then Exit Sub
        For Each i As String In dictionary.Keys
            Me.Add(i, dictionary(i))
        Next
    End Sub

    Public Sub New(ByVal pName As String, ByVal pValue As Object)
        MyBase.New(1)
        Add(New CNameValue(pName, pValue))
    End Sub
    Public Sub New(ByVal pName1 As String, ByVal pValue1 As Object, ByVal pName2 As String, ByVal pValue2 As Object)
        MyBase.New(2)
        Add(New CNameValue(pName1, pValue1))
        Add(New CNameValue(pName2, pValue2))
    End Sub
    Public Sub New(ByVal pName1 As String, ByVal pValue1 As Object, ByVal pName2 As String, ByVal pValue2 As Object, ByVal pName3 As String, ByVal pValue3 As Object)
        MyBase.New(3)
        Add(New CNameValue(pName1, pValue1))
        Add(New CNameValue(pName2, pValue2))
        Add(New CNameValue(pName3, pValue3))
    End Sub
#End Region

#Region "Overloads"
    Public Overloads Sub Add(ByVal name As String, ByVal value As Object)
        Me.Add(New CNameValue(name, value))
    End Sub
    Public Overloads Sub Remove(ByVal name As String)
        Dim match As CNameValue = Nothing
        For Each i As CNameValue In Me
            If String.Compare(i.Name, name, True) = 0 Then
                match = i
                Exit For
            End If
        Next
        If Not IsNothing(match) Then Me.Remove(match)
    End Sub
#End Region

End Class
