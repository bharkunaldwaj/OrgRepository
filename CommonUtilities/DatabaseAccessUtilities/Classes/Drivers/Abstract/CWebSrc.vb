Imports System.Net

'Covers the common logic between the webservice/webpage drivers (soap/binary)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CWebSrc : Inherits CDataSrcRemote

#Region "Constructors"
    Public Sub New(ByVal url As String, ByVal password As String)
        MyBase.New(url)
        m_password = password
        m_connectionString = DefaultPageName(url)
    End Sub
#End Region

#Region "Abstract"
    Protected MustOverride Function DefaultPageName(ByVal url As String) As String
#End Region

#Region "Events"
    Public Event AsyncOperationError(ByVal ex As Exception)
    Protected Sub Completed(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If Not IsNothing(e.Error) Then RaiseEvent AsyncOperationError(e.Error)
    End Sub
#End Region

#Region "Private - Members"
    Private m_password As String
    Private m_proxy As WebProxy
#End Region

#Region "Private/Protected - Properties"
    Protected ReadOnly Property Url() As String
        Get
            Return m_connectionString
        End Get
    End Property
    Protected ReadOnly Property Proxy() As WebProxy
        Get
            If IsNothing(m_proxy) Then
                'Use Proxy ()
                If Len(CConfigBase.ProxyAddress) = 0 Then Return Nothing

                m_proxy = New WebProxy(CConfigBase.ProxyAddress)

                'Credentials ()
                If Len(CConfigBase.ProxyUser) > 0 Then
                    If Len(CConfigBase.ProxyDomain) > 0 Then
                        m_proxy.Credentials = New NetworkCredential(CConfigBase.ProxyUser, CConfigBase.ProxyPassword, CConfigBase.ProxyDomain)
                    Else
                        m_proxy.Credentials = New NetworkCredential(CConfigBase.ProxyUser, CConfigBase.ProxyPassword)
                    End If
                End If
            End If
            Return m_proxy
        End Get
    End Property
    Protected ReadOnly Property Password() As String
        Get
            Return m_password
        End Get
    End Property
#End Region

#Region "Private - Pack/Unpack"
    Protected Function Pack(ByVal cmd As CCommand) As Byte()
        CheckTxIsNull(cmd.Transaction)
        Return CBinary.Pack(cmd, Password)
    End Function
    Protected Function Pack(ByVal obj As Object) As Byte()
        Return CBinary.Pack(obj, Password)
    End Function
    Protected Function Unpack(ByVal obj As Byte()) As Object
        Return CBinary.Unpack(obj, Password)
    End Function
#End Region

End Class
