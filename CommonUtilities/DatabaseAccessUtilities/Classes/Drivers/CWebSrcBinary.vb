Imports System.Data.OleDb
Imports System.Net
Imports System.Text


<Serializable(), CLSCompliant(True)> _
Public Class CWebSrcBinary : Inherits CWebSrc

#Region "Constants"
    Public Const QUERYSTRING As String = "c"
#End Region

#Region "Enum - ECmd"
    Public Enum ECmd
        'Low-Level
        ExecuteDataSet
        ExecuteScalar
        ExecuteNonQuery

        'High-Level
        BulkSaveDelete
        Delete
        Insert
        Update
        [Select]
        SelectCount
        UpdateOrdinals
        Paging
        PagingWithFilters
        BulkSelect

        'Remote Driver Access
        AllTableNames
        SqlToListAllTables
    End Enum
#End Region

#Region "Transport Classes - (Method parameters if >1)"
    <Serializable()> Friend Class CBulkSaveDelete
        Public Saves As ICollection
        Public Deletes As ICollection
    End Class
    <Serializable()> Friend Class CInsert
        Public TableName As String
        Public PrimaryKeyName As String
        Public InsertPrimaryKey As Boolean
        Public Data As CNameValueList
        Public OracleSequenceName As String
    End Class
    <Serializable()> Friend Class CUpdate
        Public Data As CNameValueList
        Public Where As CWhere
    End Class
    <Serializable()> Friend Class CUpdateOrdinals
        Public TableName As String
        Public PrimaryKeyName As String
        Public OrdinalName As String
        Public Data As CNameValueList
    End Class
    <Serializable()> Friend Class CPagingRequest
        Public PageIndexZeroBased As Integer
        Public PageSize As Integer
        Public TableName As String
        Public SortByColumn As String
        Public Descending As Boolean
        Public SelectColumns As String
    End Class
    <Serializable()> Friend Class CPagingWithFiltersRequest : Inherits CPagingRequest
        Public Where As CCriteriaList
    End Class
    <Serializable()> Friend Class CPagingResponse
        Public DataSet As DataSet
        Public Count As Integer
    End Class
#End Region

#Region "Constructors"
    Public Sub New(ByVal url As String)
        Me.New(url, CConfigBase.WebServicePassword)
    End Sub
    Public Sub New(ByVal url As String, ByVal password As String)
        'Connection String, Encryption Password
        MyBase.New(url, password)

        'Web client
        m_webclient = New WebClient()
        If Not IsNothing(Me.Proxy) Then m_webclient.Proxy = Me.Proxy

        'Internally-async methods:
        AddHandler m_webclient.DownloadDataCompleted, AddressOf Completed
        AddHandler m_webclient.UploadDataCompleted, AddressOf Completed
    End Sub
    Protected Overrides Function DefaultPageName(ByVal url As String) As String
        If Not url.ToLower().Contains(".aspx") And Len(url) > 0 Then
            If "/" <> url.Substring(url.Length - 1, 1) Then url &= "/"
            url &= "webservices/binary/DataSrc.aspx"
        End If
        Return url
    End Function
#End Region

#Region "Public - Driver Methods"
    Public Overrides Function ExecuteDataSet(ByVal cmd As CCommand) As System.Data.DataSet
        Return CType(SyncRequest(ECmd.ExecuteDataSet, cmd), DataSet)
    End Function
    Public Overrides Function ExecuteScalar(ByVal cmd As CCommand) As Object
        Return SyncRequest(ECmd.ExecuteScalar, cmd)
    End Function
    Public Overrides Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer
        Return Convert.ToInt32(SyncRequest(ECmd.ExecuteNonQuery, cmd))
    End Function

    'Remote Driver methods
    Public Overrides Function AllTableNames() As List(Of String)
        Return CType(SyncRequest(ECmd.AllTableNames, Nothing), List(Of String))
    End Function
    Private m_sqlToListTables As String
    Public Overrides ReadOnly Property SqlToListAllTables() As String 'Todo: make protected, only publicly implement alltablenames
        Get
            If IsNothing(m_sqlToListTables) Then
                m_sqlToListTables = CType(SyncRequest(ECmd.SqlToListAllTables, Nothing), String)
            End If
            Return m_sqlToListTables
        End Get
    End Property
#End Region

#Region "Public - Sql Methods"
    Public Overrides Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal txIsolation As IsolationLevel)
        Dim bsd As New CBulkSaveDelete()
        bsd.Saves = saves
        bsd.Deletes = deletes

        AsyncRequest(ECmd.BulkSaveDelete, bsd)
    End Sub
    Public Overrides Function Delete(ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        AsyncRequest(ECmd.Delete, where)
        Return 1
    End Function
    Public Overrides Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
        CheckTxIsNull(txOrNull)

        Dim i As New CInsert
        i.TableName = tableName
        i.PrimaryKeyName = pKeyName
        i.InsertPrimaryKey = insertPk
        i.Data = data
        i.OracleSequenceName = oracleSequenceName

        Return SyncRequest(ECmd.Insert, i)
    End Function
    Public Overrides Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(where.TxOrNull)

        'Ignores type param, always go with dataset, never datareader
        Return SyncRequest(ECmd.Select, where)
    End Function
    Public Overrides Function SelectCount(ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        Return Convert.ToInt32(SyncRequest(ECmd.SelectCount, where))
    End Function
    Public Overrides Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
        CheckTxIsNull(where.TxOrNull)

        Dim u As New CUpdate
        u.Data = data
        u.Where = where

        Return Convert.ToInt32(SyncRequest(ECmd.Update, u))
    End Function
    Public Overrides Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
        Dim uo As New CUpdateOrdinals
        uo.TableName = tableName
        uo.PrimaryKeyName = pKeyName
        uo.OrdinalName = ordinalName
        uo.Data = data

        'Return SyncRequest(ECmd.UpdateOrdinals, uo)
        AsyncRequest(ECmd.UpdateOrdinals, uo)
        Return data.Count
    End Function
    Public Overrides Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As System.Data.IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        Dim request As New CPagingWithFiltersRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns

        Dim response As CPagingResponse = CType(SyncRequest(ECmd.Paging, request), CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Public Overrides Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        CheckTxIsNull(txOrNull)

        Dim request As New CPagingWithFiltersRequest
        request.PageIndexZeroBased = pageIndexZeroBased
        request.PageSize = pageSize
        request.Descending = descending
        request.SortByColumn = sortByColumn
        request.TableName = tableName
        request.SelectColumns = selectColumns
        request.Where = criteria

        Dim response As CPagingResponse = CType(SyncRequest(ECmd.PagingWithFilters, request), CPagingResponse)
        count = response.Count
        Return response.DataSet
    End Function
    Protected Overloads Overrides Function BulkSelect(ByVal tables As System.Collections.Generic.List(Of CCommand)) As System.Collections.Generic.List(Of System.Data.DataSet)
        Return CType(SyncRequest(ECmd.BulkSelect, tables), List(Of DataSet))
    End Function
#End Region

#Region "Private - Request Sync/Async"
    Private Sub AsyncRequest(ByVal cmd As ECmd, ByVal data As Object)
        Dim wc As New WebClient
        wc.UploadDataAsync(Uri(cmd), "post", Pack(data)) 'No userstate
    End Sub
    Private Function SyncRequest(ByVal cmd As ECmd, ByVal data As Object) As Object
        Dim response As Byte() = Web.UploadData(Uri(cmd), "post", Pack(data))
        Try
            Return Unpack(response)
        Catch ex As Exception
            'Assume it was an error page instead of an encrypted zip
            If Len(Password) > 0 Then CBinary.EncryptFast(response, Password)
            Throw New Exception(String.Concat("Failed to unpack data - check password", vbCrLf, vbCrLf, ASCIIEncoding.ASCII.GetString(response)))
        End Try
    End Function
    Private Function Uri(ByVal cmd As ECmd) As Uri
        Return New Uri(String.Concat(Url, "?", QUERYSTRING, "=", CInt(cmd)))
    End Function
#End Region

#Region "Private - WebClient"
    Private m_webclient As WebClient
    Private ReadOnly Property Web() As WebClient
        Get
            Return m_webclient
        End Get
    End Property
#End Region

End Class