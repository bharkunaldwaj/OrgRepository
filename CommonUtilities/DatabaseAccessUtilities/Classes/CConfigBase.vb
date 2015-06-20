Imports System.Configuration
Imports System.Configuration.ConfigurationManager

<CLSCompliant(True)> _
Public Class CConfigBase

    'Standard Connection String
    Public Shared Function Driver() As String
        Return Config("Driver")
    End Function
    Public Shared Function ConnectionString() As String
        If ConnectionStrings.Count = 0 Then
            Return Config("ConnectionString")
        Else
            Return Config("ConnectionString", ConnectionStrings(0).ConnectionString) 'Use the first connnection string
        End If
    End Function
    Public Shared Function ConnectionString(ByVal name As String) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(name)
        If IsNothing(cs) Then Return String.Empty
        Return cs.ConnectionString
    End Function

    'Shorter Alternatives
    Public Shared Function SqlExpressPath() As String
        Return Config("SqlExpressPath", Config("SqlExpress"))
    End Function
    Public Shared Function AccessDatabasePath() As String
        Return Config("AccessDatabasePath", Config("AccessDatabase"))
    End Function
    Public Shared Function ExcelDatabasePath() As String
        Return Config("ExcelDatabasePath", Config("ExcelDatabase"))
    End Function
    Public Shared Function WebSite() As String
        Return Config("WebSite")
    End Function

    'Named connection string (refers to Connection Strings node)
    Public Shared Function ActiveConnectionString() As String
        Return Config("ActiveConnectionString")
    End Function

    Public Shared Function CommandTimeoutSecs() As Integer
        Return ConfigInt("CommandTimeoutSecs")
    End Function


    'Default Encryption key(s)
    Friend Shared Function FastEncryptionKey() As String
        Return Config("EncryptionKey", Config("WebServicePassword", "db75794fe12147539b9596b523d916e87bcbeb06cb35480abca1d98d66b403727b4cd6d4cd344e9eb79cf511ed4f04d9"))
    End Function
    Friend Shared ReadOnly Property TripleDesIV() As String
        Get
            Return Config("TripleDesIV", "D08601BA9F91BA88")
        End Get
    End Property
    Friend Shared ReadOnly Property TripleDesKey() As String
        Get
            Return Config("TripleDesKey", "8B682A81B28348D69231136CA376A90C2D3D25CC046B6406")
        End Get
    End Property


    'WebService Driver (and optional Proxy)
    Public Shared Sub CheckPassword(ByVal password As String)
        If Not String.IsNullOrEmpty(CConfigBase.WebServicePassword) Then
            If password <> CConfigBase.WebServicePassword Then Throw New Exception("Invalid webservice password")
        End If
    End Sub
    Public Shared ReadOnly Property WebServicePassword() As String
        Get
            Return Config("WebServicePassword")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyAddress() As String
        Get
            Return Config("ProxyAddress")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyUser() As String
        Get
            Return Config("ProxyUser")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyPassword() As String
        Get
            Return Config("ProxyPassword")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyDomain() As String
        Get
            Return Config("ProxyDomain")
        End Get
    End Property

    'Default Cache Timeout
    Public Shared ReadOnly Property CacheTimeoutDefault() As TimeSpan
        Get
            Return New TimeSpan(CacheTimeoutHours, CacheTimeoutMinutes, CacheTimeoutSeconds)
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutHours() As Integer
        Get
            Return ConfigInt("CacheTimeoutHours", 3)
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutMinutes() As Integer
        Get
            Return ConfigInt("CacheTimeoutMinutes", 0)
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutSeconds() As Integer
        Get
            Return ConfigInt("CacheTimeoutSeconds", 0)
        End Get
    End Property

    'Utilities
    Protected Shared Function Config(ByVal key As String) As String
        Dim s As String = AppSettings(key)
        If IsNothing(s) Then Return String.Empty
        Return s
    End Function
    Protected Shared Function Config(ByVal key As String, ByVal defaultValue As String) As String
        Config = Config(key)
        If Len(Config) = 0 Then Config = defaultValue
    End Function
    Protected Shared Function ConfigOrEx(ByVal key As String) As String
        Dim s As String = Config(key)
        If Len(s) = 0 Then Throw New Exception("Missing Config setting: " & key)
        Return s
    End Function
    Protected Shared Function ConfigBool(ByVal key As String) As Boolean
        Return ConfigBool(key, False)
    End Function
    Protected Shared Function ConfigBool(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
        Dim s As String = Config(key)
        If Len(s) = 0 Then Return defaultValue
        If 0 = String.Compare("true", s, True) Then Return True
        If 0 = String.Compare("yes", s, True) Then Return True
        Return False
    End Function
    Protected Shared Function ConfigInt(ByVal key As String) As Integer
        Return ConfigInt(key, Integer.MinValue)
    End Function
    Protected Shared Function ConfigInt(ByVal key As String, ByVal defaultValue As Integer) As Integer
        Dim s As String = Config(key)
        Dim i As Integer
        If Not Integer.TryParse(s, i) Then Return defaultValue
        Return i
    End Function


End Class
