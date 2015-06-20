<CLSCompliant(True)> _
Public Enum ECmdType
    Sql
    StoredProcedure
    ParameterisedSql
    InsertThenSelectIdentity
End Enum


<Serializable(), CLSCompliant(True)> _
Public Class CCommand

#Region "Data"
    'Query
    Public CommandType As ECmdType
    Public Text As String

    'Either/or (if stored proc or param-query)
    Public ParametersNamed As CNameValueList
    Public ParametersUnnamed As Object()

    'Optional
    <NonSerialized()> _
    Public Transaction As IDbTransaction
    Public Timeout As Integer = Integer.MinValue
#End Region

#Region "Constructors"
    'Simple
    Public Sub New(ByVal sql As String)
        Me.CommandType = ECmdType.Sql
        Me.Text = sql
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList)
        Me.New(spName, parameters, True)
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean)
        Me.Text = spName
        Me.ParametersNamed = parameters

        If isStoredProcedure Then
            Me.CommandType = ECmdType.StoredProcedure
        Else
            Me.CommandType = ECmdType.ParameterisedSql
        End If
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As Object())
        Me.Text = spName
        Me.ParametersUnnamed = parameters
        Me.CommandType = ECmdType.StoredProcedure
    End Sub

    'Transaction Supplied
    Public Sub New(ByVal sql As String, ByVal tx As IDbTransaction)
        Me.New(sql)
        Me.Transaction = tx
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal tx As IDbTransaction)
        Me.New(spName, parameters, True)
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal tx As IDbTransaction)
        Me.New(spName, parameters, isStoredProcedure)
        Me.Transaction = tx
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As Object(), ByVal tx As IDbTransaction)
        Me.New(spName, parameters)
        Me.Transaction = tx
    End Sub

    'Timeout Supplied
    Public Sub New(ByVal sql As String, ByVal timeout As Integer)
        Me.New(sql)
        Me.Timeout = timeout
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal timeout As Integer)
        Me.New(spName, parameters)
        Me.Timeout = timeout
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal timeout As Integer)
        Me.New(spName, parameters, isStoredProcedure)
        Me.Timeout = timeout
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As Object(), ByVal timeout As Integer)
        Me.New(spName, parameters)
        Me.Timeout = timeout
    End Sub
#End Region

End Class
