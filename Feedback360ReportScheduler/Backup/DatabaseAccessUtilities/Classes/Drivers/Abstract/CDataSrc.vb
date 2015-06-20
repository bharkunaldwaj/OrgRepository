Imports System
Imports System.Web
Imports System.Web.HttpContext
Imports System.Text
Imports System.Configuration
Imports System.Configuration.ConfigurationManager

<CLSCompliant(True)> _
Public Enum EQueryReturnType
    Optimal = 1
    DataReader = 2
    DataSet = 3
End Enum


<CLSCompliant(True)> _
Public MustInherit Class CDataSrc

#Region "Constants"
    Public Shared LOGGING As Boolean = False 'CApplication.IsWebApplication
#End Region

#Region "Constructors"
    Public Sub New(ByVal connectionString As String)
        m_connectionString = connectionString
    End Sub
#End Region

#Region "Members"
    Protected m_connectionString As String
#End Region

#Region "MustOverride"
    'Low-Level: Core Methods
    Public MustOverride Function ExecuteDataSet(ByVal cmd As CCommand) As DataSet
    Public MustOverride Function ExecuteScalar(ByVal cmd As CCommand) As Object
    Public MustOverride Function ExecuteNonQuery(ByVal cmd As CCommand) As Integer

    'DataReader/DataSet Generalisation
    Public MustOverride Function ExecuteQuery(ByVal cmd As CCommand, ByVal type As EQueryReturnType) As Object

    'High-Level: Transactional
    Public MustOverride Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection, ByVal il As IsolationLevel)

    'High-Level: Parameterised Sql
    Public MustOverride Function Insert(ByVal tableName As String, ByVal pKeyName As String, ByVal insertPk As Boolean, ByVal data As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal oracleSequenceName As String) As Object
    Public MustOverride Function Update(ByVal data As CNameValueList, ByVal where As CWhere) As Integer
    Public MustOverride Function UpdateOrdinals(ByVal tableName As String, ByVal pKeyName As String, ByVal ordinalName As String, ByVal data As CNameValueList) As Integer
    Public MustOverride Function Delete(ByVal where As CWhere) As Integer
    Public MustOverride Function [Select](ByVal where As CSelectWhere, ByVal type As EQueryReturnType) As Object
    Public MustOverride Function SelectCount(ByVal where As CWhere) As Integer
    Public MustOverride Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
    Public MustOverride Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object

    'Non-transactional overloads
    Public Function Paging(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String) As Object
        Return Paging(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function PagingWithFilters(ByRef count As Integer, ByVal pageIndexZeroBased As Integer, ByVal pageSize As Integer, ByVal tableName As String, ByVal descending As Boolean, ByVal sortByColumn As String, ByVal selectColumns As String, ByVal criteria As CCriteriaList) As Object
        Return PagingWithFilters(count, pageIndexZeroBased, pageSize, tableName, descending, sortByColumn, selectColumns, criteria, Nothing, EQueryReturnType.Optimal)
    End Function
#End Region

#Region "Null-Equivalent Values"
    Friend Shared Function NullValue(ByVal nativeValue As Object) As Object
        If nativeValue Is Nothing Then Return System.DBNull.Value
        If TypeOf (nativeValue) Is Integer Then Return IIf(Integer.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Guid Then Return IIf(Guid.Empty.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is DateTime Then Return IIf(DateTime.MinValue.Equals(nativeValue), System.DBNull.Value, nativeValue)
        If TypeOf (nativeValue) Is Double Then Return IIf(Double.IsNaN(CDbl(nativeValue)), System.DBNull.Value, nativeValue)
        Return nativeValue
    End Function
#End Region

#Region "Public"
    'Connection Strong
    Public ReadOnly Property ConnectionString() As String
        Get
            Return m_connectionString
        End Get
    End Property

    'HashCode
    Public Overrides Function GetHashCode() As Integer
        Return m_connectionString.GetHashCode()
    End Function

    'Casting
    Public ReadOnly Property Local() As CDataSrcLocal
        Get
            If TypeOf Me Is CDataSrcRemote Then Throw New Exception("This operation requires a local datasrc")
            Return CType(Me, CDataSrcLocal)
        End Get
    End Property
    Public ReadOnly Property IsLocal() As Boolean
        Get
            Return TypeOf Me Is CDataSrcLocal
        End Get
    End Property
    Public ReadOnly Property IsRemote() As Boolean
        Get
            Return TypeOf Me Is CDataSrcRemote
        End Get
    End Property


    'ExecuteDataSet
    Public Function ExecuteDataSet(ByVal sql As String, ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteDataSet(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteDataSet(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As DataSet
        Return ExecuteDataSet(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function


    'ExecuteScalar
    Public Function ExecuteScalar(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteScalar(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteScalar(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As Object
        Return ExecuteScalar(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function


    'ExecuteNonQuery
    Public Function ExecuteNonQuery(ByVal sql As String, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(sql, txOrNull))
    End Function
    Public Function ExecuteNonQuery(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(spName, parameterValues, txOrNull))
    End Function
    Public Function ExecuteNonQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction) As Integer
        Return ExecuteNonQuery(New CCommand(spName, pNamesAndValues, txOrNull))
    End Function

    'Non-transactional overloads
    Public Function ExecuteScalar(ByVal sql As String) As Object
        Return ExecuteScalar(New CCommand(sql, CType(Nothing, IDbTransaction)))
    End Function
    Public Function ExecuteDataSet(ByVal sql As String) As DataSet
        Return ExecuteDataSet(sql, CType(Nothing, IDbTransaction))
    End Function
    Public Function ExecuteNonQuery(ByVal sql As String) As Integer
        Return ExecuteNonQuery(sql, CType(Nothing, IDbTransaction))
    End Function
#End Region

#Region "DataReader/DataSet Generalisation (overloads)"
    Public Function ExecuteQuery(ByVal sql As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(sql, txOrNull), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal parameterValues As Object(), ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, parameterValues), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, pNamesAndValues), type)
    End Function
    Public Function ExecuteQuery(ByVal spName As String, ByVal pNamesAndValues As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return ExecuteQuery(New CCommand(spName, pNamesAndValues, isStoredProcedure), type)
    End Function
#End Region

#Region "Default DataSrc (Config Settings)"
    Private Shared _defaultDataSource As CDataSrc = Nothing
    Public Shared Property [Default]() As CDataSrc
        Get
            If IsNothing(_defaultDataSource) Then
                _defaultDataSource = GetDefaultDataSrc()
            End If
            Return _defaultDataSource
        End Get
        Set(ByVal value As CDataSrc)
            _defaultDataSource = value
        End Set
    End Property
    Private Shared Function GetDefaultDataSrc() As CDataSrc
        'Named connection string (assume sqlserver or webservice)
        If Len(CConfigBase.ActiveConnectionString) > 0 Then
            Dim cs As ConnectionStringSettings = ConnectionStrings(CConfigBase.ActiveConnectionString)
            If Not IsNothing(cs) AndAlso Not String.IsNullOrEmpty(cs.ConnectionString) Then
                If cs.ConnectionString.ToLower.Contains(".asmx") Then Return New CWebSrcSoap(cs.ConnectionString)
                If cs.ConnectionString.ToLower.Contains(".aspx") Then Return New CWebSrcBinary(cs.ConnectionString)
                Return New CSqlClient(cs.ConnectionString)
            End If
        End If

        'Explicit case: Driver+ConnectionString
        Select Case LCase(CConfigBase.Driver)
            Case "sqlclient", "sqlserver" : Return New CSqlClient(CConfigBase.ConnectionString)
            Case "mysql" : Return New CMySqlClient(CConfigBase.ConnectionString)
            Case "oledb" : Return New COleDb(CConfigBase.ConnectionString)
            Case "oracle", "oracleodp" : Return New COracleClientOdp(CConfigBase.ConnectionString)
            Case "oraclems" : Return New COracleClientMs(CConfigBase.ConnectionString)
            Case "odbc" : Return New COdbc(CConfigBase.ConnectionString)
            Case "webservice" : Return New CWebSrcSoap(CConfigBase.ConnectionString, CConfigBase.WebServicePassword)
            Case "webpage" : Return New CWebSrcBinary(CConfigBase.ConnectionString, CConfigBase.WebServicePassword)
        End Select

        'Shorthand case #1: Webservices
        If Len(CConfigBase.WebSite) > 0 Then
            Return New CWebSrcSoap(CConfigBase.WebSite & "webservices/WSDataSrc.asmx", CConfigBase.WebServicePassword)
        End If

        'Shorthand case #2: Access Database
        If Len(CConfigBase.AccessDatabasePath) > 0 Then
            Return OleDbFromAccessPath(CConfigBase.AccessDatabasePath)
        End If

        'Shorthand case #3: mdf file in App_Data
        If Len(CConfigBase.SqlExpressPath) > 0 Then
            Return SqlExpressFromPath(CConfigBase.SqlExpressPath)
        End If

        'Shorthand case #4: Excel Spreadsheet
        If Len(CConfigBase.ExcelDatabasePath) > 0 Then
            Return OleDbFromExcelPath(CConfigBase.ExcelDatabasePath) 'Note: SELECT * FROM [SheetName$]
        End If

        'Driver misspelt
        Dim msg As String = "\nDriver Options are: [SqlClient,MySql,Odbc,OleDb,Oracle,OracleMs,OracleOdp,WebService,WebPage]"
        msg &= "\nAlternatively you can use a single config setting such as 'AccessDatabasePath', 'SqlExpressPath', or 'ExcelDatabasePath'"
        If Len(CConfigBase.Driver) > 0 Then Throw New Exception("Config File Error:\nUnrecognised driver: " & CConfigBase.Driver & msg)

        'ConnectionString (in appsettings) but no driver - Default to SqlServer
        If Len(CConfigBase.ConnectionString) > 0 Then
            Return New CSqlClient(CConfigBase.ConnectionString)
        ElseIf ConnectionStringCount() > 0 AndAlso GetConnectionString(0).Length > 0 Then
            'Generic connection string (take the first one if any) - Default to SqlServer
            Return New CSqlClient(GetConnectionString(0))
        Else
            'No settings found
            Throw New Exception("Expected a pair of config settings called 'Driver' and 'ConnectionString'" & msg)
        End If
    End Function

    Public Const EXCEL_DEFAULT_TABLE_NAME As String = "[Sheet1$]"
    Public Shared Function OleDbFromAccessPath(ByVal path As String) As COleDb
        If path.ToLower.EndsWith(".accdb") Then
            Return New COleDb(String.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", MapPath(path), ";Persist Security Info=False"))
        Else
            Return New COleDb(String.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", MapPath(path), ";Persist Security Info=False"))
        End If
    End Function
    Public Shared Function SqlExpressFromPath(ByVal path As String) As CSqlClient
        Return New CSqlClient(String.Concat("Data Source=.\SQLEXPRESS; AttachDbFilename=", MapPath(path), "; Integrated Security=True; User Instance=True"))
    End Function
    Public Shared Function OleDbFromExcelPath(ByVal path As String) As COleDb
        Return OleDbFromExcelPath(path, True)
    End Function
    Public Shared Function OleDbFromExcelPath(ByVal path As String, ByVal containsHeaderRow As Boolean) As COleDb
        Return OleDbFromExcelPath(path, containsHeaderRow, True)
    End Function
    Public Shared Function OleDbFromExcelPath(ByVal path As String, ByVal containsHeaderRow As Boolean, ByVal alwaysReadIntermixed As Boolean) As COleDb
        Dim is2007Format As Boolean = path.ToLower.EndsWith(".xlsx")

        'Different providers
        Dim sb As New StringBuilder("Provider=")
        If is2007Format Then
            sb.Append("Microsoft.ACE.OLEDB.12.0;")
        Else
            sb.Append("Microsoft.Jet.OLEDB.4.0;")
        End If

        'Same path syntax
        sb.Append("Data Source=")
        sb.Append(MapPath(path))

        'Different extended properties
        If is2007Format Then
            sb.Append(";Extended Properties=""Excel 12.0 Xml;")
        Else
            sb.Append(";Extended Properties=""Excel 8.0;")
        End If
        If containsHeaderRow Then sb.Append("HDR=YES;") Else sb.Append("HDR=NO;")
        If alwaysReadIntermixed Then sb.Append("IMEX=1") Else sb.Append("IMEX=0")
        sb.Append("""") 'Extended properties must be in quotes

        Return New COleDb(sb.ToString) '*Use "select * from [SheetName$]"
    End Function

    Public Shared Function OleDbFromCsvPath(ByVal folderPath As String) As COleDb
        Return OleDbFromCsvPath(folderPath, True)
    End Function
    Public Shared Function OleDbFromCsvPath(ByVal folderPath As String, ByVal containsHeaderRow As Boolean) As COleDb
        Dim sb As New StringBuilder("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=")
        sb.Append(folderPath)
        sb.Append(";Extended Properties=""text;")
        If containsHeaderRow Then sb.Append("HDR=YES;") Else sb.Append("HDR=NO;")
        sb.Append("FMT=Delimited"";")
        Return New COleDb(sb.ToString) '*Use "select * from filename"
    End Function

    Private Shared Function MapPath(ByRef path As String) As String
        If IO.File.Exists(path) Then Return path

        'Try to interpret non-absolute path
        If Not IsNothing(Current) Then
            'Interpret path relative to website root
            With Current.Server
                If IO.File.Exists(.MapPath(path)) Then
                    path = .MapPath(path)
                ElseIf IO.File.Exists(.MapPath("~/App_Data/" & path)) Then
                    path = .MapPath("~/App_Data/" & path)
                End If
            End With
        Else
            'Interpret path relative to application dir
            Try
                Dim temp As String = String.Concat(My.Application.Info.DirectoryPath, "/", path)
                If IO.File.Exists(temp) Then path = temp
            Catch
            End Try
        End If
        If IO.File.Exists(path) Then Return path

        Throw New Exception("File does not exist: " & path)
    End Function
    'Generic connection strings
    Private Shared Function ConnectionStringCount() As Integer
        Return ConnectionStrings.Count
    End Function
    Private Shared Function GetConnectionString(ByVal key As String) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(key)
        If Not IsNothing(cs) Then Return cs.ConnectionString
        Return String.Empty
    End Function
    Private Shared Function GetConnectionString(ByVal index As Integer) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(index)
        If Not IsNothing(cs) Then Return cs.ConnectionString
        Return String.Empty
    End Function
#End Region

#Region "Logging"
    Protected Sub Log(ByVal cmd As IDbCommand)
        Dim sw As New IO.StreamWriter(LogPath, True)
        sw.Write(DateTime.Now.ToString)
        sw.Write(vbTab)
        sw.Write(cmd.CommandText.Replace(vbCrLf, "").Replace(vbLf, ""))
        sw.Write(vbTab)
        For Each i As IDbDataParameter In cmd.Parameters
            sw.Write("{")
            sw.Write(i.ParameterName)
            sw.Write("=")
            sw.Write(i.Value.ToString.Replace(vbCrLf, "").Replace(vbLf, ""))
            sw.Write("}")
            sw.Write(vbTab)
        Next
        sw.Write(vbCrLf)
        sw.Close()
    End Sub
    Private Shared _logPath As String
    Private ReadOnly Property LogPath() As String
        Get
            If IsNothing(_logPath) Then
                _logPath = HttpContext.Current.Server.MapPath("~/App_Data/dblog.txt")

                Dim folder As String = IO.Path.GetDirectoryName(_logPath)
                If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)
            End If
            Return _logPath
        End Get
    End Property
    Protected Shared Sub Rethrow(ByVal ex As Exception, ByVal sql As String)
        Throw New Exception(sql & vbCrLf & ex.Message)
    End Sub
#End Region

#Region "Select/Delete Overloads"
    Public Function SelectCount(ByVal tableName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal criteria As CCriteria, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function SelectCount(ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As Integer
        Return SelectCount(New CWhere(tableName, unsafeWhereClause, txOrNull))
    End Function

    'Non-Transactional, Optimal
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String) As Object
        Return Me.SelectAll(selectCols, tableName, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String) As Object
        Return Me.SelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, Nothing, EQueryReturnType.Optimal)
    End Function


    'Non-Transactional, Specific type
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectAll(selectCols, tableName, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, criteria, orderByCols, Nothing, type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal type As EQueryReturnType) As Object
        Return Me.SelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, Nothing, type)
    End Function

    Public Function DeleteWhere(ByVal tableName As String, ByVal c As CCriteria) As Integer
        Return DeleteWhere(tableName, c, Nothing)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteriaList) As Integer
        Return DeleteWhere(tableName, criteria, Nothing)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal keys As CNameValueList, ByVal txOrNull As IDbTransaction) As Integer
        Dim criteria As New CCriteriaList(keys)
        Return DeleteWhere(tableName, criteria, txOrNull)
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal unsafeWhereClause As String) As Integer
        Return DeleteWhere(tableName, unsafeWhereClause, Nothing)
    End Function

    'Transactional, Specific type
    Public Function SelectAll(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, criteria, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, criteria, orderByCols, txOrNull), type)
    End Function
    Public Function SelectWhere(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction, ByVal type As EQueryReturnType) As Object
        Return Me.Select(New CSelectWhere(selectCols, tableName, unsafeWhereClause, orderByCols, txOrNull), type)
    End Function

    Public Function DeleteAll(ByVal tableName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteria, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal criteria As CCriteriaList, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, criteria, txOrNull))
    End Function
    Public Function DeleteWhere(ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction) As Integer
        Return Me.Delete(New CWhere(tableName, unsafeWhereClause, txOrNull))
    End Function
#End Region

#Region "Select - Dataset"
    'SelectAll
    Public Function SelectAll_Dataset(ByVal tableName As String) As DataSet
        Return SelectAll_Dataset(tableName, Nothing)
    End Function
    Public Function SelectAll_Dataset(ByVal tableName As String, ByVal orderBy As String) As DataSet
        Return SelectAll_Dataset("*", tableName, orderBy)
    End Function
    Public Function SelectAll_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String) As DataSet
        Return CType(SelectAll(selectColumns, tableName, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function

    'SelectWhere
    Public Function SelectWhere_Dataset(ByVal tableName As String, ByVal criteria As CCriteriaList) As DataSet
        Return SelectWhere_Dataset(tableName, Nothing, criteria)
    End Function
    Public Function SelectWhere_Dataset(ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As DataSet
        Return SelectWhere_Dataset("*", tableName, orderBy, criteria)
    End Function
    Public Function SelectWhere_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal criteria As CCriteriaList) As DataSet
        Return CType(SelectWhere(selectColumns, tableName, criteria, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function
    Public Function SelectWhere_Dataset(ByVal selectColumns As String, ByVal tableName As String, ByVal orderBy As String, ByVal unsafeWhere As String) As DataSet
        Return CType(SelectWhere(selectColumns, tableName, unsafeWhere, orderBy, EQueryReturnType.DataSet), DataSet)
    End Function
#End Region

#Region "Bulk Operations"
    'Lists
    Public Sub BulkDelete(ByVal list As ICollection, ByVal txIsolation As IsolationLevel)
        Me.BulkSaveDelete(Nothing, list, txIsolation)
    End Sub
    Public Sub BulkSave(ByVal list As ICollection, ByVal txIsolation As IsolationLevel)
        Me.BulkSaveDelete(list, Nothing, txIsolation)
    End Sub

    'Non-list overloads
    Public Sub BulkDelete(ByVal cascading As CBase, ByVal txIsolation As IsolationLevel)
        Dim list As New ArrayList()
        list.Add(cascading)
        Me.BulkDelete(list, txIsolation)
    End Sub
    Public Sub BulkSave(ByVal cascading As CBase, ByVal txIsolation As IsolationLevel)
        Dim list As New ArrayList()
        list.Add(cascading)
        Me.BulkSave(list, txIsolation)
    End Sub

    'Isolation-level Defaults
    Private Const DEFAULT_TX_ISOLATION As IsolationLevel = IsolationLevel.Unspecified
    Public Sub BulkSave(ByVal saves As ICollection)
        BulkSave(saves, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkDelete(ByVal deletes As ICollection)
        BulkDelete(deletes, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkSave(ByVal cascading As CBase)
        BulkSave(cascading, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkDelete(ByVal cascading As CBase)
        BulkDelete(cascading, DEFAULT_TX_ISOLATION)
    End Sub
    Public Sub BulkSaveDelete(ByVal saves As ICollection, ByVal deletes As ICollection)
        BulkSaveDelete(saves, deletes, DEFAULT_TX_ISOLATION)
    End Sub
#End Region

#Region "Distinct"
    'Non-Transactional
    Public Function SelectDistinctAsInt(ByVal tableName As String, ByVal colName As String) As List(Of Integer)
        Return MakeListInteger(SelectDistinctSql(tableName, colName), Nothing)
    End Function
    Public Function SelectDistinctAsStr(ByVal tableName As String, ByVal colName As String) As List(Of String)
        Return MakeListString(SelectDistinctSql(tableName, colName), Nothing)
    End Function

    'Transactional
    Public Function SelectDistinctAsInt(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of Integer)
        Return MakeListInteger(SelectDistinctSql(tableName, colName), tx)
    End Function
    Public Function SelectDistinctAsStr(ByVal tableName As String, ByVal colName As String, ByVal tx As IDbTransaction) As List(Of String)
        Return MakeListString(SelectDistinctSql(tableName, colName), tx)
    End Function

    'Private
    Private Function SelectDistinctSql(ByVal tableName As String, ByVal colName As String) As String
        Return String.Concat("SELECT DISTINCT ", colName, " FROM ", tableName, " ORDER BY ", colName)
    End Function
#End Region

#Region "Single-Column Data"
    'Overloads: Sql-only
    Public Function MakeListString(ByVal sql As String) As List(Of String)
        Return MakeListString(sql, Nothing)
    End Function
    Public Function MakeListInteger(ByVal sql As String) As List(Of Integer)
        Return MakeListInteger(sql, Nothing)
    End Function
    Public Function MakeListGuid(ByVal sql As String) As List(Of Guid)
        Return MakeListGuid(sql, Nothing)
    End Function
    Public Function MakeListDate(ByVal sql As String) As List(Of DateTime)
        Return MakeListDate(sql, Nothing)
    End Function

    'Overloads: Sql+transaction
    Public Function MakeListString(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of String)
        If Me.IsLocal Then
            Return MakeListString(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListString(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListInteger(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Integer)
        If Me.IsLocal Then
            Return MakeListInteger(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListInteger(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListGuid(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of Guid)
        If Me.IsLocal Then
            Return MakeListGuid(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListGuid(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function
    Public Function MakeListDate(ByVal sql As String, ByVal txOrNull As IDbTransaction) As List(Of DateTime)
        If Me.IsLocal Then
            Return MakeListDate(Local.ExecuteReader(sql, txOrNull))
        Else
            Return MakeListDate(ExecuteDataSet(sql, txOrNull).Tables(0))
        End If
    End Function

    'Overloads: Command object
    Public Function MakeListString(ByVal cmd As CCommand) As List(Of String)
        If IsLocal Then Return MakeListString(Local.ExecuteReader(cmd))
        Return MakeListString(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListInteger(ByVal cmd As CCommand) As List(Of Integer)
        If IsLocal Then Return MakeListInteger(Local.ExecuteReader(cmd))
        Return MakeListInteger(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListGuid(ByVal cmd As CCommand) As List(Of Guid)
        If IsLocal Then Return MakeListGuid(Local.ExecuteReader(cmd))
        Return MakeListGuid(ExecuteDataSet(cmd).Tables(0))
    End Function
    Public Function MakeListDate(ByVal cmd As CCommand) As List(Of DateTime)
        If IsLocal Then Return MakeListDate(Local.ExecuteReader(cmd))
        Return MakeListDate(ExecuteDataSet(cmd).Tables(0))
    End Function

    'Overloads: No column specified (assume first column
    Public Function MakeListString(ByVal dr As IDataReader) As List(Of String)
        Return MakeListString(dr, Nothing)
    End Function
    Public Function MakeListInteger(ByVal dr As IDataReader) As List(Of Integer)
        Return MakeListInteger(dr, Nothing)
    End Function
    Public Function MakeListGuid(ByVal dr As IDataReader) As List(Of Guid)
        Return MakeListGuid(dr, Nothing)
    End Function
    Public Function MakeListDate(ByVal dr As IDataReader) As List(Of DateTime)
        Return MakeListDate(dr, Nothing)
    End Function
    Public Function MakeListString(ByVal dt As DataTable) As List(Of String)
        Return MakeListString(dt, Nothing)
    End Function
    Public Function MakeListInteger(ByVal dt As DataTable) As List(Of Integer)
        Return MakeListInteger(dt, Nothing)
    End Function
    Public Function MakeListGuid(ByVal dt As DataTable) As List(Of Guid)
        Return MakeListGuid(dt, Nothing)
    End Function
    Public Function MakeListDate(ByVal dt As DataTable) As List(Of DateTime)
        Return MakeListDate(dt, Nothing)
    End Function

    'Datareaders
    Public Function MakeListString(ByVal dr As IDataReader, ByVal colName As String) As List(Of String)
        Dim list As New List(Of String)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Nothing) Else list.Add(dr.GetString(0))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetStr(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListInteger(ByVal dr As IDataReader, ByVal colName As String) As List(Of Integer)
        Dim list As New List(Of Integer)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Integer.MinValue) Else list.Add(CInt(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetInt(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListGuid(ByVal dr As IDataReader, ByVal colName As String) As List(Of Guid)
        Dim list As New List(Of Guid)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(Guid.Empty) Else list.Add(CType(dr(0), Guid))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetGuid(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function
    Public Function MakeListDate(ByVal dr As IDataReader, ByVal colName As String) As List(Of DateTime)
        Dim list As New List(Of DateTime)()
        Try
            If IsNothing(colName) Then
                While dr.Read()
                    If dr.IsDBNull(0) Then list.Add(DateTime.MinValue) Else list.Add(CDate(dr(0)))
                End While
            Else
                While dr.Read()
                    list.Add(CAdoData.GetDate(dr, colName))
                End While
            End If
        Catch
            Throw
        Finally
            dr.Close()
        End Try
        Return list
    End Function

    'DataTables
    Public Function MakeListString(ByVal dt As DataTable, ByVal colName As String) As List(Of String)
        Dim list As New List(Of String)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each i As DataRow In dt.Rows
                If i.IsNull(0) Then list.Add(Nothing) Else list.Add(CStr(i(0)))
            Next
        Else
            For Each i As DataRow In dt.Rows
                list.Add(CAdoData.GetStr(i, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListInteger(ByVal dt As DataTable, ByVal colName As String) As List(Of Integer)
        Dim list As New List(Of Integer)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each i As DataRow In dt.Rows
                If i.IsNull(0) Then list.Add(Integer.MinValue) Else list.Add(CInt(i(0)))
            Next
        Else
            For Each i As DataRow In dt.Rows
                list.Add(CAdoData.GetInt(i, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListGuid(ByVal dt As DataTable, ByVal colName As String) As List(Of Guid)
        Dim list As New List(Of Guid)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each dr As DataRow In dt.Rows
                If dr.IsNull(0) Then list.Add(Guid.Empty) Else list.Add(CType(dr(0), Guid))
            Next
        Else
            For Each dr As DataRow In dt.Rows
                list.Add(CAdoData.GetGuid(dr, colName))
            Next
        End If
        Return list
    End Function
    Public Function MakeListDate(ByVal dt As DataTable, ByVal colName As String) As List(Of DateTime)
        Dim list As New List(Of DateTime)(dt.Rows.Count)
        If IsNothing(colName) Then
            For Each dr As DataRow In dt.Rows
                If dr.IsNull(0) Then list.Add(DateTime.MinValue) Else list.Add(CDate(dr(0)))
            Next
        Else
            For Each dr As DataRow In dt.Rows
                list.Add(CAdoData.GetDate(dr, colName))
            Next
        End If
        Return list
    End Function
#End Region

#Region "List tables"
    Public Overridable ReadOnly Property SqlToListAllTables() As String
        Get
            Return String.Empty
        End Get
    End Property
    Public Overridable Function AllTableNames() As List(Of String)
        If TypeOf Me Is COleDb Then
            'Folder path as connection string
            If m_connectionString.ToLower.Contains("fmt=delimited") Then Return AllFileNames()

            'Excel/Access etc: use connection.GetSchema
            Dim cn As System.Data.OleDb.OleDbConnection = CType(Local.Connection(), System.Data.OleDb.OleDbConnection)
            Try
                Dim list As List(Of String) = MakeListString(cn.GetSchema("Tables"), "TABLE_NAME")
                Dim shortList As New List(Of String)(list.Count)
                For Each i As String In list
                    If Not i.StartsWith("MSys") Then shortList.Add(i)
                Next
                Return shortList
            Catch
                Return Nothing
            Finally
                cn.Close()
            End Try
        End If

        If String.IsNullOrEmpty(SqlToListAllTables) Then Return Nothing

        Return MakeListString(SqlToListAllTables)
    End Function
    Private Function AllFileNames() As List(Of String)
        Dim list As New List(Of String)
        Dim i As Integer = m_connectionString.ToLower.IndexOf("data source=")
        If -1 = i Then Return list

        Dim s As String = m_connectionString.Substring(i + 12)
        i = s.IndexOf(";")
        If -1 <> i Then s = s.Substring(0, i)
        If Not IO.Directory.Exists(s) Then Return list

        For Each path As String In IO.Directory.GetFiles(s)
            Select Case IO.Path.GetExtension(path).ToLower
                Case ".csv", ".tab", ".asc" : list.Add(IO.Path.GetFileName(path))
            End Select
        Next
        Return list
    End Function
#End Region

#Region "Shared - Csv Export"
    'Export Constants
    Private Const COMMA As Char = CChar(",")
    Private Const DOUBLE_QUOTE As Char = CChar("""")
    Private Const DOUBLE_QUOTEx2 As String = """"""
    Private Const DEFAULT_NAME As String = "Export"

    'Http overloads (Dataset/Datatable)
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse)
        ExportToCsv(ds, response, DEFAULT_NAME)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String)
        ExportToCsv(ds, response, fileName, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer)
        ExportToCsv(ds, response, fileName, startAtColumn, Nothing)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String)
        ExportToCsv(ds.Tables(0), response, fileName, startAtColumn, chopColumnNamePrefix)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal chopColumnNamePrefix As String) 'For backwards compat
        ExportToCsv(ds.Tables(0), response, fileName, 0, chopColumnNamePrefix)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse)
        ExportToCsv(dt, response, DEFAULT_NAME)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String)
        ExportToCsv(dt, response, fileName, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer)
        ExportToCsv(dt, response, fileName, startAtColumn, Nothing)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String)
        ExportToCsv(dt, response, fileName, startAtColumn, chopColumnNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal chopColumnNamePrefix As String) 'For backwards compat
        ExportToCsv(dt, response, fileName, 0, chopColumnNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal response As System.Web.HttpResponse, ByVal fileName As String, ByVal startAtColumn As Integer, ByVal chopColumnNamePrefix As String, ByVal delimiter As Char)
        ExportToCsv(response, fileName)
        ExportToCsv(dt, response.OutputStream, startAtColumn, chopColumnNamePrefix, delimiter)
        response.End()
    End Sub

    'Dataset to Csv
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal filePath As String)
        ExportToCsv(ds.Tables(0), filePath, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal ds As DataSet, ByVal stream As IO.Stream)
        ExportToCsv(ds.Tables(0), stream, 0)
    End Sub

    'Main overloads - file/stream
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String)
        ExportToCsv(dt, filePath, 0)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal stream As IO.Stream)
        ExportToCsv(dt, stream, 0)
    End Sub

    'Overloads - StartAt Column
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer)
        ExportToCsv(dt, filePath, startAtColumn, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal stream As IO.Stream, ByVal startAtColumn As Integer)
        ExportToCsv(dt, stream, startAtColumn, COMMA)
    End Sub

    'Overloads - name Prefix Char
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal s As IO.Stream, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String)
        ExportToCsv(dt, New IO.StreamWriter(s), startAtColumn, chopNamePrefix, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String)
        ExportToCsv(dt, filePath, startAtColumn, chopNamePrefix, COMMA)
    End Sub

    'Overloads - Sep Char
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal s As IO.Stream, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        ExportToCsv(dt, New IO.StreamWriter(s), startAtColumn, chopNamePrefix, sepChar)
    End Sub
    Public Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal filePath As String, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        Dim writer As New System.IO.StreamWriter(filePath)
        ExportToCsv(dt, writer, startAtColumn, chopNamePrefix, sepChar)
    End Sub

    'Non-Dataset overloads (e.g. direct from high-level objects)
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal fileName As String)
        If Not fileName.ToLower.EndsWith(".csv") Then fileName &= ".csv"
        With response
            .ContentType = "text/csv"
            .AddHeader("content-disposition", String.Concat("attachment; filename=", fileName))
        End With
    End Sub
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal ParamArray headings As String())
        Dim sw As New IO.StreamWriter(response.OutputStream)
        ExportToCsv(sw, headings)
        sw.Flush()
    End Sub
    Public Shared Sub ExportToCsv(ByVal response As HttpResponse, ByVal ParamArray dataRow As Object())
        Dim sw As New IO.StreamWriter(response.OutputStream)
        ExportToCsv(sw, dataRow)
        sw.Flush()
    End Sub
    Public Shared Sub ExportToCsv(ByVal writer As IO.StreamWriter, ByVal ParamArray headings As String())
        ExportToCsv(headings, writer)
    End Sub
    Public Shared Sub ExportToCsv(ByVal writer As IO.StreamWriter, ByVal ParamArray dataRow As Object())
        ExportToCsv(dataRow, writer)
    End Sub
    Public Shared Sub ExportToCsv(ByVal headings As String(), ByVal writer As IO.StreamWriter)
        ExportToCsv(headings, writer, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As Object(), ByVal writer As IO.StreamWriter)
        ExportToCsv(data, writer, COMMA)
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As Object(), ByVal writer As IO.StreamWriter, ByVal sepChar As Char)
        Dim sb As StringBuilder = Nothing
        For Each i As Object In data
            If IsNothing(sb) Then sb = New StringBuilder() Else sb.Append(sepChar)
            sb.Append(StripNewLines(i, sepChar))
        Next
        writer.WriteLine(sb.ToString())
    End Sub
    Public Shared Sub ExportToCsv(ByVal data As String(), ByVal writer As IO.StreamWriter, ByVal sepChar As Char)
        Dim sb As StringBuilder = Nothing
        For Each i As String In data
            If IsNothing(sb) Then sb = New StringBuilder() Else sb.Append(sepChar)
            sb.Append(StripNewLines(i, sepChar))
        Next
        writer.WriteLine(sb.ToString())
    End Sub


    'Private CSV logic
    Private Shared Sub ExportToCsv(ByVal dt As DataTable, ByVal writer As IO.StreamWriter, ByVal startAtColumn As Integer, ByVal chopNamePrefix As String, ByVal sepChar As Char)
        '1. write a line with the column names
        Dim headings As New List(Of String)(dt.Columns.Count)
        For Each col As DataColumn In dt.Columns
            If col.Ordinal < startAtColumn Then Continue For
            If String.IsNullOrEmpty(chopNamePrefix) OrElse Not col.ColumnName.ToLower.StartsWith(chopNamePrefix.ToLower) Then
                headings.Add(col.ColumnName)
            Else
                headings.Add(col.ColumnName.Substring(chopNamePrefix.Length))
            End If
        Next
        ExportToCsv(headings.ToArray, writer, sepChar)

        '2. write all the data rows
        Dim data As New List(Of Object)(dt.Columns.Count)
        For Each row As DataRow In dt.Rows
            data.Clear()
            For Each col As DataColumn In dt.Columns
                If col.Ordinal < startAtColumn Then Continue For
                data.Add(row(col))
            Next
            ExportToCsv(data.ToArray, writer, sepChar)
        Next

        writer.Close()
    End Sub
    Private Shared Function StripNewLines(ByVal obj As Object, ByVal sepChar As Char) As String
        'Null Values
        If IsNothing(obj) Then Return String.Empty
        If TypeOf obj Is Integer AndAlso Integer.MinValue = CInt(obj) Then Return String.Empty
        If TypeOf obj Is Decimal AndAlso Decimal.MinValue = CDec(obj) Then Return String.Empty
        If TypeOf obj Is DateTime AndAlso DateTime.MinValue = CType(obj, DateTime) Then Return String.Empty
        If TypeOf obj Is Double AndAlso Double.IsNaN(CDbl(obj)) Then Return String.Empty
        'Binary Data
        If TypeOf obj Is Byte() Then Return String.Concat("0x", CBinary.BytesToHex(CType(obj, Byte())))

        'Special Encoding
        Return StripNewLines(obj.ToString(), sepChar)
    End Function
    Private Shared Function StripNewLines(ByVal s As String, ByVal sepChar As Char) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty

        'These 3 delimiter chars need special encoding, so encase the whole string in doublequotes, and replace any " with ""
        If s.Contains(sepChar) OrElse s.Contains(vbCr) OrElse s.Contains(vbLf) OrElse s.Contains(DOUBLE_QUOTE) Then
            Return String.Concat(DOUBLE_QUOTE, s.Replace(DOUBLE_QUOTE, DOUBLE_QUOTEx2), DOUBLE_QUOTE)
        End If
        Return s
    End Function
#End Region

End Class


