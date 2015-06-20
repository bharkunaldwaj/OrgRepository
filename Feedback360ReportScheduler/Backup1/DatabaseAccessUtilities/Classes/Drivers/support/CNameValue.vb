<Serializable(), CLSCompliant(True)> _
Public Class CNameValue
    'Data
    Public Name As String
    Public Value As Object
    Public MarkerName As String

    'Constructors
    Public Sub New(ByVal name As String, ByVal value As Object)
        Me.Name = name
        Me.Value = value
        Me.MarkerName = name.ToLower
    End Sub
End Class
