
<Serializable()>
Public Structure train_info
    Public pocket() As ULong
    Public flop As ULong
    Public turn As ULong
    Public river As ULong
    Public evs() As UInteger
    Public pin()() As Double
    Public fin()() As Double
    Public tin()() As Double
    Public rin()() As Double
    Public m As Keras.Models.BaseModel
    Public c As Keras.Models.BaseModel
    Public mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel
    Public cxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel

    Public regrets As List(Of (Double(), Double()))
    Public cummulatives As List(Of (Double(), Double()))
End Structure

Public Structure single_hand_info
    Public ev As UInteger
    Public pin() As Double
    Public fin() As Double
    Public tin() As Double
    Public rin() As Double
End Structure

<Serializable()>
Public Structure nn_data
    Public inputs() As Double
    Public outputs() As Double
End Structure

Public MustInherit Class GameTreeNode

    'Public Shared raise_vals As New Hashtable

    Public Shared logging As Boolean = False

    Public Shared rand As New ThreadSafeRandom()
    Public Shared r_pooling_only As Boolean = False
    Public Shared r_percent As Double = 1
    Public Shared c_percent As Double = 1

    Public _is_fold As Boolean = False
    Public _is_showdown As Boolean = False
    Public _is_decision As Boolean = False
    Public _is_null As Boolean = False

    Public MustOverride Function TrainProbingCFU(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double

    Public MustOverride Function TrainProbing(ByVal trainplayer As Integer, ByRef ti As train_info, ByVal ooq As Double, ByVal op As Double, ByVal probe As Boolean) As Double

    Public MustOverride Function TrainExternalSamplingCFU(ByVal trainplayer As Integer, ByRef ti As train_info) As Double

    Public MustOverride Function BestResponse(ByVal brplayer As Integer, ByRef mxgb As SharpLearning.XGBoost.Models.RegressionXGBoostModel,
                                           ByRef hi As List(Of single_hand_info), ByVal hand As Integer, ByVal op() As Double) As Double


End Class
