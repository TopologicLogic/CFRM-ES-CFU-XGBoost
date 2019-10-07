
Class Showdown : Inherits GameTreeNode
    Public value As Double
    Public value2 As Double ' Originally used for utility weighting

    Public Sub New(ByVal tvalue As Double, Optional ByVal hh As String = "")
        If hh <> "" Then Console.WriteLine(hh & vbTab & tvalue & " (showdown)")
        If tvalue.ToString.IndexOf(".") >= 0 Then MsgBox("Showdown: " & tvalue)
        Me.value = tvalue
        Me.value2 = Math.Round(tvalue + (tvalue * 0.1))
        Me._is_showdown = True
        'tsize += 2
    End Sub

    Public Overrides Function TrainProbingCFU(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        If ti.evs(trainplayer) > ti.evs(trainplayer Xor 1) Then Return value
        If ti.evs(0) = ti.evs(1) Then Return 0
        Return -value
    End Function

    Public Overrides Function TrainProbing(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        If ti.evs(trainplayer) > ti.evs(trainplayer Xor 1) Then Return value
        If ti.evs(0) = ti.evs(1) Then Return 0
        Return -value
    End Function

    Public Overrides Function TrainExternalSamplingCFU(ByVal trainplayer As Integer, ByRef ti As train_info) As Double
        If ti.evs(trainplayer) > ti.evs(trainplayer Xor 1) Then Return value
        If ti.evs(0) = ti.evs(1) Then Return 0
        Return -value
    End Function

    Public Overrides Function BestResponse(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                           ByRef hi As List(Of single_hand_info), ByVal hand As Integer, ByVal op() As Double) As Double
        Dim ev As Double = 0
        For i As Integer = 0 To hi.Count - 1
            If hi(hand).ev > hi(i).ev Then
                ev += value * op(i)
            ElseIf hi(hand).ev < hi(i).ev Then
                ev -= value * op(i)
            End If
        Next
        Return ev
    End Function

End Class
