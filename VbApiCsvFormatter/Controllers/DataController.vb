Option Strict On
Option Explicit On

Imports System.Net
Imports System.Web.Http
Imports Newtonsoft.Json

Namespace Controllers
    Public Class DataController
        Inherits ApiController

        Private data As List(Of SDOHLCV)

        Public Sub New()
            Dim json As String = "[
            {
                ""nSymbol"": ""AAPL"",
                ""nDate"": ""20181130"",
                ""nOpen"": 180.29,
                ""nHigh"": 180.33,
                ""nLow"": 177.03,
                ""nClose"": 178.58,
                ""nVolume"": 35981329
            },
            {
                ""nSymbol"": ""AAPL"",
                ""nDate"": ""20181129"",
                ""nOpen"": 182.66,
                ""nHigh"": 182.8,
                ""nLow"": 177.7,
                ""nClose"": 179.55,
                ""nVolume"": 41321868
            },
            {
                ""nSymbol"": ""AAPL"",
                ""nDate"": ""20181128"",
                ""nOpen"": 176.73,
                ""nHigh"": 181.29,
                ""nLow"": 174.93,
                ""nClose"": 180.94,
                ""nVolume"": 45438931
            },
            {
                ""nSymbol"": ""AAPL"",
                ""nDate"": ""20181127"",
                ""nOpen"": 171.51,
                ""nHigh"": 174.77,
                ""nLow"": 170.88,
                ""nClose"": 174.24,
                ""nVolume"": 40711655
            },
            {
                ""nSymbol"": ""AAPL"",
                ""nDate"": ""20181126"",
                ""nOpen"": 174.24,
                ""nHigh"": 174.95,
                ""nLow"": 170.26,
                ""nClose"": 174.62,
                ""nVolume"": 43467288
            }
        ]"
            data = JsonConvert.DeserializeObject(Of List(Of SDOHLCV))(json)
        End Sub

        Public Function [Get]() As IEnumerable(Of SDOHLCV)
            Return data
        End Function

        Public Function [Get](ByVal id As Integer) As String
            Return "value"
        End Function

        Public Sub Post(
<FromBody> ByVal value As String)
        End Sub

        Public Sub Put(ByVal id As Integer,
<FromBody> ByVal value As String)
        End Sub

        Public Sub Delete(ByVal id As Integer)
        End Sub
    End Class
End Namespace