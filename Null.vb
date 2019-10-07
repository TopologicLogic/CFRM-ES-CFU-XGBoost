
Class Null : Inherits GameTreeNode

    Public Sub New()
        Me._is_null = True
    End Sub
    Public Overrides Function TrainProbingCFU(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        Return Double.NaN
    End Function

    Public Overrides Function TrainProbing(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double
        Return Double.NaN
    End Function

    Public Overrides Function TrainExternalSamplingCFU(ByVal trainplayer As Integer, ByRef ti As train_info) As Double
        Return Double.NaN
    End Function

    Public Overrides Function BestResponse(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                           ByRef hi As List(Of single_hand_info), ByVal hand As Integer, ByVal op() As Double) As Double
        Return Double.NaN
    End Function

End Class
