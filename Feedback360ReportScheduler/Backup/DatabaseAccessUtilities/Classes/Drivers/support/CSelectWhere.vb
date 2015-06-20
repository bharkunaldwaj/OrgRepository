﻿<CLSCompliant(True), Serializable()> _
Public Class CSelectWhere : Inherits CWhere

#Region "Members"
    Public SelectCols As String
    Public OrderBy As String
#End Region

#Region "Constructor"
    'Basic Combinations
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal criteria As CCriteria, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, criteria, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal criteriaList As CCriteriaList, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, criteriaList, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, unsafeWhereClause, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
    'Overloads
    Public Sub New(ByVal selectCols As String, ByVal where As CWhere, ByVal orderByCols As String)
        MyBase.New(where.TableName, where.TxOrNull)
        Me.SelectCols = selectCols
        With where
            Me.Criteria = .Criteria
            Me.CriteriaList = .CriteriaList
            Me.UnsafeWhereClause = .UnsafeWhereClause
        End With
        Me.OrderBy = orderByCols
    End Sub
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal values As CNameValueList, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, values, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
    Public Sub New(ByVal selectCols As String, ByVal tableName As String, ByVal name As String, ByVal value As Object, ByVal orderByCols As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(tableName, name, value, txOrNull)
        Me.SelectCols = selectCols
        Me.OrderBy = orderByCols
    End Sub
#End Region

End Class
