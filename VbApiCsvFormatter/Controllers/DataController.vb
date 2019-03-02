Option Strict On
Option Explicit On

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Newtonsoft.Json
Imports System.Linq

Namespace Controllers
    Public Class DataController
        Inherits ApiController

        Private data As List(Of SDOHLCV)

        Public Sub New()
            Dim json As String = "[
            {
                ""nSymbol"": ""AAPL1"",
                ""nDate"": ""20181130"",
                ""nOpen"": 180.29,
                ""nHigh"": 180.33,
                ""nLow"": 177.03,
                ""nClose"": 178.58,
                ""nVolume"": 35981329
            },
            {
                ""nSymbol"": ""AAPL1"",
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

        Public Function [Get](ByVal Symbol As String) As HttpResponseMessage

            'Mock fetch data
            Dim queryResult = From d In data
                              Where d.nSymbol = Symbol
                              Select d

            Dim sb = New StringBuilder()
            sb.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", "Symbol", "Date", "Open", "High", "Low", "Close", "Volume"))
            For Each SDOHLCV In queryResult.ToList()
                sb.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", Escape(SDOHLCV.nSymbol), Escape(SDOHLCV.nDate), Escape(SDOHLCV.nOpen), Escape(SDOHLCV.nHigh), Escape(SDOHLCV.nLow), Escape(SDOHLCV.nClose), Escape(SDOHLCV.nVolume)))
            Next

            Dim ms As Stream = New MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()))

            Dim HttpResponseMessage As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK)
            HttpResponseMessage.Content = New StreamContent(ms)
            HttpResponseMessage.Content.Headers.ContentDisposition = New System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            HttpResponseMessage.Content.Headers.ContentDisposition.FileName = "SDOHLCV.csv"
            HttpResponseMessage.Content.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream")
            HttpResponseMessage.Content.Headers.ContentLength = ms.Length
            Return HttpResponseMessage

        End Function

        Shared _specialChars As Char() = New Char() {","c, Convert.ToChar(vbLf), Convert.ToChar(vbCr), """"c}

        Private Function Escape(ByVal o As Object) As String
            If o Is Nothing Then
                Return ""
            End If

            Dim field As String = o.ToString()

            If field.IndexOfAny(_specialChars) <> -1 Then
                Return String.Format("""{0}""", field.Replace("""", """"""))
            Else
                Return field
            End If
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