
Class Fold : Inherits GameTreeNode
    Public value As Double
    Public value2 As Double
    Public player As Integer

    ' Must pass a negative value!!!
    Public Sub New(ByVal tplayer As Integer, ByVal tvalue As Double, Optional ByVal hh As String = "")
        If hh <> "" Then Console.WriteLine(hh & vbTab & tvalue & " (fold)")
        If tvalue > 0 Then Throw New Exception("Fold nodes must be a negative value, received: " & tvalue)
        Me.value = tvalue
        Me.value2 = Math.Round(-tvalue + (-tvalue * 0.1))
        Me.player = tplayer
        Me._is_fold = True
        'tsize += 3
    End Sub

    Public Overrides Function TrainProbingCFU(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        If trainplayer = player Then Return value
        Return -value
    End Function

    Public Overrides Function TrainProbing(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        If trainplayer = player Then Return value
        Return -value
    End Function

    Public Overrides Function TrainExternalSamplingCFU(ByVal trainplayer As Integer, ByRef ti As train_info) As Double
        If trainplayer = player Then Return value
        Return -value
    End Function

    Public Overrides Function BestResponse(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                           ByRef hi As List(Of single_hand_info), ByVal hand As Integer, ByVal op() As Double) As Double
        Dim ev As Double = 0
        For i As Integer = 0 To op.Length - 1
            ev += value * op(i)
        Next
        If brplayer = player Then Return ev
        Return -ev
    End Function


End Class
