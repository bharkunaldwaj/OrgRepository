<CLSCompliant(True)> _
Public Enum ESign
    EqualTo = 1
    NotEqualTo = 2
    GreaterThan = 3
    GreaterThanOrEq = 4
    LessThan = 5
    LessThanOrEq = 6
    [Like] = 7
    [IN] = 8
End Enum

<CLSCompliant(True)> <Serializable()> _
Public Class CCriteria
    'Constructors
    Public Sub New(ByVal c As CNameValue)
        Me.New(c.Name, c.Value)
    End Sub
    Public Sub New(ByVal colName As String, ByVal colValue As Object)
        Me.New(colName, ESign.EqualTo, colValue)
    End Sub
    Public Sub New(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object)
        Me.ColumnName = colName
        Me.Sign = sign
        Me.ColumnValue = CDataSrc.NullValue(colValue)

        If Not IsNothing(colName) Then
            Me.MarkerName = colName.ToLower 'Must be unique
        Else
            Me.MarkerName = String.Empty
        End If
    End Sub


    'Members
    Public ColumnName As String
    Public Sign As ESign
    Public ColumnValue As Object
    Friend MarkerName As String
End Class
